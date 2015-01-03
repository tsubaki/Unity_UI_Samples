using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(InfiniteScroll))]
public class ItemControllerLimited : UIBehaviour, IInfiniteScrollSetup {

	[SerializeField, Range(1, 999)]
	private int max = 30;

	public void OnPostSetupItems ()
	{
		var infiniteScroll =GetComponent<InfiniteScroll> ();
		infiniteScroll.onUpdateItem.AddListener (OnUpdateItem);
		GetComponentInParent<ScrollRect> ().movementType = ScrollRect.MovementType.Elastic;

		var rectTransform = GetComponent<RectTransform> ();
		var delta = rectTransform.sizeDelta;
		delta.y = infiniteScroll.ItemScale * (max);
		rectTransform.sizeDelta = delta;
	}

	public void OnUpdateItem (int itemCount, GameObject obj)
	{
		if (itemCount < 0 || itemCount >= max) {
			obj.SetActive (false);
		} else {
			obj.SetActive (true);
			
			var item = obj.GetComponentInChildren<Item> ();
			item.UpdateItem (itemCount);
		}
	}
}
