using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectorBehaviour : MonoBehaviour {

	public string selectorState;

	void OnMouseUp ()
	{
		if (selectorState == "Move")
			this.MoveHere ();
		if (selectorState == "Choose")
			this.ChooseHere ();
		if (selectorState == "Target")
			this.TargetHere ();
		
	}

	void MoveHere ()
	{
		BattleManager daBoss = GameObject.FindWithTag ("GameManager").GetComponent<BattleManager>();
		daBoss.SetCursor (this.transform.position.x, this.transform.position.y);
		GameObject.FindWithTag ("Cursor").GetComponent<CursorBehaviour> ().FindPathHere ((int) this.transform.position.x, (int) this.transform.position.y);
	}

	void ChooseHere ()
	{
		Vector2 positioncheck = new Vector2 (this.transform.position.x,this.transform.position.y);
		this.GetComponent<BoxCollider2D> ().enabled = false;
		RaycastHit2D hit = Physics2D.Raycast(positioncheck,Vector2.zero); //Layermask 11 is Character
		CharacterBehaviour character = hit.collider.gameObject.GetComponent<CharacterBehaviour>();
		this.GetComponent<BoxCollider2D> ().enabled = true;
		BattleManager daBoss = GameObject.FindWithTag ("GameManager").GetComponent<BattleManager> ();
		daBoss.SetCursor(this.transform.position.x,this.transform.position.y);
		PortraitBehaviour displayPortrait =	GameObject.FindWithTag ("Portrait").GetComponent<PortraitBehaviour> ();
		displayPortrait.cardsActive = false;
		displayPortrait.DisplayMe (character);
		displayPortrait.cardsActive = true;
		GameObject[] selectorsToDestroy = GameObject.FindGameObjectsWithTag("Selector");
		foreach (GameObject v in selectorsToDestroy)
		{
			Destroy (v);
		}
	}

	void TargetHere ()
	{
		Debug.Log ("I am targetting this unit");
	}

}
