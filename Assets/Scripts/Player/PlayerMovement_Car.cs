using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement_Car : MonoBehaviour
{
    [Header("Car Settings")]
    public float acceleration = 10f;
    public float maxSpeed = 8f;
    public float steering = 200f;
    public float drag = 2f;
    [Range(0f, 1f)] public float grip = 0.9f;

    [Header("Visuals")]
    public SpriteRenderer sr;
    public float rotationOffset = 0f;

    [Header("Skid Marks")]
    [SerializeField] private TrailRenderer leftTrail;
    [SerializeField] private TrailRenderer rightTrail;
    public Transform leftWheel;            // posición de rueda trasera izquierda
    public Transform rightWheel;           // posición de rueda trasera derecha
    public float skidThreshold = 0.2f;    // cuánta velocidad lateral dispara derrape

    private Rigidbody2D rb;
    private float moveInput;
    private float steerInput;
    private Transform visual;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.linearDamping = drag;
        rb.freezeRotation = false;

        if (!sr)
            sr = GetComponentInChildren<SpriteRenderer>();

        visual = sr ? sr.transform : transform;
    }

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveInput = input.y;
        steerInput = input.x;
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(moveInput) > 0.01f)
            rb.AddForce(transform.up * moveInput * acceleration);

        // Limita la velocidad máxima
        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;

        // --- 2️⃣ Giro persistente ---
        // Aplica rotación directamente, independientemente de la velocidad o fricción
        if (Mathf.Abs(steerInput) > 0.01f)
        {
            float turnAmount = steerInput * steering * Time.fixedDeltaTime;
            rb.rotation -= turnAmount;
        }

        if (rb.linearVelocity.sqrMagnitude > 0.001f)
        {
            Vector2 forwardVel = transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
            Vector2 sideVel = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);
            rb.linearVelocity = forwardVel + sideVel * (1 - grip);

            bool isDrifting = sideVel.magnitude > skidThreshold;

            leftTrail.emitting = isDrifting;
            rightTrail.emitting = isDrifting;
        }

        

        visual.localRotation = Quaternion.Euler(0, 0, rb.rotation + rotationOffset);
    }

}
