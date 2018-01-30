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
		GameObject.FindWithTag ("GameManager").GetComponent<BattleManager>().SetCursor(this.transform.position.x,this.transform.position.y);
		GameObject.FindWithTag ("GameManager").GetComponent<BattleManager>().PathFinder(this.transform, (int) this.speed);
		GameObject.FindWithTag ("Portrait").GetComponent<PortraitBehaviour>().DisplayMe (this.gameObject.GetComponent<CharacterBehaviour>());
	}
}
