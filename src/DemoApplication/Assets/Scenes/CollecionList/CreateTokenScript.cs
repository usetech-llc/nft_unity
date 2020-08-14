using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using NftUnity;
using NftUnity.Models.Collection;
using NftUnity.Models.Events;
using NftUnity.Models.Item;
using Polkadot.DataStructs;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class CreateTokenScript : MonoBehaviour
{
    public Text tokenLabel;
    public GameObject firstFieldItem;
    public GameObject fieldItemTemplate;

    public UnityEvent closePopup;

    private List<GameObject> _fields = new List<GameObject>();
    private INftClient NftClient => NftClientFactory.CreateClient(GameSettings.Uri);
    private ulong _collectionId;

    public void SetCollection(ulong collectionId, Collection collection)
    {
        _collectionId = collectionId;
        ResetData();
        tokenLabel.text = $"Mint token in collection {collectionId} - {collection.Name}";

        var schema = LoadSchema(collectionId, collection);
        CreateFields(schema);
    }

    private void CreateFields(List<NftFieldSchema> schema)
    {
        var initialPosition = firstFieldItem.transform.position;
        var offset = 0f;
        foreach (var field in schema)
        {
            var fieldItem = Instantiate(fieldItemTemplate, firstFieldItem.transform);
            _fields.Add(fieldItem);
            fieldItem.transform.SetParent(firstFieldItem.transform.parent, false);
            var fieldScript = fieldItem.GetComponent<EditTokenFieldScript>();
            fieldScript.Schema = field;

            var height = fieldItem.transform.GetComponent<RectTransform>().rect.height;

            var itemOffset = initialPosition.y - offset;
            fieldItem.transform.position = new Vector3(initialPosition.x, itemOffset, initialPosition.z);
            offset += height + 4;
        }
        
        firstFieldItem.transform.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, offset);
    }
        

    private void ResetData()
    {
        foreach (var field in _fields)
        {
            Destroy(field);
        }
        
        _fields.Clear();
    }

    private static List<NftFieldSchema> LoadSchema(ulong collectionId, Collection collection)
    {
        var schema = NftCollectionSchema.TryLoad(collectionId);
        return schema?.Fields ?? new List<NftFieldSchema>()
        {
            new NftFieldSchema("Field", (int) collection.CustomDataSize)
        };
    }

    public void CreateToken()
    {
        var fieldValues = _fields
            .Select(f => f.GetComponent<EditTokenFieldScript>())
            .Select(f => (value: f.Value, schema: f.Schema))
            .ToList();
        if (fieldValues.Any(v => v.value == null))
        {
            return;
        }

        var value = fieldValues.SelectMany(v => ConvertBigint(v.value.Value, v.schema.FieldSize)).ToArray();
        
        var createItem = new CreateItem(_collectionId, value, new Address(GameSettings.Address));

        NftClient.ItemManagement.CreateItem(createItem, new Address(GameSettings.Address), GameSettings.PrivateKey);
        closePopup?.Invoke();
    }

    public void Cancel()
    {
        closePopup?.Invoke();
    }

    private IEnumerable<byte> ConvertBigint(BigInteger bigInteger, int schemaFieldSize)
    {
        int returned = 0;

        while (returned < schemaFieldSize)
        {
            yield return (byte)(bigInteger & 0xff);
            bigInteger >>= 8;
            returned++;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
