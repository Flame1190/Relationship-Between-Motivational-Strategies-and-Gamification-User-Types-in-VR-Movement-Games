using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayObjectHover : MonoBehaviour
{

    [SerializeField]
    float _rotateSpeed;

    [SerializeField]
    float _bobSpeed;

    [SerializeField]
    float _bobAmplitude;

    Vector3 _initialPos;

    public float Offset = 0;


    private void Start()
    {
        _initialPos = transform.position;
    }
    private void FixedUpdate()
    {
        transform.eulerAngles += new Vector3(0, _rotateSpeed, 0);
        transform.position = new Vector3(transform.position.x, _bobAmplitude*(Mathf.Sin(Time.time*_bobSpeed)) + _initialPos.y + Offset, transform.position.z);
    }
    public void StartOffsetChange(float newOffset, float waitTime)
    {
        StopAllCoroutines();
        StartCoroutine(SmoothOffset(newOffset, waitTime));
    }
    public void StopRotationAndBob()
    {
        _rotateSpeed = 0;
        _bobSpeed = 0;
    }
    public IEnumerator SmoothOffset(float newOffset, float waitTime)
    {
        float elapsedTime = 0;
        while (elapsedTime < waitTime)
        {
            Offset = Mathf.Lerp(Offset, newOffset, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }
        // Make sure we got there
        Offset = newOffset;
        yield return null;
    }
}
