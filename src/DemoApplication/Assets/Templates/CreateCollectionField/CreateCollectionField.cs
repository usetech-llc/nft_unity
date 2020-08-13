using System;
using System.Collections;
using System.Collections.Generic;
using OneOf;
using OneOf.Types;
using UnityEngine;
using UnityEngine.UI;

public class CreateCollectionField : MonoBehaviour
{
    public InputField nameField;
    public InputField sizeField;

    public Action<ValidatorSet> OnValidationChange;
    public ValidatorSet SizeValidator;
    public ValidatorSet NameValidator;


    public string FieldName
    {
        get => nameField.text;
        set => nameField.text = value;
    }

    public int? FieldSize
    {
        get => int.TryParse(sizeField.text, out var size) ? (int?)size : null;
        set => sizeField.text = value?.ToString() ?? "";
    }
    
    // Start is called before the first frame update
    void Start()
    {
        SizeValidator = ValidatorSet.ValidateInputField(sizeField, new List<Func<InputField, OneOf<Success, Error<string>>>>()
        {
            ValidateNotEmpty.CreateInputFieldValidator(() => "Size is required."),
            ValidateCanBeParsed.CreateInputFieldValidator<int>(int.TryParse, () => "Size must be an integer value."),
        }, v => OnValidationChange?.Invoke(v));
        
        NameValidator = ValidatorSet.ValidateInputField(nameField, new List<Func<InputField, OneOf<Success, Error<string>>>>()
        {
            ValidateNotEmpty.CreateInputFieldValidator(() => "Name is required")
        }, v => OnValidationChange?.Invoke(v));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        SizeValidator?.Unsubscribe();
        SizeValidator = null;
        NameValidator?.Unsubscribe();
        NameValidator = null;
    }
}
