using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] TargetController _targetController;
    [SerializeField] float _zoomSpeed = 2f;
    [SerializeField] float _lowBoundZ = -9;
    [SerializeField] float _upBoundZ = -14;

    private float _distanceBetween;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FindDistance();

        if(_distanceBetween <= 15)
        {

        }
        else
        {

        }
    }

    private void FindDistance()
    {
        _distanceBetween = Vector3.Distance(_player.transform.position, _targetController.GetCurrentTarget().transform.position);
    }
}
