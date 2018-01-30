using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;			//Needed for DropHandlers

public class DropBehaviour : MonoBehaviour, IDropHandler {

	public int maxHandSize = 7;

	public void OnDrop(PointerEventData eventData)
    {
		DragBehaviour d = eventData.pointerDrag.GetComponent<DragBehaviour>();
		if(d != null & this.transform.childCount < maxHandSize){
            d.originalParent = this.transform;
        }
		if(d != null & this.transform.childCount == maxHandSize & this.transform != d.originalParent){
			this.transform.GetChild(this.maxHandSize-1).SetParent(d.originalParent);
			d.originalParent = this.transform;
		}
    }
}
