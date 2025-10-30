using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_UI : Base_UI 
{ 
   public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, false);
    }
  
}
