using NftUnity.Converters;
using Polkadot.BinarySerializer;
using Polkadot.DataStructs;

namespace NftUnity.Models
{
    public class Collection
    {
        [Serialize(0)]
        public PublicKey Owner;

        [Serialize(1)]
        public ulong NextItemId;

        [Serialize(2)]
        [Converter(ConverterType = typeof(UnicodeZeroTerminatedStringConverter))]
        public string Name;

        [Serialize(3)]
        [Converter(ConverterType = typeof(UnicodeZeroTerminatedStringConverter))]
        public string Description;

        [Serialize(4)]
        [Converter(ConverterType = typeof(Utf8ZeroTerminatedStringConverter))]
        public string TokenPrefix;

        [Serialize(5)]
        public uint CustomDataSize;

        public Collection()
        {
        }

        public Collection(PublicKey owner, ulong nextItemId, string name, string description, string tokenPrefix, uint customDataSize)
        {
            Owner = owner;
            NextItemId = nextItemId;
            Name = name;
            Description = description;
            TokenPrefix = tokenPrefix;
            CustomDataSize = customDataSize;
        }
    }
}