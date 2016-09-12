using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(InfiniteScroll))]
public class ItemControllerInfinite : UIBehaviour, IInfiniteScrollSetup
{
	public void OnPostSetupItems()
	{
		GetComponent<InfiniteScroll>().onUpdateItem.AddListener(OnUpdateItem);
		GetComponentInParent<ScrollRect>().movementType = ScrollRect.MovementType.Unrestricted;
	}

	public void OnUpdateItem(int itemCount, GameObject obj)
	{
		var item = obj.GetComponentInChildren<Item>();
		item.UpdateItem(itemCount);
	}
}
