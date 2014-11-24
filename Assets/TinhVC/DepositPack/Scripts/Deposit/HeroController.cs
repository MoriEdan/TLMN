using UnityEngine;
using System.Collections;

public class HeroController : MonoBehaviour {
    public GameObject HeroContent;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Click()
    {
        Debug.Log(gameObject.GetComponent<SpriteRenderer>().sprite.name);
    }
}
