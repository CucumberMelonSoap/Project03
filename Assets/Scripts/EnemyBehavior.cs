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
    [SerializeField] float _attackCooldown = 3f;
    [SerializeField] float _attackTimer;
    private bool _lockedOnPlayer = false;
    private bool _readyToAttack = false;
    private Collider _playerCollider;

    [Header("Attack")]
    [SerializeField] GameObject _attackSystem;
    [SerializeField] float _enemyRadius = 4f;
    [SerializeField] LayerMask _playerLayer;
    [SerializeField] int _damageAmount = 8;
    public int _attackHits = 2;
    

    [Header("Health")]
    [SerializeField] int _health = 250;
    [SerializeField] ParticleSystem _deathVFX = null;

    [Header("Controllers")]
    [SerializeField] LevelController _levelController = null;
    [SerializeField] TargetController _targetController = null;

    // Start is called before the first frame update
    void Start()
    {
        _wanderTimer = _wanderCooldown;
        _attackTimer = _attackCooldown;
        _levelController = FindObjectOfType<LevelController>();
        _targetController = FindObjectOfType<TargetController>();

        SetInitialMovement();
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveToDestination();
        CheckMoveTimer();
        CheckAttackTimer();
        CheckOverlapSphere();

        /*
        if (_lockedOnPlayer)
            FollowPlayer();
        */
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
        _destinationPoint.y = 0;

        if (_currentlyMoving && (Mathf.Abs(Vector3.Distance(transform.position, _destinationPoint)) >= 1))
        {
            Vector3 towardsPoint = transform.forward * _moveSpeed * Time.deltaTime;
            towardsPoint.y = 0;

            transform.position += towardsPoint;
        }
        else
            _currentlyMoving = false;
    }

    private void CheckMoveTimer()
    {
        _wanderTimer -= Time.deltaTime;

        if(_wanderTimer <= 0)
        {
            _destinationPoint = RandomDestination();
            transform.LookAt(_destinationPoint);
            _currentlyMoving = true;
            _wanderTimer = _wanderCooldown;
        }
    }

    private void CheckOverlapSphere()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _enemyRadius, _playerLayer);
        int playerLayerValue = LayerMask.NameToLayer("Player");

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.transform.gameObject.layer == playerLayerValue)
            {
                Vector3 playerPosition = hitCollider.transform.position;
                playerPosition.y = 0;
                transform.LookAt(playerPosition);
                _lockedOnPlayer = true;
                //follow player
                _playerCollider = hitCollider;
                _destinationPoint = playerPosition;
                _wanderTimer = _wanderCooldown = 1.5f;

                if(Mathf.Abs(Vector3.Distance(transform.position, playerPosition)) <= 1.25)
                {
                    if (_readyToAttack)
                        AttackPlayer();
                }
            }
        }
    }

    private void CheckAttackTimer()
    {
        _attackTimer -= Time.deltaTime;

        if(_attackTimer <= 0)
        {
            _readyToAttack = true;
            _attackTimer = _attackCooldown;
        }
    }

    private void FollowPlayer()
    {
        //_currentlyMoving = true;
        Vector3 playerPosition = _playerCollider.transform.position;
        playerPosition.y = 0;
        transform.LookAt(playerPosition);
        //follow player
        _destinationPoint = playerPosition;
    }

    private void AttackPlayer()
    {
        _currentlyMoving = false;
        Vector3 positionAttack = (transform.forward * 1.2f) + (transform.up * 0.75f);
        Destroy(Instantiate(_attackSystem, transform.position + positionAttack, transform.rotation, transform), 0.5f);

        _readyToAttack = false;
    }

    public int GetDamage()
    {
        return _damageAmount;
    }

    public void DamageEnemy(int damage)
    {
        _health -= damage;

        if(_health <= 0)
        {
            Instantiate(_deathVFX, transform.position, transform.rotation);
            _levelController.DisableEnemyIcon();
            _targetController.ReduceEnemyCount();
            _targetController.MoveToNextEnemy();
            Destroy(gameObject);
        }
    }

}
