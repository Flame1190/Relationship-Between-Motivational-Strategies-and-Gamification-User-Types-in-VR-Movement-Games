using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour
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
        }
    }
    void EatFruit()
    {
        AnimatorController.SetTrigger("Eat");
        PlantController.GrowFruit();
        _audSource.PlayOneShot(_eatingSound);
    }
}
