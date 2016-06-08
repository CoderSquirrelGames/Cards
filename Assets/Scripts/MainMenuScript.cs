using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MainMenuScript : MonoBehaviour
{
		public GameObject BT_2, BT_3, BT_4, BT_EASY, BT_NORMAL, BT_EXTREME, BT_HARD, BT_COMPUTER, BT_PLAYER, BT_PLAYER_2, BT_LEVEL, BT_SINGLE;
		private bool BoardVisible = false, LevelVisible = false, PlayerVisible = false;
		private int BOARD = 2, LEVEL = 1;
		private string PLAYER = "COMPUTER";
		public TextScript BoardScript, LevelScript, PlayerScript ;
		// Use this for initialization

		void Awake ()
		{
				
		}
		
		void Start ()
		{
				BT_EASY.SetActive (LevelVisible);
				BT_NORMAL.SetActive (LevelVisible);
				BT_HARD.SetActive (LevelVisible);
				BT_EXTREME.SetActive (LevelVisible);
				BT_2.SetActive (BoardVisible);
				BT_3.SetActive (BoardVisible);
				BT_4.SetActive (BoardVisible);
				BT_PLAYER_2.SetActive (PlayerVisible);		
				BT_COMPUTER.SetActive (PlayerVisible);		
				BT_SINGLE.SetActive (PlayerVisible);
				
		}
	
		// Update is called once per frame
		void Update ()
		{
				BT_EASY.SetActive (LevelVisible);
				BT_NORMAL.SetActive (LevelVisible);
				BT_HARD.SetActive (LevelVisible);
				BT_EXTREME.SetActive (LevelVisible);
				BT_4.SetActive (BoardVisible);
				BT_3.SetActive (BoardVisible);
				BT_2.SetActive (BoardVisible);
				BT_PLAYER_2.SetActive (PlayerVisible);		
				BT_COMPUTER.SetActive (PlayerVisible);	
				BT_SINGLE.SetActive (PlayerVisible);	
				BoardScript.txt.text = BOARD != 6 ? BOARD + "x" + BOARD : "3x2";
				
				PlayerScript.txt.text = PLAYER;
				
				switch (LEVEL) {
				case 1:
						LevelScript.txt.text = "EASY";
						break;
				case 2:
						LevelScript.txt.text = "NORMAL";
						break;
				case 3:
						LevelScript.txt.text = "HARD";
						break;
				case 4:
						LevelScript.txt.text = "EXTREME";
						break;
			
				}
		}
	
		public void Play ()
		{
				PlayerPrefs.SetInt ("BOARD", BOARD);
				PlayerPrefs.SetInt ("LEVEL", LEVEL);
				PlayerPrefs.SetString ("PLAYER", PLAYER);
				Application.LoadLevel ("Scene01");
		}
		public void Board ()
		{
				BoardVisible = !BoardVisible;
				
		}
		public void Set4x4 ()
		{		
				BoardVisible = !BoardVisible;
				BOARD = 4;
		}
		
		public void Set2x2 ()
		{	
				BoardVisible = !BoardVisible;
				BOARD = 2;
		}
		public void Set3x2 ()
		{
				BoardVisible = !BoardVisible;
				BOARD = 6;
		
		}

		public void SetEasy ()
		{
				LevelVisible = !LevelVisible;
				LEVEL = 1;
		}
		public void SetNormal ()
		{
				LevelVisible = !LevelVisible;
				LEVEL = 2;
		}
		
		public void SetHard ()
		{
				LevelVisible = !LevelVisible;
				LEVEL = 3;
		}
		public void SetExtreme ()
		{
				LevelVisible = !LevelVisible;
				LEVEL = 4;
		}
		public void Level ()
		{
				LevelVisible = !LevelVisible;
		}
		
		public void SetPlayer2 ()
		{
				PLAYER = "PLAYER 2";
				PlayerVisible = !PlayerVisible;
				BT_LEVEL.GetComponent<Button> ().interactable = false;
		}
		public void SetComputer ()
		{
				PLAYER = "COMPUTER";
				PlayerVisible = !PlayerVisible;
				BT_LEVEL.GetComponent<Button> ().interactable = true;
		}
		public void SetSingle ()
		{
				PLAYER = "SINGLE";
				PlayerVisible = !PlayerVisible;
				BT_LEVEL.GetComponent<Button> ().interactable = false;
		}
		public void Player ()
		{
				PlayerVisible = !PlayerVisible;
		}
		
}
