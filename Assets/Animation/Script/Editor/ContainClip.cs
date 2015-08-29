using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;

public class ContainClip : EditorWindow {
	
	private AnimatorController controller;

	string clipName;
	
	[MenuItem("Assets/CombineAnimationclip")]
	static void Create()
	{
		var window = ContainClip.GetWindow(typeof(ContainClip)) as ContainClip;
		if( Selection.activeObject is AnimatorController )
			window.controller = Selection.activeObject as AnimatorController;
	}
	
	void OnGUI()
	{
		EditorGUILayout.LabelField("target clip");
		controller = EditorGUILayout.ObjectField(controller, typeof(AnimatorController), false) as AnimatorController;
		
		if( controller == null )
			return;
		
		List<AnimationClip> clipList = new List<AnimationClip>();
		
		var allAsset = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(controller));
		foreach( var asset in allAsset )
		{
			if( asset is AnimationClip)
			{
				var removeClip = asset as AnimationClip;
				if(! clipList.Contains(removeClip ) ){
					clipList.Add(removeClip);
				}
			}
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Add new clip");
		EditorGUILayout.BeginVertical("box");
		clipName = EditorGUILayout.TextField(clipName);
		if( clipList.Exists(item=> item.name == clipName ) || string.IsNullOrEmpty(clipName) )
		{
			EditorGUILayout.LabelField("can't create duplicate names or empty");
		}else{
			if( GUILayout.Button("create"))
			{
				AnimationClip animationClip = AnimatorController.AllocateAnimatorClip(clipName);
				AssetDatabase.AddObjectToAsset(animationClip, controller);
				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(controller));
				AssetDatabase.Refresh();
			}
		}
		EditorGUILayout.EndVertical();
		
		
		if( clipList.Count == 0)
			return;
		
		EditorGUILayout.Space();
		
		EditorGUILayout.LabelField("remove clip");
		EditorGUILayout.BeginVertical("box");
		
		foreach( var removeClip in clipList )
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(removeClip.name);
			if( GUILayout.Button("remove" , GUILayout.Width(100)))
			{
				Object.DestroyImmediate(removeClip, true);
				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(controller));
			}
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.EndVertical();
		
	}
} 