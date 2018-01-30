using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectorBehaviour : MonoBehaviour {

	void OnMouseUp ()
	{
		BattleManager daBoss = GameObject.FindWithTag ("GameManager").GetComponent<BattleManager>();
		daBoss.SetCursor (this.transform.position.x, this.transform.position.y);
		GameObject.FindWithTag ("Cursor").GetComponent<CursorBehaviour> ().FindPathHere ((int) this.transform.position.x, (int) this.transform.position.y);
	}
}
