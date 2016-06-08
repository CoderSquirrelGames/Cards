using UnityEngine;
using System.Collections;

public class CardScript : MonoBehaviour
{
		
		public string Value;
		public int Key;
		public bool IsFaceUp = false;
		
		public Texture Face;
		public Texture Background;
		public Vector3 NewPosition;
		private float speed = 5f;
		public bool Animate = false;
		public bool IsAnimating {
				get {
						return (transform.position != NewPosition);
				}
		}
		
		private CardsController cardsScript;
		// Use this for initialization
		void Start ()
		{
				NewPosition = transform.position;
				ChangeTexture ();
		}
	
		// Update is called once per frame
		void Update ()
		{
		

				if (IsAnimating) {								
						transform.position = Vector3.MoveTowards (transform.position, NewPosition, speed * Time.deltaTime);
				}
				ChangeTexture ();	
				
	
		}

		void OnMouseDown ()
		{
				
				Picked ();
		}
		
		
		public void Picked ()
		{
			
				GameObject CardsObjs = GameObject.Find ("Cards");
				cardsScript = CardsObjs.GetComponent<CardsController> ();

				if (!IsAnimating && cardsScript.CardsUp < 2 && !IsFaceUp) {
						IsFaceUp = !IsFaceUp;
						ChangeTexture ();
						
				}
		}
		
		
		public void ChangeTexture ()
		{
				renderer.material.mainTexture = IsFaceUp ? Face : Background;
		}

		public void AnimateCard (float x)
		{
				
				//Animate = true;
				NewPosition = new Vector3 (x, 6, Camera.main.transform.position.z + 6f);
				
		}
		
		
		public void DestroyMe ()
		{
				Destroy (gameObject);
		}

	
		
}

