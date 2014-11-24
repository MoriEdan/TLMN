using UnityEngine;
using System.Collections;

public class ChipUpdateActive : MonoBehaviour {

    public GameObject tabscroll;
    public GameObject tabcontend;
    public TabController tabController;

    public tk2dTextMesh tvChipMoney;
    public tk2dTextMesh tvChipPrice;

    public static int indexListSMS = -1;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.transform.name.Equals(tabController.tabName))
        {
            if (tabscroll.activeSelf == false && tabcontend.activeSelf == false)
            {
                tabcontend.SetActive(true);
                if (null != tvChipMoney)
                {
                    tvChipMoney.text = "Bạn chọn nạp " + Utilities.getVNCurrency(long.Parse(LamaControllib.getInstance().getListSMS()[indexListSMS - 1].Chip)) + " CHIP bằng SMS. Chọn \"đồng ý\" và gửi tin nhắn mua CHIP.";
                }
            }
            if (tabscroll.activeSelf == true && tabcontend.activeSelf == true)
            {
                tabscroll.SetActive(true);
                tabcontend.SetActive(false);
            }
        }
	}
}
