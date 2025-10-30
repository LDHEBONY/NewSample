using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QuaterView; // enum �迭�� ���� ���ͺ�, ž�� �� ��� ����
    
    [SerializeField]
    Vector3 _delta = new Vector3(0.0f, 6.0f, -5.0f); // ī�޶��� ��ġ���Ͱ� ����
    
    [SerializeField]
    GameObject _player = null; // �Ҵ�� ���� ������Ʈ�� null�� �ʱ�ȭ

    public void SetPlayer(GameObject player) { _player = player; }

    // Start is called before the first frame update
    void Start()
    {
        //_player = GameObject.FindWithTag("Player"); // �÷��̾� �±׸� ���� ������Ʈ�� ã�� �÷��̾� ������ �Ҵ�
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
        //        Debug.Log("MISS: �ƹ� �͵� �� ����");
        //    }
        //}
    }

    // Update is called once per frame
    void LateUpdate() // Update���� ������ ����
    {
        if (_mode == Define.CameraMode.QuaterView)
        {
            if(_player.IsValid() == false)
            {
                return;
            }

            RaycastHit hit;
            if(Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Block"))) // �÷��̾��� ��ġ���� ��Ÿ���ͱ��� ��Ÿ�� ũ�⸸ŭ ����ĳ��Ʈ���� �浹�� ��ü�� ��ȯ, ���̶�� ���̾��ũ ������ ����
            {
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f; // ���̰� ���� ����Ʈ���� �÷��̾��� ��ġ�� �� ���� ũ��� ��ȯ��
                transform.position = _player.transform.position + _delta.normalized * dist; // ��Ÿ������ ũ��� dist�� ���� ���� �÷��̾��� ���� ��ġ�� ������ -> �̵�
            }

            transform.position = _player.transform.position + _delta;
            transform.LookAt(_player.transform); // �÷��̾� �������� ���ƺ�
        }
    }

    public void SetQuaterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuaterView;
        _delta = delta;
    }
}
