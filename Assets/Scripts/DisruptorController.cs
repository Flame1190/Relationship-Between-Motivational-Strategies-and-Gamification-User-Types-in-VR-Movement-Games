using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisruptorController : ExperienceController
{
    int _currSelectionIndex = -1;
    [SerializeField]
    GameObject[] _potentialSelections;

    [SerializeField]
    Color _selectedColor;

    [SerializeField]
    Color _unselectedColor;

    public static DisruptorController _instance;

    [SerializeField]
    GameObject movementLeft;
    [SerializeField]
    GameObject movementRight;

    GameObject currLeftMove;
    GameObject currRightMove;

    public bool LeftComplete = false;
    public bool RightComplete = false;
    [SerializeField]
    Transform NextBlockLocation;

    float placeScaleFactor = 10;
    [SerializeField]
    Mesh[] _allMeshes;
    [SerializeField]
    Mesh[] _roofMeshes;
    [SerializeField]
    int _maxBuildingHeight = 7;
    int _currBuildingHeight = 0;
    int _currBuildingNumber = 0;
    [SerializeField]
    Material[] _allMaterials;

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
    private void Start()
    {
        NextBlockLocation.position = new Vector3(NextBlockLocation.position.x,placeScaleFactor / 40, NextBlockLocation.position.z);
    }
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

            SpawnMovementObjects();
        }
    }

    public void SpawnMovementObjects()
    {
        if (currLeftMove != null && currRightMove != null)
        {
            Destroy(currRightMove);
            Destroy(currLeftMove);
        }
        float height = UserInformation.height;
        float radius = UserInformation.reach;

        Vector3 position = Random.onUnitSphere * radius;
        position = new Vector3(position.x, Mathf.Abs(position.y) + height / 2, Mathf.Abs(position.z));
        currLeftMove = Instantiate(movementLeft, position, Quaternion.identity, this.transform);

        Vector3 position2 = Random.onUnitSphere * radius;
        print(position2.y);
        position2 = new Vector3(position2.x, Mathf.Abs(position2.y) + height / 2, Mathf.Abs(position2.z));
        currRightMove = Instantiate(movementRight, position2, Quaternion.identity, this.transform);
    }
    public void SetHandComplete(string hand)
    {
        if (hand == "Right")
        {
            RightComplete = true;
        }
        else if (hand == "Left")
        {
            LeftComplete = true;
        }
        if (RightComplete && LeftComplete)
        {
            CompletedTask();
        }
    }

    void CompletedTask()
    {
        CurrentScore++;
        print("COMPLETED TASK!");
        LeftComplete = false;
        RightComplete = false;
        GameObject newShape = Instantiate(_potentialSelections[_currSelectionIndex].transform.Find("Display-Object").gameObject, NextBlockLocation.position, NextBlockLocation.rotation);
        newShape.GetComponent<DisplayObjectHover>().StartOffsetChange(0, 0);
        Destroy(newShape.GetComponent<DisplayObjectHover>());
        Transform newTransform = newShape.transform;
        newTransform.localScale *= placeScaleFactor;
        newTransform.eulerAngles = new Vector3(-90, 0, (int) Random.Range(1,8)*45);
        // Reset the buttons

        GameObject prevSelected = _potentialSelections[_currSelectionIndex].transform.Find("Platform").gameObject;
        prevSelected.GetComponent<MeshRenderer>().material.color = _unselectedColor;
        _potentialSelections[_currSelectionIndex].transform.Find("Display-Object").GetComponent<DisplayObjectHover>().StartOffsetChange(0, 1);

        _currSelectionIndex = -1;
        _currBuildingHeight++;
        if (_currBuildingHeight < _maxBuildingHeight)
        {
            NextBlockLocation.position = new Vector3(NextBlockLocation.position.x, NextBlockLocation.position.y + placeScaleFactor / 20, NextBlockLocation.position.z);
        } else
        {
            _currBuildingHeight = 0;
            _currBuildingNumber++;
            NextBlockLocation.position = new Vector3(NextBlockLocation.position.x + placeScaleFactor/4 + placeScaleFactor / 40, placeScaleFactor / 40, NextBlockLocation.position.z);
        }
        // Replace all of the button items

        foreach (GameObject display in _potentialSelections)
        {
            GameObject displayObject = display.transform.Find("Display-Object").gameObject;
            if (_currBuildingHeight < _maxBuildingHeight-1)
            {
                
                displayObject.GetComponent<MeshFilter>().mesh = _allMeshes[(int)Random.Range(0, _allMeshes.Length)];
                displayObject.GetComponent<MeshRenderer>().material = _allMaterials[(int)Random.Range(0, _allMaterials.Length)];
            } else
            {
                displayObject.GetComponent<MeshFilter>().mesh = _roofMeshes[(int)Random.Range(0, _roofMeshes.Length)];
                displayObject.GetComponent<MeshRenderer>().material = _allMaterials[(int)Random.Range(0, _allMaterials.Length)];
            }
        }

    }
    
}
