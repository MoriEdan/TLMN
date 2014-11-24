using UnityEngine;
using System.Collections;

public class ButtonGenius : MonoBehaviour {

    public tk2dUIScrollbar vibrateScroll;
    public tk2dUIScrollbar musicScroll;
    public tk2dUIScrollbar soundScroll;
    public tk2dUIScrollbar chatScroll;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        UpdateScroll();
	}

    void VibrateOnOff()
    {
        if (vibrateScroll.Value == 0)
        {
            vibrateScroll.Value = 1f;
            vibrateScroll.GetComponentInChildren<tk2dSlicedSprite>().SetSprite("on");
        }
        else
        {
            vibrateScroll.Value = 0;
            vibrateScroll.GetComponentInChildren<tk2dSlicedSprite>().SetSprite("off");
        }
    }

    void SoundOnOff()
    {
        if (soundScroll.Value == 0)
        {
            soundScroll.Value = 1f;
            soundScroll.GetComponentInChildren<tk2dSlicedSprite>().SetSprite("on");
        }
        else
        {
            soundScroll.Value = 0;
            soundScroll.GetComponentInChildren<tk2dSlicedSprite>().SetSprite("off");
        }
    }
    void MusicOnOff()
    {
        if (musicScroll.Value == 0)
        {
            musicScroll.Value = 1f;
            musicScroll.GetComponentInChildren<tk2dSlicedSprite>().SetSprite("on");
        }
        else
        {
            musicScroll.Value = 0;
            musicScroll.GetComponentInChildren<tk2dSlicedSprite>().SetSprite("off");
        }
    }
    void ChatBoxOnOff()
    {
        if (chatScroll.Value == 0)
        {
            chatScroll.Value = 1f;
            chatScroll.GetComponentInChildren<tk2dSlicedSprite>().SetSprite("on");
        }
        else
        {
            chatScroll.Value = 0;
            chatScroll.GetComponentInChildren<tk2dSlicedSprite>().SetSprite("off");
        }
    }

    void UpdateScroll()
    {
        if (vibrateScroll.Value >= 0.5f)
        {
            vibrateScroll.Value = 1f;
            vibrateScroll.GetComponentInChildren<tk2dSlicedSprite>().SetSprite("on");
        }
        else
        {
            vibrateScroll.Value = 0;
            vibrateScroll.GetComponentInChildren<tk2dSlicedSprite>().SetSprite("off");
        }
        if (soundScroll.Value >= 0.5f)
        {
            soundScroll.Value = 1f;
            soundScroll.GetComponentInChildren<tk2dSlicedSprite>().SetSprite("on");
        }
        else
        {
            soundScroll.Value = 0;
            soundScroll.GetComponentInChildren<tk2dSlicedSprite>().SetSprite("off");
        }
        if (musicScroll.Value >= 0.5f)
        {
            musicScroll.Value = 1f;
            musicScroll.GetComponentInChildren<tk2dSlicedSprite>().SetSprite("on");
        }
        else
        {
            musicScroll.Value = 0;
            musicScroll.GetComponentInChildren<tk2dSlicedSprite>().SetSprite("off");
        }
        if (chatScroll.Value >= 0.5f)
        {
            chatScroll.Value = 1f;
            chatScroll.GetComponentInChildren<tk2dSlicedSprite>().SetSprite("on");
        }
        else
        {
            chatScroll.Value = 0;
            chatScroll.GetComponentInChildren<tk2dSlicedSprite>().SetSprite("off");
        }
    }
}
