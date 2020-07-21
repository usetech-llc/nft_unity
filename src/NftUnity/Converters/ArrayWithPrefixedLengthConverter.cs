using System;
using System.IO;
using System.Numerics;
using Polkadot.BinaryContracts;
using Polkadot.BinarySerializer;
using Polkadot.Utils;

namespace NftUnity.Converters
{
    public class ArrayWithPrefixedLengthConverter : IBinaryConverter
    {
        public void Serialize(Stream stream, object value, IBinarySerializer serializer)
        {
            var compactLength = Scale.EncodeCompactInteger(((Array) value).Length).Bytes;
            serializer.Serialize(compactLength, stream);
            serializer.Serialize(value, stream);
        }
    }
}