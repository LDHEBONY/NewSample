using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp_UI : Base_UI
{
   
    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, true);
    }

   public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopUpUI(this);
    }
}
