using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabTest : MonoBehaviour
{
    GameObject prefab;

    GameObject tank;
    // Start is called before the first frame update
    void Start()
    {
        Managers.Resource.Instatiate("Tank");
        
        //Managers.Resource.Destroy(tank);
        Destroy(tank, 3.0f);
    }


}
