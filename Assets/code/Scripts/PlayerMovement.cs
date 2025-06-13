using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    public float jumpForce = 5;
    public TextMeshProUGUI contadorTexto;
    public GameObject instrumentoEspecial;
    public GameObject instrumentoEspecial2;

    private int contador = 0;
    private Rigidbody rb;
    private bool isGrounded = true;

    private bool enTecho = false;
    public float alturaTecho = 5.42f;
    private float alturaSuelo;

    private Quaternion rotacionDeseada;
    public float velocidadRotacion = 5f;

    private bool puedeCambiarDimension = true;
    public float delayCambioDimension = 0.5f;

    private HashSet<string> notasRecolectadas = new HashSet<string>();

    [Header("Modelo Visual")]
    public Transform modeloVisual;
    private Quaternion rotacionVisual;
    public float velocidadRotacionVisual = 10f;

    private bool movimientoHabilitado = false;

    // Mecanica Dash
    public float fuerzaDash = 10f;
    public float cooldownDash = 1f;
    private bool puedeHacerDash = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        alturaSuelo = transform.position.y;

        Physics.gravity = new Vector3(0, -9.81f, 0);
        enTecho = false;
        rotacionDeseada = Quaternion.identity;

        if (instrumentoEspecial != null) instrumentoEspecial.SetActive(false);
        if (instrumentoEspecial2 != null) instrumentoEspecial2.SetActive(false);

        rotacionDeseada = transform.rotation;
        rotacionVisual = modeloVisual.localRotation;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 direction = new Vector3(horizontal, 0f, 0f);
        transform.Translate(direction * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && puedeHacerDash)
        {
            StartCoroutine(HacerDash());
        }

        if (horizontal > 0) rotacionVisual = Quaternion.Euler(0f, 0f, 0f);
        else if (horizontal < 0) rotacionVisual = Quaternion.Euler(0f, 180f, 0f);

        modeloVisual.localRotation = Quaternion.Lerp(
            modeloVisual.localRotation,
            rotacionVisual,
            Time.deltaTime * velocidadRotacionVisual
        );

        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded && !enTecho)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && isGrounded && enTecho)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.down * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && !enTecho && isGrounded && puedeCambiarDimension && movimientoHabilitado)
        {
            StartCoroutine(CambiarADimension(true));
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && enTecho && isGrounded && puedeCambiarDimension && movimientoHabilitado)
        {
            StartCoroutine(CambiarADimension(false));
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, rotacionDeseada, velocidadRotacion * Time.deltaTime);
    }

    private IEnumerator HacerDash()
    {
        puedeHacerDash = false;

        Vector3 direccionDash = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);

        if (direccionDash == Vector3.zero)
        {
            float direccionX = rotacionVisual.y == 1 ? -1f : 1f;
            direccionDash = new Vector3(direccionX, 0f, 0f);
        }

        rb.AddForce(direccionDash.normalized * fuerzaDash, ForceMode.Impulse);

        yield return new WaitForSeconds(cooldownDash);
        puedeHacerDash = true;
    }

    private void FixedUpdate()
    {
        if (enTecho && isGrounded)
        {
            rb.AddForce(Vector3.up * 9.81f, ForceMode.Acceleration);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Nota"))
        {
            NotaMusical nota = other.GetComponent<NotaMusical>();
            if (nota != null)
            {
                Debug.Log("Recolectaste la nota: " + nota.nombreNota);
                Destroy(other.gameObject);
                contador++;
                contadorTexto.text = "Objetos: " + contador;
                notasRecolectadas.Add(nota.nombreNota.ToLower());

                if (contador == 4)
                {
                    ActivarInstrumento();
                }

                if (contador == 15)
                {
                    ActivarInstrumento2();
                }
            }
        }

        if (other.CompareTag("obstacle"))
        {
            Debug.Log("Moriste");
            Die();
        }

        if (other.CompareTag("Instrumento"))
        {
            Debug.Log("Recogiste el instrumento especial!");
            Destroy(other.gameObject);
            movimientoHabilitado = true;
        }

        if (other.CompareTag("Instrumento2"))
        {
            Debug.Log("Recogiste el segundo instrumento especial!");
            Destroy(other.gameObject);
            Debug.Log("¡Felicidades!");
        }
    }

    private void ActivarInstrumento()
    {
        if (instrumentoEspecial != null)
        {
            instrumentoEspecial.SetActive(true);
            Debug.Log("¡Instrumento activado!");
        }
    }

    private void ActivarInstrumento2()
    {
        if (instrumentoEspecial2 != null)
        {
            instrumentoEspecial2.SetActive(true);
            Debug.Log("Ganaste");
        }
    }

    private void Die()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.GameOver();
        }

        this.enabled = false;
    }

    private IEnumerator CambiarADimension(bool haciaTecho)
    {
        puedeCambiarDimension = false;
        rb.velocity = Vector3.zero;

        if (haciaTecho)
        {
            enTecho = true;
            Physics.gravity = new Vector3(0, 9.81f, 0);
            rotacionDeseada = Quaternion.Euler(180, 0, 0);
        }
        else
        {
            enTecho = false;
            Physics.gravity = new Vector3(0, -9.81f, 0);
            rotacionDeseada = Quaternion.identity;
        }

        yield return new WaitForSeconds(delayCambioDimension);
        puedeCambiarDimension = true;
    }
}