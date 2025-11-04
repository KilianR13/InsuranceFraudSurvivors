

// using Unity.VisualScripting;
// using UnityEngine;
// using UnityEngine.AI;
// using UnityEngine.InputSystem;


// [RequireComponent(typeof(Rigidbody2D))] // Es obligatorio que este objeto tenga un RigidBody2D


// public class PlayerMovement_Car : MonoBehaviour
// {

//     [Header("Player Settings")]
//     public float movementSpeed;
//     public SpriteRenderer sr;
//     public BaseFacing baseFacing = BaseFacing.Right;

//     [Range(-180, 180)]
//     public float rotationOffset = 0f;

//     private Rigidbody2D rb;
//     private Transform visual;
//     private Vector2 move, lastDir = Vector2.up;
//     const float EPS = 0.01f;

//     public enum BaseFacing { Right, Left, Up, Down }

//     void Awake()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         rb.gravityScale = 0;
//         rb.freezeRotation = true; // Nuestro jugador no rotará, en caso de colisión no habrá errores.
//         if (!sr)
//         {
//             sr = GetComponentInChildren<SpriteRenderer>();
//         }
//         visual = sr ? sr.transform : transform; // Si hay sr, es el transform del sr. Si no, es el transform normal
//         // Este codigo de abajo es lo mismo que el de arriba:
//         // if (sr)
//         // {
//         //     visual = sr.transform;
//         // }
//         // else
//         // {
//         //     visual = transform;
//         // }

//     }

//     public void OnMove(InputValue value)
//     {
//         move = value.Get<Vector2>();
//     }

//     private void FixedUpdate()
//     {
//         // Si la square magnitude es mayor a 1,0, se normaliza el move. Si no, simplemente se aplica el move.
//         Vector2 dir = move.sqrMagnitude > 1f ? move.normalized : move;

//         rb.linearVelocity = dir * movementSpeed;

//     }


//     void Update()
//     {
//         // Comprueba hacia donde mira el jugador
//         // Si el jugador está pulsando las teclas, look = move. Si no, look = rb.linearVelocity
//         Vector2 look =
//             (move.sqrMagnitude >= EPS) ? move : rb.linearVelocity;

//         if (look.sqrMagnitude >= EPS) // El movimiento es mayor al umbral de EPS
//         {
//             lastDir = Snap4(look); // Manda el Vector2 "look" y devuelve la dirección.

//         }

//         float angle = AngleFromDir(lastDir) - BaseFacingToAngle(baseFacing) + rotationOffset;

//         visual.localRotation = Quaternion.Euler(0f, 0f, angle);
//     }

//     static Vector2 Snap4(Vector2 vector) // Convierte el vector en 4 direcciones puras.
//     {
//         // X predomina?
//         return Mathf.Abs(vector.x) > Mathf.Abs(vector.y)
//             ? new Vector2(Mathf.Sign(vector.x), 0f) // Devuelve izquierda o derecha. Devuelve "(+1,0)" o "(-1,0)"
//             : new Vector2(0f, Mathf.Sign(vector.y)); // Devuelve arriba o abajo. Devuelve "(0,+1)" o "(0,-1)"
//     }

//     // Devuelve los grados de rotación necesarios para llegar al objetivo. El giro necesario.
//     static float AngleFromDir(Vector2 vector)
//     {
//         if (vector.x > 0) return 0f;
//         if (vector.x < 0) return 180f;
//         if (vector.y > 0) return 90f;
//         return 270f;
//     }
    
//     static float BaseFacingToAngle(BaseFacing bf)
//     {
//         return bf == BaseFacing.Right ? 0f :
//             bf == BaseFacing.Up ? 90f :
//             bf == BaseFacing.Left ? 180f :
//             270f;
//     }
// }

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
        }

        visual.localRotation = Quaternion.Euler(0, 0, rb.rotation + rotationOffset);
    }
    void Update()
    {
        // Debug.Log($"SteerInput: {steerInput}, MoveInput: {moveInput}");
    }

}
