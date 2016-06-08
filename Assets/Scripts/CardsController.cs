using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardsController : MonoBehaviour
{
	private enum Turn
	{
		COMPUTER,
		PLAYER,
		PLAYER_2, 
		SINGLE
	}
		


	public GameObject Card;
	public Texture[] CardFaces;
	public int Score = 0, ScoreP2 = 0;
	/// <summary>
	/// The index of the key. Created to be used in random chose of the cards as well as the store the key tags instead of using string each time i will use.
	/// </summary>
	private Dictionary<int, string> KeyIndex = new Dictionary<int, string> ();
	/// <summary>
	/// The key and flag. It stores if the card has alread been chose, to don't appear duplicate cards on the table.
	/// </summary>
	private Dictionary<string, bool> KeyAndFlag = new Dictionary<string, bool> ();
	/// <summary>
	/// The cards values. Store the value of the cards only without the suits
	/// </summary>
	public Dictionary<string, string> CardsValues = new Dictionary<string, string> ();

	private List<GameObject> Cards = new List<GameObject> ();
	private CardScript C1, C2;
	private int COLUMNS, ROWS, LIMIT;
	private Turn Player;
	private GameObject[,] CardsGrid;
	private bool OnCoroutine = false;
	public List<CardScript> TurnedCards;
	public TextScript  Status, PlayerText;
	public bool CheckAnimation = false;
	public int MAX = 3, CardsUp = 0;
	public AIScript Computer;
	private string PLAYER;
	private List<CardScript>  KnownCards = new List<CardScript> ();
	
	void Awake ()
	{
		
		PLAYER = PlayerPrefs.GetString ("PLAYER");
		Player = PLAYER.Equals ("SINGLE") ? Turn.SINGLE : Turn.PLAYER;
			
				
		MAX = PlayerPrefs.GetInt ("BOARD");
		LIMIT = MAX == 6 ? MAX : MAX * MAX;
		ROWS = MAX == 6 ? 2 : MAX;
		COLUMNS = MAX == 6 ? 3 : MAX;
		CardsGrid = new GameObject[COLUMNS, ROWS];
		LoadDictionary ();
		TurnedCards = new List<CardScript> ();
		CreateCardsGrid ();
				
	}
	void Start ()
	{
		// PlayerText.txt.text = "PLAYER 1";
	}
	void Update ()
	{
		Cards.RemoveAll (item => item == null);
		KnownCards.RemoveAll (item => item == null);
		if (!OnCoroutine) {
			if (CheckAnimation) {
				if (!C1.IsAnimating && !C2.IsAnimating) {
					StartCoroutine (RemovingCards ());
				}
			} else {
								
				if (Player.Equals (Turn.PLAYER)) {
										
					PlayerText.txt.text = "PLAYER 1";
					if (Input.GetMouseButtonDown (0)) {
						Game ();

					}
				} else if (Player.Equals (Turn.COMPUTER)) {
										
					if (!Computer.IsPlaying) {
						PlayerText.txt.text = "COMPUTER";
						Computer.ComputerPick ();    
					}
				} else if (Player.Equals (Turn.PLAYER_2)) {
										
					PlayerText.txt.text = "PLAYER 2";
					if (Input.GetMouseButtonDown (0)) {
						Game ();
					
					}
				} else {
					if (Input.GetMouseButtonDown (0)) {
						Game ();
						
					}
				}
						
				if (!TheresCardOnTable ()) {
					int win;
						
					
					if (Score == ScoreP2) {
						win = 2;
					} else {
						win = Score > ScoreP2 ? 1 : 0;
					}
									
					PlayerPrefs.SetInt ("WIN", win);
					Application.LoadLevel ("SceneEndGame");
				}
			} 
		}

	}
  
	public void ChangePlayer ()
	{
			
		if (TheresCardOnTable ()) {
			C1 = C2 = null;
			if (PLAYER.Equals ("COMPUTER"))
				Player = Player.Equals (Turn.COMPUTER) ? Turn.PLAYER : Turn.COMPUTER;
			else if (PLAYER.Equals ("PLAYER 2"))
				Player = Player.Equals (Turn.PLAYER_2) ? Turn.PLAYER : Turn.PLAYER_2;

		}
	}


	void Game ()
	{
		Cards.RemoveAll (item => item == null);
		//print ("GAME " + Player);
		CardsUp = 0;
		TurnedCards.Clear ();
		foreach (GameObject c in Cards) {
			CardScript cardScript = c.GetComponent<CardScript> ();
			if (cardScript.IsFaceUp) {
				if (!KnownCards.Contains (cardScript)) {
					KnownCards.Add (cardScript);
				}
				TurnedCards.Add (cardScript);
				CardsUp++;
			}
		}
		if (CardsUp == 2) {
			CheckCardsValue ();

		}
	}


	void CleaningTexts ()
	{
		Status.txt.text = "";

	}

	//Check Methods
	public void CheckCardsValue ()
	{

		if (C1 == null && C2 == null) {
			C1 = (CardScript)TurnedCards [0];
			C2 = (CardScript)TurnedCards [1];
		}
		string c1v = CardsValues [C1.Value];
		string c2v = CardsValues [C2.Value];
		if (c1v.Equals (c2v)) {
						
			if (Player.Equals (Turn.PLAYER) || Player.Equals (Turn.SINGLE)) {
				Score++;
			} else {
				ScoreP2++;
			}
			Status.txt.text = "Matched!";
			SetMatchCards ();
		} else if (Player.Equals (Turn.SINGLE) && CouldMatch ()) {
			Status.txt.text = "Could match.";
			Score--;
			StartCoroutine (TurningCards ());
		} else {
			Status.txt.text = "Dont match.";
			StartCoroutine (TurningCards ());
		}
	}

	public bool TheresCardOnTable ()
	{
		return (Cards.Count > 0);
	}

	//Sets
	public void SetCards (CardScript c1, CardScript c2)
	{
		C1 = c1;
		C2 = c2;
	}

	void SettingValue (CardScript newCard)
	{
			
		int index = Random.Range (0, LIMIT);
		string key = KeyIndex [index];

		while (KeyAndFlag[key]) {
			index = Random.Range (0, LIMIT);
			key = KeyIndex [index];
		}

		newCard.Value = key;
		newCard.Key = index;
		newCard.Face = CardFaces [index];
		KeyAndFlag [key] = true;


	}

	void SetMatchCards ()
	{
		C1.AnimateCard (-2f);
		C2.AnimateCard (2f);
		CheckAnimation = true;
	}


	public List<GameObject> GetCards ()
	{
		return Cards;
	}
	//IEnumerators

	IEnumerator MoveObject (Transform thisTransform, Vector3 target, float overTime)
	{
	
		while (thisTransform.position != target) {
			thisTransform.position = Vector3.Lerp (thisTransform.position, target, overTime * Time.deltaTime);
			yield return null;
		}
		thisTransform.position = target;
		Debug.Log ("MoveObject");	
	}
	IEnumerator TurningCards ()
	{
				
		OnCoroutine = true;
		yield return new WaitForSeconds (1);
		C1.IsFaceUp = C2.IsFaceUp = false;
		C1.ChangeTexture ();
		C2.ChangeTexture ();
		yield return new WaitForSeconds (1);
		CleaningTexts ();	
		OnCoroutine = false;
		ChangePlayer ();
				
				
	}
	IEnumerator RemovingCards ()
	{
		OnCoroutine = true;
			
		yield return new WaitForSeconds (1);
		C1.DestroyMe ();
		C2.DestroyMe ();
				
				
		yield return new WaitForSeconds (1);
		CleaningTexts ();
		ChangePlayer ();
		CheckAnimation = false;
		OnCoroutine = false;
	}



	//Initialization Methods
	/// <summary>
	/// Load the values of CardsValues, KeyAndFlag and KeyIndex
	/// </summary>
	void LoadDictionary ()
	{

		KeyIndex.Add (0, "A of Spades");
		KeyIndex.Add (1, "A of Hearts");
		KeyIndex.Add (2, "2 of Spades");
		KeyIndex.Add (3, "2 of Hearts");
		KeyIndex.Add (4, "3 of Spades");
		KeyIndex.Add (5, "3 of Hearts");
		KeyIndex.Add (6, "4 of Spades");
		KeyIndex.Add (7, "4 of Hearts");
		KeyIndex.Add (8, "5 of Spades");
		KeyIndex.Add (9, "5 of Hearts");
		KeyIndex.Add (10, "6 of Spades");
		KeyIndex.Add (11, "6 of Hearts");
		KeyIndex.Add (12, "7 of Spades");
		KeyIndex.Add (13, "7 of Hearts");
		KeyIndex.Add (14, "8 of Spades");
		KeyIndex.Add (15, "8 of Hearts");

		for (int i = 0; i < KeyIndex.Count; i++) {
			KeyAndFlag.Add (KeyIndex [i], false);
		}

		CardsValues.Add (KeyIndex [0], "A");
		CardsValues.Add (KeyIndex [1], "A");
		CardsValues.Add (KeyIndex [2], "2");
		CardsValues.Add (KeyIndex [3], "2");
		CardsValues.Add (KeyIndex [4], "3");
		CardsValues.Add (KeyIndex [5], "3");
		CardsValues.Add (KeyIndex [6], "4");
		CardsValues.Add (KeyIndex [7], "4");
		CardsValues.Add (KeyIndex [8], "5");
		CardsValues.Add (KeyIndex [9], "5");
		CardsValues.Add (KeyIndex [10], "6");
		CardsValues.Add (KeyIndex [11], "6");
		CardsValues.Add (KeyIndex [12], "7");
		CardsValues.Add (KeyIndex [13], "7");
		CardsValues.Add (KeyIndex [14], "8");
		CardsValues.Add (KeyIndex [15], "8");

	}
	void CreateCardsGrid ()
	{
		Vector3 position;
		
		for (int x = 0; x < COLUMNS; x++) {

			for (int y = 0; y < ROWS; y++) {

				position = new Vector3 (transform.position.x + x * 3,
				                        transform.position.y + y * 4,
                        transform.position.z);

				GameObject newObj = (GameObject)Instantiate (Card, position, transform.rotation);
				CardScript cardScript = newObj.GetComponent<CardScript> ();
                
				SettingValue (cardScript);
				Cards.Add (newObj);
							
				CardsGrid [x, y] = newObj;
			}
		}

		/*		
		float xA = -17f;
		float yA = -1.7f;
		float zA = 2.28f;
		for (int x = 0; x < COLUMNS; x++) {
			for (int y = 0; y < ROWS; y++) {
				position = new Vector3 (xA, yA, zA);
				//GameObject newObj = (GameObject)Instantiate (Card, position, transform.rotation);
				GameObject newObj = (GameObject)Instantiate (Card, position, transform.rotation);
				CardScript cardScript = newObj.GetComponent<CardScript> ();
				Cards.Add (newObj);
				SettingValue (cardScript);
				zA = zA - 0.08f;    
				CardsGrid [x, y] = newObj;
			}		
		}
				
		for (int x = 0; x < COLUMNS; x++) {
			
			for (int y = 0; y < ROWS; y++) {
				Vector3 finalPosition = new Vector3 (transform.position.x + x * 3,
				                                    transform.position.y + y * 5,
				                                    transform.position.z);
				//CardsGrid [x, y].transform.position = Vector3.Lerp (CardsGrid [x, y].transform.position, finalPosition, (3.0f * Time.deltaTime));
				StartCoroutine (MoveObject (CardsGrid [x, y].transform, finalPosition, 5.0f));
			}		
		}
//*/
	}
		
		
	bool CouldMatch ()
	{
		foreach (CardScript c2 in KnownCards) {
			if (!c2.Equals (C1)) {
				if (CardsValues [C1.Value].Equals (CardsValues [c2.Value])) {
					return true;
				}
			}
		}
		return false;
	}
      

 
}
