using UnityEngine;

public class DetectarColision : MonoBehaviour
{
    // Detecta colisiones con colliders f�sicos (usando Rigidbody y Collider)
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colisi�n con: " + collision.gameObject.name);
    }

    // Opcional: si usas colliders con "Is Trigger" activado
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entr� en trigger con: " + other.gameObject.name);
    }
}