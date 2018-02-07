using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;							//Needed for Dictionaries.
using Random = UnityEngine.Random;			//Because Random appears in both System and UnityEngine.


public class BattleManager : MonoBehaviour {
    [Serializable]
    public class GenNum
    {
        public int minimum;
        public int maximum;

        public GenNum(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

	public GameObject cursorObj;
	public GameObject selectorObj;

	public int mapsizeX = 20;
    public int mapsizeY = 20;
	public GenNum roomGenNum = new GenNum(1, 1);
	public GenNum obstructionGenNum = new GenNum(8, 8);
    
    public GameObject[] floorTiles;
    public GameObject[] obstructionTiles;
    public GameObject[] enemyTiles;
    public GameObject[] boundaryTiles;

    private Transform boardHolder;
	public Transform[,] nodeMap;

    private List<Vector3> gridPositions = new List<Vector3>();

	private Dictionary<Transform, float> pathdistance = new Dictionary<Transform, float>();
	public Dictionary<Transform, Transform> pathprevious = new Dictionary<Transform, Transform>();
	public List<Transform> pathhighlight = new List<Transform>();

    //Sets up a list of potential grid positions with which to place random rooms/obstructions/etc.
    void InitializeList()
    {
        gridPositions.Clear();

        for (int x = 0; x < mapsizeX; x++)
        {
            for (int y = 0; y < mapsizeY; y++)
            {
                gridPositions.Add(new Vector3(x, y, 1f));
            }
        }
    }


    //This generates our initial map for us and starts placing the terrain based on the tileset.
	//It also sets up our Pathfinding map "nodeMap".
    void LayoutMap ()
    {
        boardHolder = new GameObject("Board").transform;
		nodeMap = new Transform[mapsizeX,mapsizeY];

        for (int x = -1; x < mapsizeX + 1; x++)
        {
            for (int y = -1; y < mapsizeY + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

				if (x == -1 || x == mapsizeX || y == -1 || y == mapsizeY)
				{
					toInstantiate = boundaryTiles [Random.Range (0, boundaryTiles.Length)];
					GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 1f), Quaternion.identity) as GameObject;
					instance.transform.SetParent (boardHolder);
				}
				else
				{
					GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 1f), Quaternion.identity) as GameObject;

					nodeMap [x, y] = instance.transform;
					instance.transform.SetParent (boardHolder);
					instance.transform.position = new Vector2 (x, y);

				}
            }
        }

	}


	//This randomly chooses a location (vector 3) of a random floor space on our board, and removes it from future searches.
    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }


	//This takes an array of tiles as an input and places a random number of them between a given minimum and maximum.
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        boardHolder = new GameObject("Obstructions").transform;
		int objectGenNum = Random.Range(minimum, maximum + 1);

		for (int i = 0; i < objectGenNum; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            GameObject tiled = Instantiate(tileChoice, randomPosition, Quaternion.identity);
			int tempX = (int)tiled.transform.position.x;
			int tempY = (int)tiled.transform.position.y;
			nodeMap[tempX,tempY] = tiled.transform;
			tiled.transform.position = new Vector2 (randomPosition.x, randomPosition.y);
            tiled.transform.SetParent(boardHolder);
        }
    }

	void SetupPathFinding()
	{
		for (int x = 0; x < mapsizeX; x++)
		{
			for (int y = 0; y < mapsizeY; y++)
			{
				if (x > 0)
					nodeMap[x, y].GetComponent<TileBehaviour>().neighbours.Add(nodeMap[x - 1, y]);
				if (x < mapsizeX - 1)
					nodeMap[x, y].GetComponent<TileBehaviour>().neighbours.Add(nodeMap[x + 1, y]);
				if (y > 0)
					nodeMap[x, y].GetComponent<TileBehaviour>().neighbours.Add(nodeMap[x, y - 1]);
				if (y < mapsizeY - 1)
					nodeMap[x, y].GetComponent<TileBehaviour>().neighbours.Add(nodeMap[x, y + 1]);
			}
		}	
	}


	//Our pathfinding algorithm.
	public void PathFinder (Transform character, int moveSpeed)
	{
		GameObject[] allSelectors = GameObject.FindGameObjectsWithTag("Selector");

		foreach (GameObject v in allSelectors) {
			Destroy (v);
		}

		pathhighlight.Clear ();
		pathdistance.Clear ();
		pathprevious.Clear ();

		if (moveSpeed > 0) {
			Transform source = nodeMap [(int)character.position.x, (int)character.position.y];

			pathdistance [source] = 0;
			pathprevious [source] = null;
			List<Transform> pathUnvisited = new List<Transform> ();

			foreach (Transform v in nodeMap) {
				if (v != source) {
					pathdistance [v] = Mathf.Infinity;
					pathprevious [v] = null;
				}
				pathUnvisited.Add (v);
			}

			//Now, all out tiles are in the list "pathUnvisited".
			while (pathUnvisited.Count > 0) {
				Transform pathMin = pathdistance.Aggregate ((l, r) => l.Value < r.Value ? l : r).Key;
				pathUnvisited.Remove (pathMin);
				if (pathdistance [pathMin] > moveSpeed)
					break;
				int girth = pathMin.gameObject.GetComponent<TileBehaviour> ().neighbours.Count;
				for (int i = 0; i < girth; i++) {
					Transform pathNeighbour = pathMin.GetComponent<TileBehaviour> ().neighbours [i];
					Vector2 positioncheck = new Vector2 (pathNeighbour.position.x,pathNeighbour.position.y);
					RaycastHit2D hit = Physics2D.Raycast(positioncheck,Vector2.zero);
					if (pathdistance.ContainsKey (pathNeighbour) == true & hit.collider == null) {
						float pathAlt = pathdistance [pathMin] + pathNeighbour.GetComponent<TileBehaviour> ().moveCost;
						if (pathAlt < pathdistance [pathNeighbour]) {
							pathdistance [pathNeighbour] = pathAlt;
							pathprevious [pathNeighbour] = pathMin;
						}
					}
				}
				pathdistance.Remove (pathMin);
				pathhighlight.Add (pathMin);

			}
			foreach (Transform v in pathhighlight) {
				Vector2 positioncheck = new Vector2 (v.position.x,v.position.y);
				RaycastHit2D hit = Physics2D.Raycast(positioncheck,Vector2.zero);
				if (hit.collider == null)
				{
					GameObject placeSelector = Instantiate (selectorObj, new Vector3 (v.transform.position.x, v.transform.position.y, -1f), Quaternion.identity);
					placeSelector.GetComponent<SelectorBehaviour> ().selectorState = "Move";
					placeSelector.transform.SetParent (this.transform);
				}
			}
		}
	}

	public void SetCursor (float xVal, float yVal)
	{
		if (GameObject.FindWithTag ("Cursor") == null)
		{
			GameObject makecursorgonow = Instantiate (cursorObj, new Vector3 (xVal, yVal, -1f), Quaternion.identity);
			makecursorgonow.transform.SetParent (this.transform);
		} else
		{
			GameObject.FindWithTag ("Cursor").transform.position = new Vector3 (xVal, yVal, -1f);
		}
	}

	//This tells the code to generate a map when the Battlemap Prefab is created.
    void Awake()
    {
        InitializeList();
        LayoutMap();
		LayoutObjectAtRandom(obstructionTiles, obstructionGenNum.minimum, obstructionGenNum.maximum);
		SetupPathFinding();
    }
}