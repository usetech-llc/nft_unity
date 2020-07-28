using System;
using Polkadot.Api;

namespace NftUnity.Extensions
{
    public static class NftClientExtensions
    {
        public static void MakeCallWithReconnect(this INftClient nftClient, Action<IApplication> call)
        {
            try
            {
                call(nftClient.GetApplication());
            }
            catch (Exception ex) when (IsDisconnectedException(ex))
            {
                nftClient.Connect();
                call(nftClient.GetApplication());
            }
        }

        public static T MakeCallWithReconnect<T>(this INftClient nftClient, Func<IApplication, T> call)
        {
            try
            {
                return call(nftClient.GetApplication());
            }
            catch (Exception ex) when (IsDisconnectedException(ex) || ex is TimeoutException)
            {
                nftClient.Connect();
                return call(nftClient.GetApplication());
            }
        }
        
        private static bool IsDisconnectedException(Exception ex)
        {
            return ex is ApplicationException applicationException &&
                   string.Equals("Not connected", applicationException.Message);
        }

    }
}