using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : CreatureController
{
    public Animator AnimatorController;

    AudioSource _audSource;

    [SerializeField]
    AudioClip _eatingSound;


    

    private void Start()
    {
        AnimatorController = GetComponent<Animator>();
        _audSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fruit")
        {
            EatFruit();
            Destroy(other.gameObject);
            SocialiserController._instance.OnScore();
            StartCoroutine(WaitForIdle());
        }
    }
    public void MoveToRandomGroundPosition(float timeToMove)
    {
        MoveToRandomPosition(timeToMove, 0);
    }

    private void Update()
    {
        AnimatorController.SetBool("Walking", _movingInProgress);

    }
    IEnumerator WaitForIdle()
    {
        yield return new WaitForSeconds(0.1f);
        //print("ANIMATION SPEED:"+_anim.GetCurrentAnimatorStateInfo(0).speed.ToString());
        while (!AnimatorController.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            yield return new WaitForEndOfFrame();
            print("FRAME");
        }
        MoveToRandomGroundPosition(2);

    }
    void EatFruit()
    {
        AnimatorController.SetTrigger("Eat");
        PlantController._instance.GrowFruit();
        _audSource.PlayOneShot(_eatingSound);
    }
}
