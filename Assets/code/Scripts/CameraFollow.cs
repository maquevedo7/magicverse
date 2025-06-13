using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform objetivo;     // El jugador
    public Vector3 offset;         // Distancia deseada desde el jugador
    public float suavizado = 5f;   // Suavizado del movimiento
    public float yFijo = 5f;       // Altura fija de la cámara

    void LateUpdate()
    {
        if (objetivo != null)
        {
            // Calculamos la posición deseada, pero sin cambiar el eje Y
            Vector3 posicionDeseada = new Vector3(
                objetivo.position.x + offset.x,
                yFijo,
                objetivo.position.z + offset.z
            );

            transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);
        }
    }
}