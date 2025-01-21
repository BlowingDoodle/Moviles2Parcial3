using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Raycast detectado en: " + hit.collider.name);

                BoxBehaviour box = hit.collider.GetComponent<BoxBehaviour>();
                if (box != null)
                {
                    box.OnBoxClicked();
                }
            }
            else
            {
                Debug.Log("Raycast no impactó ningún objeto.");
            }
        }
    }
}

