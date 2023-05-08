using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectSpawner : MonoBehaviour
{
    public GameObject SpawnedObject;


    public abstract void SpawnObject();
}
