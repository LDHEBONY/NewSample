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
            GameObject root = GameObject.Find("@UI_Root"); // UI_Root��� ������ ������Ʈ�� �Ҵ�� ������Ʈ�� ã��
            if (root == null)
            {
                root = new GameObject { name = "@UI_Root" }; // UI_Root��� �̸��� ���� ������Ʈ ���� �� �Ҵ�
            }

            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Utill.GetOrAddComponent<Canvas>(go);// ĵ�ٽ����� go��� ���ӿ�����Ʈ�� �������ų� �߰��ؼ� �Ҵ�
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
        canvas.worldCamera = Camera.main; // ���彺���̽� UI�� ī�޶� �ʿ��ϹǷ� ���� ī�޶� �Ҵ�

        return Utill.GetOrAddComponent<T>(go);
    }


    public T ShowPopUpUI<T>(string name = null) where T : PopUp_UI
    {
        if(string.IsNullOrEmpty(name)) //�Ҵ��� UI�� �̸��� �������� �ʴ´ٸ�
            name = typeof(T).Name; // ���ʸ�Ÿ�� T�� �̸��� �Ҵ�

        GameObject go = Managers.Resource.Instatiate($"UI/PopUp/{name}"); // ��� + ������Ʈ �̸�(�Ű�����) ����
        

        T popup = Utill.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

      
      

        go.transform.SetParent(Root.transform);

        return popup;
    }

    public T ShowSceneUI<T>(string name = null) where T : Scene_UI
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instatiate($"UI/Scene/{name}"); // ��� + ������Ʈ �̸�(�Ű�����) ����


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
