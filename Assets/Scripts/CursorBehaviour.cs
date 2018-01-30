using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehaviour : MonoBehaviour {

	public bool beingUsed = false;
	public List<Transform> currentPath = new List<Transform>();

	public void FindPathHere (int headingToX, int headingToY)
	{
		currentPath.Clear ();
		BattleManager daBoss = GameObject.FindWithTag ("GameManager").GetComponent<BattleManager> ();
		Transform startTile = daBoss.nodeMap [headingToX, headingToY];
		Transform endTile;
		while (daBoss.pathprevious [startTile] != null) {
			endTile = daBoss.pathprevious [startTile];
			Vector3 startposition = new Vector3 (startTile.position.x, startTile.position.y, -1F);
			Vector3 endposition = new Vector3 (endTile.position.x, endTile.position.y, -1F);
			Debug.DrawLine (startposition, endposition, Color.cyan, 1.5f);
			currentPath.Insert (0, startTile);
			startTile = endTile;
		}
		currentPath.Insert (0, daBoss.pathprevious[startTile]);
	}

	void OnMouseUp ()
	{
		if(beingUsed == false)
		{
			Transform toMove = GameObject.FindWithTag ("Portrait").GetComponent <PortraitBehaviour>().selected.transform;
			float movingSpeed = 10f;
			StartCoroutine (MoveMeHere (toMove, movingSpeed));
		}
	}

	public IEnumerator MoveMeHere(Transform whatToMove, float howFast)
	{
		beingUsed = true;
		BattleManager daBoss = GameObject.FindWithTag ("GameManager").GetComponent<BattleManager> ();

		GameObject[] allSelectors = GameObject.FindGameObjectsWithTag("Selector");
		foreach (GameObject v in allSelectors) {
			Destroy (v);
		}

		daBoss.pathhighlight.Clear();
		for (int i = 1; i < currentPath.Count; i++)
		{
			float sqrRemainingDistance = (whatToMove.position - currentPath[i].position).sqrMagnitude;
			while (sqrRemainingDistance > float.Epsilon)
			{
				Vector3 newposition = Vector3.MoveTowards (whatToMove.position, currentPath [i].position, howFast * Time.deltaTime);
				whatToMove.position = newposition;
				sqrRemainingDistance = (whatToMove.position - currentPath[i].position).sqrMagnitude;
				yield return null;
			}
			whatToMove.Translate (0, 0, -1F);
			whatToMove.GetComponent<CharacterBehaviour> ().speed -= (int) currentPath [i].gameObject.GetComponent<TileBehaviour>().moveCost;
		}
		Destroy (this.gameObject);
		beingUsed = false;
	}
}
