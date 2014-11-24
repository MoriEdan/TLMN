using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MBS
{
    public class LoginButtonControll : WULogin
    {

        public GameObject dialogForgetPass;
        public tk2dUITextInput edUsername;
        public tk2dUITextInput edPassword;

        // Use this for initialization
        void Start()
        {

            pupopError.SetActive(false);
            //wp login
            if (this == Instance)
            {
                InitWULoginGUI();
            }
        }

        //wp login
        static LoginButtonControll _instance;
        static public LoginButtonControll Instance
        {
            get
            {
                Debug.Log("tinhvc WULoginGUI Instance");
                if (null == _instance)
                {
                    LoginButtonControll[] objs = GameObject.FindObjectsOfType<LoginButtonControll>();
                    if (null != objs && objs.Length > 0)
                    {
                        _instance = objs[0];
                        for (int i = 1; i < objs.Length; i++)
                            Destroy(objs[i].gameObject);
                    }
                    else
                    {
                        GameObject newobject = new GameObject("ButtonLoginController");
                        _instance = newobject.AddComponent<LoginButtonControll>();
                    }
                }
                return _instance;
            }
        }

        public WULStates LoginState
        {
            get
            {
                return loginState.CurrentState;
            }
            set
            {
                loginState.SetState(value);
            }
        }

        mbsStateMachine<WULStates> loginState;

        virtual protected void InitWULoginGUI()
        {
            Debug.Log("tinhvc InitWULoginGUI");
            //the server's state can always be only one of two: Idling or awaiting a response
            //set this up and start in the idle state.
            serverState = new mbsStateMachine<WULServerState>();
            serverState.AddState(WULServerState.None);
            serverState.AddState(WULServerState.Contacting, ShowPleaseWait);
            serverState.SetState(WULServerState.None);

            //setup the various states our login kit could be in...
            loginState = new mbsStateMachine<WULStates>();
            loginState.AddState(WULStates.Dummy);
            loginState.AddState(WULStates.ValidateLoginStatus);
            loginState.AddState(WULStates.Logout);
            loginState.AddState(WULStates.FetchAccountDetails);
            loginState.AddState(WULStates.UpdateAccountDetails);

            SetupResponders();
        }

        void ShowPleaseWait()
        {
            Debug.Log("ShowPleaseWait");
            //Utility.ShowLoading();
        }


        void SetupResponders()
        {
            onLoggedIn += OnLoggedInCallback;
        }

        virtual public void OnLoggedInCallback(object _data)
        {

            cmlData data = (cmlData)_data;

            // logged_in = true;
            string display_name = data.String("displayname");
            string nickname = data.String("nickname");
            string idUser = data.String("id");

            LamaControllib.getInstance().getUserModel().IdUser = idUser;
            LamaControllib.getInstance().getUserModel().Username = nickname;
            LamaControllib.getInstance().getUserModel().Nickname = nickname;
            LamaControllib.getInstance().setIsRememberLogin(true, idUser, nickname, edPassword.Text);

            Debug.Log("tinhvc: " + display_name + " - " + nickname + " - " + idUser);
            LamaControllib.getInstance().sendDIDService(idUser, Utilities.GetUniqueIdentifier(), Utilities.GetDeviceName(), callBackSendDID, this);

        }

        virtual public void OnLoggedInFaileCallback(string error)
        {
            Debug.Log("tinhvc login faile");
            pupopError.SetActive(true);
        }

        void callBackSendDID(bool isSuccess, JSON jsonResult)
        {
            Debug.Log("tinhvc send device ID: " + isSuccess);
            Application.LoadLevel("OptionScreen");
        }

        public GameObject pupopError;
        void ClosePopup()
        {
            pupopError.SetActive(false);
        }

        void OnClickLogin()
        {
            cmlData fields = new cmlData();
            string userName = edUsername.Text;
            string userPass = edPassword.Text;
            fields.Set("username", userName);
            fields.Set("password", userPass);

            Debug.Log("username: " + userName);
            Debug.Log("pass: " + userPass);
            onServerContactSucceeded = onLoginSuccess;
            ContactServer(OnLoggedInFaileCallback, WULStates.LoginChallenge, fields);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ForgetPass()
        {
            iTween.MoveTo(dialogForgetPass, new Vector3(0, 0, -3), 1);
        }

        void RegisterAcc()
        {
            LamaControllib.getInstance().LoadLevel("Register");
        }

        void OnClickBack()
        {
            LamaControllib.getInstance().OnBackPress();
        }

    }

}
