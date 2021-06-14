using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tapdaq
{
	[Serializable]
	public class TDNetwork
	{
		[SerializeField]
		public readonly string name;
        [SerializeField]
        public TDSDKTargetVersion sdkTargetVersion;
        [SerializeField]
		public readonly string cocoapodsAdapterDependency;
		[SerializeField]
		public readonly string mavenAdapterDependency;
		[SerializeField]
		public readonly string mavenNetworkRepository;
		public readonly List<TDFramework> iosDependendencies = new List<TDFramework>();
		[SerializeField]
		public bool androidEnabled = false;
		[SerializeField]
		public bool iOSEnabled = false;
		[SerializeField]
		public bool bundlePresent = false;

        public TDNetwork(string network_name, TDSDKTargetVersion targetSdkVersion, string cocoapods_dependency, string maven_dependency, string maven_network_repository = null, List<TDFramework> iosDependencies = null, bool bundle_present = false)
		{
			this.name = network_name;
            this.sdkTargetVersion = targetSdkVersion;
			this.cocoapodsAdapterDependency = cocoapods_dependency;
			this.mavenAdapterDependency = maven_dependency;
			this.mavenNetworkRepository = maven_network_repository;
			if(iosDependencies != null)
            {
				this.iosDependendencies = iosDependencies;
            }
			this.bundlePresent = bundle_present;
		}

        public static bool IsAndroidSupported(TDNetwork network)
		{
			return network.mavenAdapterDependency != null;
		}

		public static bool IsIOSSupported(TDNetwork network)
		{
			return network.cocoapodsAdapterDependency != null;
		}

		public static readonly List<TDNetwork> AllNetworks = new List<TDNetwork>(new TDNetwork[]
		{
			new TDNetwork("AdColony",
				new TDSDKTargetVersion("9.0", "14"),
				"AdColony",
				"com.tapdaq.sdk:TapdaqAdColonyAdapter:",
				"https://adcolony.bintray.com/AdColony",
				new List<TDFramework>() {
					new TDFramework(TDFramework.ADSUPPORT_FRAMEWORK, false),
					new TDFramework(TDFramework.AUDIO_TOOLBOX_FRAMEWORK, false),
					new TDFramework(TDFramework.AV_FOUNDATION_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_MEDIA_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_TELEPHONY_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_SERVICES_FRAMEWORK, false),
					new TDFramework(TDFramework.MESSAGE_UI_FRAMEWORK, false),
					new TDFramework(TDFramework.APP_TRACKING_TRANSPARENCY_FRAMEWORK, true),
					new TDFramework(TDFramework.JAVASCRIPT_CORE_FRAMEWORK, true),
					new TDFramework(TDFramework.SAFARI_SERVICES_FRAMEWORK, true),
					new TDFramework(TDFramework.STORE_KIT_FRAMEWORK, true),
					new TDFramework(TDFramework.WEB_KIT_FRAMEWORK, true),
					new TDFramework(TDFramework.WATCH_CONNECTIVITY_FRAMEWORK, true),
					new TDFramework(TDFramework.LIB_Z_LIBRARY, false),
					new TDFramework(TDFramework.LIBZ125_LIBRARY, false)
				},
				false),
			new TDNetwork("AdMob",
				new TDSDKTargetVersion("9.0", "16"),
				"AdMob",
				"com.tapdaq.sdk:TapdaqAdMobAdapter:",
				null,
				new List<TDFramework>() {
					new TDFramework(TDFramework.AUDIO_TOOLBOX_FRAMEWORK, false),
					new TDFramework(TDFramework.AV_FOUNDATION_FRAMEWORK, false),
					new TDFramework(TDFramework.CF_NETWORK_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_GRAPHICS_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_MEDIA_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_TELEPHONY_FRAMEWORK, false),
					new TDFramework(TDFramework.MEDIA_PLAYER_FRAMEWORK, false),
					new TDFramework(TDFramework.MESSAGE_UI_FRAMEWORK, false),
					new TDFramework(TDFramework.MOBILE_CORE_SERVICES_FRAMEWORK, false),
					new TDFramework(TDFramework.QUARTZ_CORE_FRAMEWORK, false),
					new TDFramework(TDFramework.SECURITY_FRAMEWORK, false),
					new TDFramework(TDFramework.STORE_KIT_FRAMEWORK, false),
					new TDFramework(TDFramework.SYSTEM_CONFIGURATION_FRAMEWORK, false),
					new TDFramework(TDFramework.ADSUPPORT_FRAMEWORK, true),
					new TDFramework(TDFramework.JAVASCRIPT_CORE_FRAMEWORK, true),
					new TDFramework(TDFramework.SAFARI_SERVICES_FRAMEWORK, true),
					new TDFramework(TDFramework.WEB_KIT_FRAMEWORK, true),
					new TDFramework(TDFramework.LIB_SQLITE3_LIBRARY, false),
					new TDFramework(TDFramework.LIB_Z_LIBRARY, false)
				},
				false),
			new TDNetwork("AppLovin",
				new TDSDKTargetVersion("9.0", "16"),
				"AppLovin",
				"com.tapdaq.sdk:TapdaqAppLovinAdapter:",
				null,
				new List<TDFramework> {
					new TDFramework(TDFramework.ADSUPPORT_FRAMEWORK, false),
					new TDFramework(TDFramework.AUDIO_TOOLBOX_FRAMEWORK, false),
					new TDFramework(TDFramework.AV_FOUNDATION_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_GRAPHICS_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_MEDIA_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_TELEPHONY_FRAMEWORK, false),
					new TDFramework(TDFramework.SAFARI_SERVICES_FRAMEWORK, false),
					new TDFramework(TDFramework.STORE_KIT_FRAMEWORK, false),
					new TDFramework(TDFramework.SYSTEM_CONFIGURATION_FRAMEWORK, false),
					new TDFramework(TDFramework.UI_KIT_FRAMEWORK, false),
					new TDFramework(TDFramework.WEB_KIT_FRAMEWORK, false),
					new TDFramework(TDFramework.LIB_Z_LIBRARY, false)
				},
				true),
			new TDNetwork("Chartboost",
				new TDSDKTargetVersion("9.0", "21"),
				"Chartboost",
				"com.tapdaq.sdk:TapdaqChartboostAdapter:",
				"https://chartboostmobile.bintray.com/Chartboost",
				new List<TDFramework>() {
					new TDFramework(TDFramework.AV_FOUNDATION_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_GRAPHICS_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_MEDIA_FRAMEWORK, false),
					new TDFramework(TDFramework.FOUNDATION_FRAMEWORK, false),
					new TDFramework(TDFramework.STORE_KIT_FRAMEWORK, false),
					new TDFramework(TDFramework.UI_KIT_FRAMEWORK, false),
					new TDFramework(TDFramework.WEB_KIT_FRAMEWORK, false),
					new TDFramework(TDFramework.ADSUPPORT_FRAMEWORK, true)
				},
				false),
			new TDNetwork("Facebook AN",
				new TDSDKTargetVersion("9.0", "14"),
				"FAN",
				"com.tapdaq.sdk:TapdaqFANAdapter:",
				null,
				new List<TDFramework>() {
					new TDFramework(TDFramework.AUDIO_TOOLBOX_FRAMEWORK, false),
					new TDFramework(TDFramework.AV_FOUNDATION_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_GRAPHICS_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_IMAGE_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_MEDIA_FRAMEWORK, false),
					new TDFramework(TDFramework.FOUNDATION_FRAMEWORK, false),
					new TDFramework(TDFramework.STORE_KIT_FRAMEWORK, false),
					new TDFramework(TDFramework.UI_KIT_FRAMEWORK, false),
					new TDFramework(TDFramework.ADSUPPORT_FRAMEWORK, true),
					new TDFramework(TDFramework.CF_NETWORK_FRAMEWORK, true),
					new TDFramework(TDFramework.CORE_MOTION_FRAMEWORK, true),
					new TDFramework(TDFramework.CORE_TELEPHONY_FRAMEWORK, true),
					new TDFramework(TDFramework.LOCAL_AUTHENTICATION_FRAMEWORK, true),
					new TDFramework(TDFramework.SAFARI_SERVICES_FRAMEWORK, true),
					new TDFramework(TDFramework.SYSTEM_CONFIGURATION_FRAMEWORK, true),
					new TDFramework(TDFramework.VIDEO_TOOLBOX_FRAMEWORK, true),
					new TDFramework(TDFramework.WEB_KIT_FRAMEWORK, true),
					new TDFramework(TDFramework.LIB_CPP_LIBRARY, false),
					new TDFramework(TDFramework.LIB_XML2_LIBRARY, false)
				},
				false),
			new TDNetwork("InMobi",
				new TDSDKTargetVersion("9.0", "15"),
				"InMobi",
				"com.tapdaq.sdk:TapdaqInmobiAdapter:",
				null,
				new List<TDFramework>() {
					new TDFramework(TDFramework.ADSUPPORT_FRAMEWORK, false),
					new TDFramework(TDFramework.AUDIO_TOOLBOX_FRAMEWORK, false),
					new TDFramework(TDFramework.AV_FOUNDATION_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_TELEPHONY_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_LOCATION_FRAMEWORK, false),
					new TDFramework(TDFramework.FOUNDATION_FRAMEWORK, false),
					new TDFramework(TDFramework.MEDIA_PLAYER_FRAMEWORK, false),
					new TDFramework(TDFramework.MESSAGE_UI_FRAMEWORK, false),
					new TDFramework(TDFramework.STORE_KIT_FRAMEWORK, false),
					new TDFramework(TDFramework.SOCIAL_FRAMEWORK, false),
					new TDFramework(TDFramework.SYSTEM_CONFIGURATION_FRAMEWORK, false),
					new TDFramework(TDFramework.SECURITY_FRAMEWORK, false),
					new TDFramework(TDFramework.SAFARI_SERVICES_FRAMEWORK, false),
					new TDFramework(TDFramework.UI_KIT_FRAMEWORK, false),
					new TDFramework(TDFramework.WEB_KIT_FRAMEWORK, true),
					new TDFramework(TDFramework.LIB_SQLITE3_LIBRARY, false),
					new TDFramework(TDFramework.LIB_Z_LIBRARY, false)
				},
				false),
			new TDNetwork("ironSource",
				new TDSDKTargetVersion("9.0", "16"),
				"IronSource",
				"com.tapdaq.sdk:TapdaqIronsourceAdapter:",
				"https://dl.bintray.com/ironsource-mobile/android-sdk",
				new List<TDFramework>() {
					new TDFramework(TDFramework.ADSUPPORT_FRAMEWORK, false),
					new TDFramework(TDFramework.AUDIO_TOOLBOX_FRAMEWORK, false),
					new TDFramework(TDFramework.AV_FOUNDATION_FRAMEWORK, false),
					new TDFramework(TDFramework.CF_NETWORK_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_GRAPHICS_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_LOCATION_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_MEDIA_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_TELEPHONY_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_VIDEO_FRAMEWORK, false),
					new TDFramework(TDFramework.FOUNDATION_FRAMEWORK, false),
					new TDFramework(TDFramework.MOBILE_CORE_SERVICES_FRAMEWORK, false),
					new TDFramework(TDFramework.QUARTZ_CORE_FRAMEWORK, false),
					new TDFramework(TDFramework.SECURITY_FRAMEWORK, false),
					new TDFramework(TDFramework.STORE_KIT_FRAMEWORK, false),
					new TDFramework(TDFramework.SYSTEM_CONFIGURATION_FRAMEWORK, false),
					new TDFramework(TDFramework.LIB_Z_LIBRARY, false)
				},
				false),
			new TDNetwork("Maio",
				new TDSDKTargetVersion("9.0", "16"),
				"Maio",
				"com.tapdaq.sdk:TapdaqMaioAdapter:",
				"https://imobile-maio.github.io/maven",
				new List<TDFramework>() {
					new TDFramework(TDFramework.ADSUPPORT_FRAMEWORK, false),
					new TDFramework(TDFramework.AV_FOUNDATION_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_MEDIA_FRAMEWORK, false),
					new TDFramework(TDFramework.MOBILE_CORE_SERVICES_FRAMEWORK, false),
					new TDFramework(TDFramework.STORE_KIT_FRAMEWORK, false),
					new TDFramework(TDFramework.SYSTEM_CONFIGURATION_FRAMEWORK, false),
					new TDFramework(TDFramework.UI_KIT_FRAMEWORK, false),
					new TDFramework(TDFramework.WEB_KIT_FRAMEWORK, false),
					new TDFramework(TDFramework.LIB_Z_LIBRARY, false)
				},
				false),
			new TDNetwork("Pangle",
				new TDSDKTargetVersion("9.0", null),
				"Pangle",
				null,
				null,
				new List<TDFramework>() {
					new TDFramework(TDFramework.ACCELERATE_FRAMEWORK, false),
					new TDFramework(TDFramework.ADSUPPORT_FRAMEWORK, false),
					new TDFramework(TDFramework.AV_FOUNDATION_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_LOCATION_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_MEDIA_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_MOTION_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_TELEPHONY_FRAMEWORK, false),
					new TDFramework(TDFramework.MAP_KIT_FRAMEWORK, false),
					new TDFramework(TDFramework.MEDIA_PLAYER_FRAMEWORK, false),
					new TDFramework(TDFramework.MOBILE_CORE_SERVICES_FRAMEWORK, false),
					new TDFramework(TDFramework.STORE_KIT_FRAMEWORK, false),
					new TDFramework(TDFramework.SYSTEM_CONFIGURATION_FRAMEWORK, false),
					new TDFramework(TDFramework.WEB_KIT_FRAMEWORK, false),
					new TDFramework(TDFramework.UI_KIT_FRAMEWORK, false),
					new TDFramework(TDFramework.LIB_BZ2_LIBRARY, false),
					new TDFramework(TDFramework.LIB_CPP_LIBRARY, false),
					new TDFramework(TDFramework.LIB_RESOLVE, false),
					new TDFramework(TDFramework.LIB_SQLITE3_LIBRARY, false),
					new TDFramework(TDFramework.LIB_XML2_LIBRARY, false),
					new TDFramework(TDFramework.LIB_Z_LIBRARY, false)
				},
				true),
			new TDNetwork("Tapjoy",
				new TDSDKTargetVersion("9.0", "14"),
				"Tapjoy",
				"com.tapdaq.sdk:TapdaqTapjoyAdapter:",
				"https://tapjoy.bintray.com/maven",
				new List<TDFramework>() {
					new TDFramework(TDFramework.CF_NETWORK_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_DATA_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_GRAPHICS_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_LOCATION_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_MOTION_FRAMEWORK, false),
					new TDFramework(TDFramework.FOUNDATION_FRAMEWORK, false),
					new TDFramework(TDFramework.IMAGE_IO_FRAMEWORK, false),
					new TDFramework(TDFramework.MAP_KIT_FRAMEWORK, false),
					new TDFramework(TDFramework.MEDIA_PLAYER_FRAMEWORK, false),
					new TDFramework(TDFramework.MOBILE_CORE_SERVICES_FRAMEWORK, false),
					new TDFramework(TDFramework.QUARTZ_CORE_FRAMEWORK, false),
					new TDFramework(TDFramework.SECURITY_FRAMEWORK, false),
					new TDFramework(TDFramework.SYSTEM_CONFIGURATION_FRAMEWORK, false),
					new TDFramework(TDFramework.UI_KIT_FRAMEWORK, false),
					new TDFramework(TDFramework.WEB_KIT_FRAMEWORK, false),
					new TDFramework(TDFramework.ADSUPPORT_FRAMEWORK, true),
					new TDFramework(TDFramework.CORE_TELEPHONY_FRAMEWORK, true),
					new TDFramework(TDFramework.STORE_KIT_FRAMEWORK, true),
					new TDFramework(TDFramework.LIB_CPP_LIBRARY, false),
					new TDFramework(TDFramework.LIB_SQLITE3_LIBRARY, false),
					new TDFramework(TDFramework.LIB_XML2_LIBRARY, false),
					new TDFramework(TDFramework.LIB_Z_LIBRARY, false)
				},
				true),
			
			new TDNetwork("UnityAds",
				new TDSDKTargetVersion("9.0", "14"),
				"UnityAds",
				"com.tapdaq.sdk:TapdaqUnityAdsAdapter:",
				null,
				new List<TDFramework>() { },
				false),
			new TDNetwork("Vungle",
				new TDSDKTargetVersion("9.0", "14"),
				"Vungle",
				"com.tapdaq.sdk:TapdaqVungleAdapter:",
				null,
				new List<TDFramework>() {
					new TDFramework(TDFramework.ADSUPPORT_FRAMEWORK, false),
					new TDFramework(TDFramework.AUDIO_TOOLBOX_FRAMEWORK, false),
					new TDFramework(TDFramework.AV_FOUNDATION_FRAMEWORK, false),
					new TDFramework(TDFramework.CF_NETWORK_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_GRAPHICS_FRAMEWORK, false),
					new TDFramework(TDFramework.CORE_MEDIA_FRAMEWORK, false),
					new TDFramework(TDFramework.MEDIA_PLAYER_FRAMEWORK, false),
					new TDFramework(TDFramework.QUARTZ_CORE_FRAMEWORK, false),
					new TDFramework(TDFramework.STORE_KIT_FRAMEWORK, false),
					new TDFramework(TDFramework.SYSTEM_CONFIGURATION_FRAMEWORK, false),
					new TDFramework(TDFramework.WEB_KIT_FRAMEWORK, true),
					new TDFramework(TDFramework.UI_KIT_FRAMEWORK, true),
					new TDFramework(TDFramework.FOUNDATION_FRAMEWORK, true),
					new TDFramework(TDFramework.LIB_Z_LIBRARY, false)
				},
				false),
			new TDNetwork("YouAppi",
				new TDSDKTargetVersion("10.0", "16"),
				"YouAppi",
				"com.tapdaq.sdk:TapdaqYouAppiAdapter:",
				"http://repository.youappi.com/repository/release",
				new List<TDFramework>() { },
				false),
		});
	}

    [Serializable]
    public class TDSDKTargetVersion
    {
        [SerializeField]
        public readonly string iOS;
        [SerializeField]
        public readonly string android;

        public TDSDKTargetVersion(string iosTarget, string androidTarget)
        {
            iOS = iosTarget;
            android = androidTarget;
        }
    }
}

