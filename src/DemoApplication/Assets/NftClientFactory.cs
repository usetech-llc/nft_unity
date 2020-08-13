using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using NftUnity;
using Scenes.Login;
using UnityEngine;

public static class NftClientFactory
{
    private static readonly ConcurrentDictionary<string, INftClient> Clients = new ConcurrentDictionary<string, INftClient>();
    
    public static INftClient CreateClient(string uri)
    {
        return Clients.GetOrAdd(uri, u =>
        {
            var connectionString = uri;
            var nftClientSettings = new NftClientSettings(connectionString, maxReconnectCount: 200);
            return new NftClient(nftClientSettings);
        });
    }
}
