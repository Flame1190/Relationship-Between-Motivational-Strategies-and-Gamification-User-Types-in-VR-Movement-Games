using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Detects when this object has been moved enough by the player and then lets the DisruptorController know
public class MovementGoal : MonoBehaviour
{
    [SerializeField]
    string _handType = "Right";

    [SerializeField]
    int _goalScore = 3;

    int _currScore = 0;
    [SerializeField]
    Vector3 _axis = new Vector3(1, 0, 0);

    [SerializeField]
    float _goalDistance = 0.45f;
    float _initialGoalDist;

    Vector3 _initialPos;
    Vector3 _goalPos;

    private void Start()
    {
        _initialGoalDist = _goalDistance;
        _initialPos = transform.position;
        _goalPos = _initialPos + _axis * _goalDistance;
    }
    private void Update()
    {
        Vector3 newVector = _initialPos;
        if ((transform.position.x >= _goalPos.x && _goalDistance > 0) || (transform.position.x <= _goalPos.x && _goalDistance == 0))
        {
            if (_goalDistance > 0)
            {
                _goalDistance = 0;
            } else
            {
                _goalDistance = _initialGoalDist;
            }
            _goalPos = _initialPos + _axis * _goalDistance;
            _currScore += 1;
            if (_handType == "Right")
            {
                HapticsManager.Vibrate(1, 1, HapticsManager.Hand.Right);
            } else
            {
                HapticsManager.Vibrate(1, 1, HapticsManager.Hand.Left);
            }
            if (_currScore >= _goalScore)
            {
                // Done
                if (_handType == "Right")
                {
                    HapticsManager.CancelVibration(HapticsManager.Hand.Right);
                }
                else
                {
                    HapticsManager.CancelVibration(HapticsManager.Hand.Left);
                }
                DisruptorController._instance.SetHandComplete(_handType);
                Destroy(this.gameObject);
            }
        }

    }
}
