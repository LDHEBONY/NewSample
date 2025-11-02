using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogInScene : BaseScene
{
    LoginManager _loginManager = new LoginManager();

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.LogIn;

        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < 5; i++)
            list.Add(Managers.Resource.Instatiate("UnityChan"));

        foreach (GameObject obj in list)
        {
            Managers.Resource.Destroy(obj);
        }
    }

    private void Update()
    {
        if (_loginManager._isSignIn == true)
        {

            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                Managers.Scene.LoadScene(Define.Scene.Game);
            }
        }
    }

    public override void Clear()
    {
        Debug.Log("LoginScene Clear!");
    }

}
