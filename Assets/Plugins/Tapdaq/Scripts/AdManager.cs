using System;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tapdaq {
	public class AdManager {
		
		private static AdManager reference;

		public static AdManager instance {
			get {
				if (AdManager.reference == null) {
					AdManager.reference = new AdManager ();
				}
				return AdManager.reference;
			}
		}

		internal AdManager () {}

		private const string unsupportedPlatformMessage = "We support iOS and Android platforms only.";
		private const string TAPDAQ_PLACEMENT_DEFAULT = "default";
#if UNITY_IPHONE
		
		//================================= Interstitials ==================================================
		[DllImport ("__Internal")]
		private static extern void _ConfigureTapdaq(string appIdChar, string clientKeyChar, string testDevicesChar, bool isDebugMode, bool autoReloadAds,
                                                    string pluginVersion, int isUserSubjectToGDPR, int isConsentGiven, int isAgeRestrictedUser, string userIdChar, bool forwardUserId);

        [DllImport ("__Internal")]
        private static extern void _SetDelegate();

		[DllImport ("__Internal")]
		private static extern bool _IsInitialised();

		[DllImport ("__Internal")]
		private static extern void _LaunchMediationDebugger();

		[DllImport ("__Internal")]
		private static extern void _SetUserSubjectToGDPR(int userSubjectToGDPR);

		[DllImport ("__Internal")]
		private static extern int _UserSubjectToGDPR();

		[DllImport ("__Internal")]
		private static extern void _SetGdprConsent(int gdprConsent);

		[DllImport ("__Internal")]
		private static extern int _GdprConsent();

		[DllImport ("__Internal")]
		private static extern void _SetAgeRestrictedUser(int ageRestrictedUser);
		
		[DllImport ("__Internal")]
		private static extern int _AgeRestrictedUser();

		[DllImport("__Internal")]
		private static extern void _SetUserSubjectToUSPrivacy(int userSubjectToUSPrivacy);

		[DllImport("__Internal")]
		private static extern int _UserSubjectToUSPrivacy();

		[DllImport("__Internal")]
		private static extern void _SetUSPrivacy(int USPrivacy);

		[DllImport("__Internal")]
		private static extern int _USPrivacy();

		[DllImport ("__Internal")]
        private static extern void _SetAdMobContentRating(string rating);
        
        [DllImport ("__Internal")]
        private static extern string _GetAdMobContentRating();

		[DllImport("__Internal")]
		private static extern void _SetAdvertiserTracking(int enabled);

		[DllImport("__Internal")]
		private static extern int _AdvertiserTracking();

		[DllImport ("__Internal")]
        private static extern void _SetUserId(string userId);
        
        [DllImport ("__Internal")]
        private static extern string _GetUserId();

        [DllImport ("__Internal")]
        private static extern void _SetForwardUserId(bool forwardUserId);

        [DllImport ("__Internal")]
        private static extern bool _ShouldForwardUserId();

        [DllImport ("__Internal")]
        private static extern void _SetMuted(bool muted);

        [DllImport ("__Internal")]
        private static extern bool _IsMuted();

        [DllImport ("__Internal")]
        private static extern void _SetUserDataString(string key, string value);

        [DllImport ("__Internal")]
        private static extern void _SetUserDataInteger(string key, int value);
        
        [DllImport ("__Internal")]
        private static extern void _SetUserDataBoolean(string key, bool value);

        [DllImport ("__Internal")]
        private static extern string _GetUserDataString(string key);
        
        [DllImport ("__Internal")]
        private static extern int _GetUserDataInteger(string key);
        
        [DllImport ("__Internal")]
        private static extern bool _GetUserDataBoolean(string key);

        [DllImport ("__Internal")]
        private static extern void _RemoveUserData(string key);

        [DllImport ("__Internal")]
        private static extern string _GetAllUserData();

        [DllImport ("__Internal")]
		private static extern string _GetNetworkStatuses();

		// interstitial
		[DllImport ("__Internal")]
		private static extern void _ShowInterstitialWithTag(string tag);

		[DllImport ("__Internal")]
		private static extern void _LoadInterstitialWithTag(string tag);

		[DllImport ("__Internal")]
		private static extern bool _IsInterstitialReadyWithTag(string tag);

		[DllImport("__Internal")]
		private static extern string _GetInterstitialFrequencyCapError(string tag);

		// banner
		[DllImport ("__Internal")]
		private static extern void _LoadBannerForSize(string tag, string sizeString);

		[DllImport("__Internal")]
		private static extern void _LoadBannerWithSize(string tag, int width, int height);

		[DllImport ("__Internal")]
        private static extern void _ShowBanner(string tag, string position);

        [DllImport ("__Internal")]
        private static extern void _ShowBannerWithPosition(string tag, int x, int y);

        [DllImport("__Internal")]
        private static extern void _HideBanner(string tag);

		[DllImport("__Internal")]
		private static extern void _DestroyBanner(string tag);

		[DllImport("__Internal")]
		private static extern bool _IsBannerReady(string tag);

		// video
		[DllImport ("__Internal")]
		private static extern void _ShowVideoWithTag (string tag);

		[DllImport("__Internal")]
		private static extern void _LoadVideoWithTag(string tag);

		[DllImport("__Internal")]
		private static extern bool _IsVideoReadyWithTag(string tag);

		[DllImport("__Internal")]
		private static extern string _GetVideoFrequencyCapError(string tag);

		// reward video
		[DllImport ("__Internal")]
		private static extern void _ShowRewardedVideoWithTag (string tag, string hashedUserId);

		[DllImport ("__Internal")]
		private static extern void _LoadRewardedVideoWithTag(string tag);

		[DllImport ("__Internal")]
		private static extern bool _IsRewardedVideoReadyWithTag(string tag);

		[DllImport("__Internal")]
		private static extern string _GetRewardedVideoFrequencyCapError(string tag);

		/////////// Stats
		[DllImport ("__Internal")]
		private static extern void _SendIAP(string transactionId, string productId, string name, double price, string currency, string locale);

		/////////// Rewards
		[DllImport ("__Internal")]
		private static extern System.IntPtr _GetRewardId(string tag);

#endif

		#region Class Variables

		private TDSettings settings;

		#endregion


		public static void Init()
		{
			instance._Init(GetUserSubjectToGdprStatus(), GetConsentStatus(), GetAgeRestrictedUserStatus(), GetUserId(), ShouldForwardUserId());
		}

		// Obsolete as of 15/04/2020. Plugin Version 7.6.0
		[Obsolete("Please use 'Init' and set values separately")]
		public static void Init (TDStatus isUserSubjectToGDPR = TDStatus.UNKNOWN, TDStatus isConsentGiven = TDStatus.UNKNOWN, TDStatus isAgeRestrictedUser = TDStatus.UNKNOWN, string userId = null, bool shouldForwardUserId = false) {
            instance._Init (isUserSubjectToGDPR, isConsentGiven, isAgeRestrictedUser, userId, shouldForwardUserId);
		}

		// Obsolete as of 02/07/2019. Plugin Version 7.2.0
		[Obsolete ("Please use 'Init' and set values separately")]
        public static void InitWithConsent(TDStatus isUserSubjectToGDPR, TDStatus isConsentGiven, TDStatus isAgeRestrictedUser)
        {
            instance._Init(isUserSubjectToGDPR, isConsentGiven, isAgeRestrictedUser, null, false);
        }

		// Obsolete as of 13/06/2018. Plugin Version 6.2.4
        [Obsolete ("Please, use 'InitWithConsent (TDStatus isUserSubjectToGDPR, TDStatus isConsentGiven)' method.")]
		public static void InitWithConsent (bool isConsentGiven) {
            instance._Init ((isConsentGiven ? TDStatus.TRUE : TDStatus.FALSE), (isConsentGiven ? TDStatus.TRUE : TDStatus.FALSE), TDStatus.UNKNOWN, null, false);
		}

        // Obsolete as of 24/09/2018. Plugin Version 6.4.0
        [Obsolete("Please, use 'InitWithConsent (TDStatus isUserSubjectToGDPR, TDStatus isConsentGiven, TDStatus isAgeRestrictedUser)' method.")]
		public static void InitWithConsent (TDStatus isConsentGiven) {
            instance._Init (isConsentGiven, isConsentGiven, TDStatus.UNKNOWN, null, false);
		}

		// Obsolete as of 13/06/2018. Plugin Version 6.2.4
        [Obsolete ("Please, use 'InitWithConsent (TDStatus isUserSubjectToGDPR, TDStatus isConsentGiven, TDStatus isAgeRestrictedUser)' method.")]
		public static void InitWithConsent (bool isConsentGiven, bool isAgeRestrictedUser) {
            instance._Init ((isConsentGiven ? TDStatus.TRUE : TDStatus.FALSE), (isConsentGiven ? TDStatus.TRUE : TDStatus.FALSE), (isAgeRestrictedUser ? TDStatus.TRUE : TDStatus.FALSE), null, false);
		}

        private void _Init (TDStatus isUserSubjectToGDPR, TDStatus isConsentGiven, TDStatus isAgeRestrictedUser, string userId, bool shouldForwardUserId) {
			if (!settings) {
				settings = TDSettings.getInstance();
			}

			var applicationId = "";
			var clientKey = "";

			#if UNITY_IPHONE
			applicationId = settings.ios_applicationID;
			clientKey = settings.ios_clientKey;
			#elif UNITY_ANDROID
			applicationId = settings.android_applicationID;
			clientKey = settings.android_clientKey;
			#endif

			LogMessage(TDLogSeverity.debug, "TapdaqSDK/Application ID -- " + applicationId);
			LogMessage(TDLogSeverity.debug, "TapdaqSDK/Client Key -- " + clientKey);

            Initialize (applicationId, clientKey, isUserSubjectToGDPR, isConsentGiven, isAgeRestrictedUser, userId, shouldForwardUserId);
		}

        private void Initialize (string appID, string clientKey, TDStatus isUserSubjectToGDPR, TDStatus isConsentGiven, TDStatus isAgeRestrictedUser, string userId, bool shouldForwardUserId) {
			LogUnsupportedPlatform ();

			LogMessage (TDLogSeverity.debug, "TapdaqSDK/Initializing");

			#if UNITY_IPHONE
			var testDevices = new TestDevicesList (settings.testDevices, TestDeviceType.iOS).ToString ();
			TDDebugLogger.Log ("testDevices:\n" + testDevices);
			CallIosMethod(() => _ConfigureTapdaq(appID, clientKey, testDevices, 
                                                 settings.isDebugMode, settings.autoReloadAds, TDSettings.pluginVersion, (int)isUserSubjectToGDPR, (int)isConsentGiven, (int)isAgeRestrictedUser, userId, shouldForwardUserId));
			#elif UNITY_ANDROID
			var testDevices = new TestDevicesList (settings.testDevices, TestDeviceType.Android).ToString ();
			TDDebugLogger.Log ("testDevices:\n" + testDevices);
			CallAndroidStaticMethod("InitiateTapdaq", appID, clientKey, testDevices,
                                    settings.isDebugMode, settings.autoReloadAds, TDSettings.pluginVersion, (int)isUserSubjectToGDPR, (int)isConsentGiven, (int)isAgeRestrictedUser, userId, shouldForwardUserId);
			#endif
		}

        #region Platform specific method calling

#if UNITY_IPHONE

		private static void CallIosMethod(Action action) {
            TDEventHandler.instance.Init();

			LogUnsupportedPlatform ();
			if(Application.platform == RuntimePlatform.IPhonePlayer) {
				if(AdManager.instance != null && action != null) {
                    _SetDelegate();
					action.Invoke();
				}
			}
		}

#elif UNITY_ANDROID

        private static T GetAndroidStatic<T>(string methodName, params object[] paramList) {
			LogUnsupportedPlatform();
			if(Application.platform == RuntimePlatform.Android) {
				try {
					using (AndroidJavaClass tapdaqUnity = new AndroidJavaClass("com.tapdaq.unityplugin.TapdaqUnity")) {
						return tapdaqUnity.CallStatic<T> (methodName, paramList);
					}
				} catch (Exception e) {
					TDDebugLogger.LogException (e);
				}
			}
			TDDebugLogger.LogError ("Error while call static method");
			return default(T);
		}
			
		private static void CallAndroidStaticMethod(string methodName, params object[] paramList) {
			CallAndroidStaticMethodFromClass ( "com.tapdaq.unityplugin.TapdaqUnity", methodName, true, paramList);
		}

		private static void CallAndroidStaticMethodFromClass(string className, 
			string methodName, bool logException, params object[] paramList) {
            TDEventHandler.instance.Init();

			LogUnsupportedPlatform();
			if(Application.platform == RuntimePlatform.Android) {
				try {
					using (AndroidJavaClass androidClass = new AndroidJavaClass(className)) {
						androidClass.CallStatic (methodName, paramList);
					}
				} catch (Exception e) {
					if (logException) {
						TDDebugLogger.Log ("CallAndroidStaticMethod:  " + methodName + "    FromClass: " 
							+ className + " failed. Message: " + e.Message);
					}
				}
			}
		}

		#endif
		#endregion

		private static void LogObsoleteWithTagMethod(string methodName) {
			TDDebugLogger.LogError("'" + methodName + "WithTag(string tag)' is Obsolete. Please, use '" + methodName +"(string tag)' instead");
		}

		private static void LogUnsupportedPlatform() {
			if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor) {
				LogMessage (TDLogSeverity.warning, unsupportedPlatformMessage);
			}
		}

		public void _UnexpectedErrorHandler (string msg) {
			TDDebugLogger.Log (":: Ad test ::" + msg);
			LogMessage (TDLogSeverity.error, msg);
		}

		public static void LogMessage (TDLogSeverity severity, string message) {
			string prefix = "Tapdaq Unity SDK: ";
			if (severity == TDLogSeverity.warning) {
				TDDebugLogger.LogWarning (prefix + message);
			} else if (severity == TDLogSeverity.error) {
				TDDebugLogger.LogError (prefix + message);
			} else {
				TDDebugLogger.Log (prefix + message);
			}
		}

		public void FetchFailed (string msg) {
			TDDebugLogger.Log (msg);
			LogMessage (TDLogSeverity.debug, "unable to fetch more ads");
		}

		public static void OnApplicationPause(bool isPaused) {
			#if UNITY_IPHONE
			#elif UNITY_ANDROID
			if (isPaused) {
				CallAndroidStaticMethod("OnPause");
			} else {
				CallAndroidStaticMethod("OnResume");
			}
			#endif

		}

		public static bool IsInitialised() {
			bool ready = false;
			#if UNITY_IPHONE
			CallIosMethod(() => ready = _IsInitialised());
			#elif UNITY_ANDROID
			ready = GetAndroidStatic<bool>("IsInitialised");
			#endif
			return ready;
		}

		public static void LaunchMediationDebugger () {
			#if UNITY_IPHONE
			_LaunchMediationDebugger ();
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("ShowMediationDebugger");
			#endif
		}

		// Obsolete as of 15/04/2020. Plugin Version 7.6.0
		[Obsolete("Please, use 'SetUserSubjectToGdprStatus(TDStatus isUserSubjectToGDPR)' method.")]
		public static void SetUserSubjectToGDPR (TDStatus isUserSubjectToGDPR) {
			SetUserSubjectToGdprStatus(isUserSubjectToGDPR);
		}

		public static void SetUserSubjectToGdprStatus(TDStatus status)
		{
            #if UNITY_IPHONE
			CallIosMethod(() => _SetUserSubjectToGDPR((int)status));
            #elif UNITY_ANDROID
			CallAndroidStaticMethod("SetUserSubjectToGDPR", (int)status);
            #endif
		}

		// Obsolete as of 15/04/2020. Plugin Version 7.6.0
		[Obsolete("Please, use 'GetUserSubjectToGdprStatus()' method.")]
		public static TDStatus IsUserSubjectToGDPR() {
			return GetUserSubjectToGdprStatus();
		}

		public static TDStatus GetUserSubjectToGdprStatus()
		{
			int result = 2;
            #if UNITY_IPHONE
			CallIosMethod(() => result = _UserSubjectToGDPR());
            #elif UNITY_ANDROID
			result = GetAndroidStatic<int>("GetUserSubjectToGdprStatus");
            #endif
			return (TDStatus)result;
		}

		// Obsolete as of 15/04/2020. Plugin Version 7.6.0
		[Obsolete("Please, use 'SetConsentStatus(TDStatus isConsentGiven)' method.")]
		public static void SetConsentGiven (bool isConsentGiven) {
			SetConsentStatus(isConsentGiven ? TDStatus.TRUE : TDStatus.FALSE);
		}

		public static void SetConsentStatus(TDStatus status)
		{
            #if UNITY_IPHONE
			CallIosMethod(() => _SetGdprConsent((int)status));
#elif UNITY_ANDROID
			CallAndroidStaticMethod("SetConsentGiven", (int)status);
#endif
		}

		// Obsolete as of 15/04/2020. Plugin Version 7.6.0
		[Obsolete("Please, use 'GetConsentStatus()' method.")]
		public static bool IsConsentGiven() {
			return GetConsentStatus() == TDStatus.TRUE;
		}

		public static TDStatus GetConsentStatus()
		{
			int result = (int)TDStatus.UNKNOWN;
            #if UNITY_IPHONE
			CallIosMethod(() => result = _GdprConsent());
            #elif UNITY_ANDROID
			result = GetAndroidStatic<int>("GetConsentStatus");
            #endif
			return (TDStatus)result;
		}

		// Obsolete as of 15/04/2020. Plugin Version 7.6.0
		[Obsolete("Please, use 'SetAgeRestrictedUserStatus(TDStatus isAgeRestrictedUser)' method.")]
		public static void SetIsAgeRestrictedUser (bool isAgeRestrictedUser) {
			SetAgeRestrictedUserStatus(isAgeRestrictedUser ? TDStatus.TRUE : TDStatus.FALSE);
		}

		public static void SetAgeRestrictedUserStatus(TDStatus status)
		{
            #if UNITY_IPHONE
			CallIosMethod(() => _SetAgeRestrictedUser((int)status));
#elif UNITY_ANDROID
			CallAndroidStaticMethod("SetAgeRestrictedUser", (int)status);
#endif
		}

		// Obsolete as of 15/04/2020. Plugin Version 7.6.0
		[Obsolete("Please, use 'GetAgeRestrictedUserStatus()' method.")]
		public static bool IsAgeRestrictedUser() {
			return GetAgeRestrictedUserStatus() == TDStatus.TRUE;
		}

		public static TDStatus GetAgeRestrictedUserStatus()
		{
			int result = (int)TDStatus.UNKNOWN;
            #if UNITY_IPHONE
			CallIosMethod(() => result = _AgeRestrictedUser());
            #elif UNITY_ANDROID
			result = GetAndroidStatic<int>("GetAgeRestrictedUserStatus");
            #endif
			return (TDStatus)result;
		}

		public static void SetUserSubjectToUSPrivacyStatus(TDStatus status)
		{
            #if UNITY_IPHONE
			CallIosMethod(() => _SetUserSubjectToUSPrivacy((int)status));
            #elif UNITY_ANDROID
			CallAndroidStaticMethod("SetUserSubjectToUSPrivacyStatus", (int)status);
            #endif
		}

		public static TDStatus GetUserSubjectToUSPrivacyStatus()
		{
			int result = (int)TDStatus.UNKNOWN;
            #if UNITY_IPHONE
			CallIosMethod(() => result = _UserSubjectToUSPrivacy());
            #elif UNITY_ANDROID
			result = GetAndroidStatic<int>("GetUserSubjectToUSPrivacyStatus");
            #endif
			return (TDStatus)result;
		}

		public static void SetUSPrivacyStatus(TDStatus status)
		{
            #if UNITY_IPHONE
			CallIosMethod(() => _SetUSPrivacy((int)status));
            #elif UNITY_ANDROID
			CallAndroidStaticMethod("SetUSPrivacyStatus", (int)status);
            #endif
		}

		public static TDStatus GetUSPrivacyStatus()
		{
			int result = (int)TDStatus.UNKNOWN;
            #if UNITY_IPHONE
			CallIosMethod(() => result = _USPrivacy());
            #elif UNITY_ANDROID
			result = GetAndroidStatic<int>("GetUSPrivacyStatus");
            #endif
			return (TDStatus)result;
		}


		public static void SetAdMobContentRating(String rating) {
            #if UNITY_IPHONE
            CallIosMethod(() => _SetAdMobContentRating(rating));
            #elif UNITY_ANDROID
            CallAndroidStaticMethod("SetAdMobContentRating", rating);
            #endif
        }

        public static string GetAdMobContentRating()
        {
            string result = null;
            #if UNITY_IPHONE
            CallIosMethod(() => result = _GetAdMobContentRating());
            #elif UNITY_ANDROID
            result = GetAndroidStatic<String>("GetAdMobContentRating");
            #endif
            return result;
        }

		public static void SetAdvertiserTrackingEnabled(TDStatus status)
		{
			#if UNITY_IPHONE
			CallIosMethod(() => _SetAdvertiserTracking((int)status));
			#endif
		}

		public static TDStatus GetAdvertiserTrackingEnabled()
		{
			int result = (int)TDStatus.UNKNOWN;
			#if UNITY_IPHONE
			CallIosMethod(() => result = _AdvertiserTracking());
			#endif
			return (TDStatus)result;
		}

		public static void SetUserId(String userId)
        {
            #if UNITY_IPHONE
            CallIosMethod(() => _SetUserId(userId));
            #elif UNITY_ANDROID
            CallAndroidStaticMethod("SetUserId", userId);
            #endif
        }

        public static string GetUserId()
        {
            string result = null;
            #if UNITY_IPHONE
            CallIosMethod(() => result = _GetUserId());
            #elif UNITY_ANDROID
            result = GetAndroidStatic<String>("GetUserId");
            #endif
            return result;
        }

        public static void SetForwardUserId(bool forwardUserId)
        {
            #if UNITY_IPHONE
            CallIosMethod(() => _SetForwardUserId(forwardUserId));
            #elif UNITY_ANDROID
            CallAndroidStaticMethod("SetForwardUserId", forwardUserId);
            #endif
        }

        public static bool ShouldForwardUserId()
        {
            bool result = false;
            #if UNITY_IPHONE
            CallIosMethod(() => result = _ShouldForwardUserId());
            #elif UNITY_ANDROID
            result = GetAndroidStatic<bool>("ShouldForwardUserId");
            #endif
            return result;
        }

		public static void SetMuted(bool muted)
		{
            #if UNITY_IPHONE
            CallIosMethod(() => _SetMuted(muted));
            #elif UNITY_ANDROID
			CallAndroidStaticMethod("SetMuted", muted);
            #endif
		}

		public static bool IsMuted()
		{
			bool result = false;
            #if UNITY_IPHONE
            CallIosMethod(() => result = _IsMuted());
            #elif UNITY_ANDROID
			result = GetAndroidStatic<bool>("GetMuted");
            #endif
			return result;
		}

		public static void SetUserData(string key, string value)
		{
            #if UNITY_IPHONE
            CallIosMethod(() => _SetUserDataString(key, value));
            #elif UNITY_ANDROID
			CallAndroidStaticMethod("SetUserDataString", key, value);
            #endif
		}

		public static void SetUserData(string key, int value)
		{
            #if UNITY_IPHONE
            CallIosMethod(() => _SetUserDataInteger(key, value));
            #elif UNITY_ANDROID
            CallAndroidStaticMethod("SetUserDataInteger", key, value);
            #endif
		}

		public static void SetUserData(string key, bool value)
		{
            #if UNITY_IPHONE
            CallIosMethod(() => _SetUserDataBoolean(key, value));
            #elif UNITY_ANDROID
            CallAndroidStaticMethod("SetUserDataBoolean", key, value);
            #endif
		}

		public static string GetUserDataString(string key)
		{
			string result = "";
            #if UNITY_IPHONE
            CallIosMethod(() => result = _GetUserDataString(key));
            #elif UNITY_ANDROID
            result = GetAndroidStatic<string>("GetUserDataString", key);
            #endif
			return result;
		}

		public static int GetUserDataInteger(string key)
		{
			int result = 0;
            #if UNITY_IPHONE
            CallIosMethod(() => result = _GetUserDataInteger(key));
            #elif UNITY_ANDROID
            result = GetAndroidStatic<int>("GetUserDataInteger", key);
            #endif
			return result;
		}

		public static bool GetUserDataBoolean(string key)
		{
			bool result = false;
            #if UNITY_IPHONE
            CallIosMethod(() => result = _GetUserDataBoolean(key));
            #elif UNITY_ANDROID
			result = GetAndroidStatic<bool>("GetUserDataBoolean", key);
            #endif
			return result;
		}

		public static Dictionary<string, object> GetAllUserData()
		{
			string result = "";
            #if UNITY_IPHONE
			CallIosMethod(() => result = _GetAllUserData());
            #elif UNITY_ANDROID
			result = GetAndroidStatic<string>("GetAllUserData");
            #endif
			return JsonConvert.DeserializeObject<Dictionary<string, object>>(result); ;
		}

		public static void RemoveUserData(string key)
		{
            #if UNITY_IPHONE
            CallIosMethod(() => _RemoveUserData(key));
            #elif UNITY_ANDROID
			CallAndroidStaticMethod("RemoveUserData", key);
            #endif
		}

		// interstitial
		public static void LoadInterstitial(string tag = TAPDAQ_PLACEMENT_DEFAULT)
        {
            #if UNITY_IPHONE
            CallIosMethod(() => _LoadInterstitialWithTag(tag));
            #elif UNITY_ANDROID
            CallAndroidStaticMethod("LoadInterstitial", tag);
            #endif
        }

        [Obsolete("Please, use 'LoadInterstitial(string tag)' method.")]
        public static void LoadInterstitialWithTag(string tag)
        {
            LogObsoleteWithTagMethod("LoadInterstitial");
            LoadInterstitial(tag);
        }

        public static void ShowInterstitial (string tag = TAPDAQ_PLACEMENT_DEFAULT) {
			#if UNITY_IPHONE
			CallIosMethod(() => _ShowInterstitialWithTag(tag));
			#elif UNITY_ANDROID
            CallAndroidStaticMethod("ShowInterstitial", tag);
			#endif
		}

        public static bool IsInterstitialReady(string tag = TAPDAQ_PLACEMENT_DEFAULT) {
			bool ready = false;
			#if UNITY_IPHONE
			CallIosMethod(() => ready = _IsInterstitialReadyWithTag(tag));
			#elif UNITY_ANDROID
			ready = GetAndroidStatic<bool>("IsInterstitialReady", tag);
			#endif
			return ready;
		}

		[Obsolete ("Please, use 'IsInterstitialReady(string tag)' method.")]
		public static bool IsInterstitialReadyWithTag(string tag) {
			LogObsoleteWithTagMethod("IsInterstitialReady");
			return IsInterstitialReady(tag);
		}

        public static TDAdError GetInterstitialFrequencyCapError(string tag = TAPDAQ_PLACEMENT_DEFAULT)
        {
			TDAdError error = null;
			string result = "";
            #if UNITY_IPHONE
			CallIosMethod(() => result = _GetInterstitialFrequencyCapError(tag));
            #elif UNITY_ANDROID
            result = GetAndroidStatic<string>("GetInterstitialFrequencyCapError", tag);
            #endif
			if (!String.IsNullOrEmpty(result))
            {
				error = JsonConvert.DeserializeObject<TDAdError>(result);
			}
			
			return error;
        }
			
		// banner

		public static bool IsBannerReady(string tag = TAPDAQ_PLACEMENT_DEFAULT) {
			bool ready = false;
			#if UNITY_IPHONE
			CallIosMethod(() => ready = _IsBannerReady(tag));
			#elif UNITY_ANDROID
			ready = GetAndroidStatic<bool>("IsBannerReady", tag);
			#endif
			return ready;
		}

		public static void RequestBanner (TDMBannerSize size, string tag = TAPDAQ_PLACEMENT_DEFAULT) {
			#if UNITY_IPHONE
			CallIosMethod(() => _LoadBannerForSize(tag, size.ToString()));
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("LoadBanner", tag, size.ToString());
			#endif
		}

		public static void RequestBanner(int width, int height, string tag = TAPDAQ_PLACEMENT_DEFAULT)
		{
            #if UNITY_IPHONE
			CallIosMethod(() => _LoadBannerWithSize(tag, width, height));
            #elif UNITY_ANDROID
			CallAndroidStaticMethod("LoadBanner", tag, width, height);
            #endif
		}

		public static void ShowBanner (TDBannerPosition position, string tag = TAPDAQ_PLACEMENT_DEFAULT) {
			#if UNITY_IPHONE
			CallIosMethod(() => _ShowBanner(tag, position.ToString()));
			#elif UNITY_ANDROID
			CallAndroidStaticMethod("ShowBanner", tag, position.ToString());
			#endif
		}

		public static void ShowBanner(int x, int y, string tag = TAPDAQ_PLACEMENT_DEFAULT)
		{
            #if UNITY_IPHONE
			CallIosMethod(() => _ShowBannerWithPosition(tag, x, y));
            #elif UNITY_ANDROID
			CallAndroidStaticMethod("ShowBanner", tag, x, y);
            #endif
		}

		public static void HideBanner(string tag = TAPDAQ_PLACEMENT_DEFAULT)
	    {
            #if UNITY_IPHONE
			CallIosMethod(() => _HideBanner(tag));
            #elif UNITY_ANDROID
			CallAndroidStaticMethod("HideBanner", tag);
			#endif
	    }

		public static void DestroyBanner(string tag = TAPDAQ_PLACEMENT_DEFAULT)
		{
            #if UNITY_IPHONE
			CallIosMethod(() => _DestroyBanner(tag));
            #elif UNITY_ANDROID
			CallAndroidStaticMethod("DestroyBanner", tag);
            #endif
		}


		// video
		public static void LoadVideo(string tag = TAPDAQ_PLACEMENT_DEFAULT)
        {
            #if UNITY_IPHONE
            CallIosMethod(() => _LoadVideoWithTag (tag));
            #elif UNITY_ANDROID
            CallAndroidStaticMethod("LoadVideo", tag);
            #endif
        }

        [Obsolete("Please, use 'LoadVideo(string tag)' method.")]
        public static void LoadVideoWithTag(string tag)
        {
            LogObsoleteWithTagMethod("LoadVideo");
            LoadVideo(tag);
        }

        public static void ShowVideo (string tag = TAPDAQ_PLACEMENT_DEFAULT) {
			#if UNITY_IPHONE
			CallIosMethod(() => _ShowVideoWithTag (tag));
			#elif UNITY_ANDROID
            CallAndroidStaticMethod("ShowVideo", tag);
			#endif
		}

        public static bool IsVideoReady(string tag = TAPDAQ_PLACEMENT_DEFAULT) {
			bool ready = false;
			#if UNITY_IPHONE
			CallIosMethod(() => ready = _IsVideoReadyWithTag(tag));
			#elif UNITY_ANDROID
			ready = GetAndroidStatic<bool>("IsVideoReady", tag);
			#endif
			return ready;
		}

		[Obsolete ("Please, use 'IsVideoReady(string tag)' method.")]
		public static bool IsVideoReadyWithTag(string tag) {
			LogObsoleteWithTagMethod("IsVideoReady");
			return IsVideoReady(tag);
		}

		public static TDAdError GetVideoFrequencyCapError(string tag = TAPDAQ_PLACEMENT_DEFAULT)
		{
			TDAdError error = null;
			string result = "";
            #if UNITY_IPHONE
			CallIosMethod(() => result = _GetVideoFrequencyCapError(tag));
            #elif UNITY_ANDROID
            result = GetAndroidStatic<string>("GetVideoFrequencyCapError", tag);
            #endif
			if (!String.IsNullOrEmpty(result))
			{
				error = JsonConvert.DeserializeObject<TDAdError>(result);
			}

			return error;
		}

		// rewarded video
		public static void LoadRewardedVideo(string tag = TAPDAQ_PLACEMENT_DEFAULT)
        {
            #if UNITY_IPHONE
            CallIosMethod(() => _LoadRewardedVideoWithTag (tag));
            #elif UNITY_ANDROID
            CallAndroidStaticMethod("LoadRewardedVideo", tag);
            #endif
        }

        [Obsolete("Please, use 'LoadRewardedVideo(string tag)' method.")]
        public static void LoadRewardedVideoWithTag(string tag)
        {
            LogObsoleteWithTagMethod("LoadRewardedVideo");
            LoadRewardedVideo(tag);
        }

        [Obsolete("UserId should now be set on Init or using SetUserId")]
        public static void ShowRewardVideo (string tag, string hashedUserId) {
			#if UNITY_IPHONE
            CallIosMethod(() => _ShowRewardedVideoWithTag (tag, hashedUserId));
			#elif UNITY_ANDROID
            CallAndroidStaticMethod("ShowRewardedVideo", tag, hashedUserId);
			#endif
		}

        public static void ShowRewardVideo(string tag = TAPDAQ_PLACEMENT_DEFAULT)
        {
            ShowRewardVideo(tag, null);
        }

        public static bool IsRewardedVideoReady(string tag = TAPDAQ_PLACEMENT_DEFAULT) {
			bool ready = false;
			#if UNITY_IPHONE
			CallIosMethod(() => ready = _IsRewardedVideoReadyWithTag(tag));
			#elif UNITY_ANDROID
            ready = GetAndroidStatic<bool>("IsRewardedVideoReady", tag);
			#endif
			return ready;
		}

        [Obsolete("Please, use 'IsRewardedVideoReady(string tag)' method.")]
        public static bool IsRewardedVideoReadyWithTag(string tag)
        {
            LogObsoleteWithTagMethod("IsRewardedVideoReady");
            return IsRewardedVideoReady(tag);
        }

		public static TDAdError GetRewardedVideoFrequencyCapError(string tag = TAPDAQ_PLACEMENT_DEFAULT)
		{
			TDAdError error = null;
			string result = "";
            #if UNITY_IPHONE
			CallIosMethod(() => result = _GetRewardedVideoFrequencyCapError(tag));
            #elif UNITY_ANDROID
            result = GetAndroidStatic<string>("GetRewardedVideoFrequencyCapError", tag);
            #endif
			if (!String.IsNullOrEmpty(result))
			{
				error = JsonConvert.DeserializeObject<TDAdError>(result);
			}

			return error;
		}

		// Obsolete as of 31/05/2018. Plugin Version 6.2.3
		[Obsolete ("For Android use 'SendIAP_Android(String in_app_purchase_data, String in_app_purchase_signature, String name, double price, String currency, String locale)' \n" +
		           "For iOS use 'SendIAP_iOS(String transactionId, String productId, String name, double price, String currency, String locale)' methods.")]
		public static void SendIAP (String name, double price, String locale) {
			#if UNITY_IPHONE
			SendIAP_iOS(null, null, name, price, null, locale);
			#elif UNITY_ANDROID
			SendIAP_Android(null, null, name, price, null, locale);
			#endif
		}

		// iOS
		public static void SendIAP_iOS (String transactionId, String productId, String name, double price, String currency, String locale) {
			#if UNITY_IPHONE
			CallIosMethod(() => _SendIAP(transactionId, productId, name, price, currency, locale));
			#endif
		}

		// Android
		public static void SendIAP_Android (String in_app_purchase_data, String in_app_purchase_signature, String name, double price, String currency, String locale) {
			#if  UNITY_ANDROID
			CallAndroidStaticMethod("SendIAP", in_app_purchase_data, in_app_purchase_signature, name, price, currency, locale);
			#endif
		}

		public static String GetRewardId (String tag) {
			#if UNITY_IPHONE
			return Marshal.PtrToStringAnsi(_GetRewardId(tag));
			#elif UNITY_ANDROID
			return GetAndroidStatic<string>("GetRewardId", tag);
            #endif

            return null;
		}

		public static List<TDNetworkStatus> GetNetworkStatuses()
		{
			List<TDNetworkStatus> networkStatuses = new List<TDNetworkStatus>();
			string result = "";
#if UNITY_IPHONE
			CallIosMethod(() => result = _GetNetworkStatuses());
#elif UNITY_ANDROID
			result = GetAndroidStatic<string>("GetNetworkStatuses");
#endif
            if(!String.IsNullOrEmpty(result))
			{
				TDDebugLogger.Log(result);
				networkStatuses = JsonConvert.DeserializeObject<List<TDNetworkStatus>>(result);
			}
			return networkStatuses;
		}
	}
}