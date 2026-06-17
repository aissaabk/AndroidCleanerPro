using System.Collections.Generic;

namespace AndroidCleanerPro.Services
{
    public static class AdDatabase
    {
        public static List<string> AdSdkSignatures = new()
        {
            "com.bytedance.sdk",
            "com.applovin",
            "com.google.android.gms.ads",
            "com.facebook.ads",
            "com.unity3d.ads",
            "ironsource",
            "vungle",
            "startapp",
            "inmobi"
        };
    }
}