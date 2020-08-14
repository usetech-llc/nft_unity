using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class EditTokenFieldScript : MonoBehaviour
{
    public Text fieldLabelText;
    public InputField valueField;
    private NftFieldSchema _schema;

    public string FieldName
    {
        get => fieldLabelText.text;
        set => fieldLabelText.text = value ?? "";
    }

    public BigInteger? Value
    {
        get => BigInteger.TryParse(valueField.text, out var result) ? (BigInteger?)result : null;
        set => valueField.text = value?.ToString() ?? "";
    }

    public NftFieldSchema Schema
    {
        get => _schema;
        set
        {
            _schema = value;
            FieldName = $"{value.FieldName}, ({value.FieldSize} {(value.FieldSize == 1 ? "byte" : "bytes")})";
            Value = 0;
        }
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
