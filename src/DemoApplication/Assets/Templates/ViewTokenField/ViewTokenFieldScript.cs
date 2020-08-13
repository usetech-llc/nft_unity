using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewTokenFieldScript : MonoBehaviour
{
    public Text fieldText;
    public InputField sizeField;
    public InputField valueField;
    private int _size;


    public string Field
    {
        get => fieldText.text;
        set => fieldText.text = value;
    }

    public int Size
    {
        get => _size;
        set
        {
            _size = value;
            sizeField.text = value.ToString();
        }
    }

    public string Value
    {
        get => valueField.text;
        set => valueField.text = value;
    }
}
