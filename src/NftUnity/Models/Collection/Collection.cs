using NftUnity.Converters;
using NftUnity.Models.Collection.AccessModeEnum;
using NftUnity.Models.Collection.CollectionModeEnum;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs;

namespace NftUnity.Models.Collection
{
    public class Collection
    {
        [Serialize(0)]
        public PublicKey Owner = null!;

        [Serialize(1)]
        public CollectionMode CollectionMode = null!;

        [Serialize(2)]
        public AccessMode AccessMode = null!;

        [Serialize(3)]
        public ulong NextItemId;

        [Serialize(4)]
        public uint DecimalPoints;

        [Serialize(5)]
        [Converter(ConverterType = typeof(UnicodeZeroTerminatedStringConverter))]
        public string Name = null!;

        [Serialize(6)]
        [Converter(ConverterType = typeof(UnicodeZeroTerminatedStringConverter))]
        public string Description = null!;

        [Serialize(7)]
        [Converter(ConverterType = typeof(Utf8ZeroTerminatedStringConverter))]
        public string TokenPrefix = null!;

        [Serialize(8)]
        public uint CustomDataSize;

        [Serialize(9)]
        [Converter(ConverterType = typeof(Utf8StringConverter))]
        public string OffChainSchema = null!;

        [Serialize(10)]
        public PublicKey Sponsor = null!;

        [Serialize(11)]
        public PublicKey UnconfirmedSponsor = null!;
        
        public Collection()
        {
        }

        public Collection(PublicKey owner, ulong nextItemId, string name, string description, string tokenPrefix, uint customDataSize, PublicKey sponsor, PublicKey unconfirmedSponsor, CollectionMode collectionMode, AccessMode accessMode, uint decimalPoints, string offChainSchema)
        {
            Owner = owner;
            NextItemId = nextItemId;
            Name = name;
            Description = description;
            TokenPrefix = tokenPrefix;
            CustomDataSize = customDataSize;
            Sponsor = sponsor;
            UnconfirmedSponsor = unconfirmedSponsor;
            CollectionMode = collectionMode;
            AccessMode = accessMode;
            DecimalPoints = decimalPoints;
            OffChainSchema = offChainSchema;
        }
    }
}