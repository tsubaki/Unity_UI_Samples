using UnityEngine;

public class ActiveDeactiveObject : MonoBehaviour
{
	[SerializeField]
	GameObject target;

	public void UpdateRegister(bool isTrue)
	{
		if(!isTrue) ResumeAnimator.RestoreAnimator(target);
		target.SetActive(isTrue);
	}

#if UNITY_EDITOR
	[ContextMenu("Add Resume Animator")]
	public void AddResumeAnimatorInChildren()
	{
		ResumeAnimator.AddResumeAnimatorInChildren (target);
	}
#endif
}
