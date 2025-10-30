using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


//1. 위치 벡터
//2. 방향 벡터



public class PlayControler : BaseController
{
    PlayerStat _stat;

  

    // Start is called before the first frame update

    bool _moveToDest = false;
    Vector3 _destPos;
    Button_UI button;
    Inven_UI inven;
    bool _soundplayed = false;
    IEnumerator Isoundcoroot;
    string LayerBox;

  
    
    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
    

    //프레임 체크
    float _lastMouseInputTime = 0f;
    const float NO_INPUT_THRESHOLD = 2f;

    bool _stopSkill = false;




    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Player;
        _stat = gameObject.GetComponent<PlayerStat>();

        Debug.Log("[INIT] Start() 호출됨");

        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard; // 키액션이란 이벤트를 구독해주고 입력이 감지될 때 호출할 함수를 정의한다.

        Managers.Input.MouseAction -= OnMouseEvent; // Managers에 있는 InputManager변수 Input를 빼온다.
        Managers.Input.MouseAction += OnMouseEvent;
        Debug.Log("[PlayerController] MouseAction 구독 완료");


        if (gameObject.GetComponentInChildren<HPBar_UI>() == null)
        {
            Managers.UI.MakeWorldSpaceUI<HPBar_UI>("UI_HPBar", this.transform);
        }
    
        // 마우스액션 이벤트를 구독해주고 입력감지했을때 호출할 함수 정의

        //Managers.Resource.Instatiate("UI/PopUp/Scorebutton_UI");
        //button = Managers.UI.ShowPopUpUI<Button_UI>("Scorebutton_UI"); // ShowPopUI<클래스 포함>("프리팹 이름")

        if (Camera.main == null)
            Debug.LogError("Camera exception == null");
        else
            Debug.Log($"Camera.main = {Camera.main.name}");

        //Managers.UI.ClosePopUpUI(ui);

        //Temp
        //inven = Managers.UI.ShowSceneUI<Inven_UI>();

        //Isoundcoroot = soundcoroot();

        _lastMouseInputTime = Time.time;
    }

    float wait_run_ratio = 0;

    protected override void UpdateDie()
    {
        //아무것도 못함
    }

    IEnumerator soundcoroot()
    {
        yield return new WaitForSecondsRealtime(1.0f);

        if (_soundplayed == false)
        {
            //Managers.Sound.Play(Define.Sound.Effect, "UnityChan/univ0006");
            _soundplayed = true;
        }

    }

    private void OnGUI()
    {
        string status;
        

        if (inven == null)
            status = "inven == null (할당 안됨)";
        else if (!inven.gameObject.activeSelf)
            status = "inven 비활성 상태 (할당은 됨)";
        else
            status = "inven 활성 (정상 작동 중)";

        GUI.Box(new Rect(10, 10, 200, 25), $"State : {State}");
        GUI.Box(new Rect(10, 40, 200, 25), $"Input : {Input.GetMouseButton(0)}");
        GUI.Box(new Rect(10, 70, 200, 25), $"Inven_UI : {status}");
        GUI.Box(new Rect(10, 100, 200, 25), $"PointLayer : {LayerBox}");
        GUI.Box(new Rect(10, 130, 200, 25), $"Target : {_lockTarget}");

    }


    protected override void UpdateMoving()
    {
        //StartCoroutine(Isoundcoroot);

        if(_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
           
            float distance = (_destPos - transform.position).magnitude;

        if (distance <= 1)
            {
                State = Define.State.Skill;
                return;
            }


        }


        Vector3 dir = _destPos - transform.position;
        dir.y = 0;
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
            
        }
        else
        {

            //transform.position += dir.normalized * moveDist;

            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {

                if (Input.GetMouseButtonDown(0) == false)
                {
                    State = Define.State.Idle;
                }
                
                return;
            }

            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
            //transform.LookAt(_destPos);
            
        }
    

        // 애니메이션 처리
        // wait_run_ratio = Mathf.Lerp(wait_run_ratio, 1, 10.0f * Time.deltaTime);
        
        //anim.SetFloat("wait_run_ratio", wait_run_ratio);
        //anim.Play("RUN_WAIT");
       

        Managers.UI.ClosePopUpUI(button);
    } // 부모의 가상함수 호출, 덮어씌울때 부모의 가상함수가 보호될것

    protected override void UpdateIdle()
    {
        //StopCoroutine(Isoundcoroot);
        _soundplayed = false;
        //Managers.Sound.Play(Define.Sound.Effect, "UnityChan/univ0006");
        // wait_run_ratio = Mathf.Lerp(wait_run_ratio, 0, 10.0f * Time.deltaTime);
        
        // anim.SetFloat("wait_run_ratio", wait_run_ratio);
        // anim.Play("RUN_WAIT");
        //현재 게임 상태 넘겨주기
        

        if (button == null)
        {
            button = Managers.UI.ShowPopUpUI<Button_UI>("Scorebutton_UI");
        }
    }

    protected override void UpdateSkill()
    {   
        if(_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(this.transform.position, ray.direction, Color.yellow, 2.0f); // 스킬상태작동확인용 레이, 마우스따라서 방향이 움직임
            Debug.Log("UpdateSkill");
        }
              
    }

    void OnHitEvent()
    {
        if(_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);

        }

        Debug.Log("OnHitEvent()");
        
        if(_stopSkill)
        {
            State = Define.State.Idle; // 스킬이 끝나면 Idle상태로 전환
        }
        else
        {
            State = Define.State.Skill; // 스킬이 끝나지 않으면 Skill상태로 유지
        }


    }

    void Update()
    {

        // 마우스 커서 업데이트
        //if (_moveToDest)
        //{
        //    Vector3 dir = _destPos - transform.position;
        //    if(dir.magnitude < 0.0001f)
        //    {
        //        _moveToDest = false; // 목적지가 가까워졌을때 movetodest를 false로 전환
        //    }
        //    else
        //    {
        //        float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude); // min과 max사이의 값을 보장해주어야한다.;


        //        transform.position += dir.normalized * moveDist;

        //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        //        transform.LookAt(_destPos);
        //        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        //    }
        //}

        switch (State)
        {
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Moving:
                UpdateMoving();
                break;
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Skill:
                UpdateSkill();
                break;
        }

        //if(_moveToDest)
        //{
        //    wait_run_ratio = Mathf.Lerp(wait_run_ratio, 1, 10.0f * Time.deltaTime);
        //    Animator anim = GetComponent<Animator>();
        //    anim.SetFloat("wait_run_ratio", wait_run_ratio);
        //    anim.Play("RUN_WAIT");
        //}
        //else
        //{
        //    wait_run_ratio = Mathf.Lerp(wait_run_ratio, 0, 10.0f * Time.deltaTime);
        //    Animator anim = GetComponent<Animator>();
        //    anim.SetFloat("wait_run_ratio", wait_run_ratio);
        //    anim.Play("RUN_WAIT");
        //}

        if (_soundplayed == true)
        {
            Debug.Log(State);
        }

        if(Input.GetMouseButtonDown(0)||Input.GetMouseButton(0)||Input.GetMouseButtonUp(0))
        {
            _lastMouseInputTime = Time.time; // 마우스 입력이 감지되면 시간을 갱신
        }
        if(Time.time - _lastMouseInputTime >= NO_INPUT_THRESHOLD)
        {
            State = Define.State.Idle; // 입력이 없으면 Idle 상태로 전환
            
        }

    }

    // Update is called once per frame
    //GameObject(Player)
    //Transform
    //PlayControler

    void UpdateMouseCursor()
    {
       
    }

    void OnKeyboard()
    {
        //transformdirection(local->world)
        // 여기서 사용되는 좌표는 월드 좌표계 기준
        // InverseTransformdirection(world->local)
        //transform.Translate(자기가 바라보는 기준으로 위치변환)

        //transform.rotation
        //_yangle += Time.deltaTime * _speed;

        //절대 회전값
        //transform.eulerAngles = new Vector3(0.0f, _yangle, 0.0f);

        //+- delta
        //transform.Rotate(new Vector3(0.0f, Time.deltaTime * 100.0f, 0.0f));

        //transform.rotation = Quaternion.Euler(new Vector3(0.0f, _yangle, 0.0f));
        //짐벌락 : 두축이 회전하여 겹칠때 일어나는 현상
        //쿼터니언 : 회전할때 축의 기준이 되는 값

        if (Input.GetKey(KeyCode.W))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.forward);
            //transform.position += transform.TransformDirection(Vector3.forward * Time.deltaTime * _speed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
            transform.position += (Vector3.forward * Time.deltaTime * _stat.MoveSpeed);
            //Slerp : 목표의회전을 일정 시간을 두고 실행한다.

        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            transform.position += (Vector3.back * Time.deltaTime * _stat.MoveSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            transform.position += (Vector3.left * Time.deltaTime * _stat.MoveSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            transform.position += (Vector3.right * Time.deltaTime * _stat.MoveSpeed);
        }
        //transform

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inven == null)
            {
                inven = Managers.UI.ShowSceneUI<Inven_UI>("Inven_UI");
            }
            else
            {

            }
        }

        _moveToDest = false;
    }


    
    void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (State) 
        {
            case Define.State.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Skill:
                {
                    if(evt == Define.MouseEvent.PointerDown)
                    {
                        _stopSkill = true;
                    }
                }
                break;

        }


    }

    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        Debug.Log("[CLICK] OnMouseClicked() 호출됨");

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("[Raycast] Blocked by UI");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.red, 1.0f);
        RaycastHit hit;
        //Debug.Log("OnMouseClick");

        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);

        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                {
                    if (raycastHit)
                    {
                        _destPos = hit.point;
                        State = Define.State.Moving;
                        _stopSkill = false;

                        Debug.Log("Raycast Camera" + hit.collider.gameObject.tag);
                        Debug.Log("[State]_state = Moving");

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                        {
                            //Debug.Log("Monster Click");
                            _lockTarget = hit.collider.gameObject;
                            LayerBox = "Monster Click";
                        }
                        else
                        {
                            //Debug.Log("Ground Click");
                            _lockTarget = null;
                            //LayerBox = "Ground Click";
                        }
                    }
                }
                break;
            case Define.MouseEvent.Press:
                {
                    if (_lockTarget == null && raycastHit)
                    {
                        _destPos = hit.point;
                    }

                }
                break;
            case Define.MouseEvent.PointerUp:
                {
                    _stopSkill = true;
                }
                break;

            default:
                Debug.Log("Raycast Camera" + "No Hit");
                break;


        }

    }
}
