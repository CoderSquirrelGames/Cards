using UnityEngine;
using System.Collections;

public class EndGameController : MonoBehaviour
{
	void Start ()
	{
		
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public void PlayAgain ()
	{
		Application.LoadLevel ("SceneMenu");
	}
}
