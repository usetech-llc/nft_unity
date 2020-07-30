using Polkadot.BinarySerializer;

namespace NftUnity.Converters
{
    public class AddressConverterAttribute : ConverterAttribute
    {
        public AddressConverterAttribute()
        {
            ConverterType = typeof(AddressConverter);
        }
    }
}