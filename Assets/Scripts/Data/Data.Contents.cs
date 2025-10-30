using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    #region Stat
    [System.Serializable] // 메모리에서 들고있는걸 파일로 변환 가능
    public class Stat
    {
        public int level;
        public int maxhp;
        public int attack;
        public int totalExp;
    }//Json에 있는 변수랑 클래스에 있는 변수랑 똑같이 맞춰줘야한다.


    [System.Serializable]
    public class StatData : ILoader<int, Stat>
    {
        public List<Stat> stats = new List<Stat>();

        public Dictionary<int, Stat> MakeDict()
        {
            Dictionary<int, Stat> dict = new Dictionary<int, Stat>();
            foreach (Stat stat in stats)
            {
                dict.Add(stat.level, stat);
            }
            return dict;
        }

    }
    #endregion
}