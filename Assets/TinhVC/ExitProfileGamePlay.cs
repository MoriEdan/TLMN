using UnityEngine;
using System.Collections;

public class ExitProfileGamePlay : MonoBehaviour {

    public GameObject btnGift;
    public GameObject btnChangeAvatar;
    public GameObject btnAddFriend;
    public GameObject btnReport;
    public GameObject btnUpLevel;
    public GameObject btnCloseProfile;

    public GameObject tvMoneyChip;
    public GameObject tvMoneyXen;

    public GameObject tvLevel;
    public GameObject tvPersentLevel;
    public tk2dUIScrollbar progressBarLevel;
    public GameObject tvUsername;
    public GameObject avatarProfile;
    public GameObject tvHadPlay;
    public GameObject tvHadWin;
    public GameObject tvMostWin;

    string idUser = "";
	// Use this for initialization
	void Start () {
        idUser = TLMNButtonController.idUser;
        LamaControllib.getInstance().getAllMoneyUserService(idUser, getMoneyUserCallback, this);
        LamaControllib.getInstance().getXPModelService(idUser, getXPUserCallback, this);
        LamaControllib.getInstance().getAvatarPartnerService(idUser, getAvatarCallback, this);
        if (idUser == LamaControllib.getInstance().getUserModel().IdUser)
        {
            Debug.Log("My Profile");
            showMyProfile(true);
        }
        else
        {
            Debug.Log("Public Profile");
            showMyProfile(false);
        }
       
        boxMessNotify.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
    }

    void getAvatarCallback(bool isSuccess, JSON jsonResult)
    {
        if (isSuccess)
        {
            StartCoroutine(Utilities.setAvatarUser(avatarProfile, LamaControllib.getInstance().getPartnerModel().Avatar, 0.5f));
            tvUsername.GetComponent<tk2dTextMesh>().text = LamaControllib.getInstance().getPartnerModel().Nickname;
        }
    }

    void showMyProfile(bool myProfile)
    {
        if (myProfile)
        {
            btnChangeAvatar.SetActive(true);
            btnUpLevel.SetActive(true);
            btnReport.SetActive(false);
            btnAddFriend.SetActive(false);
        }
        else
        {
            btnChangeAvatar.SetActive(false);
            btnUpLevel.SetActive(false);
            btnReport.SetActive(true);
            btnAddFriend.SetActive(true);
        }

    }

    void getMoneyUserCallback(bool isSuccess, JSON jsonResult)
    {
        if (isSuccess)
        {
            tvMoneyChip.GetComponent<tk2dTextMesh>().text = "$ " + Utilities.getVNCurrency(long.Parse(LamaControllib.getInstance().getMoneyUserModel().MoneyChip)) + " Đ";
            tvMoneyXen.GetComponent<tk2dTextMesh>().text = "$ " + Utilities.getVNCurrency(long.Parse(LamaControllib.getInstance().getMoneyUserModel().MoneyXen)) + " Đ";
        }
    }

    void getXPUserCallback(bool isSuccess, JSON jsonResult)
    {
        if (isSuccess)
        {
            tvLevel.GetComponent<tk2dTextMesh>().text = "Lv: " + LamaControllib.getInstance().getXPModel().Level;
            progressBarLevel.Value = float.Parse(LamaControllib.getInstance().getXPModel().Persent);
            tvPersentLevel.GetComponent<tk2dTextMesh>().text = float.Parse(LamaControllib.getInstance().getXPModel().Persent) * 100 + "%";
        }
    }

    void enableButton(bool isEnable)
    {
        this.btnGift.GetComponent<tk2dButton>().enabled = isEnable;
        this.btnReport.GetComponent<tk2dButton>().enabled = isEnable;
        this.btnUpLevel.GetComponent<tk2dButton>().enabled = isEnable;
        this.btnChangeAvatar.GetComponent<tk2dButton>().enabled = isEnable;
        this.btnAddFriend.GetComponent<tk2dButton>().enabled = isEnable;
        this.btnCloseProfile.GetComponent<tk2dUIItem>().enabled = isEnable;
    }

    void AddFriend()
    {
        LamaControllib.getInstance().addFriendService(LamaControllib.getInstance().getUserModel().IdUser, idUser, addFriendCallback, this);
    }
    void addFriendCallback(bool isSuccess, JSON jsonResult)
    {
        if (isSuccess)
        {
            showBoxAddFr("Yêu cầu kết bạn đã được gửi đến thành viên \"XXX\". Vui lòng đợi phản hồi!");
        }
        else
        {
            showBoxAddFr("Yêu cầu kết bạn đến thành viên \"XXX\" không thành công. Vui lòng thử lại!");
        }
    }


    void Report()
    {
        Debug.Log("add report");
        LamaControllib.getInstance().sendReportService(LamaControllib.getInstance().getUserModel().IdUser, idUser, reportCallback, this);

    }

    void reportCallback(bool isSuccess, JSON jsonResult)
    {
        if (isSuccess)
        {
            showBoxAddFr("Bạn vừa gửi yêu cầu xem xét hoạt động của thành viên \"XXX\" đến ban quản trị!");
        }
        else
        {
            showBoxAddFr("Yêu cầu xem xét hoạt động của thành viên \"XXX\" không thành công. Vui lòng thử lại!");
        }
    }

    void Gift()
    {
        Debug.Log("Tặng quà!");
    }

    void EditName()
    {
        Debug.Log("Edit Name User!");
    }

    void GiftQuick()
    {
        Debug.Log("Tặng quà tại bàn!");
    }

    void Change_avatar()
    {
        Debug.Log("Thay avatar!");
    }

    void Up_level()
    {
        Debug.Log("Nâng hạng!");
    }

    public GameObject tvMessageAddfriend;
    public GameObject boxMessNotify;
    void showBoxAddFr(string mess)
    {
        enableButton(false);
        boxMessNotify.SetActive(true);
        tvMessageAddfriend.GetComponent<tk2dTextMesh>().text = mess;
    }

    void closeBoxNotify()
    {
        enableButton(true);
        boxMessNotify.SetActive(false);
    }

    public void CloseProfile()
    {
        transform.parent.gameObject.transform.parent.gameObject.SendMessage("CloseProfile");
        //GameObject.Destroy(gameObject.transform.parent.gameObject);
    }
}
