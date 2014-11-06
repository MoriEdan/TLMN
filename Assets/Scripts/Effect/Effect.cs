using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour {

    public ParticleSystem heart;
    public ParticleSystem diamond;
    public ParticleSystem club;
    public ParticleSystem spade;

    private float timeDisplay;

	// Use this for initialization
	void Start () {
        timeDisplay = 10.0f;
        ParticleSystem heartLeft = Instantiate(heart, gameObject.transform.position, Quaternion.identity) as ParticleSystem;
        heartLeft.transform.parent = transform;
        heartLeft.transform.Rotate(new Vector3(0, 90, 0));
        ParticleSystem heartRight = Instantiate(heart, gameObject.transform.position, Quaternion.identity) as ParticleSystem;
        heartRight.transform.parent = transform;
        heartRight.transform.Rotate(new Vector3(0, -90, 0));
        ParticleSystem diamondLeft = Instantiate(diamond, gameObject.transform.position, Quaternion.identity) as ParticleSystem;
        diamondLeft.transform.parent = transform;
        diamondLeft.transform.Rotate(new Vector3(0, 90, 0));
        ParticleSystem diamondRight = Instantiate(diamond, gameObject.transform.position, Quaternion.identity) as ParticleSystem;
        diamondRight.transform.parent = transform;
        diamondRight.transform.Rotate(new Vector3(0, -90, 0));
        ParticleSystem clubLeft = Instantiate(club, gameObject.transform.position, Quaternion.identity) as ParticleSystem;
        clubLeft.transform.parent = transform;
        clubLeft.transform.Rotate(new Vector3(0, 90, 0));
        ParticleSystem clubRight = Instantiate(club, gameObject.transform.position, Quaternion.identity) as ParticleSystem;
        clubRight.transform.parent = transform;
        clubRight.transform.Rotate(new Vector3(0, -90, 0));
        ParticleSystem spadeLeft = Instantiate(spade, gameObject.transform.position, Quaternion.identity) as ParticleSystem;
        spadeLeft.transform.parent = transform;
        spadeLeft.transform.Rotate(new Vector3(0, 90, 0));
        ParticleSystem spadeRight = Instantiate(spade, gameObject.transform.position, Quaternion.identity) as ParticleSystem;
        spadeRight.transform.parent = transform;
        spadeRight.transform.Rotate(new Vector3(0, -90, 0));
	}
	
	// Update is called once per frame
	void Update () {
        if(timeDisplay < 0.0f)
        {
            gameObject.SetActive(false);
            
        }
        else
        {
            timeDisplay -= Time.deltaTime;
        }
	
	}

    public float TimeDisplay
    {
        get { return timeDisplay; }
        set { timeDisplay = value; }
    }
}
