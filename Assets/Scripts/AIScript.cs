using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AIScript : MonoBehaviour
{

	public CardsController Game;
	private List<CardScript> TurnedCards;
	public bool IsPlaying = false, Picked = false;
	private int LEVEL = 1;
	private CardScript[] MatchCard = new CardScript[2];
	void Start ()
	{
		LEVEL = PlayerPrefs.GetInt ("LEVEL");
		TurnedCards = new List<CardScript> ();

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Game.TheresCardOnTable ()) {
			TurnedCards.RemoveAll (item => item == null);
			foreach (CardScript cs in Game.TurnedCards) {
				if (!TurnedCards.Contains (cs) && cs != null) {
					TurnedCards.Add (cs);
				}
			}
		}
	}
		
	public void ComputerPick ()
	{
		switch (LEVEL) {
		case 1:
			StartCoroutine (ComputerEasyMove ());
			break;
		case 2:
			StartCoroutine (ComputerNormalMove ());
			break;
		case 3:
			StartCoroutine (ComputerHardMove ());
			break;
		case 4:
			StartCoroutine (ComputerExtremeMove ());
			break;
			
		}
	
	}
	
	IEnumerator ComputerExtremeMove ()
	{
		CardScript card1, card2;
		IsPlaying = true;
		if (FindMatchCards ()) {
			card1 = MatchCard [0];
			card2 = MatchCard [1];
		} else {
			if (TurnedCards.Count > 1 && Random.Range (0, 100) > 50) {
				int index = Random.Range (0, TurnedCards.Count);
				card1 = TurnedCards [index];
				do {
					card2 = GetUnknown ();
				} while (card2.Equals(card1));
			} else {
				card1 = GetUnknown ();
				card2 = HasMatch (card1);
				if (card2 == null) {
					do {
						card2 = GetUnknown ();
					} while (card2.Equals(card1));
				}
								
			}
		}
		yield return new WaitForSeconds (1);
		card1.IsFaceUp = true;
		yield return new WaitForSeconds (1);
		card2.IsFaceUp = true;
		Game.SetCards (card1, card2);
		yield return new WaitForSeconds (1);
		IsPlaying = false;
		Game.CheckCardsValue ();
	}
	IEnumerator ComputerEasyMove ()
	{
		CardScript card1, card2;
		IsPlaying = true;
		card1 = GetUnknown ();
		card2 = GetUnknown ();
		do {
			card2 = GetUnknown ();
		} while (card2.Equals(card1));

		yield return new WaitForSeconds (1);
		card1.IsFaceUp = true;
		yield return new WaitForSeconds (1);
		card2.IsFaceUp = true;
		Game.SetCards (card1, card2);
		yield return new WaitForSeconds (1);
		IsPlaying = false;
		Game.CheckCardsValue ();

	}

	IEnumerator ComputerNormalMove ()
	{
		CardScript card1, card2 = null;
		IsPlaying = true;
		card1 = GetUnknown ();
				
		if (Random.Range (0, 100) > 50) {
			card2 = HasMatch (card1);
						
		}
		if (card2 == null) {
			do {
				card2 = GetUnknown ();
			} while (card2.Equals(card1));
		}
		yield return new WaitForSeconds (1);
		card1.IsFaceUp = true;
		yield return new WaitForSeconds (1);
		card2.IsFaceUp = true;
		Game.SetCards (card1, card2);
		yield return new WaitForSeconds (1);
		IsPlaying = false;
		Game.CheckCardsValue ();
	}
	IEnumerator ComputerHardMove ()
	{
		CardScript card1, card2 = null;
		IsPlaying = true;
		card1 = GetUnknown ();
		
		if (Random.Range (0, 100) > 30) {
			if (FindMatchCards ()) {
				card1 = MatchCard [0];
				card2 = MatchCard [1];
			} else {
				if (TurnedCards.Count > 1 && Random.Range (0, 100) > 50) {
					int index = Random.Range (0, TurnedCards.Count);
					card1 = TurnedCards [index];
					do {
						card2 = GetUnknown ();
					} while (card2.Equals(card1));
				} else {
					card1 = GetUnknown ();
					card2 = HasMatch (card1);
					if (card2 == null) {
						do {
							card2 = GetUnknown ();
						} while (card2.Equals(card1));
					}
					
				}
			}
		} else {
		
		
			if (Random.Range (0, 100) % 2 == 0) {
				card2 = HasMatch (card1);
			
			}
			if (card2 == null) {
				do {
					card2 = GetUnknown ();
				} while (card2.Equals(card1));
			}
						
		}
		yield return new WaitForSeconds (1);
		card1.IsFaceUp = true;
		yield return new WaitForSeconds (1);
		card2.IsFaceUp = true;
		Game.SetCards (card1, card2);
		yield return new WaitForSeconds (1);
		IsPlaying = false;
		Game.CheckCardsValue ();
	}
	CardScript GetUnknown ()
	{
		int cardIndex = Random.Range (0, Game.GetCards ().Count);
		GameObject n = Game.GetCards () [cardIndex];
		//Debug.Log (cardIndex + " " + Game.GetCards ().Count);
		return n.GetComponent<CardScript> ();
	}


	bool FindMatchCards ()
	{
		bool matched = false;
		foreach (CardScript c in TurnedCards) {
			if (matched)
				break;
			foreach (CardScript c2 in TurnedCards) {
				if (c.Value != c2.Value) {
					if (Game.CardsValues [c.Value].Equals (Game.CardsValues [c2.Value])) {
						MatchCard [0] = c;
						MatchCard [1] = c2;
						matched = true;
					}
				}
				if (matched)
					break;
			}
		}
		return matched;
	}
	
	
	CardScript HasMatch (CardScript card1)
	{
		foreach (CardScript c2 in TurnedCards) {
			if (!c2.Equals (card1)) {
				if (Game.CardsValues [card1.Value].Equals (Game.CardsValues [c2.Value])) {
					return c2;
				}
			}
		}
		return null;
	}
}
