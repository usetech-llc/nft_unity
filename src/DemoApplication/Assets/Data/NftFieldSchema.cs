using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class NftFieldSchema
{
    [NotNull] 
    public string FieldName;
    public int FieldSize;

    public NftFieldSchema()
    {
    }

    public NftFieldSchema([NotNull] string fieldName, int fieldSize)
    {
        FieldName = fieldName;
        FieldSize = fieldSize;
    }
}
