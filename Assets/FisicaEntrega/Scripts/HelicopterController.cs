using UnityEngine;

public class HelicopterController : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [Header("Helicopter Settings")]
    [SerializeField] private float _responsiveness = 500f;
    [SerializeField] private float _throttlePower = 25f;
    [SerializeField] private float _gravityCompensation = 9.8f;
    [SerializeField] private float _yawPower = 100f;

    [Header("Rotor Settings")]
    [SerializeField] private Transform _rotor;         // Referencia al rotor
    [SerializeField] private float _rotorSpeed = 3000f; // Velocidad de giro (ajusta libremente)
    [SerializeField] private float _returnSpeed = 5f;

    // Estas variables las moveremos directamente con teclas
    private float _throttle;
    private float _roll;
    private float _pitch;
    private float _yaw;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = true;  // Asegurarte de que el Rigidbody tenga la gravedad activa
    }

    private void Update()
    {
        HandleInputs();
        HandleRotor();
    }

    private void FixedUpdate()
    {
        ApplyPhysics();
    }

    private void HandleInputs()
    {
        // Entradas de ejes
        _roll  = Input.GetAxis("Roll");   // A/D (o joystick lateral)
        _pitch = Input.GetAxis("Pitch");  // W/S (o joystick vertical)
        _yaw   = Input.GetAxis("Yaw");    // Q/E (o joystick de giro)

        // En este método simplificamos el throttle:
        // - Espacio => throttle positivo (subir).
        // - Shift => throttle negativo (bajar).
        // - Nada => throttle = 0.

        if (Input.GetKey(KeyCode.Space))
        {
            _throttle = 1f;  // 100% ascenso
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            _throttle = -1f; // 100% descenso
        }
        else
        {
            _throttle = 0f;  // Sin empuje vertical
        }
    }

    private void HandleRotor()
    {
        // Si no se ha asignado el rotor, salimos para evitar errores
        if (_rotor == null) return;

        // Si mantienes Espacio, gira el rotor
        if (Input.GetKey(KeyCode.Space))
        {
            _rotor.Rotate(Vector3.up, _rotorSpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            // Deja de rotar y vuelve gradualmente a 0
            _rotor.localRotation = Quaternion.Lerp(
                _rotor.localRotation,
                Quaternion.identity,
                Time.deltaTime * _returnSpeed
            );
        }
    }

    private void ApplyPhysics()
    {
        // ForceMode.Force aplica una fuerza continua dependiente de la masa y el deltaTime
        // Calculamos el "efecto neto" del throttle menos la gravedad (si throttle es 1, sumará throttlePower, si es -1, restará)
        float effectiveThrottle = (_throttle * _throttlePower) - _gravityCompensation;

        // Aplicamos la fuerza "arriba/abajo" en el eje del helicóptero
        Vector3 verticalForce = transform.up * effectiveThrottle;
        _rigidbody.AddForce(verticalForce, ForceMode.Force);

        // Torques para roll, pitch y yaw
        _rigidbody.AddTorque(transform.right   * _pitch * _responsiveness, ForceMode.Force);
        _rigidbody.AddTorque(transform.forward * -_roll * _responsiveness, ForceMode.Force);
        _rigidbody.AddTorque(transform.up      * _yaw * _yawPower,        ForceMode.Force);
    }
}
