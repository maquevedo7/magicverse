using UnityEngine;

public class DetectarColision : MonoBehaviour
{
    // Detecta colisiones con colliders físicos (usando Rigidbody y Collider)
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colisión con: " + collision.gameObject.name);
    }

    // Opcional: si usas colliders con "Is Trigger" activado
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entró en trigger con: " + other.gameObject.name);
    }
}