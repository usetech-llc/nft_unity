using System;
using System.IO;
using System.Text;
using Polkadot.BinarySerializer;
using Polkadot.Utils;

namespace NftUnity.Converters
{
    public class UnicodeStringConverter : IBinaryConverter
    {
        public void Serialize(Stream stream, object value, IBinarySerializer serializer)
        {
            var stringValue = (string) value;
            if (stringValue == null)
            {
                serializer.Serialize(0, stream);
                return;
            }
            var compactLength = Scale.EncodeCompactInteger(stringValue.Length).Bytes;
            serializer.Serialize(compactLength, stream);
            serializer.Serialize(Encoding.Unicode.GetBytes(stringValue), stream);
        }
    }
}