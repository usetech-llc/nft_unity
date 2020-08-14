using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using NftUnity.Models.Item;
using UnityEngine;
using UnityEngine.UI;

public class TokenContentScript : MonoBehaviour
{
    public Text tokenLabel;
    public InputField ownerField;
    public GameObject firstFieldItem;
    public GameObject fieldItemTemplate;


    public void SetToken(ItemKey key, NftItem item)
    {
        tokenLabel.text = $"Token {key.ItemId}";
        ownerField.text = item.Owner.ToAddress();

        var values = ParseFields(key, item);
        RenderFields(values);
    }

    private void RenderFields(List<(string field, int size, string value)> values)
    {
        var initialPosition = firstFieldItem.transform.position;
        var height = firstFieldItem.transform.GetComponent<RectTransform>().rect.height;
        var offset = 0f;
        foreach (var (field, size, value) in values)
        {
            var fieldItem = Instantiate(fieldItemTemplate, firstFieldItem.transform);
            fieldItem.transform.SetParent(firstFieldItem.transform.parent, false);
            var fieldScript = fieldItem.GetComponent<ViewTokenFieldScript>();
            fieldScript.Field = field; 
            fieldScript.Size = size; 
            fieldScript.Value = value; 

            var itemOffset = initialPosition.y - offset;
            fieldItem.transform.position = new UnityEngine.Vector3(initialPosition.x, itemOffset, initialPosition.z);
            offset += height + 4;
        }
        
        firstFieldItem.transform.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, values.Count * (height + 4));
    }

    private List<(string field, int size, string value)> ParseFields(ItemKey key, NftItem item)
    {
        var values = new List<(string field, int size, string value)>();
        var schema = NftCollectionSchema.TryLoad(key.CollectionId);
        values = new List<(string field, int size, string value)>();
        if (schema == null)
        {
            values.Add(("value", item.Data.Length, ParseBigInt(item.Data, 0, item.Data.Length).ToString()));
        }
        else
        {
            var offset = 0;
            foreach (var field in schema.Fields)
            {
                values.Add((field.FieldName, field.FieldSize, ParseBigInt(item.Data, offset, field.FieldSize).ToString()));
                offset += field.FieldSize;
            }
        }
        return values;
    }

    private BigInteger ParseBigInt(byte[] itemData, int offset, int length)
    {
        var parsed = BigInteger.Zero;
        for (int i = offset + length - 1; i >= offset; i--)
        {
            parsed <<= 8;
            byte value = 0;
            if (itemData.Length > i)
            {
                value = itemData[i];
            }
            parsed |= value;
        }

        return parsed;
    }
}
