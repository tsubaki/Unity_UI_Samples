using UnityEngine;
using System.Collections;

public class ActiveDeactiveObject : MonoBehaviour
{
	[SerializeField]
	GameObject
		target;

	public void UpdateRegister (bool isTrue)
	{
		if (!isTrue)
			ResumeAnimator.RestoreAnimator (target);
		target.SetActive (isTrue);

	}

	[ContextMenu("Add Resume Animator")]
	public void AddResumeAnimatorInChildren ()
	{
		ResumeAnimator.AddResumeAnimatorInChildren (target);
	}
}