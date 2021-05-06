using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] PlayerMovement _player;
    [SerializeField] float _zoomSpeed = 2f;
    [SerializeField] float _lowBoundZ = -9;
    [SerializeField] float _upBoundZ = -14;

    private Vector3 _fromEdge;
    private Camera _mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = GetComponent<Camera>();
        _player = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
        _fromEdge = _mainCamera.WorldToViewportPoint(_player.transform.position);

        if (_fromEdge.x >= 0.95 && _mainCamera.transform.position.z > _upBoundZ)
        {
            ZoomOut();
        }
        else if(_fromEdge.x < 0.6 && _mainCamera.transform.position.z < _lowBoundZ)
        { 
            ZoomIn();
        }

    }

    private void ZoomOut()
    {       
        _mainCamera.transform.Translate(Vector3.back * _zoomSpeed);
    }

    private void ZoomIn()
    {
        _mainCamera.transform.Translate(Vector3.forward * _zoomSpeed);
    }

}
