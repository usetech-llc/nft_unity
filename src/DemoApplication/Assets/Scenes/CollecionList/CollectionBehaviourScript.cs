using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NftUnity;
using NftUnity.Extensions;
using NftUnity.Models.Collection;
using NftUnity.Models.Events;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class CollectionBehaviourScript : MonoBehaviour
{
    public GameObject balanceTextBackground;
    public Text balanceText;

    public GameObject firstItem;
    public GameObject collectionItemTemplate;

    public GameObject createCollectionPopup;
    public GameObject createCollectionPopupScript;
    
    private INftClient Client => NftClientFactory.CreateClient(GameSettings.Uri);
    private ConcurrentQueue<Action> _updateQueue = new ConcurrentQueue<Action>();
    private Dictionary<ulong, Collection> _loadedCollections = new Dictionary<ulong, Collection>();
    private Dictionary<ulong, GameObject> _collectionGameObjects = new Dictionary<ulong, GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        Client.CollectionManagement.CollectionCreated += OnCollectionCreated;
        LoadBalance();
        LoadCollections();
    }

    private void OnCollectionCreated(object sender, Created e)
    {
        LoadCollection(e.Id);
    }

    private void LoadCollection(ulong id)
    {
        Task.Run(() =>
        {
            var collection = Client.CollectionManagement.GetCollection(id);
            if (collection == null)
            {
                return;
            }
            _updateQueue.Enqueue(() =>
            {
                _loadedCollections[id] = collection;
                RenderCollection(id);
            });
        });
    }

    private void RenderCollection(ulong id)
    {
        if (!_collectionGameObjects.TryGetValue(id, out var collectionGameObject))
        {
            collectionGameObject = Instantiate(collectionItemTemplate, firstItem.transform);
            collectionGameObject.transform.SetParent(firstItem.transform.parent, false);
            _collectionGameObjects[id] = collectionGameObject;
        }

        collectionGameObject.GetComponent<CollectionItem>().SetCollection(_loadedCollections[id], id);
        SortCollections();
    }

    private void SortCollections()
    {
        var sorted = _collectionGameObjects
            .OrderByDescending(p => p.Key)
            .Select(p => p.Value);
        var initialPosition = firstItem.transform.position;
        var y = initialPosition.y;
        var height = firstItem.transform.GetComponent<RectTransform>().rect.height;
        foreach (var collectionGameObject in sorted)
        {
            collectionGameObject.transform.position = new Vector3(initialPosition.x, y, initialPosition.z);
            y -= height + 4;
        }

        firstItem.transform.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (height + 4) * _collectionGameObjects.Count);
    }

    private void LoadCollections()
    {
        Task.Run(() =>
        {
            var lastId = Client.CollectionManagement.NextCollectionId();
            if (lastId == null)
            {
                throw new NullReferenceException();
            }

            for (ulong i = 0; i <= lastId; i++)
            {
                var i1 = i;
                _updateQueue.Enqueue(() => LoadCollection(i1));
            }
        });
    }

    private void LoadBalance()
    {
        Task.Run(() =>
        {
            Client.MakeCallWithReconnect(application =>
            {
                var address = GameSettings.Address;
                return application.SubscribeAccountInfo(
                    address,
                    accountInfo =>
                    {
                        _updateQueue.Enqueue(
                            () =>
                            {
                                balanceText.text = $"Balance: {accountInfo.AccountData.Free/new BigInteger(1000000000000000L)}";
                                var preferredWidth = balanceText.preferredWidth;
                                balanceTextBackground.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, preferredWidth + 8);
                            });
                    });
            }, Client.Settings.MaxReconnectCount);
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (_updateQueue.TryDequeue(out var update))
        {
            update();
        }
    }
    
    void OnDestroy()
    {
        Client.CollectionManagement.CollectionCreated -= OnCollectionCreated;
    }

    public void ShowCreateCollectionPopup()
    {
        var createCollectionPopupBehaviour =
            createCollectionPopupScript.GetComponent<CreateCollectionPopupBehaviour>();
        createCollectionPopupBehaviour.Reset();
        createCollectionPopupBehaviour.NftClient = Client;
        createCollectionPopup.SetActive(true);
    }

    public void CloseCreateCollectionPopup()
    {
        createCollectionPopup.SetActive(false);
    }
}
