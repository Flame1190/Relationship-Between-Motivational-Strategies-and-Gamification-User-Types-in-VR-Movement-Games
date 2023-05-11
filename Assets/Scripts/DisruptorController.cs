using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisruptorController : MonoBehaviour
{
    int _currSelectionIndex = -1;
    [SerializeField]
    GameObject[] _potentialSelections;

    [SerializeField]
    Color _selectedColor;

    [SerializeField]
    Color _unselectedColor;

    public void OnSelectObject(int newIndex)
    {
        if (newIndex != _currSelectionIndex) {
            if (_currSelectionIndex != -1)
            {
                GameObject prevSelected = _potentialSelections[_currSelectionIndex].transform.Find("Platform").gameObject;
                prevSelected.GetComponent<MeshRenderer>().material.color = _unselectedColor;
                _potentialSelections[_currSelectionIndex].transform.Find("Display-Object").GetComponent<DisplayObjectHover>().StartOffsetChange(0, 1);
            }
            _currSelectionIndex = newIndex;
            _potentialSelections[newIndex].transform.Find("Platform").GetComponent<MeshRenderer>().material.color = _selectedColor;
            _potentialSelections[newIndex].transform.Find("Display-Object").GetComponent<DisplayObjectHover>().StartOffsetChange(.1f, 1);
        }
    }

    
}
