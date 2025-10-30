using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_UI : PopUp_UI
{
    

    [SerializeField]
    Text _text;

    enum Buttons // 버튼 목록 추가
    {
        PointButton
    }



    enum Texts // 텍스트 목록 추가
    {
        PointText,
        ScoreText
    }

    enum GameObjects // GameObject는 MonoBehavior나 Component로부터 상속받은 클래스가 아니니 FindChild로 찾을 수 없다.
    {
        Object01,
        
    }
    enum Images
    {
        SampleImage,
    }

  
    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons)); // 버튼 컴포넌트를 찾아 해당 오브젝트를 매핑
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));

        // enum 문자열과 동일한 이름을 가진 텍스트를 가져온다(enum에 있는 문자열과 이름을 같게 지정)
        GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClicked);

        GameObject go = GetImage((int)Images.SampleImage).gameObject;

        BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);

    }



    int _score = 0;
    public void OnButtonClicked(PointerEventData data)
    {
        Debug.Log("ButtonClicked");

        _score++;
        GetText((int)Texts.ScoreText).text = $"점수: {_score}";
    }


    
}


