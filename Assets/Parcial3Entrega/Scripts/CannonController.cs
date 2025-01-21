using UnityEngine;
using UnityEngine.UI;

public class CannonController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Slider powerSlider;
    public float maxPower = 100f;
    public float chargeRate = 10f;
    public float rotationSpeed = 20f;

    private float currentPower = 0f;
    private bool isCharging = false;

    void Update()
    {
        // Manejar la carga de potencia
        if (Input.GetMouseButtonDown(0))
        {
            isCharging = true;
        }

        if (Input.GetMouseButton(0) && isCharging)
        {
            currentPower += chargeRate * Time.deltaTime;
            currentPower = Mathf.Clamp(currentPower, 0, maxPower);
            powerSlider.value = currentPower / maxPower;
        }

        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            isCharging = false;
            Shoot();
            currentPower = 0;
            powerSlider.value = 0;
        }

        // Ajustar el ángulo vertical del cañón
        float verticalRotation = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.right * verticalRotation);

        // Ajustar el ángulo horizontal del cañón
        float horizontalRotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * horizontalRotation, Space.World);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // Aplica la fuerza en la dirección del cañón
        Vector3 force = firePoint.forward * currentPower;
        rb.AddForce(force, ForceMode.Impulse);
    }
}
