using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager {

    [SerializeField]
    private Button _loginbutton;


    private async void Awake()
    {
        if(UnityServices.State == ServicesInitializationState.Uninitialized) // 유니티 서비스가 초기화되지 않은 상태라면
        {
            Debug.Log("Services Initalizing");
            await UnityServices.InitializeAsync(); // 유니티 서비스를 비동기적으로 초기화

            _loginbutton = GameObject.Find("LoginButton").GetComponent<Button>();
            _loginbutton.onClick.AddListener(StartAnonymousSignIn);
        }
        else
        {

        }
    }
    
    public bool _isSignIn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public async void StartAnonymousSignIn()
    {
        await SignUpAnonymouslyAsync();
    }

    private async Task SignUpAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            _isSignIn = true;

        }
        catch (AuthenticationException ex) // 로그인 과정에서 발생하는 예외 처리
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex) // 네트워크 요청 실패에 대한 예외 처리
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
}
