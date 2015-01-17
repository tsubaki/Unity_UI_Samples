using UnityEngine;
using System.Collections;
using System.Linq;
using System;

[DisallowMultipleComponent]
public class ResumeAnimator : MonoBehaviour
{
	private	Animator anim;
	private LayerInfo[] layerInfo = null;
	private object[] parameters ;
	[SerializeField, HideInInspector]
	AnimatorParameterInfo[]
		parameterInfos;

	void Awake ()
	{
		anim = GetComponent<Animator> ();
		layerInfo = new LayerInfo[anim.layerCount];
		for (int i=0; i<layerInfo.Length; i++) {
			layerInfo [i] = new LayerInfo ();
		}
	}

	void OnEnable ()
	{
		Restore ();
	}

	public void Restore ()
	{
		for (int currentLayerCount=0; currentLayerCount<anim.layerCount; currentLayerCount++) {
			var layer = layerInfo [currentLayerCount];
			anim.Play (layer.nameHash, currentLayerCount, layer.time);
		}
		if (parameters == null)
			return;

		for (int i=0; i<parameterInfos.Length; i++) {
			var parameterInfo = parameterInfos [i];
			if (parameterInfo.type == ResumeAnimator.AnimatorParameterInfo.ParameterType.BOOL) {
				anim.SetBool (parameterInfo.hashName, (bool)parameters [i]);
			} else if (parameterInfo.type == ResumeAnimator.AnimatorParameterInfo.ParameterType.FLOAT) {
				anim.SetFloat (parameterInfo.hashName, (float)parameters [i]);
			} else if (parameterInfo.type == ResumeAnimator.AnimatorParameterInfo.ParameterType.INT) {
				anim.SetInteger (parameterInfo.hashName, (int)parameters [i]);
			}
		}
	}

	public void Save ()
	{
		for (int currentLayerCount=0; currentLayerCount<anim.layerCount; currentLayerCount++) {
			layerInfo [currentLayerCount].nameHash = anim.GetCurrentAnimatorStateInfo (currentLayerCount).nameHash;
			layerInfo [currentLayerCount].time = anim.GetCurrentAnimatorStateInfo (currentLayerCount).normalizedTime;
		}

		parameters = new object[parameterInfos.Length];
		for (int i=0; i<parameterInfos.Length; i++) {
			var parameterInfo = parameterInfos [i];
			if (parameterInfo.type == ResumeAnimator.AnimatorParameterInfo.ParameterType.BOOL) {
				parameters [i] = anim.GetBool (parameterInfo.hashName);
			} else if (parameterInfo.type == ResumeAnimator.AnimatorParameterInfo.ParameterType.FLOAT) {
				parameters [i] = anim.GetFloat (parameterInfo.hashName);
			} else if (parameterInfo.type == ResumeAnimator.AnimatorParameterInfo.ParameterType.INT) {
				parameters [i] = anim.GetInteger (parameterInfo.hashName);
			}
		}
	}

	public static void RestoreAnimator (GameObject target)
	{
		foreach (var resumeAnim in target.GetComponentsInChildren<ResumeAnimator>()) {
			resumeAnim.Save ();
		}
	}

#if UNITY_EDITOR

	public static void AddResumeAnimatorInChildren (GameObject obj)
	{
		var components = obj.GetComponentsInChildren<Animator> ();
		foreach (var item in components) {
			ResumeAnimator resumeAnim = item.GetComponent<ResumeAnimator> ();
			if (resumeAnim == null)
				resumeAnim = item.gameObject.AddComponent<ResumeAnimator> ();
			resumeAnim.Reset();
		}
	}

	public void Reset ()
	{
		Animator animparam = GetComponent<Animator> ();
		UnityEditor.Animations.AnimatorController controller = animparam.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
		var parameterCount = controller.parameters.Length;
		parameterInfos = new AnimatorParameterInfo[parameterCount];

		for (int i=0; i<parameterCount; i++) {
			var param = controller.parameters[i];
			parameterInfos [i].hashName = Animator.StringToHash (controller.parameters[i].name);

			if (param.type == AnimatorControllerParameterType.Bool) {
				parameterInfos [i].type = ResumeAnimator.AnimatorParameterInfo.ParameterType.BOOL;
			} else if (param.type == AnimatorControllerParameterType.Float) {
				parameterInfos [i].type = ResumeAnimator.AnimatorParameterInfo.ParameterType.FLOAT;
			} else if (param.type == AnimatorControllerParameterType.Int) {
				parameterInfos [i].type = ResumeAnimator.AnimatorParameterInfo.ParameterType.INT;
			} else if (param.type == AnimatorControllerParameterType.Trigger) {
				parameterInfos [i].type = ResumeAnimator.AnimatorParameterInfo.ParameterType.TRIGGER;
			}
		}
	}
#endif

	[System.Serializable]
	struct LayerInfo
	{
		public int nameHash;
		public float time;
	}

	[System.Serializable]
	struct AnimatorParameterInfo
	{
		public ParameterType type;
		public int hashName;
		
		public enum ParameterType
		{
			NONE,
			BOOL,
			FLOAT,
			INT,
			TRIGGER
		}
	}
}
