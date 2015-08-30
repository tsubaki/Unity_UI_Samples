using UnityEngine;
using System.Collections;

public class AnimatorParameter : ScriptableObject
{
	[SerializeField]
	RuntimeAnimatorController animatorController;
	public AnimatorParameterInfo[] parameterInfos;
	[System.Serializable]
	public struct AnimatorParameterInfo
	{
		public ParameterType type;
		public int hashName;
		//public string name;
		public enum ParameterType
		{
			NONE,
			BOOL,
			FLOAT,
			INT,
			TRIGGER
		}
	}
	#if UNITY_EDITOR
	[ContextMenu("Setup")]
	void Setup()
	{
		UnityEditor.Animations.AnimatorController controller = animatorController as UnityEditor.Animations.AnimatorController;
		parameterInfos = new AnimatorParameterInfo[controller.parameters.Length];
		UnityEditor.AssetDatabase.RenameAsset(UnityEditor.AssetDatabase.GetAssetPath(this), animatorController.name);
		name = animatorController.name;
		for(int i = 0; i < controller.parameters.Length; i++) {
			var param = controller.parameters[i];
			parameterInfos[i].hashName = param.nameHash;
			if(param.type == AnimatorControllerParameterType.Bool) {
				parameterInfos[i].type = AnimatorParameterInfo.ParameterType.BOOL;
			}
			else if(param.type == AnimatorControllerParameterType.Float) {
				parameterInfos[i].type = AnimatorParameterInfo.ParameterType.FLOAT;
			}
			else if(param.type == AnimatorControllerParameterType.Int) {
				parameterInfos[i].type = AnimatorParameterInfo.ParameterType.INT;
			}
			else if(param.type == AnimatorControllerParameterType.Trigger) {
				parameterInfos[i].type = AnimatorParameterInfo.ParameterType.TRIGGER;
			}
		}
		
		UnityEditor.EditorUtility.SetDirty(this);
	}
	
	[UnityEditor.CustomEditor(typeof(AnimatorParameter)), UnityEditor.CanEditMultipleObjects]
	class AnimatorParameterEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI ();

			UnityEditor.EditorGUILayout.Space();
			if(GUILayout.Button("Setup")){
				foreach(var obj in targets) {
					var animparameter = obj as AnimatorParameter;
					animparameter.Setup();
				}
			}
		}
	}
	#endif
}
