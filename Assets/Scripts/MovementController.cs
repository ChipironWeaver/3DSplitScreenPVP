using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private Transform _cameraTransform;

    [Header("Settings")] [SerializeField] private float _moveSpeed = 20;
    [SerializeField] private float _rotateSpeed = 100;
    [SerializeField] private float _xMinAngle = -60f;
    [SerializeField] private float _xMaxAngle = 60f;
    [SerializeField] private float _jumpForce = 50;
    [SerializeField] private float _gravityForce = -9.81f;
    [SerializeField] public bool isActive = true;

    private CharacterController _characterController;
    private Vector3 _move;
    private float _verticalVelocity;
    private bool _isGrounded;
    private Vector2 _mouse;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (isActive)
        {


            // Horizontal movement
            Vector3 moveDirection = transform.TransformDirection(_move);
            moveDirection *= _moveSpeed;

            // Check grounded
            _isGrounded = _characterController.isGrounded;
            if (_isGrounded && _verticalVelocity < 0)
            {
                _verticalVelocity = -2f;
            }

            // Gravity
            _verticalVelocity += _gravityForce * Time.deltaTime;
            moveDirection.y = _verticalVelocity;

            // Apply movement and gravity
            _characterController.Move(moveDirection * Time.deltaTime);

            // Get rotation inputs

            // Calculate body rotation
            Vector3 bodyRotation = new Vector3(0, _mouse.x, 0) * (_rotateSpeed * Time.deltaTime);

            // Apply body rotation
            transform.Rotate(bodyRotation);

            // Calculate camera rotation
            Vector3 cameraRotation =
                new Vector3(Mathf.Clamp(-_mouse.y, -30f, 30f), 0, 0) * (_rotateSpeed * Time.deltaTime);
            cameraRotation = _cameraTransform.eulerAngles + cameraRotation;
            cameraRotation.x = ClampAngle(cameraRotation.x, _xMinAngle, _xMaxAngle);

            // Apply camera rotation
            _cameraTransform.eulerAngles = cameraRotation;
        }
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + min);
        return Mathf.Min(angle, max);
    }


    private void OnMove(InputValue input)
    {
        Vector2 movement = input.Get<Vector2>();
        _move.x = movement.x;
        _move.z = movement.y;
    }

    private void OnJump(InputValue input)
    {
        if (_isGrounded)
        {
            _verticalVelocity = Mathf.Sqrt(_jumpForce * -2f * _gravityForce);
        }
    }

    private void OnLook(InputValue input)
    {
        Vector2 rotation = input.Get<Vector2>();
        _mouse = rotation;
    }
}