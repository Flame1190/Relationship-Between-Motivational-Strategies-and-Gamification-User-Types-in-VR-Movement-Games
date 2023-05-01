using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Input;

public class ButtonController : MonoBehaviour
{

    PokeInteractable _thisPokeInteractable;
    void Start()
    {
        _thisPokeInteractable = GetComponent < PokeInteractable > ();
        OVRInput.SetControllerVibration(1, 1000, OVRInput.Controller.RTouch);
        OVRInput.Update();
    }
    private void Update()
    {
        OVRInput.Update();
    }
    private void FixedUpdate()
    {
        OVRInput.FixedUpdate();
    }
    // Update is called once per frame
    public void PrintInteractors()
    {
        IEnumerator enumeratorPoke =_thisPokeInteractable.Interactors.GetEnumerator();
        enumeratorPoke.Reset();
        enumeratorPoke.MoveNext();
        
        PokeInteractor selectedController = (PokeInteractor)enumeratorPoke.Current;
        Handedness currHand = selectedController.GetComponent<ControllerRef>().Handedness;
        print("Current Hand" + currHand.ToString());
        OVRInput.SetControllerVibration(.3f, 0.3f, currHand == Handedness.Left ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch);
        if (currHand == Handedness.Left)
        {
            OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.LTouch);
        } else if (currHand == Handedness.Right)
        {
            print("RIGHT");
            OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
        }
        
    }
}
