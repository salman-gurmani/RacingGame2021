using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using Tapdaq;

namespace TDEditor {
	[CustomEditor (typeof(TDSettings))]
    public class TDSettingsEditor : UnityEditor.Editor
    {
		private TDSettings settings;

		private bool showOther = false;

		private bool showTestDevices = false;
		private bool showSKAdNetworkIds = false;
		private string skAdNetworkIdKey = "";
		private string skAdNetworkIdValue = "";

		private GUIStyle foldoutStyle;

		private List<TestDevice> deviceToDelete = new List<TestDevice>();

		private Texture whiteTexture;

		void OnEnable () {
			whiteTexture = Texture2D.whiteTexture;
			settings = (TDSettings)target;
			TDManifestResolver.FixAndroidManifest ();
		}

        private void OnDisable()
        {
            TDManifestResolver.FixAndroidManifest();
        }

        public override void OnInspectorGUI () {
			foldoutStyle = EditorStyles.foldout;
			foldoutStyle.fontStyle = FontStyle.Bold;

			serializedObject.Update ();

			GUILayout.Label ("You must have an App ID and Client Key to use Tapdaq", EditorStyles.boldLabel);

			if (GUILayout.Button ("Visit Tapdaq.com")) {
				Application.OpenURL ("https://tapdaq.com/dashboard/apps");
			}

			GUILayout.Label ("Current version:", EditorStyles.boldLabel);
			GUILayout.Label ("Unity plugin: \t" + TDSettings.pluginVersion);

			GUILayout.Space (15);

			DrawSeparator(2);

			GUILayout.Space (15);

			GUILayout.Label ("App Settings", EditorStyles.boldLabel);

			settings.ios_applicationID = EditorGUILayout.TextField ("iOS Application ID", settings.ios_applicationID);
			settings.ios_clientKey = EditorGUILayout.TextField ("iOS Client Key", settings.ios_clientKey);
            settings.admob_appid_ios = EditorGUILayout.TextField("iOS AdMob AppId", settings.admob_appid_ios);

			GUILayout.Space(20);

			settings.android_applicationID = EditorGUILayout.TextField ("Android Application ID", settings.android_applicationID);
			settings.android_clientKey = EditorGUILayout.TextField ("Android Client Key", settings.android_clientKey);
            settings.admob_appid_android = EditorGUILayout.TextField("Android AdMob AppId", settings.admob_appid_android);

            GUILayout.Space (15);

			settings.autoReloadAds = EditorGUILayout.Toggle("Auto-reload Ads", settings.autoReloadAds);

			GUILayout.Space (15);

			settings.isDebugMode = EditorGUILayout.Toggle("Debug Mode?", settings.isDebugMode);

			GUILayout.Space (15);

			// SKAdNetworkIds

			//GUILayout.Label("SKAdNetworkIds", EditorStyles.boldLabel, GUILayout.MaxWidth(100));
			showSKAdNetworkIds = EditorGUILayout.Foldout(showSKAdNetworkIds, "SKAdNetworkIds", foldoutStyle);
			if (showSKAdNetworkIds)
			{
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Name", GUILayout.MaxWidth(120));
				EditorGUILayout.LabelField("ID", GUILayout.MaxWidth(120));
				GUILayout.EndHorizontal();

				List<TDKeyValuePair> skAdNetworkIdsList = new List<TDKeyValuePair>(TDSettings.getInstance().skAdNetworkIds);

				foreach (TDKeyValuePair pair in skAdNetworkIdsList)
				{
					GUILayout.BeginHorizontal();
					EditorGUILayout.TextField(pair.getKey(), GUILayout.MaxWidth(120));
					EditorGUILayout.TextField(pair.getValue(), GUILayout.MaxWidth(120));
					if (GUILayout.Button("Remove", GUILayout.Width(75)))
					{
						//Invoke remove
						skAdNetworkIdsList.Remove(pair);
						TDSettings.getInstance().skAdNetworkIds = skAdNetworkIdsList;
						skAdNetworkIdKey = skAdNetworkIdValue = null;
						return;
					}
					GUILayout.EndHorizontal();
				}
				
				GUILayout.BeginHorizontal();

				string placeholderKey = "SKAdNetworkId";
				int keyCount = skAdNetworkIdsList.Count;

				while(skAdNetworkIdsList.Find(f => f.getKey() == placeholderKey + keyCount) != null)
				{
					keyCount++;
				}

				if (String.IsNullOrEmpty(skAdNetworkIdKey) || skAdNetworkIdsList.Find(f => f.getKey() == skAdNetworkIdKey) != null)
				{
					skAdNetworkIdKey = placeholderKey + keyCount;
				}

				skAdNetworkIdKey = EditorGUILayout.TextField(skAdNetworkIdKey, GUILayout.MaxWidth(120));
				skAdNetworkIdValue = EditorGUILayout.TextField(skAdNetworkIdValue, GUILayout.MaxWidth(120));
				if (GUILayout.Button("Add", GUILayout.Width(75)))
				{
					if (String.IsNullOrEmpty(skAdNetworkIdValue))
					{
						EditorUtility.DisplayDialog("Error", "SKAdNetworkId value is null or empty", "OK");

					}
					else if (skAdNetworkIdsList.Find(f => f.getKey() == skAdNetworkIdKey) != null)
					{

						EditorUtility.DisplayDialog("Error", "SKAdNetworkId key already exists", "OK");
					}
					else
					{
						//Invoke Add
						TDKeyValuePair pair = new TDKeyValuePair(skAdNetworkIdKey, skAdNetworkIdValue);
						skAdNetworkIdsList.Add(pair);
						TDSettings.getInstance().skAdNetworkIds = skAdNetworkIdsList;
						skAdNetworkIdValue = "";
					}
				}
				TDSettings.getInstance().skAdNetworkIds = skAdNetworkIdsList;
				GUILayout.EndHorizontal();
			}

			GUILayout.Space(15);

			ShowTestDevices ();

			GUILayout.Space (15);

			// Adapters
			DrawSeparator(2);

			GUILayout.Space (25);

            EditorGUI.BeginChangeCheck();
            settings.useCocoapodsMaven = EditorGUILayout.Toggle("Cocoapods/Maven", settings.useCocoapodsMaven);
            if(EditorGUI.EndChangeCheck())
            {
                if (settings.useCocoapodsMaven == false)
                {
                    SaveAdapters();
                } else if(TapdaqUninstallScript.IsManualIntegrationPresent())
				{
					int result = EditorUtility.DisplayDialogComplex("Enable Cocoapods/Maven",
									  "An existing Tapdaq integration is present and should be removed before using Cocoapods/Maven",
				"Cancel", "Remove", "Continue without removing");
                    
				    switch (result)
					{
						case 0:
							{
								//Cancel
								settings.useCocoapodsMaven = false;
								break;
							}
						case 1:
							{
								//Remove
								TapdaqUninstallScript.DeleteManualIntegration();
								break;
							}
						default:
							break;
					}
				}
			}

            GUILayout.Space(10);

            if(settings.useCocoapodsMaven)
            {
                GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Network", EditorStyles.boldLabel, GUILayout.MaxWidth(100));
                GUILayout.Label("iOS", EditorStyles.boldLabel, GUILayout.MaxWidth(100));
                GUILayout.Label("Android", EditorStyles.boldLabel, GUILayout.MaxWidth(100));
                GUILayout.EndHorizontal();

                bool isNetworkWithBundlePresent = false;

                EditorGUI.BeginChangeCheck();
                foreach (TDNetwork network in TDSettings.getInstance().networks)
                {
                    GUILayout.Space(10);

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(network.name, GUILayout.MaxWidth(100));
                    if(network.cocoapodsAdapterDependency != null)
                    {
                        network.iOSEnabled = EditorGUILayout.Toggle("", network.iOSEnabled, GUILayout.MaxWidth(100));
                        if (network.iOSEnabled && network.bundlePresent) {
                        	isNetworkWithBundlePresent = true;
                        }
                    }
                    if(network.mavenAdapterDependency != null)
                    {
                        network.androidEnabled = EditorGUILayout.Toggle(network.androidEnabled, GUILayout.MaxWidth(100));
                    }
                    GUILayout.EndHorizontal();
                }
                if(EditorGUI.EndChangeCheck()) {
		        	settings.shouldAddUnityIPhoneTargetToPodfile = isNetworkWithBundlePresent;
		        }
                // Only display with setting for Unity 2019.3 and above.
                #if UNITY_2019_3_OR_NEWER
                GUILayout.Space(15);
                DrawSeparator(2);
                settings.shouldAddUnityIPhoneTargetToPodfile = EditorGUILayout.ToggleLeft(new GUIContent("iOS: Add empty Unity-iPhone target to Podfile", "Enabling this option will add an empty Unity-iPhone target to the generated Podfile. This is a workaround for a bug in the interaction between External Dependency Manager and Unity 2019.3 and above. All pods are being added to UnityFramework target which may cause some of the dependencies to not find their resources that are expected to be located in main bundle(Unity-iPhone)."), settings.shouldAddUnityIPhoneTargetToPodfile);
				#endif
                GUILayout.EndVertical();                

                GUILayout.Space(25);

                ShowButton("Save Adapters", 130, Color.white, () => {
                    SaveAdapters();

                    // Save settings
                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(settings);
                    }

                });
            }

			// Save settings
			if (GUI.changed) {
				EditorUtility.SetDirty (settings);
			}

		}

		private void SaveAdapters() {
            TDDependencies.RegisterDependencies();

#if UNITY_ANDROID
            GooglePlayServices.PlayServicesResolver.MenuForceResolve();
#endif
        }

        private void ShowTestDevices() {

			showTestDevices = EditorGUILayout.Foldout (showTestDevices, "Test devices", foldoutStyle);

			deviceToDelete.Clear ();

			if (showTestDevices) {
				foreach (var device in settings.testDevices) {
					ShowTestDevice (device);
				}

				ShowAddTestDeviceView ();
			}

			foreach (var device in deviceToDelete) {
				settings.testDevices.Remove (device);
			}
		}

		string newTestDeviceName = "";
		TestDeviceType newTestDeviceType = TestDeviceType.Android;

		private void ShowAddTestDeviceView() {

			GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel);

			GUILayout.Label ("Add new device.", labelStyle);

			newTestDeviceName = EditorGUILayout.TextField ("Name:", newTestDeviceName);
			newTestDeviceType = (TestDeviceType)EditorGUILayout.EnumPopup ("Type:", newTestDeviceType);

			ShowButton ("Add", 90, Color.green, () => {
				if (!string.IsNullOrEmpty (newTestDeviceName)) {
					var testDevice = new TestDevice (newTestDeviceName, newTestDeviceType);
					settings.testDevices.Add (testDevice);
					newTestDeviceName = "";
				}
			});

			DrawSeparator (2);
		}

		private void ShowDeleteDialog(TestDevice device) {
			if (EditorUtility.DisplayDialog ("Delete Test Device",
				"Are you sure you want to delete test device " + device.name + "?",
				"Delete", "Cancel")) {

				deviceToDelete.Add (device);
			}
		}

		private void ShowTestDevice(TestDevice device) {

			GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel);
			var isAndroid = device.type == TestDeviceType.Android;

			GUILayout.Label (device.name + " (" + device.type.ToString () + ")", labelStyle);

			device.adMobId = EditorGUILayout.TextField ("AdMob ID:", device.adMobId);
			device.facebookId = EditorGUILayout.TextField ("Facebook ID:", device.facebookId);

			ShowButton ("Delete", 50, new Color (0.85f, 0.15f, 0), () => {
				ShowDeleteDialog (device);
			});

			DrawSeparator (2);
		}

		private void DrawSeparator(int height) {
			GUILayout.Box (whiteTexture, GUILayout.ExpandWidth (true), GUILayout.Height (height));
		}

		private void ShowButton(string text, int width, Color color, Action action) {
			GUI.backgroundColor = color;
			if (GUILayout.Button (text, GUILayout.Width (width))) {
				if (action != null)
					action.Invoke ();
			}
			GUI.backgroundColor = Color.white;
		}
    }
}
