using UnityEngine;
using System;
using System.Linq;

[DisallowMultipleComponent]
public class ResumeAnimator : MonoBehaviour
{
	[System.Serializable]
	struct LayerInfo
	{
		public int nameHash;
		public float time;
	}

	Animator anim;
	LayerInfo[] layerInfo = null;
	object[] parameters;
	
	[SerializeField]
	AnimatorParameter parameter;
	
	void Awake()
	{
		anim = GetComponent<Animator>();
		layerInfo = new LayerInfo[anim.layerCount];
		for (int i = 0; i < layerInfo.Length; i++) {
			layerInfo[i] = new LayerInfo();
		}
	}

	void OnEnable()
	{
		Restore ();
	}

	public void Restore()
	{
		for(int currentLayerCount = 0; currentLayerCount<anim.layerCount; currentLayerCount++) {
			var layer = layerInfo [currentLayerCount];
			anim.Play (layer.nameHash, currentLayerCount, layer.time);
		}
		if(parameter == null || parameters == null) return;
		
		for(int i = 0; i < parameter.parameterInfos.Length; i++) {
			if(parameters[i] == null) continue;
			
			var parameterInfo = parameter.parameterInfos[i];
			if(parameterInfo.type == AnimatorParameter.AnimatorParameterInfo.ParameterType.BOOL) {
				anim.SetBool (parameterInfo.hashName, (bool)parameters [i]);
			}
			else if(parameterInfo.type == AnimatorParameter.AnimatorParameterInfo.ParameterType.FLOAT) {
				anim.SetFloat (parameterInfo.hashName, (float)parameters [i]);
			}
			else if(parameterInfo.type == AnimatorParameter.AnimatorParameterInfo.ParameterType.INT) {
				anim.SetInteger (parameterInfo.hashName, (int)parameters [i]);
			}
			else if(parameterInfo.type == AnimatorParameter.AnimatorParameterInfo.ParameterType.TRIGGER) {
				anim.SetBool (parameterInfo.hashName, (bool)parameters [i]);
			}
		}
	}

	public void Save ()
	{
		for(int currentLayerCount = 0; currentLayerCount < anim.layerCount; currentLayerCount++) {
			layerInfo[currentLayerCount].nameHash = anim.GetCurrentAnimatorStateInfo(currentLayerCount).fullPathHash;
			layerInfo[currentLayerCount].time = anim.GetCurrentAnimatorStateInfo(currentLayerCount).normalizedTime;
		}
		
		if(parameter == null) return;
		
		if(parameters == null) parameters = new object[parameter.parameterInfos.Length];
		
		for(int i = 0; i < parameter.parameterInfos.Length; i++) {
			var parameterInfo = parameter.parameterInfos[i];
			if(parameterInfo.type == AnimatorParameter.AnimatorParameterInfo.ParameterType.BOOL) {
				parameters [i] = anim.GetBool(parameterInfo.hashName);
			}
			else if(parameterInfo.type == AnimatorParameter.AnimatorParameterInfo.ParameterType.FLOAT) {
				parameters [i] = anim.GetFloat(parameterInfo.hashName);
			}
			else if(parameterInfo.type == AnimatorParameter.AnimatorParameterInfo.ParameterType.INT) {
				parameters [i] = anim.GetInteger(parameterInfo.hashName);
			}
			else if(parameterInfo.type == AnimatorParameter.AnimatorParameterInfo.ParameterType.TRIGGER) {
				parameters [i] = anim.GetBool(parameterInfo.hashName);
			}
		}
	}

	public static void RestoreAnimator(GameObject target)
	{
		foreach(var resumeAnim in target.GetComponentsInChildren<ResumeAnimator>()) {
			resumeAnim.Save ();
		}
	}

#if UNITY_EDITOR
	public static void AddResumeAnimatorInChildren(GameObject obj)
	{
		var components = obj.GetComponentsInChildren<Animator>();
		foreach(var item in components) {
			ResumeAnimator resumeAnim = item.GetComponent<ResumeAnimator>();
			if (resumeAnim == null) resumeAnim = item.gameObject.AddComponent<ResumeAnimator>();
		}
	}
#endif
}
