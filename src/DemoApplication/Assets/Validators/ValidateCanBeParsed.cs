using System;
using System.Collections;
using System.Collections.Generic;
using OneOf;
using OneOf.Types;
using UnityEngine;
using UnityEngine.UI;

public delegate bool ParseFunction<T>(string value, out T parsed);

public class ValidateCanBeParsed
{
    public static Func<InputField, OneOf<Success, Error<string>>> CreateInputFieldValidator<T>(ParseFunction<T> parse, Func<string> errorMessage)
    {
        return field =>
        {
            if (parse(field.text, out var _))
            {
                return new Success();
            }

            return new Error<string>(errorMessage());
        };
    }
}
