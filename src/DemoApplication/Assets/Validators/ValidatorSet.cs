using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OneOf;
using OneOf.Types;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ValidatorSet
{
    public Action Unsubscribe
    {
        get;
        private set;
    }

    public Action ClearErrors
    {
        get;
        private set;
    }

    public Action Validate;
    
    public IList<string> ValidationErrors;

    public bool IsValid => !ValidationErrors?.Any() ?? true;

    /// <summary>
    /// Adds validations to the input field.
    /// </summary>
    /// <param name="field"></param>
    /// <param name="validators"></param>
    /// <returns></returns>
    public static ValidatorSet ValidateInputField(InputField field, ICollection<Func<InputField, OneOf<Success, Error<string>>>> validators, Action<ValidatorSet> onValidationChanged)
    {
        var defaultFieldColor = field.targetGraphic.color;
        ValidatorSet validatorSet = new ValidatorSet();

        void Validate(string value)
        {
            var errors = validators.Select(v => v(field))
                .Where(v => v.IsT1)
                .Select(v => v.AsT1.Value)
                .ToList();
            if (errors.Count > 0)
            {
                field.targetGraphic.color = Color.red;
            }
            else
            {
                field.targetGraphic.color = defaultFieldColor;
            }

            validatorSet.ValidationErrors = errors;
            onValidationChanged?.Invoke(validatorSet);
        }
        validatorSet.Unsubscribe = () => field.onValueChanged.RemoveListener(Validate);
        validatorSet.ClearErrors = () =>
        {
            validatorSet.ValidationErrors.Clear();
            field.targetGraphic.color = defaultFieldColor;
        };

        validatorSet.Validate = () => Validate(field.text);

        field.onValueChanged.AddListener(Validate);
        return validatorSet;
    }
}
