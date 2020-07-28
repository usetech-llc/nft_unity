using System;
using System.IO;
using System.Text;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Extensions;

namespace NftUnity.Converters
{
    public class UnicodeZeroTerminatedStringConverter : IBinaryConverter
    {
        public void Serialize(Stream stream, object value, IBinarySerializer serializer, object[] parameters)
        {
            var stringValue = (string) value;
            if (string.IsNullOrEmpty(stringValue))
            {
                var zeroLength = Scale.EncodeCompactInteger(1).Bytes;
                stream.Write(zeroLength, 0, zeroLength.Length);
                stream.WriteByte(0);
                stream.WriteByte(0);
                return;
            }

            var length = stringValue.Length + 1;
            var compactLength = Scale.EncodeCompactInteger(length).Bytes;
            
            stream.Write(compactLength, 0, compactLength.Length);

            var bytes = Encoding.Unicode.GetBytes(stringValue);
            stream.Write(bytes, 0, bytes.Length);
            
            stream.WriteByte(0);
            stream.WriteByte(0);
        }

        public object Deserialize(Type type, Stream stream, IBinarySerializer deserializer, object[] parameters)
        {
            var bigLength = Scale.DecodeCompactInteger(stream).Value;
            var arrayLength = (int)bigLength;
            if (arrayLength == 0 || arrayLength == 1)
            {
                stream.ReadByteThrowIfStreamEnd();
                stream.ReadByteThrowIfStreamEnd();
                return "";
            }

            var bytes = new byte[(arrayLength - 1)*2];
            stream.Read(bytes, 0, bytes.Length);
            
            stream.ReadByteThrowIfStreamEnd();
            stream.ReadByteThrowIfStreamEnd();
            return Encoding.Unicode.GetString(bytes);
        }
    }
}