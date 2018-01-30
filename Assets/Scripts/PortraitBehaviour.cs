using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;				//Needed for UI sprites, the "Image" component, and the "Text" component.

public class PortraitBehaviour : MonoBehaviour {

	public CharacterBehaviour selected;

	public void DisplayMe (CharacterBehaviour character)
	{
		selected = character;
		this.transform.Find("Picture").GetComponent<Image> ().sprite = character.unitPortrait;
		this.transform.Find("Name").GetComponent<Text> ().text = character.unitName;
		this.transform.Find("Hitpoints").GetComponent<Text> ().text = ("HP: " + character.unitHP + " / " + character.unitHPMax);
		this.transform.Find("Actionpoints").GetComponent<Text> ().text = ("AP: " + character.unitAP + " / 3");
	}

}
