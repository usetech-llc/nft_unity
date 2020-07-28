using System;
using System.IO;
using System.Text;
using Polkadot.BinarySerializer;
using Polkadot.Utils;

namespace NftUnity.Converters
{
    public class UnicodeStringConverter : IBinaryConverter
    {
        public void Serialize(Stream stream, object value, IBinarySerializer serializer, object[] parameters)
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

        public object Deserialize(Type type, Stream stream, IBinarySerializer deserializer, object[] parameters)
        {
            var length = (int)Scale.DecodeCompactInteger(stream).Value;
            if (length == 0)
            {
                return "";
            }

            var strBytes = new byte[length * 2];
            stream.Read(strBytes, 0, strBytes.Length);

            return Encoding.Unicode.GetString(strBytes);
        }
    }
}