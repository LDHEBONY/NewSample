using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawnsite : MonoBehaviour
{
    [SerializeField]
    int _monsterCount = 0;
    int _reserveCount = 0;

    [SerializeField]
    int _keepMonsterCount = 0;

    [SerializeField]
    Vector3 _spawnPos;

    [SerializeField]
    float _spawnRadius = 15.0f;

    [SerializeField]
    float _spawnTime = 5.0f;

    public void AddMonsterCount(int value) { _monsterCount += value; }
    public void SetKeepMonsterCount(int count) { _keepMonsterCount = count; }
    // Start is called before the first frame update
    void Start()
    {
        Managers.Game.OnSpawnEvent -= AddMonsterCount;
        Managers.Game.OnSpawnEvent += AddMonsterCount;
    }

    // Update is called once per frame
    void Update()
    {
        while (_reserveCount + _monsterCount < _keepMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
        }

        Debug.DrawRay(this.transform.position, Vector3.right*_spawnRadius, Color.red, 20.0f);
        Debug.DrawRay(this.transform.position, Vector3.up * _spawnRadius, Color.red, 20.0f);
    }

    IEnumerator ReserveSpawn()
    {
        _reserveCount++;
        yield return new WaitForSeconds(Random.Range(0, _spawnTime));
        GameObject obj = Managers.Game.Spawn(Define.WorldObject.Monster, "Necromanser");
        NavMeshAgent nma = obj.GetOrAddComponent<NavMeshAgent>();

        Vector3 randompos;


        while (true)
        {
            Vector3 randDir = Random.insideUnitCircle * Random.Range(0,_spawnRadius);
            randDir.y = 0;
            randompos = _spawnPos + randDir;

            NavMeshPath path = new NavMeshPath();
            if (nma.CalculatePath(randompos, path))
            {
                break;
            }

        }

        obj.transform.position = randompos;
        _reserveCount--;
    }

    void OnDrawGizomos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(this.transform.position, _spawnRadius);
    }
}
