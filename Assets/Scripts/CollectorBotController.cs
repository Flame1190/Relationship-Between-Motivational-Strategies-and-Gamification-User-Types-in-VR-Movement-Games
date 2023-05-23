using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorBotController : MonoBehaviour
{
    Vector3 _currPosition;

    Vector3 _newPosition;

    bool lookingInProgress;
    bool _movingInProgress;

    [SerializeField]
    Transform _playerTransform;
    private void Start()
    {
        transform.rotation = Quaternion.LookRotation(_playerTransform.position - transform.position);
    }
    public void MoveToRandomPosition(float timeToPos)
    {
        if (!_movingInProgress)
        {
            Vector3 targetPos = Random.onUnitSphere * UserInformation.reach*1.25f;
            targetPos = new Vector3(targetPos.x, Mathf.Abs(targetPos.y) + UserInformation.height / 2, Mathf.Abs(targetPos.z));
            MoveToPosition(targetPos, timeToPos);
            _movingInProgress = true;
        }
    }
    public void MoveToPosition(Vector3 target, float timeToPos)
    {
        StartCoroutine(LerpPosition(target, timeToPos));
    }
    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        StartCoroutine(LookAtSmoothly(targetPosition, duration * 0.1f));
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            if (time >= duration*0.7f && !lookingInProgress)
            {
                lookingInProgress = true;
                StartCoroutine(LookAtSmoothly(_playerTransform.position, duration * 0.1f));
            }
            yield return null;
        }
        transform.position = targetPosition;
        _movingInProgress = false;
    }
    IEnumerator LookAtSmoothly(Vector3 targetPos, float duration)
    {
        Quaternion targetRot = Quaternion.LookRotation(targetPos - transform.position);
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, time / duration);
            time += Time.deltaTime;
            
            yield return null;
        }
        lookingInProgress = false;
        transform.rotation = Quaternion.LookRotation(targetPos - transform.position);

    }
}
