using UnityEngine;
using System.Collections;

public class ResumeAnimator : MonoBehaviour
{
	private	Animator anim;
	private LayerInfo[] layerInfo;

	private Hashtable param = new Hashtable();

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
		Resume();
	}

	public void Resume()
	{
		for (int currentLayerCount=0; currentLayerCount<anim.layerCount; currentLayerCount++) {
			var layer = layerInfo [currentLayerCount];
			anim.Play (layer.nameHash, currentLayerCount, layer.time);
		}
	}

	public void Restore ()
	{
		for (int currentLayerCount=0; currentLayerCount<anim.layerCount; currentLayerCount++) {
			layerInfo [currentLayerCount].nameHash = anim.GetCurrentAnimatorStateInfo (currentLayerCount).nameHash;
			layerInfo [currentLayerCount].time = anim.GetCurrentAnimatorStateInfo (currentLayerCount).normalizedTime;
		}
	}

	public static void RestoreAnimator(GameObject target)
	{
		foreach (var resumeAnim in target.GetComponentsInChildren<ResumeAnimator>()) {
			resumeAnim.Restore ();
		}
	}

	[System.Serializable]
	class LayerInfo
	{
		public int nameHash = 0;
		public float time = 0;
	}
}
