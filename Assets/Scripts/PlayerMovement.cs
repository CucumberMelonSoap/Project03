//PROJECT 3
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController _characterController = null;
    [SerializeField] Transform _groundCheck = null;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float _characterSpeed = 12f;
    [SerializeField] float _gravity = -9.81f;
    [SerializeField] float _groundDistance = 0.4f;
    [SerializeField] float _jumpHeight = 3f;
    [SerializeField] LayerMask enemyMask;

    private Vector3 _playerVelocity;
    private bool _isGrounded;
    private bool _isOnEnemy;
    private DetectEnemy _currentEnemy;
    private float _targetRefreshRate;

    [SerializeField] TargetController _targetController;

    // Start is called before the first frame update
    void Start()
    {
        _targetRefreshRate = 0;
    }

    // Update is called once per frame
    void Update()
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
            float moveBack = -0.75f;
            _characterController.Move(transform.forward * moveBack);
        }

        float horizontalMovement = 0;
        float verticalMovement;

        if (transform.localRotation.y > 0)
            verticalMovement = Input.GetAxis("Horizontal");
        else
            verticalMovement = Input.GetAxis("Horizontal") * -1;

        Vector3 moveAmount = (transform.right * horizontalMovement) + (transform.forward * verticalMovement);

 
        _characterController.Move(moveAmount * _characterSpeed * Time.deltaTime);
        

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }

        _playerVelocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_playerVelocity * Time.deltaTime);
    }

    public void LookAtEnemy()
    {
        if(_currentEnemy != null && _isGrounded)
        {
            Vector3 lookAtPoint = _currentEnemy.transform.position;
            lookAtPoint.y = 1;
            transform.LookAt(lookAtPoint);
        }
    }

    private void UpdateEnemyPosition()
    {
        _targetRefreshRate -= Time.deltaTime;

        if(_targetRefreshRate <= 0 && _isGrounded)
        {
            _currentEnemy = _targetController.GetCurrentTarget();
            LookAtEnemy();

            _targetRefreshRate = 0.25f;
        }
        
        
    }
}
