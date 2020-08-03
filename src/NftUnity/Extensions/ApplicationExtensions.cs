using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Polkadot.Api;
using Polkadot.BinarySerializer.Extensions;
using Polkadot.DataStructs;
using Polkadot.Utils;

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

        [return: MaybeNull]
        public static TStorageItem GetStorageObject<TStorageItem, TParam>(this IApplication application, TParam param,
            string module, string storageName)
        {
            var request = application.GetStorage(param, module, storageName);
            if (string.IsNullOrEmpty(request))
            {
                return default;
            }

            var nullableType = Nullable.GetUnderlyingType(typeof(TStorageItem));
            if (nullableType != null)
            {
                using var ms = new MemoryStream(request.HexToByteArray());
                return (TStorageItem) application.Serializer.Deserialize(nullableType, ms);
            }
            return application.Serializer.DeserializeAssertReadAll<TStorageItem>(request.HexToByteArray());
        }
    }
}