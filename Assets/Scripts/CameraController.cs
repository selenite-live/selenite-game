using UnityEngine;

public class SimpleCameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;  // Sensibilité de la souris

    void Update()
    {
        // Lire les mouvements de la souris
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Appliquer la rotation horizontale (gauche/droite) sur l'objet caméra
        transform.Rotate(Vector3.up * mouseX);

        // Appliquer la rotation verticale (haut/bas) sur l'objet caméra
        Camera.main.transform.Rotate(Vector3.left * mouseY);
    }
}
