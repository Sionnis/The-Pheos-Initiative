using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayBehaviour : MonoBehaviour {

	private PortraitBehaviour findPortrait;
	public GameObject selectorObj;

	void Awake ()
	{
		findPortrait = GameObject.FindWithTag ("Portrait").GetComponent<PortraitBehaviour> ();
	}

	public void StartPlayingCards ()
	{
		//BattleManager daBoss = GameObject.FindWithTag ("GameManager").GetComponent<BattleManager>();
		findPortrait.cardsActive = true;
		Destroy (GameObject.FindWithTag ("Cursor"));
		GameObject[] allSelectors = GameObject.FindGameObjectsWithTag("Selector");
		foreach (GameObject v in allSelectors)
		{
			Destroy (v);
		}
		GameObject[] allCharacters = GameObject.FindGameObjectsWithTag("Character");
		foreach (GameObject v in allCharacters)
		{
			GameObject makeselectorgonow = Instantiate (selectorObj, v.transform.position, Quaternion.identity);
			makeselectorgonow.GetComponent<SelectorBehaviour> ().selectorState = "Choose";
			makeselectorgonow.transform.SetParent (this.transform);
		}
	}
}
