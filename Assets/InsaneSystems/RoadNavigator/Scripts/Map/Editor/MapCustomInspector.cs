using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace InsaneSystems.RoadNavigator
{
	[CustomEditor(typeof(Map))]
	public class MapCustomInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (GUILayout.Button("Setup map image for editor"))
				(target as Map).SetupMapImage();
		}
	}
}