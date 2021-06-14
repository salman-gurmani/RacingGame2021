using System;

namespace Tapdaq
{
	public class TDFramework
	{
		public static readonly string ACCELERATE_FRAMEWORK = "Accelerate.framework";
		public static readonly string ADSUPPORT_FRAMEWORK = "AdSupport.framework";
		public static readonly string APP_TRACKING_TRANSPARENCY_FRAMEWORK = "AppTrackingTransparency.framework";
		public static readonly string AUDIO_TOOLBOX_FRAMEWORK = "AudioToolbox.framework";
		public static readonly string AV_FOUNDATION_FRAMEWORK = "AVFoundation.framework";
		public static readonly string CF_NETWORK_FRAMEWORK = "CFNetwork.framework";
		public static readonly string CORE_DATA_FRAMEWORK = "CoreData.framework";
		public static readonly string CORE_GRAPHICS_FRAMEWORK = "CoreGraphics.framework";
		public static readonly string CORE_IMAGE_FRAMEWORK = "CoreImage.framework";
		public static readonly string CORE_LOCATION_FRAMEWORK = "CoreLocation.framework";
		public static readonly string CORE_MEDIA_FRAMEWORK = "CoreMedia.framework";
		public static readonly string CORE_MOTION_FRAMEWORK = "CoreMotion.framework";
		public static readonly string CORE_TELEPHONY_FRAMEWORK = "CoreTelephony.framework";
		public static readonly string CORE_SERVICES_FRAMEWORK = "CoreServices.framework";
		public static readonly string CORE_VIDEO_FRAMEWORK = "CoreVideo.framework";
		public static readonly string FOUNDATION_FRAMEWORK = "Foundation.framework";
		public static readonly string IMAGE_IO_FRAMEWORK = "ImageIO.framework";
		public static readonly string JAVASCRIPT_CORE_FRAMEWORK = "JavaScriptCore.framework";
		public static readonly string LOCAL_AUTHENTICATION_FRAMEWORK = "LocalAuthentication.framework";
		public static readonly string MAP_KIT_FRAMEWORK = "MapKit.framework";
		public static readonly string MEDIA_PLAYER_FRAMEWORK = "MediaPlayer.framework";
		public static readonly string MESSAGE_UI_FRAMEWORK = "MessageUI.framework";
		public static readonly string MOBILE_CORE_SERVICES_FRAMEWORK = "MobileCoreServices.framework";
		public static readonly string QUARTZ_CORE_FRAMEWORK = "QuartzCore.framework";
		public static readonly string SAFARI_SERVICES_FRAMEWORK = "SafariServices.framework";
		public static readonly string SECURITY_FRAMEWORK = "Security.framework";
		public static readonly string SOCIAL_FRAMEWORK = "Social.framework";
		public static readonly string STORE_KIT_FRAMEWORK = "StoreKit.framework";
		public static readonly string SYSTEM_CONFIGURATION_FRAMEWORK = "SystemConfiguration.framework";
		public static readonly string WATCH_CONNECTIVITY_FRAMEWORK = "WatchConnectivity.framework";
		public static readonly string UI_KIT_FRAMEWORK = "UIKit.framework";
		public static readonly string VIDEO_TOOLBOX_FRAMEWORK = "VideoToolbox.framework";
		public static readonly string WEB_KIT_FRAMEWORK = "WebKit.framework";

		public static readonly string LIB_BZ2_LIBRARY = "libbz2.tbd";
		public static readonly string LIB_CPP_LIBRARY = "libc++.tbd";
		public static readonly string LIB_RESOLVE = "libresolv.tbd";
		public static readonly string LIB_SQLITE3_LIBRARY = "libsqlite3.tbd";
		public static readonly string LIB_XML2_LIBRARY = "libxml2.tbd";
		public static readonly string LIB_Z_LIBRARY = "libz.dylib";
		public static readonly string LIBZ125_LIBRARY = "libz.1.2.5.tbd";

		public readonly string Name;
		public readonly bool Weak;

		public TDFramework(string name, bool weak)
		{
			Name = name;
			Weak = weak;
		}
	}
}
