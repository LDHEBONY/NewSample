using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inven_Item_UI : Base_UI
{
    enum GameObjects
    {
        ItemIcon,
        ItemNameText,
    }

    string _name;

    // Start is called before the first frame update

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        Get<GameObject>((int)GameObjects.ItemNameText).GetComponent<Text>().text = _name;

        Get<GameObject>((int)GameObjects.ItemIcon).BindEvent((PointerEventData) => { Debug.Log($"������ Ŭ��! {_name}"); });
        //Ŭ�� �� �̺�Ʈ�� �߰������� ����� �α׸� ���� ��, ���ٽ��� �̿��Ͽ� => �� �̿��� �����ϰ� �ۼ�
    }


    public void SetInfo(string name)
    {
        _name = name;
    }
}
