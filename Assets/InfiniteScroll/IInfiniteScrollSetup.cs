using UnityEngine;
using System.Collections;

public interface IInfiniteScrollSetup
{
	void OnPostSetupItems();
	void OnUpdateItem (int itemCount, GameObject obj);

}
