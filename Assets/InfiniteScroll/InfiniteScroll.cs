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

	public Direction direction;

	public OnItemPositionChange onUpdateItem = new OnItemPositionChange ();

	[System.NonSerialized]
	public LinkedList<RectTransform>	m_itemList = new LinkedList<RectTransform> ();

	protected float m_diffPreFramePosition = 0;

	protected int m_currentItemNo = 0;

	public enum Direction
	{
		Vertical,
		Horizontal,
	}



	// cache component

	private RectTransform m_rectTransform;
	protected RectTransform rectTransform {
		get {
			if (m_rectTransform == null)
				m_rectTransform = GetComponent<RectTransform> ();
			return m_rectTransform;
		}
	}

	private float AnchoredPosition
	{
		get{
			return  (direction == Direction.Vertical ) ? 
					-rectTransform.anchoredPosition.y:
					rectTransform.anchoredPosition.x;
		}
	}

	private float m_itemScale = -1;
	public float ItemScale {
		get {
			if (m_ItemBase != null && m_itemScale == -1) {
					m_itemScale = (direction == Direction.Vertical ) ? 
					m_ItemBase.sizeDelta.y : 
					m_ItemBase.sizeDelta.x ;
			}
			return m_itemScale;
		}
	}

	protected override void Start ()
	{
		var controllers =GetComponents<MonoBehaviour>()
				.Where(item => item is IInfiniteScrollSetup )
				.Select(item => item as IInfiniteScrollSetup )
				.ToList();

		// create items

		var scrollRect = GetComponentInParent<ScrollRect>();
		scrollRect.horizontal = direction == Direction.Horizontal;
		scrollRect.vertical = direction == Direction.Vertical;
		scrollRect.content = rectTransform;

		m_ItemBase.gameObject.SetActive (false);
		
		for (int i=0; i<m_instantateItemCount; i++) {
			var item = GameObject.Instantiate (m_ItemBase) as RectTransform;
			item.SetParent (transform, false);
			item.name = i.ToString ();
			item.anchoredPosition = 
					(direction == Direction.Vertical ) ?
					new Vector2 (0, -ItemScale * (i)) : 
					new Vector2 (ItemScale * (i), 0) ;
			m_itemList.AddLast (item);

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

		while (AnchoredPosition - m_diffPreFramePosition  < -ItemScale * 2 ) {

			m_diffPreFramePosition -= ItemScale;

			var item = m_itemList.First.Value;
			m_itemList.RemoveFirst();
			m_itemList.AddLast(item);

			var pos = ItemScale * m_instantateItemCount + ItemScale * m_currentItemNo;
			item.anchoredPosition = (direction == Direction.Vertical ) ? new Vector2 (0, -pos) : new Vector2 (pos, 0);

			onUpdateItem.Invoke (m_currentItemNo + m_instantateItemCount, item.gameObject);

			m_currentItemNo ++;

		}

		while (AnchoredPosition- m_diffPreFramePosition > 0 ) {

			m_diffPreFramePosition += ItemScale;

			var item = m_itemList.Last.Value;
			m_itemList.RemoveLast();
			m_itemList.AddFirst(item);

			m_currentItemNo --;

			var pos = ItemScale * m_currentItemNo;
			item.anchoredPosition = (direction == Direction.Vertical ) ? new Vector2 (0, -pos): new Vector2 (pos, 0);
			onUpdateItem.Invoke (m_currentItemNo, item.gameObject);
		}

	}

	[System.Serializable]
	public class OnItemPositionChange : UnityEngine.Events.UnityEvent<int, GameObject>{}
}
