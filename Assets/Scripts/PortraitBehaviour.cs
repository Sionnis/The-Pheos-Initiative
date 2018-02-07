using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;				//Needed for UI sprites, the "Image" component, and the "Text" component.

public class PortraitBehaviour : MonoBehaviour {

	public CharacterBehaviour selected;
	private Transform whichPortrait;
	public bool cardsActive = false;

	public void DisplayMe (CharacterBehaviour character)
	{
		selected = character;

		if (cardsActive == false)
			whichPortrait = this.transform;
		else
			whichPortrait = this.transform.Find ("TargetPortrait");

		whichPortrait.Find("Picture").GetComponent<Image> ().sprite = character.unitPortrait;
		whichPortrait.Find("Name").GetComponent<Text> ().text = character.unitName;
		whichPortrait.Find("Hitpoints").GetComponent<Text> ().text = ("HP: " + character.unitHP + " / " + character.unitHPMax);
		whichPortrait.Find("Actionpoints").GetComponent<Text> ().text = ("AP: " + character.unitAP + " / 3");
	}
}
