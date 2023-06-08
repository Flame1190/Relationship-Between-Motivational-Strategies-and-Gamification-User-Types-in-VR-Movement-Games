using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawner : ObjectSpawner
{
    [SerializeField]
    bool _spawnAroundPlayer = false;
    

    [SerializeField]
    float _raduis = 1;
    public override void SpawnObject()
    {
        Transform itemToSpawn;
        if (SpawnedObjects.Length > 0)
        {
             itemToSpawn = Instantiate(SpawnedObjects[Random.Range(0, SpawnedObjects.Length)]).transform;
        }
        else
        {
             itemToSpawn = Instantiate(SpawnedObject).transform;
        }
        if (!_spawnAroundPlayer)
        {
            itemToSpawn.position = Random.onUnitSphere * _raduis + transform.position;
        } else
        {
            
            itemToSpawn.position = Random.onUnitSphere * UserInformation.reach;
            itemToSpawn.position = new Vector3(itemToSpawn.position.x, (Mathf.Abs(itemToSpawn.position.y) + UserInformation.height / 2), Mathf.Abs(itemToSpawn.position.z));
        }

    }
    private void Start()
    {
        SpawnObject();
    }
}
