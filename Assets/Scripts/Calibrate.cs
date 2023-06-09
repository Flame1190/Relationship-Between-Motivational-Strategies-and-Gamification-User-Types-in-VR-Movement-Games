using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibrate : MonoBehaviour
{
    [SerializeField]
    Transform _rightControllerRef;
    [SerializeField]
    Transform _leftControllerRef;
    [SerializeField]
    Transform _headRef;



    [SerializeField]
    GameObject _debugSphere;

    [SerializeField]
    bool _debugMode = false;

    [SerializeField]
    float _displayUserReach;

    [SerializeField]
    float _displayUserHeight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            Debug.Log("A button pressed");
            GetUserReachAndHeight();
        }
        _displayUserReach = UserInformation.reach;
        _displayUserHeight = UserInformation.height;
    }


    void GetUserReachAndHeight()
    {
        float reach = 0;

        reach = Vector3.Distance(new Vector3(0, _rightControllerRef.position.y, 0), _rightControllerRef.position);
        UserInformation.reach = reach;

        print("Reach: " + reach.ToString());


        float height = 0;
        height = _headRef.position.y;
        UserInformation.height = height;

        if (_debugMode)
        {
            _debugSphere.transform.localScale = new Vector3(1, 1, 1) * reach;
            _debugSphere.transform.position = new Vector3(0, height, 0);
        }
    }
}
