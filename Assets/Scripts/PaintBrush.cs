using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{


    [SerializeField]
    OVRInput.Button _paintButton;

    float _framesPerUpdate = 5;

    

    List<LineRenderer> _lineRenderers = new List<LineRenderer>();

    int _frameCounter = 0;

    [SerializeField]
    Material _lineMaterial;
    [SerializeField]
    float _lineWidth = 0.07f;
    bool _isPainting = false;

    LineRenderer _currLineRenderer;
    private void Update()
    {
        _frameCounter++;
        if (!_isPainting && OVRInput.GetDown(_paintButton))
        {
            _isPainting = true;
            GameObject currGameObject = new GameObject("Paint Line");
            _currLineRenderer = currGameObject.AddComponent<LineRenderer>();
            _currLineRenderer.material = _lineMaterial;
            _currLineRenderer.widthMultiplier = _lineWidth;
            _currLineRenderer.SetPosition(0, transform.position);
            _currLineRenderer.positionCount = 1;
            currGameObject.transform.position = transform.position;
        }
        if (OVRInput.GetUp(_paintButton))
        {
            _isPainting = false;
        }

        if (_frameCounter % _framesPerUpdate == 0 && OVRInput.Get(_paintButton) && _isPainting)
        {
            _frameCounter = 0;
            print("PAINTING");
            _currLineRenderer.positionCount++;
            _currLineRenderer.SetPosition(_currLineRenderer.positionCount - 1, transform.position);
        }
    }
}
