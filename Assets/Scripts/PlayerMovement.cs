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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, groundMask);

        if (_isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -2f;
        }

        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        Vector3 moveAmount = (transform.right * horizontalMovement) + (transform.forward * verticalMovement);

 
        _characterController.Move(moveAmount * _characterSpeed * Time.deltaTime);
        

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }

        _playerVelocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_playerVelocity * Time.deltaTime);
    }
}
