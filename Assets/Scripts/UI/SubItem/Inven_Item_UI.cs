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

        Get<GameObject>((int)GameObjects.ItemIcon).BindEvent((PointerEventData) => { Debug.Log($"아이템 클릭! {_name}"); });
        //클릭 등 이벤트를 추가했을때 디버깅 로그를 띄우는 법, 람다식을 이용하여 => 을 이용해 간결하게 작성
    }


    public void SetInfo(string name)
    {
        _name = name;
    }
}
