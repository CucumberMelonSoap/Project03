///PROJECT 3
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] bool _currentlyMoving;
    private Vector3 _destinationPoint;

    [Header("Bounds")]
    [SerializeField] int _xLowerBound = 0;
    [SerializeField] int _xUpperBound = 25;
    [SerializeField] int _zLowerBound = 0;
    [SerializeField] int _zUpperBound = 10;

    [Header("Timers")]
    [SerializeField] float _wanderCooldown = 2f;
    [SerializeField] float _wanderTimer;

    [Header("Health")]
    [SerializeField] int _health = 250;

    // Start is called before the first frame update
    void Start()
    {
        _wanderTimer = _wanderCooldown;
        SetInitialMovement();
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveToDestination();
    }

    private Vector3 RandomDestination()
    {
        int xValue = Random.Range(_xLowerBound, _xUpperBound + 1);
        int zValue = Random.Range(_zLowerBound, _zUpperBound + 1);

        Vector3 destination = new Vector3(xValue, 0f, zValue);
        Debug.Log(destination);

        return destination;
    }

    private void SetInitialMovement()
    {
        _destinationPoint = RandomDestination();
        transform.LookAt(_destinationPoint);
        _currentlyMoving = true;
    }

    private void MoveToDestination()
    {
        if (_currentlyMoving && (Mathf.Abs(Vector3.Distance(transform.position, _destinationPoint)) >= 0.8))
        {
            Vector3 towardsPoint = transform.forward * _moveSpeed * Time.deltaTime;
            towardsPoint.y = 0;

            transform.position += towardsPoint;
        }
        else
            _currentlyMoving = false;
    }
}
