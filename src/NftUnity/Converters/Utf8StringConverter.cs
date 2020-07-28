using System;
using System.IO;
using System.Text;
using NftUnity.Extensions;
using Polkadot.BinarySerializer;
using Polkadot.Utils;

namespace NftUnity.Converters
{
    public class Utf8StringConverter : IBinaryConverter
    {
        public void Serialize(Stream stream, object value, IBinarySerializer serializer, object[] parameters)
        {
            var stringValue = (string) value;
            if (stringValue == null)
            {
                serializer.Serialize(0, stream);
                return;
            }
            var compactLength = Encoding.UTF8.GetByteCount(stringValue).ToCompactBytes();
            serializer.Serialize(compactLength, stream);
            serializer.Serialize(Encoding.UTF8.GetBytes(stringValue), stream);
        }

        public object Deserialize(Type type, Stream stream, IBinarySerializer deserializer, object[] parameters)
        {
            var bytesCount = Scale.DecodeCompactInteger(stream).Value;
            if (bytesCount == 0)
            {
                return "";
            }
            
            var bytes = new byte[(int)bytesCount];
            stream.Read(bytes, 0, bytes.Length);

            return Encoding.UTF8.GetString(bytes);
        }
    }
}