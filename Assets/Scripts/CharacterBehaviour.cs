using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;			//Needed for ???

public class CharacterBehaviour : MonoBehaviour {

	public Sprite unitPortrait;
	public string unitName;
	public int unitHPMax;
	public int unitHP;
	public int unitAP;
	public int speed;

	//Selecting the unit.
	void OnMouseUp ()
	{
		BattleManager daBoss = GameObject.FindWithTag ("GameManager").GetComponent<BattleManager> ();
		daBoss.SetCursor(this.transform.position.x,this.transform.position.y);
		daBoss.PathFinder(this.transform, (int) this.speed);
		GameObject.FindWithTag ("Portrait").GetComponent<PortraitBehaviour>().DisplayMe (this.gameObject.GetComponent<CharacterBehaviour>());
	}
}
