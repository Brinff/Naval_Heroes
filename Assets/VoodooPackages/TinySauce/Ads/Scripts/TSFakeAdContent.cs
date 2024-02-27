using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voodoo.Tiny.Sauce.Internal.Ads
{
    //[CreateAssetMenu(fileName = "Assets/VoodooPackages/TinySauce/Ads/Resources/FakeAds/FakeAd", menuName = "TinySauce/FakeAd")]
    public class TSFakeAdContent : ScriptableObject
    {
        public TSFakeAdContent Load() => Resources.Load<TSFakeAdContent>($"FakeAds/{name}");
        
        [Header("Platform URLs")]
        [Tooltip("URL of the AppStore page of the game")]
        public string iosUrl;
        [Tooltip("URL of the PlayStore page of the game")]
        public string androidUrl;
        
        [Header("Image")]
        [Tooltip("Image as ad")]
        public Sprite sprite;
    }
}
