using NftUnity.Converters;
using Polkadot.BinarySerializer;
using Polkadot.DataStructs;

namespace NftUnity.Models
{
    public class Collection
    {
        [Serialize(0)]
        public PublicKey Owner = null!;

        [Serialize(1)]
        public ulong NextItemId;

        [Serialize(2)]
        [Converter(ConverterType = typeof(UnicodeZeroTerminatedStringConverter))]
        public string Name = null!;

        [Serialize(3)]
        [Converter(ConverterType = typeof(UnicodeZeroTerminatedStringConverter))]
        public string Description = null!;

        [Serialize(4)]
        [Converter(ConverterType = typeof(Utf8ZeroTerminatedStringConverter))]
        public string TokenPrefix = null!;

        [Serialize(5)]
        public uint CustomDataSize;

        [Serialize(6)]
        public PublicKey Sponsor = null!;

        [Serialize(7)]
        public PublicKey UnconfirmedSponsor = null!;
        
        public Collection()
        {
        }

        public Collection(PublicKey owner, ulong nextItemId, string name, string description, string tokenPrefix, uint customDataSize, PublicKey sponsor, PublicKey unconfirmedSponsor)
        {
            Owner = owner;
            NextItemId = nextItemId;
            Name = name;
            Description = description;
            TokenPrefix = tokenPrefix;
            CustomDataSize = customDataSize;
            Sponsor = sponsor;
            UnconfirmedSponsor = unconfirmedSponsor;
        }
    }
}