using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;

public class NetworkManager : GlobalSingleton<NetworkManager>
{
    private Transform titlePanel;
    private Transform loginPanel;
    private Transform signupPanel;
    private string playerIDCache;


    protected override void Awake()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            Debug.Log("2");
            Debug.Log(PhotonNetwork.IsConnected);
        }
        FindAllPanels();
    }

    protected override void Update()
    {

    }


    #region UI ������Ʈ ã�ƿ��� �޼���
    private void FindAllPanels() 
    {
        titlePanel = GameObject.Find("Panel_Title").transform;
        loginPanel = titlePanel.Find("Panel_Login").transform;
        signupPanel = titlePanel.Find("Panel_Signup").transform;
    }
    #endregion

    #region ���� ����-PlayFab
    public void StartQuick() 
    {
        Debug.Log("PlayFab authenticating using Custom ID...");

        var request = new LoginWithCustomIDRequest { CustomId = PlayFabSettings.DeviceUniqueIdentifier,
            CreateAccount = true };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }
    #endregion

    #region �α���-PlayFab
    public void Login()
    {
        TMP_InputField emailInput = loginPanel.Find("InputField_Email").GetComponent<TMP_InputField>();
        TMP_InputField passwordInput = loginPanel.Find("InputField_Password").GetComponent<TMP_InputField>();

        var request = new LoginWithEmailAddressRequest { Email = emailInput.text, 
            Password = passwordInput.text };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result) 
    {
        Debug.Log("PlayFab authenticated. Requesting photon token...");

        playerIDCache = result.PlayFabId;
        var tokenRequest = new GetPhotonAuthenticationTokenRequest() 
        { PhotonApplicationId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime };

        PlayFabClientAPI.GetPhotonAuthenticationToken(tokenRequest, AuthenticateWithPhoton, OnLoginFailure);

        //Debug.LogFormat("�α��� ����");
        loginPanel.GetComponentInChildren<TMP_Text>().color = Color.green;
        loginPanel.GetComponentInChildren<TMP_Text>().text = "�α��� ����";
    }

    private void OnLoginFailure(PlayFabError error) 
    {
        Debug.LogFormat("�α��� ����\n���� �ڵ� : {0}", error);
        loginPanel.GetComponentInChildren<TMP_Text>().color = Color.red;
        loginPanel.GetComponentInChildren<TMP_Text>().text = "�α��� ����, ���� �ڵ� : " + error;
    }
    #endregion

    #region Email ȸ������-PlayFab
    public void Register() 
    {
        TMP_InputField emailInput = signupPanel.Find("InputField_Email").GetComponent<TMP_InputField>();
        TMP_InputField passwordInput = signupPanel.Find("InputField_Password").GetComponent<TMP_InputField>();
        TMP_InputField nicknameInput = signupPanel.Find("InputField_Nickname").GetComponent<TMP_InputField>();

        var request = new RegisterPlayFabUserRequest { Email = emailInput.text, 
            Password = passwordInput.text, Username = nicknameInput.text, DisplayName = nicknameInput.text };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result) 
    {
        Debug.LogFormat("���� ��� ����");
        signupPanel.GetComponentInChildren<TMP_Text>().color = Color.green;
        signupPanel.GetComponentInChildren<TMP_Text>().text = "���� ��� ����";
    }

    private void OnRegisterFailure(PlayFabError error) 
    {
        Debug.LogFormat("���� ��� ����\n���� �ڵ� : {0}", error);
        signupPanel.GetComponentInChildren<TMP_Text>().color = Color.red;
        signupPanel.GetComponentInChildren<TMP_Text>().text = "���� ��� ����, ���� �ڵ� : " +  error;
    }
    #endregion

    #region Photon Token ��û �� ����-Photon
    private void AuthenticateWithPhoton(GetPhotonAuthenticationTokenResult tokenResult)
    {
        Debug.LogFormat("Photon token acquired: " + tokenResult.PhotonCustomAuthenticationToken + "  Authentication complete.");

        var customAuth = new AuthenticationValues { AuthType = CustomAuthenticationType.Custom };
        
        customAuth.AddAuthParameter("username", playerIDCache);
        customAuth.AddAuthParameter("token", tokenResult.PhotonCustomAuthenticationToken);

        PhotonNetwork.AuthValues = customAuth;

        Debug.Log("1");
        Debug.Log(PhotonNetwork.IsConnected);
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("3");
        Debug.Log(PhotonNetwork.IsConnected);
        //Debug.Log(PhotonNetwork.ConnectUsingSettings());
    }
    #endregion
}