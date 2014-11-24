using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.IO;

namespace MBS {
	public enum WULServerState	{ None, Contacting }

	public class WUServer : MonoBehaviour {


       readonly ushort[] iso8859_2 = {
                                                        0x00A0, 0x0104, 0x02D8, 0x0141, 0x00A4, 0x013D, 0x015A, 0x00A7, 0x00A8, 0x0160, 
                                                        0x015E, 0x0164, 0x0179, 0x00AD, 0x017D, 0x017B, 0x00B0, 0x0105, 0x02DB, 0x0142, 
                                                        0x00B4, 0x013E, 0x015B, 0x02C7, 0x00B8, 0x0161, 0x015F, 0x0165, 0x017A, 0x02DD, 
                                                        0x017E, 0x017C, 0x0154, 0x00C1, 0x00C2, 0x0102, 0x00C4, 0x0139, 0x0106, 0x00C7, 
                                                        0x010C, 0x00C9, 0x0118, 0x00CB, 0x011A, 0x00CD, 0x00CE, 0x010E, 0x0110, 0x0143, 
                                                        0x0147, 0x00D3, 0x00D4, 0x0150, 0x00D6, 0x00D7, 0x0158, 0x016E, 0x00DA, 0x0170, 
                                                        0x00DC, 0x00DD, 0x0162, 0x00DF, 0x0155, 0x00E1, 0x00E2, 0x0103, 0x00E4, 0x013A, 
                                                        0x0107, 0x00E7, 0x010D, 0x00E9, 0x0119, 0x00EB, 0x011B, 0x00ED, 0x00EE, 0x010F, 
                                                        0x0111, 0x0144, 0x0148, 0x00F3, 0x00F4, 0x0151, 0x00F6, 0x00F7, 0x0159, 0x016F, 
                                                        0x00FA, 0x0171, 0x00FC, 0x00FD, 0x0163, 0x02D9 };




		static WUServer __wuserver;
		static public WUServer WUServer_Instance { get {if (null == __wuserver) __wuserver = GameObject.FindObjectOfType<WULogin>(); return __wuserver;} }

        static readonly string
            //tinhvc debug
            //path_end = "/wp-content/plugins/wuss_login/unity_functions.php";
        path_end = "http://cent.vn/wp-content/plugins/1pay/unity_functions.php";


		static public bool 
			logged_in = false;

		static public Action<object>
			onServerContactFailed,
			onServerContactSucceeded;

        //public string
        //    online_url = "http://www.mysite.com",
        //    offline_url = "http://localhost";
		
		public bool 
			online = false,
			print_response_headers = false,
			print_debug_info = false;

		protected mbsStateMachine<WULServerState> 
			serverState;

		public string URL 
		{
			get 
			{
				return path_end;
			}
		}

		static public void OnServerContactFailed(object data = null)
		{
			if (null != onServerContactFailed)
				onServerContactFailed(data);
		}

		static public void OnServerContactSucceeded(object data = null)
		{
			if (null != onServerContactSucceeded)
				onServerContactSucceeded(data);
		}

        public void ContactServer( eventErrorListener errorCallback, WULStates action, cmlData data = null)
		{
            StartCoroutine(CallServer(errorCallback,action, data));
		}

        int count = 0;
        public delegate void eventErrorListener(string eventString);
		IEnumerator CallServer( eventErrorListener errorCallback, WULStates action, cmlData data = null)
		{
            
			if (null == data)
				data = new cmlData();

			WWWForm f = new WWWForm();
            //tinhvc debug
            //f.AddField("unity",1);
            //f.AddField("action", (int)action);
            //f.AddField("wuss","login");
            Debug.Log("tinhvc action: " + (int)action);
            foreach (string s in data.Keys)
            {
                Debug.Log("tinhvc: "+s);
                f.AddField(s, data.String(s));
            }
				


           
			serverState.SetState (WULServerState.Contacting);

            Debug.Log("tinhvc result decode: serverState");

			#if UNITY_WEBPLAYER
			WWW w = new WWW(URL, f.data);
			#else
			WWW w = new WWW(URL, f.data, WUCookie.Cookie);
			#endif
			yield return w;

			serverState.SetState(WULServerState.None);

			if (w.error != null)
			{
                Debug.Log("tinhvc error login: " + w.error);
				StatusMessage.Message = w.error;
				//OnServerContactFailed(w.error);
                errorCallback(w.error);
			} else 
			{
				string result_string = w.text;

				/* ------------ Hack Alert -------------------------
				 * I found that certain plugins attempt to write to the headers after
				 * this script has run it's course. Unfortunately, since I send results
				 * to Unity it means php can no longer write headers / cookies so this results
				 * in my data being returned to Unity followed by a warning message from 
				 * Apache or php or WP  or whomsoever warning me that it couldn't write the cookies
				 * 
				 * Since this kit is entirely self contained, we do not care about other
				 * plugins and what they try to do and how badly they fail to do so, so
				 * I simply scan the results for a warning message appearing in the middle of
				 * the results and if so, I simply erase the warning and evaluate the results again.
				 * 
				 * As of the version 1.2 update this should no longer be needed but I leave it in for good measure
				 */
               // Debug.Log("tinhvc result encode: " + result_string);

				int warning_index = result_string.IndexOf( "<br />" );
				if ( warning_index > 0)
					result_string = result_string.Substring(0, warning_index );


                Debug.Log("tinhvc result: " + result_string);
				//result_string = EncoderFix.Base64Decode(result_string);

                //MBS.ButtonLoginController.sDebug = result_string;
                //MBS.ButtonLoginController.isDebug = true;

                CML results = new CML();
				results.ParseFile(result_string);

               // Debug.Log("tinhvc result decode: " + result_string);

				if (print_response_headers)
					foreach(var x in w.responseHeaders)
						Debug.Log(x.Key + " = " + x.Value + " : " + x.GetType() ) ;

				switch(action)
				{
				case WULStates.LoginChallenge:
				case WULStates.ValidateLoginStatus:
					WUCookie.ExtractCookie( w.responseHeaders );
					break;
					
				case WULStates.Logout:
					WUCookie.ClearCookie();
					WUCookie.StoreCookie();
					break;
				}

				if (results.Count == 0)
				{
                    Debug.Log("tinhvc results.Count =0");
					//StatusMessage.Message = "No results returned";
					//OnServerContactFailed("No results returned");
                    errorCallback("No results returned");
				} else
				{
					//should only ever be one but for the sake of demonstration, let's test for multiple...
					List<cmlData> errors = results.NodesWithField("success", "false");
					

                        if (null != errors)
                        {
                            if (action != WULStates.ValidateLoginStatus)
                                foreach (cmlData error in errors)
                                {
                                    errorCallback(error.String("message"));
                                }
                        } else{

                            cmlData LOGIN = results.GetFirstNodeOfType("LOGIN");
                            if (null != LOGIN)
                            {
                                //notifyLabel_.text = "Login success";
                                if (action == WULStates.ProfileImage)
                                    LOGIN.Set("gravatar_type", data.String("gravatar_type"));
                                OnServerContactSucceeded(LOGIN);
                                WUCookie.ClearCookie();
                                WUCookie.StoreCookie();
                                foreach (string s in LOGIN.Keys)
                                {
                                    Debug.Log("tinhvc: " + s + ": " + LOGIN.String(s));
                                   // f.AddField(s, LOGIN.String(s));
                                }
                            }
                            else {
                                //notifyLabel_.text = "Login faile";
                            }

                            cmlData DEVICE = results.GetFirstNodeOfType("DEVICE");
                            if (null != DEVICE)
                            {
                                //if (action == WULStates.ProfileImage)
                                //    DEVICE.Set("gravatar_type", data.String("gravatar_type"));
                                OnServerContactSucceeded(DEVICE);
                            }
                            //else {
                            //    PlayerPrefs.SetInt(Utility.key_SendDID, 1);
                            //    Debug.Log("tinhvc send DID success");
                            //}
                        }

					if (print_debug_info)
					{
						List<cmlData> Debugs = results.AllDataOfType("debug");
						if (null != Debugs)
						{
							foreach(cmlData d in Debugs)
								Debug.Log (d.ToString());
						}
					}
				}
			}			
		}

	}
}
