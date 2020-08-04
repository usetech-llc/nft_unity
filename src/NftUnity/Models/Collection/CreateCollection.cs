using System;
using System.Text;
using NftUnity.Converters;
using NftUnity.Models.Collection.CollectionModeEnum;
using Polkadot.BinarySerializer;

namespace NftUnity.Models.Collection
{
    public class CreateCollection
    {
        private string _name = null!;
        private string _description = null!;
        private string _tokenPrefix = null!;

        [Serialize(0)]
        [Converter(ConverterType = typeof(UnicodeStringConverter))]
        public string Name
        {
            get => _name;
            set
            {
                if (value.Length > 63)
                {
                    throw new ArgumentException($@"Maximum length exceeded.
Property: {nameof(CreateCollection)}.{nameof(Name)}.
Maximum length: 63.
Value: {value} is {value.Length} long.");
                }
                _name = value;
            }
        }

        [Serialize(1)]
        [Converter(ConverterType = typeof(UnicodeStringConverter))]
        public string Description
        {
            get => _description;
            set
            {
                if (value.Length > 255)
                {
                    throw new ArgumentException($@"Maximum length exceeded.
Property: {nameof(CreateCollection)}.{nameof(Description)}.
Maximum length: 255.
Value: {value} is {value.Length} long.");
                }
                _description = value;
            }
        }

        [Serialize(2)]
        [Converter(ConverterType = typeof(Utf8StringConverter))]
        public string TokenPrefix
        {
            get => _tokenPrefix;
            set
            {
                var length = Encoding.UTF8.GetByteCount(value);
                if (length > 15)
                {
                    throw new ArgumentException($@"Maximum length exceeded.
Property: {nameof(CreateCollection)}.{nameof(Description)}.
Maximum length: 15.
Value: {value} is {length} long.");
                }
                _tokenPrefix = value;
            }
        }

        [Serialize(3)]
        public CollectionMode Mode = null!;

        public CreateCollection()
        {
        }

        public CreateCollection(string name, string description, string tokenPrefix, CollectionMode mode)
        {
            Name = name;
            Description = description;
            TokenPrefix = tokenPrefix;
            Mode = mode;
        }
    }
}