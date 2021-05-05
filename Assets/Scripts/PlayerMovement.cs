//PROJECT 3
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] CharacterController _characterController = null;
    [SerializeField] TargetController _targetController;
    [SerializeField] LevelController _levelController;

    [Header("Movement")]
    [SerializeField] Transform _groundCheck = null;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float _characterSpeed = 12f;
    [SerializeField] float _gravity = -9.81f;
    [SerializeField] float _groundDistance = 0.15f;
    [SerializeField] float _jumpHeight = 3f;
    [SerializeField] LayerMask enemyMask;

    private Vector3 _playerVelocity;
    private bool _isGrounded;
    private bool _isOnEnemy;
    private bool _watchingEnemy;
    private EnemyBehavior _currentEnemy;
    private float _targetRefreshRate;

    // Start is called before the first frame update
    void Start()
    {
        LookAtEnemy();
        _watchingEnemy = true;
        _levelController = GameObject.FindObjectOfType<LevelController>();

    }

    // Update is called once per frame
    void Update()
    {
        //if game is not done
        if(!_levelController.GetGameState())
        {
            UpdateEnemyPosition();

            _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, groundMask);
            _isOnEnemy = Physics.CheckSphere(_groundCheck.position, _groundDistance, enemyMask);

            if (_isGrounded && _playerVelocity.y < 0)
            {
                _playerVelocity.y = -2f;
            }

            if (_isOnEnemy)
            {
                float moveBack = -0.1f;
                _characterController.Move(transform.forward * moveBack);
            }

            float verticalMovement = Input.GetAxis("Horizontal");

            if (verticalMovement < 0 && transform.rotation.y > 0)
                transform.Rotate(0, -180, 0);
            else if (verticalMovement > 0 && transform.rotation.y < 0)
                transform.Rotate(0, 180, 0);
            else
                transform.Rotate(0, 0, 0);

            if (transform.localRotation.y < 0)
                verticalMovement *= -1;

            Vector3 moveAmount = transform.forward * verticalMovement;
            _characterController.Move(moveAmount * _characterSpeed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            }

            _playerVelocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_playerVelocity * Time.deltaTime);

        }
        else
        {
            _isGrounded = false;
            _isOnEnemy = false;
            _watchingEnemy = false;
        }
    }

    public void LookAtEnemy()
    {
        if(_currentEnemy != null && _isGrounded && !_isOnEnemy)
        {
            Vector3 lookAtPoint = _currentEnemy.transform.position;
            lookAtPoint.y = 1;
            transform.LookAt(lookAtPoint);

            _watchingEnemy = false;
        }
    }

    private void UpdateEnemyPosition()
    {
        _targetRefreshRate -= Time.deltaTime;

        if(_targetRefreshRate <= 0 && _isGrounded)
        {
            _currentEnemy = _targetController.GetCurrentTarget();

            if (_watchingEnemy)
                LookAtEnemy();

            _targetRefreshRate = 0.3f;
        }
        
    }

    public void SetEnemyWatch(bool watching)
    {
        _watchingEnemy = watching;
    }

    public bool GetIsGrounded()
    {
        return _isGrounded;
    }
}
