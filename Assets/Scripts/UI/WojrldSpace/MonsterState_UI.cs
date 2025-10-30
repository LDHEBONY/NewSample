using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterState_UI : Base_UI
{
    enum Images
    {
        MonsterState

    }

    MonsterController _monsterctrl;
    public override void Init()
    {
        Bind<Image>(typeof(Images));
        _monsterctrl = transform.parent.GetComponent<MonsterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        Transform parent = transform.parent;
        transform.position = (parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y)) + (Vector3.left);
        transform.rotation = Camera.main.transform.rotation;


        SetState(_monsterctrl.State);
    }

    public void SetState(Define.State state)
    {
       if(state == Define.State.Moving)
       {
            GetImage((int)Images.MonsterState).color = Color.yellow;
       }
       if(state == Define.State.Idle)
       {
            GetImage((int)Images.MonsterState).color = Color.green;
       }
         if(state == Define.State.Skill)
        {
            GetImage((int)Images.MonsterState).color = Color.red;
        }
    }
}
