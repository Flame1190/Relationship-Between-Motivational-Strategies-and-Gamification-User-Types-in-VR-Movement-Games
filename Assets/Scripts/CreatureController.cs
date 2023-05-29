using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    Vector3 _currPosition;

    Vector3 _newPosition;

    bool _lookingInProgress;
    public bool _movingInProgress;

    [SerializeField]
    Transform _playerTransform;

    protected float _partWayThroughMovement;

    public virtual void MoveToRandomPosition(float timeToPos, float yMultiplier=1, float distanceMultiplier=1, bool doRot=true)
    {
        if (!_movingInProgress)
        {
            Vector3 targetPos = Random.onUnitSphere * UserInformation.reach * 1.25f;
            targetPos = new Vector3(targetPos.x, yMultiplier*(Mathf.Abs(targetPos.y) + UserInformation.height / 2), Mathf.Abs(targetPos.z))*distanceMultiplier;
            MoveToPosition(targetPos, timeToPos, doRot);
            _movingInProgress = true;
        }
    }

    public void MoveToPosition(Vector3 target, float timeToPos, bool doRot=true)
    {
        StartCoroutine(LerpPosition(target, timeToPos, 1, doRot));
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration, float yMultiplier = 1, bool doRot=true)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        if (doRot)
        StartCoroutine(LookAtSmoothly(targetPosition, duration * 0.1f, yMultiplier));
        //StartCoroutine(ScaleBox(new Vector3(0.0001f, _initialBoxScale.y, 0.0001f), duration * 0.1f));

        _partWayThroughMovement = 0;
        while (time < duration)
        {
            _partWayThroughMovement = time / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, _partWayThroughMovement);
            time += Time.deltaTime;
            if (time >= duration * 0.7f && !_lookingInProgress)
            {
                _lookingInProgress = true;
                
                StartCoroutine(LookAtSmoothly(_playerTransform.position, duration * 0.1f, yMultiplier));
                //StartCoroutine(ScaleBox(_initialBoxScale, duration * 0.1f));

            }
            yield return null;
        }
        transform.position = targetPosition;
        _movingInProgress = false;
        CompletedMovement();

    }
    IEnumerator LookAtSmoothly(Vector3 targetPos, float duration,float yMultiplier=1)
    {
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation(targetPos - transform.position);
        if (yMultiplier == 0)
        {
            targetRot = Quaternion.LookRotation(new Vector3(targetPos.x, transform.position.y, targetPos.z) - transform.position);
        }
        else
        {
            targetRot = Quaternion.Euler(new Vector3(targetRot.eulerAngles.x * yMultiplier, targetRot.eulerAngles.y, targetRot.eulerAngles.z));
        }
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.rotation = Quaternion.Slerp(startRot, targetRot, time / duration);
            time += Time.deltaTime;

            yield return null;
        }
        _lookingInProgress = false;
        transform.rotation = targetRot;
    }

    public virtual void CompletedMovement()
    {

    }
}
