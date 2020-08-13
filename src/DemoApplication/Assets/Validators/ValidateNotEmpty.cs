using System;
using System.Collections;
using System.Collections.Generic;
using OneOf;
using OneOf.Types;
using UnityEngine;
using UnityEngine.UI;

public class ValidateNotEmpty
{
    public static Func<InputField, OneOf<Success, Error<string>>> CreateInputFieldValidator(Func<string> messageTemplate)
    {
        return (field) =>
        {
            if (string.IsNullOrWhiteSpace(field.text))
            {
                return new Error<string>(messageTemplate());
            }

            return new Success();
        };
    }
}
