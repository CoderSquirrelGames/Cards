using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class FinalScoreScript : MonoBehaviour
{
	public Text txt;

	// Use this for initialization
	void Start ()
	{
		txt = GetComponent<Text> (); 				
		switch (PlayerPrefs.GetInt ("WIN")) {
		case 0:
			txt.text = "Player 2 win!";
			break;
		case 1:
			txt.text = "Player 1 win!";
			break;
		case 2:
			txt.text = "DRAW!";
			break;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
