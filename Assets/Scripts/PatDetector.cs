using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Input;

public class PatDetector : MonoBehaviour
{
    [SerializeField]
    AlienController _alien;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Controller")
        {
            ObjectReference test = other.GetComponent<ObjectReference>();
            print(test == null);
            GameObject gameInteractorReference = test.ThisObjectReferences;
            GrabInteractor grabInteractor = gameInteractorReference.GetComponent<GrabInteractor>();

            if (grabInteractor.State == InteractorState.Normal || grabInteractor.State == InteractorState.Hover)
            {
                _alien.AnimatorController.SetTrigger("Pat");
            }
        }
    }
}
