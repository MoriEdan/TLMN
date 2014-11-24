using UnityEngine;
using System.Collections;

public class ButtonUpDownSlider : MonoBehaviour {

    public ScrollProgressBar scrollProgress;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void ClickUp()
    {
        scrollProgress.scrollBar.Value += 0.1f;
    }

    void ClickDown()
    {
        scrollProgress.scrollBar.Value -= 0.1f;
    }
}
