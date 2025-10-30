using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class TestCollision : MonoBehaviour
{
    //1.������ Rigidbody ����
    //2. ������ collider ����
    //3. ��뿡�� collider�� �ִ�.
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision: " + collision.gameObject.name);
    }

    //1. �Ѵ� collider�� �־�� �Ѵ�
    //2. ���� �ϳ��� istrigger�� �����־�� �Ѵ�
    //3. ���� �ϳ��� rigidbody�� �־�� �Ѵ�.

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
            //int mask = (1 << 7) | (1<<6); // ��Ʈ�÷��׸� �̿��� ����Ʈ ���� �̿�, ���̾� ��ȣ�� ������ �������� ������

            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100.0f, mask))
            {
                Debug.Log("Raycast Camera @ " + hit.collider.gameObject.name);
            }

        }





        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        //    // ���� ī�޶��� ȭ�鿡������ �������������� ���Ͱ��� ���Ѵ�.(���콺������x��ǥ, ���콺������ y��ǥ, ī�޶󿡼� ���忡 �ִ� �������� �Ÿ�)
        //    Vector3 dir = mousePos - Camera.main.transform.position; // ���콺 Ŭ�� �������� ����ī�޶� ������Ʈ�� ��ġ�� �� ���� ���Ͱ��� ���Ѵ�.
        //    dir = dir.normalized; // ���� dir���͸� ����ȭ�Ͽ� ũ�⸦ ���Ѵ�.

        //    Debug.DrawRay(Camera.main.transform.position, dir*100.0f, Color.green, 1.0f);

        //    RaycastHit hit;
        //    if (Physics.Raycast(Camera.main.transform.position, dir, out hit, 100.0f)) // ī�޶� ��ġ���� �Ÿ���ŭ�� ���� ���־ ���� ������Ʈ�� ��ȯ, (100.0f��ŭ�� �Ÿ��� ����)
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
        Vector3 look = transform.TransformDirection(Vector3.forward); // ������ǥ�� ���� ��ǥ�� ��ȯ�ؼ� ������ ����
        Debug.DrawRay(transform.position + Vector3.up, look * 10, Color.red); // �÷��̾��� ��ġ���� ������ ��ǥ�� �Ÿ���ŭ ���̸� ����ش�.

        RaycastHit[] hits; // ���̰� ���� ������Ʈ���� ����
        hits = Physics.RaycastAll(transform.position + Vector3.up, look, 10); // ���̰� �����ϴ� ������Ʈ���� ����

        foreach (RaycastHit c_hit in hits) // ����ĳ��Ʈ ������ ���� hits��� ����Ʈ �ȿ��� �ݺ�
        {
            Debug.Log($"Raycast {c_hit.collider.gameObject.name}");
        }
    }
}
