using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;

public class NftCollectionSchema
{
    [NotNull] 
    public List<NftFieldSchema> Fields;

    public ulong Id;

    public NftCollectionSchema()
    {
    }

    public NftCollectionSchema(List<NftFieldSchema> fields, ulong id)
    {
        Fields = fields;
        Id = id;
    }

    public void Save()
    {
        var serialized = JsonConvert.SerializeObject(this);
        File.WriteAllText(FilePath(Id), serialized);
    }

    private static string FilePath(ulong collectionId)
    {
        return $"collection-schema-{collectionId}";
    }

    [CanBeNull]
    public static NftCollectionSchema TryLoad(ulong collectionId)
    {
        var path = FilePath(collectionId);
        if (!File.Exists(path))
        {
            return null;
        }

        try
        {
            var content = File.ReadAllText(path);
            var schema = JsonConvert.DeserializeObject<NftCollectionSchema>(content);
            return schema;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
