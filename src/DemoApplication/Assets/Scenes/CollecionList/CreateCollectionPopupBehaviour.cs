using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NftUnity;
using NftUnity.Extensions;
using NftUnity.Models.Collection;
using NftUnity.Models.Collection.CollectionModeEnum;
using NftUnity.Models.Events;
using OneOf;
using OneOf.Types;
using Polkadot.DataStructs;
using Polkadot.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CreateCollectionPopupBehaviour : MonoBehaviour
{
    public InputField nameField;
    public InputField descriptionField;
    public InputField tokenPrefixField;
    public GameObject firstFieldItem;
    public GameObject fieldItemTemplate;
    public Button createCollectionButton;
    public Text totalSizeText;
    public INftClient NftClient;

    public UnityEvent onCloseCollection;

    private ValidatorSet NameValidator;
    private ValidatorSet DescriptionValidator;
    private List<CreateCollectionField> _fieldScripts = new List<CreateCollectionField>();
    private List<GameObject> _fields = new List<GameObject>();

    private ConcurrentQueue<Action> _updateQueue = new ConcurrentQueue<Action>();
    private bool _savingInProgress = false;

    // Start is called before the first frame update
    void Start()
    {
        NameValidator = ValidatorSet.ValidateInputField(nameField, new List<Func<InputField, OneOf<Success, Error<string>>>>()
        {
            ValidateNotEmpty.CreateInputFieldValidator(() => "Collection name is required.")
        }, v => EnableCreateCollectionIfValid());
        DescriptionValidator = ValidatorSet.ValidateInputField(descriptionField, new List<Func<InputField, OneOf<Success, Error<string>>>>()
        {
            ValidateNotEmpty.CreateInputFieldValidator(() => "Collection description is required.")
        }, v => EnableCreateCollectionIfValid());
    }

    private void EnableCreateCollectionIfValid()
    {
    }

    private IEnumerable<ValidatorSet> AllValidators()
    {
        yield return NameValidator;
        yield return DescriptionValidator;
        foreach (var field in _fieldScripts)
        {
            yield return field.NameValidator;
            yield return field.SizeValidator;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_updateQueue.TryDequeue(out var update))
        {
            update();
        }
    }

    private void OnDestroy()
    {
        NameValidator?.Unsubscribe();
        NameValidator = null;
        DescriptionValidator?.Unsubscribe();
        DescriptionValidator = null;
    }

    public void AddField()
    {
        var field = Instantiate(fieldItemTemplate, firstFieldItem.transform);
        _fields.Add(field);
        field.transform.SetParent(firstFieldItem.transform.parent, false);

        var fieldScript = field.GetComponent<CreateCollectionField>();
        _fieldScripts.Add(fieldScript);
        fieldScript.OnValidationChange = v =>
        {
            EnableCreateCollectionIfValid();
            RecalculateTotalSize();
        };
        
        var initialPosition = firstFieldItem.transform.position;
        var height = field.transform.GetComponent<RectTransform>().rect.height;
        var itemOffset = initialPosition.y - (_fieldScripts.Count - 1) * (height + 4);
        field.transform.position = new Vector3(initialPosition.x, itemOffset, initialPosition.z);
        
        firstFieldItem.transform.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _fields.Count * (height + 4));
    }

    private void RecalculateTotalSize()
    {
        var size = CustomDataSize();

        totalSizeText.text = $"Token Data Size: {size.ToString()}";
    }

    private int CustomDataSize()
    {
        var size = _fieldScripts?
            .Select(f => f.FieldSize)
            .Sum();
        return size ?? 0;
    }

    public void Reset()
    {
        NameValidator?.ClearErrors();
        DescriptionValidator?.ClearErrors();
        foreach (var fieldScript in _fieldScripts)
        {
            fieldScript.OnValidationChange = null;
        }
        
        _fieldScripts?.Clear();
        foreach (var field in _fields)
        {
            Destroy(field);
        }
        _fields?.Clear();
        firstFieldItem?.transform.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, firstFieldItem.transform.parent.parent.GetComponent<RectTransform>().rect.height);
        RecalculateTotalSize();
        nameField.text = "";
        descriptionField.text = "";
        tokenPrefixField.text = "";
    }

    public void Create()
    {
        if (_savingInProgress)
        {
            return;
        }

        _savingInProgress = true;
        
        foreach (var validator in AllValidators())
        {
            validator.Validate();
        }

        if (AllValidators().SelectMany(v => v.ValidationErrors).Any())
        {
            return;
        }
        
        var size = (uint)CustomDataSize(); 
        var address = GameSettings.Address;
        var publicKey = AddressUtils.GetPublicKeyFromAddr(address);


        var fieldsSchema = _fieldScripts
            .Select(f => new NftFieldSchema(f.FieldName, f.FieldSize ?? 0))
            .ToList();


        void Cleanup()
        {
            NftClient.CollectionManagement.CollectionCreated -= Handler;
            _updateQueue.Enqueue(() => onCloseCollection?.Invoke());
            _savingInProgress = false;
        }

        void Handler(object sender, Created created)
        {
            if (publicKey.Bytes.SequenceEqual(created.Account.Bytes))
            {
                var schema = new NftCollectionSchema(fieldsSchema, created.Id);
                schema.Save();
                Cleanup();
            }
        }

        NftClient.CollectionManagement.CollectionCreated += Handler;
        
        var createCollection = new CreateCollection(nameField.text, descriptionField.text, tokenPrefixField.text, new CollectionMode(new Nft(size)));
        NftClient.CollectionManagement.CreateCollection(createCollection, new Address(address), GameSettings.PrivateKey);
    }

    public void Cancel()
    {
        onCloseCollection?.Invoke();
    }
}
