using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class TestCollision : MonoBehaviour
{
    //1.나한테 Rigidbody 있음
    //2. 나한테 collider 있음
    //3. 상대에게 collider가 있다.
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision: " + collision.gameObject.name);
    }

    //1. 둘다 collider가 있어야 한다
    //2. 둘중 하나는 istrigger가 켜져있어야 한다
    //3. 둘중 하나는 rigidbody가 있어야 한다.

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger ! : " + other.gameObject.name);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.blue, 1.0f);

            LayerMask mask = LayerMask.GetMask("Monster") | LayerMask.GetMask("Wall");
            //int mask = (1 << 7) | (1<<6); // 비트플래그를 이용한 시프트 연산 이용, 레이어 번호는 이진수 연산으로 구성됨

            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100.0f, mask))
            {
                Debug.Log("Raycast Camera @ " + hit.collider.gameObject.name);
            }

        }





        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        //    // 메인 카메라의 화면에서부터 월드지점까지의 벡터값을 구한다.(마우스포지션x좌표, 마우스포지션 y좌표, 카메라에서 월드에 있는 평면까지의 거리)
        //    Vector3 dir = mousePos - Camera.main.transform.position; // 마우스 클릭 지점에서 메인카메라 오브젝트의 위치를 뺀 값의 벡터값을 구한다.
        //    dir = dir.normalized; // 구한 dir벡터를 정규화하여 크기를 구한다.

        //    Debug.DrawRay(Camera.main.transform.position, dir*100.0f, Color.green, 1.0f);

        //    RaycastHit hit;
        //    if (Physics.Raycast(Camera.main.transform.position, dir, out hit, 100.0f)) // 카메라 위치부터 거리만큼의 빛을 쏴주어서 맞힌 오브젝트를 반환, (100.0f만큼의 거리로 제한)
        //    {
        //        Debug.Log($"Raycast Camera" + hit.collider.gameObject.name);
        //    }

        //    //RaycastHit hit;
        //    //if (Physics.Raycast(transform.position + Vector3.up, look, out hit, 10))
        //    //{
        //    //    Debug.Log("Raycast : " + hit.collider.gameObject.name);
        //    //}
        //}
    }

    void StraightRay()
    {
        Vector3 look = transform.TransformDirection(Vector3.forward); // 로컬좌표를 월드 좌표로 전환해서 변수에 저장
        Debug.DrawRay(transform.position + Vector3.up, look * 10, Color.red); // 플레이어의 위치에서 지정된 좌표의 거리만큼 레이를 쏘아준다.

        RaycastHit[] hits; // 레이가 맞춘 오브젝트들을 저장
        hits = Physics.RaycastAll(transform.position + Vector3.up, look, 10); // 레이가 관통하는 오브젝트들을 감지

        foreach (RaycastHit c_hit in hits) // 레이캐스트 변수를 만들어서 hits라는 리스트 안에서 반복
        {
            Debug.Log($"Raycast {c_hit.collider.gameObject.name}");
        }
    }
}
