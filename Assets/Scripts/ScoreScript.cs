using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ScoreScript : MonoBehaviour
{
		private CardsController cardsScript;
		private Text Score;
		public int Which;
		// Use this for initialization
		void Start ()
		{
				GameObject CardsObjs = GameObject.Find ("Cards");
				cardsScript = CardsObjs.GetComponent<CardsController> ();
				
				Score = GetComponent<Text> (); 
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Which == 1)
						Score.text = "score " + cardsScript.Score;
				else 
						Score.text = "score " + cardsScript.ScoreP2;
		}
}
