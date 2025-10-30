using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    Stat _stat;

    [SerializeField]
    float _scanRange = 10f;

    [SerializeField]
    float _attackRange = 2f;

    Vector3 _destPos;

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Monster;
        _stat = gameObject.GetComponent<Stat>();

        if (gameObject.GetComponentInChildren<HPBar_UI>() == null)
        {
            Managers.UI.MakeWorldSpaceUI<HPBar_UI>("UI_HPBar", this.transform);
        }
        if (gameObject.GetComponentInChildren<MonsterState_UI>() == null)
        {
            Managers.UI.MakeWorldSpaceUI<MonsterState_UI>("MonsterState", this.transform);
        }

    }

    protected override void UpdateIdle()
    {

        GameObject player = Managers.Game.GetPlayer();

        if (player == null)
        {
            return;
        }

        float distance = (player.transform.position - transform.position).magnitude;

        if(distance <= _scanRange)
        {
            _lockTarget = player;
            State = Define.State.Moving;
            return;
        }
    }

    protected override void UpdateMoving()
    {
        Debug.Log("Monster Update Moving");

        if (_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;

            float distance = (_destPos - transform.position).magnitude;

            if (distance <= _attackRange)
            {
                NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
                nma.SetDestination(transform.position);
                State = Define.State.Skill;
                return;
            }


        }


        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;

        }
        else
        {
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            //nma.CalculatePath
            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude); // min과 max사이의 값을 보장해주어야한다.;
            nma.Move(dir.normalized * moveDist);

            nma.SetDestination(_destPos);
            nma.speed = _stat.MoveSpeed;


            //transform.position += dir.normalized * moveDist;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
            //transform.LookAt(_destPos);
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }

    }

    protected override void UpdateSkill()
    {
        

        if (_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
            Debug.Log("Monster Update Skill");

        }
    }

    void OnHitEvent()
    {

        if(_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);

            if(targetStat.Hp <= 0)
            {
                GameObject.Destroy(targetStat.gameObject);
            }

            if (targetStat.Hp > 0)
            {
                float distance = (_lockTarget.transform.position - transform.position).magnitude;
                if(distance <= _attackRange)
                {
                    State = Define.State.Skill;
                }
                else
                {
                    State = Define.State.Moving;
                }
            }
            else
            {
                State = Define.State.Idle;
            }
        }
        else
        {
            State = Define.State.Idle;
        }
    }
}
