using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class AlienController : CreatureController
{
    public Animator AnimatorController;

    AudioSource _audSource;

    [SerializeField]
    AudioClip _eatingSound;


    [SerializeField]
    bool _makeMove;

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
            if (_makeMove)
            {
                SocialiserController._instance.OnScore();
                StartCoroutine(WaitForIdle());
            } else if (AchieverController._instance != null)
            {
                AchieverController._instance.OnScore();
            } else if (PlayerController._instance != null)
            {
                OVRHand.Hand currHand;
                if (other.gameObject.GetComponent<TagSet>().ContainsTag("NotLeftHand"))
                {
                    currHand = OVRHand.Hand.HandRight;
                } else
                {
                    currHand = OVRHand.Hand.HandLeft;
                }



                PlayerController._instance.OnScore(currHand);
            }
            
                
            
        }
    }
    public void MoveToRandomGroundPosition(float timeToMove)
    {
        MoveToRandomPosition(timeToMove, 0);
    }

    private void Update()
    {
        if (_makeMove)
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
        if (_makeMove)
        {
            AnimatorController.SetTrigger("Eat");
            PlantController._instance.GrowFruit();
        }
        _audSource.PlayOneShot(_eatingSound);
    }
}
