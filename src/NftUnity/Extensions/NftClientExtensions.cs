using System;
using Polkadot.Api;

namespace NftUnity.Extensions
{
    public static class NftClientExtensions
    {
        public static void MakeCallWithReconnect(this INftClient nftClient, Action<IApplication> call, int retryCount)
        {
            try
            {
                call(nftClient.GetApplication());
            }
            catch (Exception ex) when (IsDisconnectedException(ex))
            {
                nftClient.Connect();
                if (retryCount <= 1)
                {
                    call(nftClient.GetApplication());
                }
                else
                {
                    MakeCallWithReconnect(nftClient, call, retryCount - 1);
                }
            }
        }

        public static T MakeCallWithReconnect<T>(this INftClient nftClient, Func<IApplication, T> call, int retryCount)
        {
            try
            {
                return call(nftClient.GetApplication());
            }
            catch (Exception ex) when (IsDisconnectedException(ex) || ex is TimeoutException)
            {
                nftClient.Connect();
                if (retryCount <= 1)
                {
                    return call(nftClient.GetApplication());
                }
                else
                {
                    return MakeCallWithReconnect(nftClient, call, retryCount - 1);
                }
            }
        }
        
        private static bool IsDisconnectedException(Exception ex)
        {
            return ex is ApplicationException applicationException &&
                   string.Equals("Not connected", applicationException.Message);
        }

    }
}