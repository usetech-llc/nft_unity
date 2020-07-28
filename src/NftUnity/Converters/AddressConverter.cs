using System;
using System.IO;
using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.Utils;

namespace NftUnity.Converters
{
    public class AddressConverter : IBinaryConverter
    {
        public void Serialize(Stream stream, object value, IBinarySerializer serializer, object[] parameters)
        {
            var address = (Address) value;
            if (address == null)
            {
                return;
            }

            var key = AddressUtils.GetPublicKeyFromAddr(address).Bytes;
            serializer.Serialize(key, stream);
        }

        public object Deserialize(Type type, Stream stream, IBinarySerializer deserializer, object[] parameters)
        {
            var key = new byte[32];
            stream.Read(key, 0, key.Length);

            var address = AddressUtils.GetAddrFromPublicKey(new PublicKey() {Bytes = key});
            return new Address()
            {
                Symbols = address
            };
        }
    }
}