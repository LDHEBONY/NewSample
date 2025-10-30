using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    protected int _exp; // 상속받은 클래스에서 변수 사용가능, 다른클래스에서 접근 불가
    [SerializeField]
    protected int _gold;

    public int Exp { get { return _exp; } 
        
        
        set { 
            _exp = value;

            int level = Level;
            while (true)
            {
                Data.Stat stat;
                if (Managers.Data.StatDict.TryGetValue(level + 1, out stat) == false)
                    break;
                if(_exp < stat.totalExp)
                {
                    break;
                }
                level++;
            }

            if(level != Level)
            {
                Debug.Log("Level Up");
                Level = level;
                SetStat(Level);
            }
        
            } 
     
    }
    public int Gold { get { return _gold; } set { _gold = value; } }

    private void Start()
    {
        _level = 1;

        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;

        Data.Stat stat = dict[1];
        
        SetStat(_level);


        _defense = 5;
        _movespeed = 5.0f;
        _exp = 0;
        _gold = 0;
    }

    public void SetStat(int level)
    {
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;

        Data.Stat stat = dict[level];

        _hp = stat.maxhp;
        _maxHp = stat.maxhp;
        _attack = stat.attack;
    }

    protected override void OnDead(Stat attacker)
    {
        Debug.Log("Player Dead");
    }
}
