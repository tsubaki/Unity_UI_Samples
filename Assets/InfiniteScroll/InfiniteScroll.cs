using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class InfiniteScroll : UIBehaviour
{
	[SerializeField]
	private RectTransform m_ItemBase;

	[SerializeField, Range(0, 30)]
	int m_instantateItemCount = 9;

	[HideInInspector]
	public List<RectTransform>	m_itemList = new List<RectTransform> ();

	public OnItemPositionChange onUpdateItem = new OnItemPositionChange ();

	protected float m_diffPreFramePosiitonY = 0;

	protected int m_currentItemNo = 0;

	// cache component

	private RectTransform m_rectTransform;
	protected RectTransform _RectTransform {
		get {
			if (m_rectTransform == null)
				m_rectTransform = GetComponent<RectTransform> ();
			return m_rectTransform;
		}
	}

	private float m_itemHeight = -1;
	public float ItemHeight {
		get {
			if (m_ItemBase != null) {
				m_itemHeight = m_ItemBase.sizeDelta.y;
			}
			return m_itemHeight;
		}
	}

	protected override void Start ()
	{
		var controllers =GetComponents<MonoBehaviour>()
				.Where(item => item is IInfiniteScrollSetup )
				.Select(item => item as IInfiniteScrollSetup )
				.ToList();

		// create items

		m_ItemBase.gameObject.SetActive (false);
		
		for (int i=0; i<m_instantateItemCount; i++) {
			var item = GameObject.Instantiate (m_ItemBase) as RectTransform;
			item.SetParent (transform, false);
			item.name = i.ToString ();
			item.anchoredPosition = new Vector2 (0, -ItemHeight * (i));
			m_itemList.Add (item);

			item.gameObject.SetActive (true);

			foreach( var controller in controllers ){
				controller.OnUpdateItem(i, item.gameObject);
			}
		}

		foreach(  var controller in controllers  ){
			controller.OnPostSetupItems();
		}
	}

	void Update ()
	{
		var itemListLastCount = m_instantateItemCount - 1; 

		while (_RectTransform.anchoredPosition.y - m_diffPreFramePosiitonY  > ItemHeight) {
			m_diffPreFramePosiitonY += ItemHeight;

			var item = m_itemList [0];
			m_itemList.RemoveAt (0);
			m_itemList.Add (item);

			item.anchoredPosition = new Vector2 (0, (-ItemHeight * (itemListLastCount)) - ItemHeight * m_currentItemNo);
			onUpdateItem.Invoke (m_currentItemNo + itemListLastCount, item.gameObject);

			m_currentItemNo ++;
		}

		while (_RectTransform.anchoredPosition.y - m_diffPreFramePosiitonY  < ItemHeight * 2) {
			m_diffPreFramePosiitonY -= ItemHeight;

			var item = m_itemList [itemListLastCount];
			m_itemList.RemoveAt (itemListLastCount);
			m_itemList.Insert (0, item);

			m_currentItemNo --;

			item.anchoredPosition = new Vector2 (0, -ItemHeight * m_currentItemNo);
			onUpdateItem.Invoke (m_currentItemNo, item.gameObject);
		}
	}

	public class OnItemPositionChange : UnityEngine.Events.UnityEvent<int, GameObject>{}
}
