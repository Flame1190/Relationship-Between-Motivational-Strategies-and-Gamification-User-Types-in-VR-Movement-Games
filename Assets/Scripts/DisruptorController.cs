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


    [SerializeField]
    Mesh _tntMesh;
    [SerializeField]
    Material[] _tntMaterials;
    [SerializeField]
    float _tntSpawnChance = 0.1f;

    bool _tntSelected = false;

    List<GameObject> _lastPlacedBuilding = new List<GameObject>();

    List<GameObject> _currSpawnedMovers = new List<GameObject>();

    [SerializeField]
    AudioClip _tntSound;
    [SerializeField]
    GameObject _tntParticle;

    AudioSource _tntAudioSource;

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
        _tntAudioSource = NextBlockLocation.GetComponent<AudioSource>();
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

            _tntSelected = _potentialSelections[newIndex].transform.Find("Display-Object").transform.tag == "isTNT";
            
            
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
        foreach (GameObject mover in _currSpawnedMovers)
        {
            Destroy(mover);
        }
        _currSpawnedMovers = new List<GameObject>();
        RightComplete = false;
        LeftComplete = false;
        float height = UserInformation.height;
        float radius = UserInformation.reach;

        Vector3 position = Random.onUnitSphere * radius;
        position = new Vector3(position.x, Mathf.Abs(position.y) + height / 2, Mathf.Abs(position.z));
        currLeftMove = Instantiate(movementLeft, position, Quaternion.identity, this.transform);

        Vector3 position2 = Random.onUnitSphere * radius;
        print(position2.y);
        position2 = new Vector3(position2.x, Mathf.Abs(position2.y) + height / 2, Mathf.Abs(position2.z));
        currRightMove = Instantiate(movementRight, position2, Quaternion.identity, this.transform);

        _currSpawnedMovers.Add(currLeftMove);
        _currSpawnedMovers.Add(currRightMove);
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
        foreach (GameObject mover in _currSpawnedMovers)
        {
            Destroy(mover);
        }
        _currSpawnedMovers = new List<GameObject>();
        CurrentScore++;
        print("COMPLETED TASK!");
        LeftComplete = false;
        RightComplete = false;

        if (!_tntSelected)
        {
            GameObject newShape = Instantiate(_potentialSelections[_currSelectionIndex].transform.Find("Display-Object").gameObject, NextBlockLocation.position, NextBlockLocation.rotation);
            newShape.GetComponent<DisplayObjectHover>().StartOffsetChange(0, 0);
            Destroy(newShape.GetComponent<DisplayObjectHover>());
            Transform newTransform = newShape.transform;
            newTransform.localScale *= placeScaleFactor;
            newTransform.eulerAngles = new Vector3(-90, 0, (int)Random.Range(1, 8) * 45);

            _lastPlacedBuilding.Add(newShape);
            // Reset the buttons

            GameObject prevSelected = _potentialSelections[_currSelectionIndex].transform.Find("Platform").gameObject;
            prevSelected.GetComponent<MeshRenderer>().material.color = _unselectedColor;
            _potentialSelections[_currSelectionIndex].transform.Find("Display-Object").GetComponent<DisplayObjectHover>().StartOffsetChange(0, 1);

            _currSelectionIndex = -1;
            _currBuildingHeight++;
            if (_currBuildingHeight < _maxBuildingHeight)
            {
                NextBlockLocation.position = new Vector3(NextBlockLocation.position.x, NextBlockLocation.position.y + placeScaleFactor / 20, NextBlockLocation.position.z);
            }
            else
            {
                _currBuildingHeight = 0;
                _currBuildingNumber++;
                _lastPlacedBuilding = new List<GameObject>();
                NextBlockLocation.position = new Vector3(NextBlockLocation.position.x + placeScaleFactor / 4 + placeScaleFactor / 40, placeScaleFactor / 40, NextBlockLocation.position.z);
            }
        }
        else
        {
            // TNT set off
            foreach (GameObject obj in _lastPlacedBuilding)
            {
                Destroy(obj);
            }
            GameObject prevSelected = _potentialSelections[_currSelectionIndex].transform.Find("Platform").gameObject;
            prevSelected.GetComponent<MeshRenderer>().material.color = _unselectedColor;
            _potentialSelections[_currSelectionIndex].transform.Find("Display-Object").GetComponent<DisplayObjectHover>().StartOffsetChange(0, 1);

            _currSelectionIndex = -1;
            _lastPlacedBuilding = new List<GameObject>();
            NextBlockLocation.position = new Vector3(NextBlockLocation.position.x, placeScaleFactor / 40, NextBlockLocation.position.z);
            _currBuildingHeight = 0;
            if (_tntSound != null)
            {
                _tntAudioSource.PlayOneShot(_tntSound);
            }
            Instantiate(_tntParticle, NextBlockLocation.position, NextBlockLocation.rotation);


        }
        // Replace all of the button items
        int numOfTnt;
        numOfTnt = 0;
        foreach (GameObject display in _potentialSelections)
            {
                GameObject displayObject = display.transform.Find("Display-Object").gameObject;
            if (_currBuildingHeight < _maxBuildingHeight - 1)
            {

                displayObject.GetComponent<MeshFilter>().mesh = _allMeshes[(int)Random.Range(0, _allMeshes.Length)];
                displayObject.GetComponent<MeshRenderer>().materials = new Material[1] { _allMaterials[(int)Random.Range(0, _allMaterials.Length)] };
                displayObject.transform.tag = "Building";
                if (_currBuildingHeight >= 1)
                {
                    if (Random.Range(0.0f, 1.0f) <= _tntSpawnChance && numOfTnt <= 0)
                    {
                        numOfTnt++;
                        displayObject.transform.tag = "isTNT";
                        displayObject.GetComponent<MeshFilter>().mesh = _tntMesh;
                        displayObject.GetComponent<MeshRenderer>().materials = _tntMaterials;
                    }
                }
            }
            else
            {
                displayObject.transform.tag = "Building";
                displayObject.GetComponent<MeshFilter>().mesh = _roofMeshes[(int)Random.Range(0, _roofMeshes.Length)];
                displayObject.GetComponent<MeshRenderer>().materials = new Material[1] {_allMaterials[(int)Random.Range(0, _allMaterials.Length)]};
                }
            }
        

    }
    
}
