using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;					//Needed for Layouts
using UnityEngine.EventSystems; 		//Needed for DragHandlers

public class DragBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	float xoffset = 0;
	float yoffset = 0;
	public Transform originalParent = null;

	GameObject placeholderCard = null;

    public void OnBeginDrag(PointerEventData eventData)
    {
		xoffset = eventData.position.x - this.transform.position.x;
        yoffset = eventData.position.y - this.transform.position.y;

        placeholderCard = new GameObject();
        placeholderCard.transform.SetParent(this.transform.parent);
        LayoutElement handspacer = placeholderCard.AddComponent<LayoutElement>();
		handspacer.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        handspacer.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
		handspacer.flexibleHeight = 0;
        handspacer.flexibleWidth = 0;
		placeholderCard.transform.GetComponent<RectTransform> ().sizeDelta = new Vector2 (	this.GetComponent<LayoutElement>().preferredWidth,
																							this.GetComponent<LayoutElement>().preferredHeight);
        placeholderCard.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
        
        originalParent = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
		this.transform.position = new Vector3(eventData.position.x - xoffset,eventData.position.y - yoffset,0);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
		this.transform.SetParent(originalParent);
        this.transform.SetSiblingIndex(placeholderCard.transform.GetSiblingIndex());
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(placeholderCard);
    }
}
