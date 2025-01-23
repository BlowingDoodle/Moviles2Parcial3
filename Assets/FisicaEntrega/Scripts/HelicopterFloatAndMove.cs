using UnityEngine;

public class HelicopterFloatAndMove : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _rotor;

    [Header("Ascenso / Gravedad")]
    [SerializeField] private float _ascendForce = 15f;
    // Fuerza hacia arriba cuando se mantiene Espacio
    // Aumenta este valor para subir más rápido.

    [Header("Movimiento Horizontal")]
    [SerializeField] private float _moveForce = 5f;
    // Fuerza de empuje para moverse con WASD

    [Header("Rotor Settings")]
    [SerializeField] private float _rotorSpeed = 3000f; // Velocidad de giro del rotor
    [SerializeField] private float _returnSpeed = 5f;   // Velocidad de retorno a 0 al soltar Espacio

    private void Awake()
    {
        // Si no asignas el Rigidbody en el Inspector, lo buscamos en este objeto
        if (_rigidbody == null)
            _rigidbody = GetComponent<Rigidbody>();

        // Gravedad desactivada inicialmente para "flotar"
        if (_rigidbody != null)
            _rigidbody.useGravity = false;
    }

    private void Update()
    {
        HandleFloat();
        HandleMovement();
        HandleRotor();
    }

    private void HandleFloat()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            // Desactiva la gravedad mientras mantienes Espacio
            _rigidbody.useGravity = false;

            // Aplica fuerza ascendente para que suba
            _rigidbody.AddForce(Vector3.up * _ascendForce, ForceMode.Force);
        }
        else
        {
            // Activa la gravedad al soltar Espacio
            _rigidbody.useGravity = true;
        }
    }

    private void HandleMovement()
    {
        // Lee ejes horizontales y verticales (WASD o flechas)
        float moveX = Input.GetAxis("Horizontal"); // A/D (o flechas izquierda/derecha)
        float moveZ = Input.GetAxis("Vertical");   // W/S (o flechas arriba/abajo)

        // Crea un vector de movimiento en el plano XZ según la orientación del helicóptero
        // - transform.forward => adelante
        // - transform.right   => derecha
        Vector3 moveDirection = transform.forward * moveZ + transform.right * moveX;

        // Aplica fuerza para moverse
        _rigidbody.AddForce(moveDirection * _moveForce, ForceMode.Force);
    }

    private void HandleRotor()
    {
        if (_rotor == null) return;

        // Si estás manteniendo Espacio, gira el rotor
        if (Input.GetKey(KeyCode.Space))
        {
            _rotor.Rotate(Vector3.up, _rotorSpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            // Al soltar Espacio, vuelve suavemente a su rotación original (0)
            _rotor.localRotation = Quaternion.Lerp(
                _rotor.localRotation,
                Quaternion.identity,
                Time.deltaTime * _returnSpeed
            );
        }
    }
}

