using UnityEngine;
using System.Collections;

public class SignupController : MonoBehaviour {

    public tk2dUIToggleControl toogleCheck;
    public tk2dUITextInput edUsername;
    public tk2dUITextInput edEmail;
    public tk2dUITextInput edPassword;
    public tk2dUITextInput edConfirmPass;
    public GameObject pupopError;

	// Use this for initialization
	void Start () {

        pupopError.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClickSignup()
    {
        if(checkInputSignup()){
            LamaControllib.getInstance().signupUserService(edUsername.Text, edPassword.Text, edEmail.Text, callBackSignup, this);
        }
    }

    bool isSignupSuccess = false;

    void callBackSignup(bool isSuccess, JSON jsonResult)
    {
        isSignupSuccess = isSuccess;
        if (isSuccess)
        {
            showError("Đăng ký thành công!");
            LamaControllib.getInstance().getUserModel().Username = edUsername.Text;
            LamaControllib.getInstance().getUserModel().Nickname = edUsername.Text;
            LamaControllib.getInstance().setIsRememberLogin(true, LamaControllib.getInstance().getUserModel().IdUser ,edUsername.Text, edPassword.Text);
        }
        else
        {
            showError(jsonResult.ToString("VALUE"));
        }
    }

    void callBackSendDID(bool isSuccess, JSON jsonResult)
    {
        Debug.Log("tinhvc send device ID: " + isSuccess);
        Application.LoadLevel("OptionScreen");
    }


    void OnCickBack()
    {
        LamaControllib.getInstance().OnBackPress();
    }

    void OnCickClosePopup()
    {
        pupopError.SetActive(false);
        if (isSignupSuccess)
        {
            LamaControllib.getInstance().sendDIDService(LamaControllib.getInstance().getUserModel().IdUser, Utilities.GetUniqueIdentifier(), Utilities.GetDeviceName(), callBackSendDID, this);
        }
    }

    void OnClickLaw()
    {
        Application.OpenURL("http://cent.vn/dieu-khoan-dang-ky");
    }

  	public bool checkInputSignup() {
		if (this.edEmail.Text.Equals("")) {
            showError("Email không được để trống!");
			return false;
		}
		if (!(bool) Utilities.IsValidEmail(this.edEmail.Text)) {
			showError("Email không đúng cấu trúc!");
			return false;
		}
        if (this.edUsername.Text.Equals(""))
        {
            showError("Tên đăng nhập không được để trống!");
            return false;
        }

        if (this.edPassword.Text.Equals(""))
        {
            showError("Mật khẩu không được để trống!");
            return false;
        }

        if (this.edConfirmPass.Text.Equals(""))
        {
            showError("Vui lòng nhập lại mật khẩu!");
            return false;
        }

        if (!this.edPassword.Text.Equals(this.edConfirmPass.Text))
        {
            showError("Mật khẩu nhập lại không đúng!");
            return false;
        }

        if (!toogleCheck.IsOn)
        {
            showError("Vui lòng đọc và xác nhận điều khoản!");
            return false;
        }
		return true;
	}

    private void showError(string text)
    {
        pupopError.SetActive(true);
        pupopError.GetComponentInChildren<tk2dTextMesh>().text = text;
    }
}
