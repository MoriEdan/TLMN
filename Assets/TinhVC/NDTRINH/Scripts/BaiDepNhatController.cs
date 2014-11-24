using UnityEngine;
using System.Collections;

public class BaiDepNhatController : MonoBehaviour {
    bool isDrag, isClick;
    float dragTime;
    public Camera cmr;
    private Vector3 screenPoint;
    private Vector3 offset;
	// Use this for initialization
	void Start () {
        isDrag = false;
        dragTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnMouseDrag()
    {
        if (dragTime < 0f)
        {
            isDrag = true;
            //Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            
            Vector3 curScreenPoint = new Vector3(0, Input.mousePosition.y, 0);
            Vector3 curPosition = new Vector3(0, cmr.ScreenToWorldPoint(curScreenPoint).y,0) + offset;
                
            //curPosition = new Vector3(curPosition.x, curPosition.y, curPosition.z);
            //curPosition = new Vector3(0, curPosition.y, 0);
            transform.position = curPosition;
            
        }
        else
        {
            dragTime -= Time.deltaTime;
            Debug.Log(dragTime);
        }
    }
    void OnMouseDown()
    {
        //gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        isClick = true;
        screenPoint = cmr.WorldToScreenPoint(gameObject.transform.position);
        //offset = gameObject.transform.position - cmr.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        offset = gameObject.transform.position - cmr.ScreenToWorldPoint(new Vector3(0, Input.mousePosition.y, 0));
    }
}
