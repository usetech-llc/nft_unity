using System.Numerics;
using Polkadot.Source.Utils;

namespace NftUnity.Extensions
{
    public static class ConvertExtensions
    {
        public static byte[] ToCompactBytes(this uint value)
        {
            return Scale.EncodeCompactInteger(new BigInteger(value)).Bytes;
        }
    }
}