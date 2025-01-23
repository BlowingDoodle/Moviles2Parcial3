using UnityEngine;

public class HelicopterController : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [Header("Helicopter Settings")]
    [SerializeField] private float _responsiveness = 500f;
    [SerializeField] private float _throttlePower = 25f;
    [SerializeField] private float _gravityCompensation = 9.8f;
    [SerializeField] private float _yawPower = 100f;

    private float _throttle;
    private float _roll;
    private float _pitch;
    private float _yaw;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = true; // Ensure gravity is enabled
    }

    private void Update()
    {
        HandleInputs();
    }

    private void FixedUpdate()
    {
        ApplyPhysics();
    }

    private void HandleInputs()
    {
        // Get control inputs from axes
        _roll = Input.GetAxis("Roll");   // A/D or joystick lateral
        _pitch = Input.GetAxis("Pitch"); // W/S or joystick vertical
        _yaw = Input.GetAxis("Yaw");     // Q/E or joystick of yaw

        // Control throttle with Space and LeftShift
        if (Input.GetKey(KeyCode.Space))
        {
            _throttle += Time.deltaTime * _throttlePower; // Increase throttle
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            _throttle -= Time.deltaTime * _throttlePower; // Decrease throttle
        }

        // Clamp throttle to prevent extreme values
        _throttle = Mathf.Clamp(_throttle, 0, _throttlePower * 2); // Allow for higher throttle
    }

    private void ApplyPhysics()
    {
        // Calcula la fuerza de elevación, considerando la gravedad
        float effectiveThrottle = _throttle - _gravityCompensation;

        // Si el throttle es menor que la gravedad, el helicóptero debería descender
        Vector3 upwardForce = transform.up * Mathf.Max(effectiveThrottle, 0);
        _rigidbody.AddForce(upwardForce, ForceMode.Force);

        // Aplica los torques de pitch, roll y yaw
        _rigidbody.AddTorque(transform.right * _pitch * _responsiveness, ForceMode.Force);
        _rigidbody.AddTorque(transform.forward * -_roll * _responsiveness, ForceMode.Force);
        _rigidbody.AddTorque(transform.up * _yaw * _yawPower, ForceMode.Force);
    }

}

