using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(InfiniteScroll))]
public class ItemControllerLoop : UIBehaviour, IInfiniteScrollSetup
{
	private bool isSetuped = false;

	public void OnPostSetupItems()
	{
		GetComponentInParent<ScrollRect>().movementType = ScrollRect.MovementType.Unrestricted;
		isSetuped = true;
	}

	public void OnUpdateItem(int itemCount, GameObject obj)
	{
		if(isSetuped == true) return;

		var item = obj.GetComponentInChildren<Item>();
		item.UpdateItem(itemCount);
	}
}
