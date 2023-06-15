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
    List<GameObject> _templates;

    int _currTemplateIndex;

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

    [SerializeField]
    PointableCanvasModule _VREvents;



    public List<GameObject> PreviousLines = new List<GameObject>();

    private void Start()
    {
        OnColourChange("Left", Color.white);
        OnColourChange("Right", Color.white);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _VREvents.enabled = false;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _VREvents.enabled = true;
        }
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

    public void NextTemplate()
    {
        if (_currTemplateIndex < _templates.Count-1)
        {
            ClearAllLines();
            _templates[_currTemplateIndex].SetActive(false);
            _currTemplateIndex++;
            _templates[_currTemplateIndex].SetActive(true);
        }
    }


    public void ClearAllLines()
    {
        for (int i = 0; i < PreviousLines.Count; i++)
        {
            Destroy(PreviousLines[i]);
        }
        PreviousLines.Clear();
    }
}
