using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static Unity.Burst.Intrinsics.X86;

public class GameScene : BaseScene
{
    [SerializeField]
    GameObject[] Boundrys;

    class Test
    {
        public int Id = 0;
    }
    class CoroutineTest : IEnumerable 
    { 
        public IEnumerator GetEnumerator()
        {
            yield return new Test() { Id =1 };
            yield return null;
            yield return new Test() { Id = 2 };
            yield return new Test() { Id = 3 };
            yield return new Test() { Id = 4 };
        }
    
    
    
    }


    protected override void Init() 
    {
        base.Init();

        SceneType = Define.Scene.Game;

        //Managers.UI.ShowSceneUI<Inven_UI>();

        CoroutineTest test = new CoroutineTest();
        foreach (var t in test)
        {
            Test value = (Test)t;
            Debug.Log(value);
        }

        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;

        gameObject.GetOrAddComponent<CursorController>();

        GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "UnityChan");
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);

        //Managers.Game.Spawn(Define.WorldObject.Monster, "Necromanser");

        Boundrys = GameObject.FindGameObjectsWithTag("Boundry"); //범위 체크포인트 리스트 받아옴, 체크포인트의 벡터값을 받아옴
        DrawSpawnRange(Boundrys);
   
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            Managers.Scene.LoadScene(Define.Scene.LogIn);
        }
    }

    private void DrawSpawnRange(GameObject[] sites)
    {
        Vector3[] siteVector = new Vector3[sites.Length];

        Vector3 temp = new Vector3();

        for (int i = 0; i < sites.Length; i++)
        {
            siteVector[i] = sites[i].transform.position;
        }

        for (int i = 0; i < (siteVector.Length); i++)
        {
            for (int j = 0; j < (siteVector.Length - 1); j++)
            {
                if (siteVector[j].x > siteVector[j + 1].x)
                {
                    temp = siteVector[j];
                    siteVector[j] = siteVector[j + 1];
                    siteVector[j + 1] = temp;
                }
            }
            //Debug.Log("SITE: " + i + siteVector[i]);
        }

        int radius = (int)(Vector3.Distance(siteVector[0], siteVector[siteVector.Length - 1]) / 2);
        Debug.Log("RADIUS: " + radius);

        Vector3 randDir = Random.insideUnitCircle * Random.Range(0, radius);
        randDir.y = 0;
        Vector3 randompos = Vector3.zero + randDir;
        Debug.DrawRay(randompos, Vector3.left * radius, Color.blue, 20.0f);

        int spawnsitenum = 3;

        for (int i = 0; i < spawnsitenum; i++)
        {
            GameObject go = new GameObject { name = "Spawnsite" };
            Spawnsite pool = go.GetOrAddComponent<Spawnsite>();
            go.transform.position = randompos;
            pool.SetKeepMonsterCount(1);
        }
    

    }

    public override void Clear()
    {
       
    }

 
}
