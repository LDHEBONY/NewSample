using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager //: MonoBehaviour
{
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null; // ���콺 �̺�Ʈ �׼��� �޾ƿ� ���� ����


    bool _pressed = false;
    float _pressTime = 0; // ���콺 ���� �ð� ������ ����
    // Update is called once per frame
    public void OnUpdate()
    {
        Debug.Log("[InputManager] OnUpdate"); // �̰� �ݵ�� �ߴ°�?

        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (EventSystem.current != null)
            {
                Debug.LogWarning("[InputManager] UI ���� �־� ���콺 �Է� ���õ�");
            }

            return;
        }
            


        if (Input.anyKey && KeyAction != null)
        {
            KeyAction.Invoke();
        }

        if(MouseAction != null) 
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!_pressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);
                    _pressTime = Time.time;
                }
                Debug.Log("[InputManager] Click ������"); // Ȯ�� �α�
                MouseAction.Invoke(Define.MouseEvent.Press); // ���콺�� ������ ���� �̺�Ʈ �߻�
                _pressed = true;
            }
            else
            {
                if (_pressed)
                {
                    if(Time.time < _pressTime + 0.2f)
                    {
                        MouseAction.Invoke(Define.MouseEvent.Click); // ���콺�� ���� �� 0.2�� �̳��� ���� Ŭ�� �̺�Ʈ �߻�
                        MouseAction.Invoke(Define.MouseEvent.PointerUp); // ���콺�� ���� ���� �̺�Ʈ �߻�
                    }                   
                    _pressed = false;
                    _pressTime = 0; // ���콺 ���� �ð� �ʱ�ȭ
                }
            }

        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
