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

    enum Buttons // ��ư ��� �߰�
    {
        PointButton
    }



    enum Texts // �ؽ�Ʈ ��� �߰�
    {
        PointText,
        ScoreText
    }

    enum GameObjects // GameObject�� MonoBehavior�� Component�κ��� ��ӹ��� Ŭ������ �ƴϴ� FindChild�� ã�� �� ����.
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

        Bind<Button>(typeof(Buttons)); // ��ư ������Ʈ�� ã�� �ش� ������Ʈ�� ����
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));

        // enum ���ڿ��� ������ �̸��� ���� �ؽ�Ʈ�� �����´�(enum�� �ִ� ���ڿ��� �̸��� ���� ����)
        GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClicked);

        GameObject go = GetImage((int)Images.SampleImage).gameObject;

        BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);

    }



    int _score = 0;
    public void OnButtonClicked(PointerEventData data)
    {
        Debug.Log("ButtonClicked");

        _score++;
        GetText((int)Texts.ScoreText).text = $"����: {_score}";
    }


    
}


