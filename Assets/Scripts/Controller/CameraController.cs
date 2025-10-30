using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QuaterView; // enum 배열을 통해 쿼터뷰, 탑뷰 등 모드 설정
    
    [SerializeField]
    Vector3 _delta = new Vector3(0.0f, 6.0f, -5.0f); // 카메라의 위치벡터값 조정
    
    [SerializeField]
    GameObject _player = null; // 할당될 게임 오브젝트를 null로 초기화

    public void SetPlayer(GameObject player) { _player = player; }

    // Start is called before the first frame update
    void Start()
    {
        //_player = GameObject.FindWithTag("Player"); // 플레이어 태그를 가진 오브젝트를 찾아 플레이어 변수에 할당
    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    Debug.DrawRay(ray.origin, ray.direction * 100f, Color.yellow, 5f);

        //    if (Physics.Raycast(ray, out var hit, 100f, ~0))
        //    {
        //        Debug.Log($"[Raycast] HIT: {hit.collider.name}, Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
        //    }
        //    else
        //    {
        //        Debug.Log("MISS: 아무 것도 안 맞음");
        //    }
        //}
    }

    // Update is called once per frame
    void LateUpdate() // Update보다 다음에 실행
    {
        if (_mode == Define.CameraMode.QuaterView)
        {
            if(_player.IsValid() == false)
            {
                return;
            }

            RaycastHit hit;
            if(Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Block"))) // 플레이어의 위치에서 델타벡터까지 델타의 크기만큼 레이캐스트에서 충돌한 객체를 반환, 월이라는 레이어마스크 조건이 있음
            {
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f; // 레이가 맞은 포인트에서 플레이어의 위치를 뺀 값을 크기로 변환함
                transform.position = _player.transform.position + _delta.normalized * dist; // 델타벡터의 크기와 dist를 곱한 값에 플레이어의 현재 위치를 더해줌 -> 이동
            }

            transform.position = _player.transform.position + _delta;
            transform.LookAt(_player.transform); // 플레이어 방향으로 돌아봄
        }
    }

    public void SetQuaterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuaterView;
        _delta = delta;
    }
}
