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

    // Variables controladas por el input
    private float _throttle;
    private float _roll;
    private float _pitch;
    private float _yaw;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        // Asegurarnos de usar gravedad, pero la compensaremos manualmente
        _rigidbody.useGravity = true;
        
        // Opcional: puedes agregar algo de Drag para estabilidad:
        //_rigidbody.drag = 0.5f;
        //_rigidbody.angularDrag = 0.8f;
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
        _roll  = Input.GetAxis("Roll");   // A/D  (o joystick lateral)
        _pitch = Input.GetAxis("Pitch");  // W/S  (o joystick vertical)
        _yaw   = Input.GetAxis("Yaw");    // Q/E  (o joystick de giro)

        // Simplificamos el throttle con Espacio y Shift
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
        if (_rotor == null) return;

        // Si mantienes Espacio, hacemos girar el rotor
        if (Input.GetKey(KeyCode.Space))
        {
            _rotor.Rotate(Vector3.up, _rotorSpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            // Al soltar, vuelve gradualmente a 0
            _rotor.localRotation = Quaternion.Lerp(
                _rotor.localRotation,
                Quaternion.identity,
                Time.deltaTime * _returnSpeed
            );
        }
    }

    private void ApplyPhysics()
    {
        // 1) Aplicar fuerza ascendente (local) según el throttle 
        //    (transform.up cambiará al inclinar el helicóptero)
        float upwardForce = _throttle * _throttlePower;
        
        // 2) Fuerza de sustentación local
        Vector3 lift = transform.up * upwardForce;
        
        // 3) Compensación de la gravedad en el eje global (para que siempre "jale" hacia abajo)
        Vector3 gravityComp = Vector3.down * _gravityCompensation;

        // 4) Suma de fuerzas: sustentación local - gravedad global
        Vector3 totalForce = lift + gravityComp;

        // Aplicamos la fuerza resultante
        _rigidbody.AddForce(totalForce, ForceMode.Force);

        // Aplicar torques de roll, pitch y yaw
        // - pitch controla la inclinación adelante/atrás
        // - roll controla la inclinación lateral
        // - yaw controla la rotación sobre el eje vertical
        _rigidbody.AddTorque(transform.right   * _pitch * _responsiveness, ForceMode.Force);
        _rigidbody.AddTorque(transform.forward * -_roll * _responsiveness, ForceMode.Force);
        _rigidbody.AddTorque(transform.up      * _yaw   * _yawPower,       ForceMode.Force);
    }
}