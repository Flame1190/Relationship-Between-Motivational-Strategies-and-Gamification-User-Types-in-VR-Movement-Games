using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using UnityEngine.UI;
public class FreeSpiritController : ExperienceController
{
    #region Singleton Setup
    public static FreeSpiritController _instance;
    private void Awake()
    {
        if (_instance == null || _instance == this)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    [SerializeField]
    Image _leftHandImage;

    [SerializeField]
    Image _rightHandImage;

    [SerializeField]
    PaintBrush _leftPaintBrush;
    [SerializeField]
    PaintBrush _rightPaintBrush;

    [SerializeField]
    RayInteractable _rayInteractable;

    public List<GameObject> PreviousLines = new List<GameObject>();

    private void Start()
    {
        OnColourChange("Left", Color.white);
        OnColourChange("Right", Color.white);
    }
    public void OnColourChange(string handString, Color matColour)
    {
        if (handString == "Left")
        {
            _leftPaintBrush._lineMaterial.SetColor("_Color", matColour);
            //image = GetComponent<Image>();
            var tempColor = matColour;
            tempColor.a = 0.25f;
            _leftHandImage.color = tempColor;
        } else
        {
            _rightPaintBrush._lineMaterial.SetColor("_Color", matColour);
            var tempColor = matColour;
            tempColor.a = 0.25f;
            _rightHandImage.color = tempColor;
        }
    }

    public void OnUndo()
    {
        if (PreviousLines.Count <= 0)
        {
            return;
        }
        GameObject lastElement = PreviousLines[PreviousLines.Count - 1];
        PreviousLines.RemoveAt(PreviousLines.Count - 1);
        Destroy(lastElement);
        
    }
}
