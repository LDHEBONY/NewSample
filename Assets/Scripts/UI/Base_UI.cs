using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Base_UI : MonoBehaviour
{

    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init(); // 상속된 자식 클래스에서 공유할 수 있도록 공통적인 정의를 제공, 객체로 만드는 접근제한자 static나 가상의 함수나 클래스를 선언하는 virtual은 선언할 수 없다

    private void Start()
    {
        Init();
    }

    protected void Bind<T>(Type type) where T : UnityEngine.Object// 리플렉션 : 컴파일 시에 알수 없었던 타입이나 멤버들을 찾아내고 사용할 수 있게 해주는 매커니즘
    {
        string[] names = Enum.GetNames(type);

        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
            {
                objects[i] = Utill.FindChild(gameObject, names[i], true);
            }
            else
            {
                objects[i] = Utill.FindChild<T>(gameObject, names[i], true);
            }

            if (objects[i] == null)
            {
                Debug.Log($"Failed to bind!{names[i]}");
            }
            else
            {
                Debug.Log($"Bind Success{names[i]}");
            }
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    protected GameObject GetObject(int idx)
    {
        return Get<GameObject>(idx);
    }

    protected Text GetText(int idx)
    {
        return Get<Text>(idx);
    }

    protected Button GetButton(int idx)
    {
        return Get<Button>(idx);
    }

    protected Image GetImage(int idx)
    {
        return Get<Image>(idx);
    }

    public static void BindEvent(GameObject go, Action<PointerEventData>action, Define.UIEvent type = Define.UIEvent.Click)
    {

        EventHandler_UI evt = Utill.GetOrAddComponent<EventHandler_UI>(go);

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;

                break;

        }
        
        evt.OnDragHandler += ((PointerEventData data) => { evt.gameObject.transform.position = data.position; });
    }

}
