using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : CreatureController
{
     public static PlantController _instance;
    [SerializeField]
    Animator _anim;

    [SerializeField]
    ObjectSpawner _objSpawner;

    bool _movingUp = false;

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

    
    public void GrowFruit()
    {
        _anim.SetBool("Down",true);
        MoveToRandomPosition(3, 0, 1.2f, false);


    }

    private void FixedUpdate()
    {
        if (_partWayThroughMovement >= 0.5f && _movingUp == false)
        {
            _movingUp = true;
            _anim.SetBool("Down",false);

            StartCoroutine(WaitForSprout());
        }
        
    }
    IEnumerator WaitForSprout()
    {
        //print("ANIMATION SPEED:"+_anim.GetCurrentAnimatorStateInfo(0).speed.ToString());
        while (!_anim.GetCurrentAnimatorStateInfo(0).IsName("PlantIdle"))
        {
            yield return new WaitForEndOfFrame();

        }
        _objSpawner.SpawnObject();
        print("SPAWNED)");
        _partWayThroughMovement = 0;
        _movingUp = false;
    }


}
