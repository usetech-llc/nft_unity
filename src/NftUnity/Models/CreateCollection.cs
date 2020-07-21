using System;
using NftUnity.Converters;
using Polkadot.BinarySerializer;

namespace NftUnity.Models
{
    public class CreateCollection
    {
        [Serialize(0)]
        [Converter(typeof(UnicodeStringConverter))]
        public string Name { get; set; } = null!;
        [Serialize(1)]
        [Converter(typeof(UnicodeStringConverter))]
        public string Description { get; set; } = null!;
        [Serialize(2)]
        [Converter(typeof(Utf8StringConverter))]
        public string TokenPrefix { get; set; } = null!;
        [Serialize(3)]
        public uint CustomDataSize { get; set; }

        public CreateCollection()
        {
        }

        public CreateCollection(string name, string description, string tokenPrefix, uint customDataSize)
        {
            Name = name;
            Description = description;
            TokenPrefix = tokenPrefix;
            CustomDataSize = customDataSize;
        }
    }
}