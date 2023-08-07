using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _isGrounded;
    
    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 1f;
    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _isGrounded = _controller.isGrounded;
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        
        _controller.Move(transform.TransformDirection(moveDirection) * (speed * Time.deltaTime));

        if (_isGrounded && _playerVelocity.y < 0)
            _playerVelocity.y = -2f;
        _playerVelocity.y += gravity * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (_isGrounded)
        {
            _playerVelocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
    }
}
