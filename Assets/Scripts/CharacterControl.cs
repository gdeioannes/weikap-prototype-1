using UnityEngine;
using System.Collections;

public class CharacterControl : MonoBehaviour {

	public GameObject characterSprite;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKey(KeyCode.RightArrow)){
			characterSprite.GetComponent<Rigidbody2D>().AddForce(new Vector2(5,0));
		}

		if(Input.GetKey(KeyCode.LeftArrow)){
			characterSprite.GetComponent<Rigidbody2D>().AddForce(new Vector2(-5,0));
		}

		if(Input.GetKey(KeyCode.UpArrow)){
			characterSprite.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,5));
		}
	}
}
