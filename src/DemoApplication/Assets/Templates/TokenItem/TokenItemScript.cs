using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TokenItemScript : MonoBehaviour
{
    public Text idText;
    public GameObject background;

    public bool IsActive
    {
        get => BackgroundImage.color == Color.gray;
        set => BackgroundImage.color = value ? Color.gray : _defaultColor;
    }

    public ulong Id
    {
        get => _id;
        set
        {
            _id = value;
            idText.text = value.ToString();
        }
    }

    public UnityAction<ulong> OnSelect;

    private Color _defaultColor;
    private ulong _id;
    private Image BackgroundImage => background.GetComponent<Image>();


    // Start is called before the first frame update
    void Start()
    {
        _defaultColor = idText.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click()
    {
        OnSelect?.Invoke(_id);
    }
}
