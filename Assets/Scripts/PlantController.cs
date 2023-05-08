using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
     static PlantController _instance;

    private void Awake()
    {
        if (_instance == null || _instance == this)
        {
            _instance = this;
        } else
        {
            Destroy(this);
        }
    }


    public static void GrowFruit()
    {
        _instance.GetComponent<ObjectSpawner>().SpawnObject();
    }
}
