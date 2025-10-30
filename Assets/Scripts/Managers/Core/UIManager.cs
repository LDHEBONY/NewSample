using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    int _order = 0;

    Stack<PopUp_UI> _popupStack = new Stack<PopUp_UI>();
    Scene_UI _sceneui = null;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root"); // UI_Root라는 가상의 오브젝트에 할당될 오브젝트를 찾음
            if (root == null)
            {
                root = new GameObject { name = "@UI_Root" }; // UI_Root라는 이름을 가진 오브젝트 생성 후 할당
            }

            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Utill.GetOrAddComponent<Canvas>(go);// 캔바스에서 go라는 게임오브젝트를 가져오거나 추가해서 할당
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public T MakeSubItem<T>(string name = null, Transform parent = null) where T : Base_UI
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instatiate($"UI/SubItem/{name}");

        if (parent != null)
        {
            go.transform.SetParent(parent);
        }
        
        
        return Utill.GetOrAddComponent<T>(go);
    }

    public T MakeWorldSpaceUI<T>(string name = null, Transform parent = null) where T : Base_UI
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instatiate($"UI/WorldSpace/{name}");

        if (parent != null)
        {
            go.transform.SetParent(parent);
        }

        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main; // 월드스페이스 UI는 카메라가 필요하므로 메인 카메라를 할당

        return Utill.GetOrAddComponent<T>(go);
    }


    public T ShowPopUpUI<T>(string name = null) where T : PopUp_UI
    {
        if(string.IsNullOrEmpty(name)) //할당할 UI의 이름이 존재하지 않는다면
            name = typeof(T).Name; // 제너릭타입 T의 이름을 할당

        GameObject go = Managers.Resource.Instatiate($"UI/PopUp/{name}"); // 경로 + 오브젝트 이름(매개변수) 지정
        

        T popup = Utill.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

      
      

        go.transform.SetParent(Root.transform);

        return popup;
    }

    public T ShowSceneUI<T>(string name = null) where T : Scene_UI
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instatiate($"UI/Scene/{name}"); // 경로 + 오브젝트 이름(매개변수) 지정


        T sceneUI = Utill.GetOrAddComponent<T>(go);
        _sceneui = sceneUI;

       
        go.transform.SetParent(Root.transform);

        return sceneUI;
    }


    public void ClosePopUpUI(PopUp_UI popup)
    {

        if (_popupStack.Count == 0)
            return;

        if(_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed");
            return;
        }

        ClosePopUpUI();
    }


    public void ClosePopUpUI()
    {
        if (_popupStack.Count == 0)
            return;

        PopUp_UI popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;
        //_order--;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
        {
            ClosePopUpUI();
        }
    }

    public void Clear()
    {
        CloseAllPopupUI();
        _sceneui = null;
    }

    
}
