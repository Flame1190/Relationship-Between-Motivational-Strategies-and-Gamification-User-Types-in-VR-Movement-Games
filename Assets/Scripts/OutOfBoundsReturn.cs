using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsReturn : MonoBehaviour
{
    Vector3 _initialPos;
    bool waitingForOutOfBounds;
    [Tooltip("Time the object can be out of reach before it's position is reset")]
    [SerializeField]
    float _timeToReset;
    void Start()
    {
        _initialPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(new Vector3(0, UserInformation.height*2/3, 0), transform.position) > UserInformation.reach && !waitingForOutOfBounds)
        {
            StartCoroutine(CountdownRespawn());
            waitingForOutOfBounds = true;
        }
    }
    IEnumerator CountdownRespawn()
    {
        yield return new WaitForSeconds(_timeToReset);
        if (waitingForOutOfBounds)
        {
            transform.position = _initialPos;
            waitingForOutOfBounds = false;

        }
    }
}
