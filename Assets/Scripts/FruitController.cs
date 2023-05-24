using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightPlus;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using Oculus.Interaction.HandGrab;

public class FruitController : MonoBehaviour
{

    public Texture SpeechBubbleTexture;
    HighlightEffect _hEffect;
    AudioSource _audSource;

    [SerializeField]
    HandGrabInteractable _grabInteractable;

    [SerializeField]
    AudioClip _treePickSound;

    [SerializeField]
    AudioClip _pickUpSound;

    [SerializeField]
    Color _highlightColour;

    [SerializeField]
    Color _hoverColor;

    bool onTree = true;

    private void Start()
    {
        _hEffect = GetComponent<HighlightEffect>();
        _audSource = GetComponent<AudioSource>();
    }
    public void HelloWorld()
    {
        print("Hello, World!");
    }
    public void HighlightGrab()
    {
        _hEffect.outlineColor = _highlightColour;
    }
    public void HighlightReset()
    {
        _hEffect.outlineColor = Color.white;
    }
    public void HighlightHover()
    {
        _hEffect.outlineColor = _hoverColor;
    }
    public void GrabFruitAudio()
    {
        IEnumerator enumeratorPoke = _grabInteractable.Interactors.GetEnumerator();
        enumeratorPoke.Reset();
        enumeratorPoke.MoveNext();

        HandGrabInteractor selectedController = (HandGrabInteractor)enumeratorPoke.Current;
        Handedness currHand = selectedController.GetComponent<HandRef>().Handedness;
        HapticsManager.Vibrate(1, 1, currHand == Handedness.Left ? HapticsManager.Hand.Left : HapticsManager.Hand.Right);
        if (onTree)
        {
            onTree = false;
            _audSource.PlayOneShot(_treePickSound);
        }
        _audSource.PlayOneShot(_pickUpSound);
    }
}
