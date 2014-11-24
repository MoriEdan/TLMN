using UnityEngine;
using System.Collections;

public class XenUpdateActive : MonoBehaviour {

    public GameObject tabscroll;
    public GameObject tabcontend;
    public TabController tabController;

    public tk2dTextMesh tvXenMoney;
    public tk2dTextMesh tvXenPrice;
    // Use this for initialization

    public static int indexListSMS = -1;

    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.name.Equals(tabController.tabName))
        {
            
            if (tabscroll.activeSelf == false && tabcontend.activeSelf == false)
            {
                tabcontend.SetActive(true);
                if (null != tvXenMoney)
                {
                    tvXenMoney.text = "Bạn chọn nạp " + Utilities.getVNCurrency(long.Parse(LamaControllib.getInstance().getListSMS()[indexListSMS - 1].Xen)) + " XEN bằng SMS. Chọn \"đồng ý\" và gửi tin nhắn mua XEN.";
                }
            }
            if (tabscroll.activeSelf == true && tabcontend.activeSelf == true)
            {
                tabscroll.SetActive(true);
                tabcontend.SetActive(false);
            }
        }
    }
    void SetUpdateActive()
    {

    }
}
