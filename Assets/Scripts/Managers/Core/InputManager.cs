using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager //: MonoBehaviour
{
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null; // 마우스 이벤트 액션을 받아올 변수 생성


    bool _pressed = false;
    float _pressTime = 0; // 마우스 누른 시간 추적용 변수
    // Update is called once per frame
    public void OnUpdate()
    {
        Debug.Log("[InputManager] OnUpdate"); // 이거 반드시 뜨는가?

        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (EventSystem.current != null)
            {
                Debug.LogWarning("[InputManager] UI 위에 있어 마우스 입력 무시됨");
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
                Debug.Log("[InputManager] Click 감지됨"); // 확인 로그
                MouseAction.Invoke(Define.MouseEvent.Press); // 마우스를 누르는 순간 이벤트 발생
                _pressed = true;
            }
            else
            {
                if (_pressed)
                {
                    if(Time.time < _pressTime + 0.2f)
                    {
                        MouseAction.Invoke(Define.MouseEvent.Click); // 마우스를 누른 후 0.2초 이내에 떼면 클릭 이벤트 발생
                        MouseAction.Invoke(Define.MouseEvent.PointerUp); // 마우스를 떼는 순간 이벤트 발생
                    }                   
                    _pressed = false;
                    _pressTime = 0; // 마우스 누른 시간 초기화
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
