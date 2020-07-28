using Polkadot.Api;
using Polkadot.DataStructs;

namespace NftUnity.Extensions
{
    public static class ApplicationExtensions
    {
        public static string SubmitExtrinsicObject<T>(this IApplication application, T param, string module, string method,
            Address sender, string privateKey)
        {
            var methodBytes = application.Serializer.Serialize(param);

            return application.SubmitExtrinsic(methodBytes, module, method, sender, privateKey);
        }
    }
}