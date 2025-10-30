using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object // 오브젝트라는 클래스에서 상속받는 형식이여야 할 것으로 제약
    {
        if(typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');

            if(index >= 0)
            {
                name = name.Substring(index + 1);
            }

            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
            {
                return go as T;
            }
        }

        return Resources.Load<T>(path); // 제너릭 형식 사용법  : 메소드<반환형식>(매개변수)로 반환
    } // 제네릭 타입으로 형식을 정하고 래핑(경로를 지정)

    public GameObject Instatiate(string path, Transform parent = null)
    {
        // 1. original 이미 들고있으면 바로 사용
        GameObject original = Load<GameObject>($"Prefabs/{path}"); // 경로 지정 후 오브젝트를 불러옴
        if(original == null) // 프리팹이 존재하지 않을 때
        {
            Debug.Log($"Failed to load prefab : {path}"); // 경로를 불러올수 없다고 알림
            return null;
        }

        if(original.GetComponent<Poolable>() != null)
        {
            return Managers.Pool.Pop(original, parent).gameObject;
        }

        //2. 풀링된 오브젝트 확인
        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if(go == null)
        {
            return;
        } // 삭제하고자하는 오브젝트가 존재하지 않을 때 다시 반환


        //풀링이 필요한 오브젝트면 매니저에 위탁
        Poolable poolable = go.GetComponent<Poolable>();
        if(poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }
}
