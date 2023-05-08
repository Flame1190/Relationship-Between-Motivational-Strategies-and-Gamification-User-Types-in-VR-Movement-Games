using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawner : ObjectSpawner
{
    [SerializeField]
    float _raduis = 1;
    public override void SpawnObject()
    {
        Transform itemToSpawn = Instantiate(SpawnedObject).transform;
        itemToSpawn.position = Random.onUnitSphere * _raduis + transform.position;
    }
    private void Start()
    {
        SpawnObject();
    }
}
