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
    [SerializeField]
    GameObject _collectionBox;
    [SerializeField]
    Color _wrongBoxColour;
    [SerializeField]
    Color _rightBoxColour;
    Color _initialBoxColour;


    Vector3 _initialBoxScale;

    private void Start()
    {
        _initialBoxScale = _collectionBox.transform.localScale;
        _initialBoxColour = _collectionBox.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");
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



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tool")
        {
            if (other.transform.name == PhilantropistController._instance.CorrectTool)
            {
                //Destroy(other.gameObject);
                StartCoroutine(ChangeBoxColourThenReturnIt(_rightBoxColour));
                MoveToRandomPosition(2);
                PhilantropistController._instance.CorrectAnswer();
            } else
            {
                // Incorrect
                StartCoroutine(ChangeBoxColourThenReturnIt(_wrongBoxColour));
                PhilantropistController._instance.DisplayAstronaut.SetTrigger("Wrong");
            }
        }
    }

    IEnumerator ChangeBoxColourThenReturnIt(Color thisColor) {
        _collectionBox.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", thisColor);
        yield return new WaitForSeconds(.7f);
        _collectionBox.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", _initialBoxColour);

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
        StartCoroutine(ScaleBox(new Vector3(0.0001f, _initialBoxScale.y, 0.0001f), duration * 0.1f));

        
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            if (time >= duration*0.7f && !lookingInProgress)
            {
                lookingInProgress = true;
                StartCoroutine(LookAtSmoothly(_playerTransform.position, duration * 0.1f));
                StartCoroutine(ScaleBox(_initialBoxScale, duration * 0.1f));

            }
            yield return null;
        }
        transform.position = targetPosition;
        _movingInProgress = false;
    }
    IEnumerator LookAtSmoothly(Vector3 targetPos, float duration)
    {
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation(targetPos - transform.position);
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.rotation = Quaternion.Slerp(startRot, targetRot, time / duration);
            time += Time.deltaTime;
            
            yield return null;
        }
        lookingInProgress = false;
        transform.rotation = targetRot;

    }
    IEnumerator ScaleBox(Vector3 targetScale, float duration)
    {
        float time = 0;
        Vector3 startScale = _collectionBox.transform.localScale;
        while (time < duration)
        {
            _collectionBox.transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            time += Time.deltaTime;

            yield return null;
        }
        _collectionBox.transform.localScale = targetScale;
    }
}
