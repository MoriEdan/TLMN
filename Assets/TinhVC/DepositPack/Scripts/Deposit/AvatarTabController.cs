using UnityEngine;
using System.Collections;

public class AvatarTabController : MonoBehaviour {
    public tk2dTextMesh HeroTextMesh, GirlTextMesh;
    public GameObject HeroBtn, GirlBtn;
    public GameObject HeroContent, GirlContent;


    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void HeroClick()
    {
        HeroTextMesh.color = new Color32(133, 17, 17, 255);
        GirlTextMesh.color = new Color(255, 255, 255);

        HeroBtn.SetActive(true);
        GirlBtn.SetActive(false);

        HeroContent.SetActive(true);
        GirlContent.SetActive(false);
    }

    void GirlClick()
    {
        GirlTextMesh.color = new Color32(133, 17, 17, 255);
        HeroTextMesh.color = new Color(255, 255, 255);

        GirlBtn.SetActive(true);
        HeroBtn.SetActive(false);

        HeroContent.SetActive(false);
        GirlContent.SetActive(true);
    }
}
