using System.Collections;
using System.Collections.Generic;
using Polkadot.DataStructs;
using Polkadot.Utils;
using UnityEngine;

public static class Extensions
{
    public static string ToAddress(this PublicKey publicKey)
    {
        return AddressUtils.GetAddrFromPublicKey(publicKey);
    }
    
}
