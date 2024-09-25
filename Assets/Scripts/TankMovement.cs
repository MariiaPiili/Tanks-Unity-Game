using UnityEngine;

public class TankMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private KeyCode _keyRight;
    [SerializeField] private KeyCode _keyLeft;
    [SerializeField] private KeyCode _keyUp;
    [SerializeField] private KeyCode _keyDown;

    private Rigidbody _rigidbody;
    private int _lastRotation;
    private float _horizont;
    private float _vertical;
    private float _direction;
    private float _angle;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        UpdateHorizontalMovement();
        UpdateVerticalMovement();
    }

    private void UpdateHorizontalMovement()
    {
        if (Input.GetKey(_keyLeft))
        {
            _lastRotation = -90;
            _horizont = -1;
        }
        else if (Input.GetKey(_keyRight))
        {
            _lastRotation = 90;
            _horizont = 1;
        }
        else
        {
            _horizont = 0;
        }
    }

    private void UpdateVerticalMovement()
    {
        if (Input.GetKey(_keyUp))
        {
            _lastRotation = 0;
            _vertical = 1;
        }
        else if (Input.GetKey(_keyDown))
        {
            _lastRotation = 180;
            _vertical = -1;
        }
        else
        {
            _vertical = 0;
        }
    }

    private void FixedUpdate()
    {
        transform.eulerAngles = Vector3.up * _lastRotation;

        _rigidbody.AddForce(Vector3.right * _speed * _horizont, ForceMode.VelocityChange);
        _rigidbody.AddForce(Vector3.forward * _speed * _vertical, ForceMode.VelocityChange);
    }
}
