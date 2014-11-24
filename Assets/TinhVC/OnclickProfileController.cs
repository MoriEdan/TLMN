using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnclickProfileController : MonoBehaviour
{

    public GameObject cameraGamePlay;
    public GameObject cameraProfile;
    public GameObject cameraDeposit;
    public GameObject cameraFriendList;

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

    public GameObject btnGift;
    public GameObject btnChangeAvatar;
    public GameObject btnAddFriend;
    public GameObject btnReport;
    public GameObject btnUpLevel;
    public GameObject btnCloseProfile;

    ArrayList arrBaiDepNhat;
    GameObject objAddFr, objReport, objBoxAddFr, objBoxReport;

    public GameObject avatar1;
    public GameObject avatar11;
    public GameObject avatar2;
    public GameObject avatar22;

    // Use this for initialization
    void Start()
    {
        objAddFr = GameObject.Find("addFriend");
        objReport = GameObject.Find("report");
        objBoxAddFr = GameObject.Find("BoxAddFriend");
        objBoxReport = GameObject.Find("BoxReport");
        boxMessNotify.SetActive(false);

        arrBaiDepNhat = new ArrayList { "1", "2", "3" };
    }

    // Update is called once per frame
    void Update()
    {

    }

    void setAvatarShow(bool isShow)
    {
        if (VariableApplication.iPlayerCount == 2)
        {
            avatar1.SetActive(isShow);
            avatar2.SetActive(isShow);
        }
        else
        {
            avatar1.SetActive(isShow);
            avatar11.SetActive(isShow);
            avatar2.SetActive(isShow);
            avatar22.SetActive(isShow);
        }

    }

    void showProfile(bool isShow)
    {
        if (isShow)
        {
            cameraGamePlay.GetComponent<UIPanel>().alpha = 0;
            setAvatarShow(false);

        }
        else
        {
            cameraGamePlay.GetComponent<UIPanel>().alpha = 1;
            setAvatarShow(true);
        }

    }

    void showDeposit(bool isShow)
    {
        if (isShow)
        {
            cameraGamePlay.GetComponent<UIPanel>().alpha = 0;
            setAvatarShow(false);
        }
        else
        {
            cameraGamePlay.GetComponent<UIPanel>().alpha = 1;
            setAvatarShow(true);
        }

    }

    void showFriendList(bool isShow)
    {
        if (isShow)
        {
            cameraGamePlay.GetComponent<UIPanel>().alpha = 0;
            setAvatarShow(false);
        }
        else
        {
            cameraGamePlay.GetComponent<UIPanel>().alpha = 1;
            setAvatarShow(true);
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


    public void OnClickAvatar1()
    {
        if (VariableApplication.isViewer)
        {
            idUser = VariableApplication.idUser1;
        }
        else
        {
            idUser = LamaControllib.getInstance().getUserModel().IdUser;
        }

        getInfoProfile();
    }

    public void OnClickAvatar2()
    {
        if (CloneNumberController.isStartGame)
        {
            idUser = VariableApplication.idUser11;
            getInfoProfile();
        }
    }

    public void OnClickAvatar3()
    {
        if (CloneNumberController.isStartGame)
        {
            idUser = VariableApplication.idUser2;
            getInfoProfile();
        }
    }

    public void OnClickAvatar4()
    {
        if (CloneNumberController.isStartGame)
        {
            idUser = VariableApplication.idUser22;
            getInfoProfile();
        }
    }

    GameObject cloneProfile;
    void getInfoProfile()
    {
        showProfile(true);
        cloneProfile = Instantiate(cameraProfile, transform.position, transform.rotation) as GameObject;
        cloneProfile.transform.parent = transform;
        cloneProfile.transform.localPosition = new Vector3(0, 0, -10);
        cloneProfile.transform.localScale = new Vector3(1, 1, 1);
    }

    void getAvatarCallback(bool isSuccess, JSON jsonResult)
    {
        if (isSuccess)
        {
            StartCoroutine(Utilities.setAvatarUser(avatarProfile, LamaControllib.getInstance().getPartnerModel().Avatar, 0.5f));
            tvUsername.GetComponent<tk2dTextMesh>().text = LamaControllib.getInstance().getPartnerModel().Nickname;
        }
    }

    GameObject cloneDeposit;
    public void OnClickDeposit()
    {
        showDeposit(true);
        cloneDeposit = Instantiate(cameraDeposit, transform.position, transform.rotation) as GameObject;
        cloneDeposit.transform.parent = transform;
        cloneDeposit.transform.localPosition = new Vector3(0, 0, -10);
        cloneDeposit.transform.localScale = new Vector3(1, 1, 1);
    }

    GameObject cloneFriendList;
    public void OnClickFriendList()
    {
        showFriendList(true);
        cloneFriendList = Instantiate(cameraFriendList, transform.position, transform.rotation) as GameObject;
        cloneFriendList.transform.parent = transform;
        cloneFriendList.transform.localPosition = new Vector3(0, 0, -10);
        cloneFriendList.transform.localScale = new Vector3(1, 1, 1);
    }

    public void CloseProfile()
    {
        GameObject.Destroy(cloneProfile);
        showProfile(false);
    }

    public void CloseDeposit()
    {
        enableButton(true);
        GameObject.Destroy(cloneDeposit);
        showDeposit(false);
    }

    public void CloseFriendList()
    {
        enableButton(true);
        GameObject.Destroy(cloneFriendList);
        showFriendList(false);
    }

    public static string idUser = "-1";


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
}
