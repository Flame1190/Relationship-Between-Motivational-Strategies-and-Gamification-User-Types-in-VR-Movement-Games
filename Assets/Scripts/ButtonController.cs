using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using UnityEngine.XR;
using Oculus.Interaction.Input;

public class ButtonController : MonoBehaviour
{

    PokeInteractable _thisPokeInteractable;
    void Start()
    {
        _thisPokeInteractable = GetComponent < PokeInteractable > ();
        //OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
    }
  
    public void StopHandVibration()
    {
        IEnumerator enumeratorPoke = _thisPokeInteractable.Interactors.GetEnumerator();
        enumeratorPoke.Reset();
        enumeratorPoke.MoveNext();

        PokeInteractor selectedController = (PokeInteractor)enumeratorPoke.Current;
        Handedness currHand = selectedController.GetComponent<ControllerRef>().Handedness;

        if (currHand == Handedness.Left)
        {
          
            HapticsManager.CancelVibration(HapticsManager.Hand.Left);
        }
        else if (currHand == Handedness.Right)
        {
            HapticsManager.CancelVibration(HapticsManager.Hand.Right);
        }
    }

    public void PrintInteractors()
    {
        IEnumerator enumeratorPoke =_thisPokeInteractable.Interactors.GetEnumerator();
        enumeratorPoke.Reset();
        enumeratorPoke.MoveNext();
        
        PokeInteractor selectedController = (PokeInteractor)enumeratorPoke.Current;
        Handedness currHand = selectedController.GetComponent<ControllerRef>().Handedness;
        print("Current Hand" + currHand.ToString());
        //OVRInput.SetControllerVibration(1, 1, currHand == Handedness.Left ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch);
        //HapticsManager.Vibrate(1, 1, currHand == Handedness.Left ? HapticsManager.Hand.Left : HapticsManager.Hand.Right);

 
        if (currHand == Handedness.Left)
        {
            //OVRPlugin.Curr = OVRPlugin.InteractionProfile.TouchPro;
            //OVRInput.SetControllerLocalizedVibration(OVRInput.HapticsLocation.Hand, 1, 1, OVRInput.Controller.LTouch);
            print("HERE");
            HapticsManager.Vibrate(10, 0.3f, HapticsManager.Hand.Left);
        } else if (currHand == Handedness.Right)
        {
            print("RIGHT");
            print(OVRInput.GetActiveController());
            HapticsManager.Vibrate(10, 0.3f, HapticsManager.Hand.Right);
            //OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.Touch);
        }
        
    }
}
