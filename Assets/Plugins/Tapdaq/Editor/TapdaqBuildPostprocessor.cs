using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using Tapdaq;


using TDEditor.iOS.Xcode;


public class TapdaqBuildPostprocessor : MonoBehaviour{
	private static string FrameworksPath = "Pods/";
	private static string FrameworksDir = "iOS/";
	private const string BuildPathKey = "IOSBuildProjectPath";
	[MenuItem ("Tapdaq/Run iOS Build Postprocess", false, 2222)]
	private static void RunIOSPostProcessManually()	{
		var path = EditorPrefs.GetString (BuildPathKey, null);
		OnPostprocessBuild (BuildTarget.iOS, path);
	}

	[MenuItem ("Tapdaq/Run iOS Build Postprocess", true)]
	static bool validateRunPostBuilder(){
		Debug.Log("validateRunPost");
		var path = EditorPrefs.GetString (BuildPathKey, null);
		if( path == null || !Directory.Exists( path ) )
			return false;

		var projectFile = Path.Combine( path, "Unity-iPhone.xcodeproj/project.pbxproj" );
		if( !File.Exists( projectFile ) )
			return false;

		return true;
	}

	private static void processExistingiOSPaths (string targetPath)  {
		Debug.Log("processExtisting");
        if (!Directory.Exists(targetPath + "/" + FrameworksPath)) {
            FrameworksPath = "Frameworks/";
            FrameworksDir = "";
        }

		foreach (string dirPath in Directory.GetDirectories(targetPath + "/" + FrameworksPath)) {
			var dirName = Path.GetFileName(dirPath);
			if (String.Compare(dirName, "ios", true) == 0) {
				FrameworksDir = dirName + "/";
			}
		}
	}

    [PostProcessBuildAttribute(45)]//must be between 40 and 50 to ensure that it's not overriden by Podfile generation (40) and that it's added before "pod install" (50)
    private static void PostProcessBuildEditPodfile_iOS(BuildTarget target, string buildPath)
    {
        if (target == BuildTarget.iOS && TDSettings.getInstance().shouldAddUnityIPhoneTargetToPodfile)
        {
            #if UNITY_2019_3_OR_NEWER
            bool networkWithBundlePresent = false;
            foreach (TDNetwork network in TDNetwork.AllNetworks) {
                if (network.iOSEnabled && network.bundlePresent) {
                    TDDebugLogger.Log("Network " + network.name + " has bundle");
                    networkWithBundlePresent = true;
                }
            }
            // Return if adding a target is not required by any enabled network
            if (!networkWithBundlePresent) { return ; }
            using (StreamWriter streamWriter = File.AppendText(buildPath + "/Podfile"))
            {

                TDDebugLogger.Log("Adding Unity-iPhone target");
                streamWriter.WriteLine("\ntarget 'Unity-iPhone' do\n\nend");
            }
            #endif
        }
    }


	[PostProcessBuild(101)]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject) {
		if (buildTarget != BuildTarget.iOS) return;
        TDDebugLogger.Log("OnPostprocessBuild - Path: " + pathToBuiltProject);
        
		EditorPrefs.SetString (BuildPathKey, pathToBuiltProject);

        FrameworksPath = (TDSettings.getInstance().useCocoapodsMaven ? "Pods/" : "Frameworks/Plugins/");
        TDDebugLogger.Log("FrameworksPath: " + FrameworksPath);

        var path = PBXProject.GetPBXProjectPath(pathToBuiltProject);
        if (!File.Exists(path))
        {
            TDDebugLogger.LogError(string.Format("pbxproj '{0}' does not exists", path));
            return;
        }

        var proj = new PBXProject();
        proj.ReadFromString(File.ReadAllText(path));

        var unityIPhoneTargetGuid = GetUnityIPhoneTargetGuid(path);
        var unityFrameworkTargetGuid = GetUnityFrameworkTargetGuid(path);

        processExistingiOSPaths(pathToBuiltProject);

        SetBuildProperties(proj, unityIPhoneTargetGuid, unityFrameworkTargetGuid);

        // Pass Unity-iPhone target only when UnityFramework is not available
        AddLibraries(proj, unityFrameworkTargetGuid == null ? unityIPhoneTargetGuid : unityFrameworkTargetGuid, pathToBuiltProject);

        SetPListProperties(pathToBuiltProject);

        File.WriteAllText(path, proj.WriteToString());
    }

    private static string GetUnityFrameworkTargetGuid(string path) {
        //var proj = new UnityEditor.iOS.Xcode.PBXProject();
        //proj.ReadFromString(File.ReadAllText(path));
        //string unityFrameworkTargetGuid = null;

        //var unityFrameworkTargetGuidMethod = proj.GetType().GetMethod("GetUnityFrameworkTargetGuid");

        //if (unityFrameworkTargetGuidMethod != null) {
        //    unityFrameworkTargetGuid = (string)unityFrameworkTargetGuidMethod.Invoke(proj, null);
        //}
        //return unityFrameworkTargetGuid;
        return null;
    }


    private static string GetUnityIPhoneTargetGuid(string path) {
        //var proj = new UnityEditor.iOS.Xcode.PBXProject();
        //proj.ReadFromString(File.ReadAllText(path));
        //string mainTargetGuid = null;

        //var unityMainTargetGuidMethod = proj.GetType().GetMethod("GetUnityMainTargetGuid");                       
        //if (unityMainTargetGuidMethod != null) {
        //    mainTargetGuid = (string)unityMainTargetGuidMethod.Invoke(proj, null);
        //} else {
        //    mainTargetGuid = proj.TargetGuidByName ("Unity-iPhone");
        //}
        //return mainTargetGuid;
        return null;
    }

    private static void SetBuildProperties(PBXProject proj, string unityIPhoneTargetGuid, string unityFrameworkTargetGuid) {
		proj.SetBuildProperty(unityIPhoneTargetGuid, "LD_RUNPATH_SEARCH_PATHS", "$(inherited) @executable_path/Frameworks");
		proj.SetBuildProperty(unityIPhoneTargetGuid, "IPHONEOS_DEPLOYMENT_TARGET", GetIOSDeploymentTarget(proj));
        proj.SetBuildProperty(unityIPhoneTargetGuid, "CLANG_ENABLE_MODULES", "YES");
        proj.UpdateBuildProperty(unityIPhoneTargetGuid, "OTHER_LDFLAGS", new string [] { "-ObjC" }, new string [] {});

        var targetGuidForFrameworks = unityFrameworkTargetGuid == null ? unityIPhoneTargetGuid : unityFrameworkTargetGuid;

        // This flag is also required in UnityFramework target
        if (unityFrameworkTargetGuid != null) {
            proj.UpdateBuildProperty(unityFrameworkTargetGuid, "OTHER_LDFLAGS", new string [] { "-ObjC" }, new string [] {});
        }

        List<string> frameworks = new List<string>();
        List<string> weakFrameworks = new List<string>();

        // Tapdaq Dependencies
        frameworks.Add(TDFramework.ADSUPPORT_FRAMEWORK);
        frameworks.Add(TDFramework.FOUNDATION_FRAMEWORK);
        frameworks.Add(TDFramework.QUARTZ_CORE_FRAMEWORK);
        frameworks.Add(TDFramework.SECURITY_FRAMEWORK);
        frameworks.Add(TDFramework.SYSTEM_CONFIGURATION_FRAMEWORK);
        frameworks.Add(TDFramework.UI_KIT_FRAMEWORK);

        foreach (TDNetwork network in TDNetwork.AllNetworks)
        {
            if(AssetDatabase.FindAssets(String.Concat(network.name, "Adapter.framework")).Length > 0)
            {
                foreach(TDFramework framework in network.iosDependendencies)
                {
                    if(framework.Weak)
                    {
                        if(!weakFrameworks.Contains(framework.Name))
                        {
                            weakFrameworks.Add(framework.Name);
                        }
                    } else
                    {
                        if (!frameworks.Contains(framework.Name))
                        {
                            frameworks.Add(framework.Name);
                        }
                    }
                }
            }
        }

        foreach(string framework in weakFrameworks)
        {
            proj.AddFrameworkToProject(targetGuidForFrameworks, framework, true);
        }
        foreach (string framework in frameworks)
        {
            proj.AddFrameworkToProject(targetGuidForFrameworks, framework, false);
        }

        var path = EditorPrefs.GetString (BuildPathKey, null);

        string dir = path + "/Pods/";
        if (Directory.Exists(dir) && Directory.GetFiles(dir, "YouAppiAdapter.framework", SearchOption.AllDirectories) != null
            || AssetDatabase.FindAssets("YouAppiAdapter.framework").Length > 0)
        {
            Debug.Log("SET YOUAPPI PROPS");
            proj.SetBuildProperty(unityIPhoneTargetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
            proj.SetBuildProperty(unityIPhoneTargetGuid, "DEFINES_MODULE", "YES");
            proj.SetBuildProperty(unityIPhoneTargetGuid, "ENABLE_BITCODE", "NO");
        }
    }

    private static void AddLibraries(PBXProject proj, string target, string projectPath) {
        bool shouldUseNewFolderStructure = !Directory.Exists(projectPath + "/" + FrameworksPath + FrameworksDir + "Tapdaq");
		Debug.Log("Use new folder structure: " + shouldUseNewFolderStructure);
        
 		foreach(TDNetwork network in TDNetwork.AllNetworks) {
			if (network.name.Equals("YouAppiAdapter", StringComparison.CurrentCultureIgnoreCase)) {
                if (TDSettings.getInstance().useCocoapodsMaven)
                {
                    proj.EmbedFramework(target, projectPath + "/Pods/YouAppiMoatSDK/YouAppiMoat.framework");
                }
                else
                {
                    proj.EmbedFramework(target, FrameworksPath + FrameworksDir + (shouldUseNewFolderStructure ? "" : "Tapdaq/NetworkSDKs/YouAppiAdapter/") + "YouAppiMoat.framework");
                }
            }
		}  

        if (AssetDatabase.FindAssets("Tapjoy.framework").Length > 0)
        {
            if (!proj.ContainsFileByProjectPath("TapjoyResources.bundle"))
            {
                var fullPath = FrameworksPath + FrameworksDir
                    + (shouldUseNewFolderStructure ? "" : "Tapdaq/NetworkSDKs/TapjoyAdapter/") + "Tapjoy.framework/Resources/TapjoyResources.bundle";
                if (!Directory.Exists(fullPath))
                {
                    Debug.Log("TapjoyResources.bundle does not exist");
                }
                proj.AddFileToBuild(target, proj.AddFile(fullPath, "TapjoyResources.bundle", PBXSourceTree.Source));
            }
        }
    }

	private static void SetPListProperties(string pathToBuiltProject) {
		
		var plistPath = pathToBuiltProject + "/Info.plist";
		var plist = new PlistDocument();

		plist.ReadFromString(File.ReadAllText(plistPath));
		var rootDict = plist.root;

		if(AssetDatabase.FindAssets("AdColonyAdapter.framework").Length > 0) {
			rootDict.SetString("NSMotionUsageDescription", "Some ad content may require access to accelerometer for interactive ad experience.");
			rootDict.SetString("NSPhotoLibraryUsageDescription", "Some ad content may require access to the photo library.");
			rootDict.SetString("NSCalendarsUsageDescription", "Some ad content may create a calendar event.");
			rootDict.SetString("NSCameraUsageDescription", "Some ad content may access camera to take picture.");
		}

		var transportSecurityKey = "NSAppTransportSecurity";

		if (rootDict [transportSecurityKey] == null) {
			rootDict.CreateDict (transportSecurityKey);
		}

		var appTransportSecurity = rootDict [transportSecurityKey].AsDict ();
		
		appTransportSecurity.SetBoolean ("NSAllowsArbitraryLoads", true);

		if(TDSettings.getInstance().admob_appid_ios.Length > 0) {
            rootDict.SetString("GADApplicationIdentifier", TDSettings.getInstance().admob_appid_ios);
		}

        //SKAdNetworkIds
        List<TDKeyValuePair> skAdNetworkIds = TDSettings.getInstance().skAdNetworkIds;
        if (skAdNetworkIds.Count > 0)
        {
            PlistElementArray skAdNetworkItemsArray = rootDict.CreateArray("SKAdNetworkItems");
            foreach (TDKeyValuePair pair in skAdNetworkIds)
            {
                PlistElementDict itemDict = skAdNetworkItemsArray.AddDict();
                itemDict.SetString("SKAdNetworkIdentifier", pair.getValue());
            }
        }

		// Write to file
		File.WriteAllText(plistPath, plist.WriteToString());
	}

	private static string GetIOSDeploymentTarget(PBXProject proj) {
		var target = proj.TargetGuidByName("Unity-iPhone");
		var deploymentTargets = proj.GetBuildProperties (target, "IPHONEOS_DEPLOYMENT_TARGET");

		var deploymentTarget = "0";
		if (deploymentTargets.Count > 0) {
			deploymentTarget = deploymentTargets [0];
		}

		if (string.IsNullOrEmpty (deploymentTarget))
			deploymentTarget = "0";

		Regex rgx = new Regex("[^0-9.]");
		var numberOnly = rgx.Replace(deploymentTarget, "");

		var version = Tapdaq.TDExtensionMethods.ParseFloat (numberOnly, 0);

		if (version >= 8.0f)
			return deploymentTarget;

		TDDebugLogger.LogWarning ("TapdaqBuildPostprocessor changes iOS build target version from " + deploymentTarget + " to = 8.0");

		return "8.0";
	}
}
