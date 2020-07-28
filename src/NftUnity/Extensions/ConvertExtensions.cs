using System.Numerics;
using Polkadot.BinarySerializer;
using Polkadot.Source.Utils;
using Polkadot.Utils;

namespace NftUnity.Extensions
{
    public static class ConvertExtensions
    {
        public static byte[] ToCompactBytes(this uint value)
        {
            return Scale.EncodeCompactInteger(value).Bytes;
        }

        public static byte[] ToCompactBytes(this int value)
        {
            return Scale.EncodeCompactInteger(value).Bytes;
        }
    }
}