using NftUnity.Converters;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace NftUnity.Models.Collection
{
    public class SetOffChainSchema
    {
        [Serialize(0)]
        public ulong CollectionId;

        [Serialize(1)]
        [Converter(ConverterType = typeof(Utf8StringConverter))]
        public string Schema = null!;

        public SetOffChainSchema()
        {
        }

        public SetOffChainSchema(ulong collectionId, string schema)
        {
            CollectionId = collectionId;
            Schema = schema;
        }
    }
}