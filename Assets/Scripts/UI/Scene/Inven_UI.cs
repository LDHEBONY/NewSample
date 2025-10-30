using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inven_UI : Scene_UI
{
    enum GameObjects 
    { 
        GridPanel
    }


    // Start is called before the first frame update

    // Update is called once per frame
    public override void Init()   
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));

        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);

        foreach (Transform child in gridPanel.transform)
            Managers.Resource.Destroy(child.gameObject);

        //실제 인벤토리 정보 참고
        for(int i = 0; i < 8; i++)
        {

            GameObject item = Managers.UI.MakeSubItem<Inven_Item_UI>(parent : gridPanel.transform).gameObject;


            Inven_Item_UI invenitem = item.GetOrAddComponent<Inven_Item_UI>();
            invenitem.SetInfo($"Excliver NO.{i}");
        }
    }
}
