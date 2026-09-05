using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement_Car : MonoBehaviour
{
    [Header("Car Settings")]
    public float acceleration = 10f;
    public float maxSpeed = 8f;
    public float minSpeedToSteer = 2f;
    public float steering = 200f;
    public float drag = 2f;
    private float accelInput;
    private float brakeInput;
    [Range(0f, 1f)] public float grip = 0.9f;

    [Header("Visuals")]
    public SpriteRenderer sr;
    public float rotationOffset = 0f;

    [Header("Skid Marks")]
    [SerializeField] private TrailRenderer leftTrail;   // Rear left wheel's trail
    [SerializeField] private TrailRenderer rightTrail;  // Rear right wheel's trail
    public Transform leftWheel;                         // Rear left wheel
    public Transform rightWheel;                        // Rear right wheel
    public float skidThreshold = 0.2f;                  // How much side velocity is required to start "drifting"
    public AudioSource driftSFX;
    internal bool isDrifting;

    [Header("Engine SFX")]
    public AudioSource engineSFX;
    public float minEnginePitch = 0.7f;
    public float maxEnginePitch = 2.0f;
    public float engineVolume = 0.8f;

    [Header("Input Settings")]
    public float deadzone = 0.2f;   // This is a bad practice. Should implement in options menu


    

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

        if (engineSFX)
        {
            engineSFX.loop = true;
            engineSFX.volume = 0.1f;  // Makes sure the base volume of the engine is 0.1f
            engineSFX.pitch = minEnginePitch;
            engineSFX.Play();
        }

        if (!sr)
        {
            sr = GetComponentInChildren<SpriteRenderer>();
        }

        visual = sr ? sr.transform : transform;
    }


    public void SilenceAllSound()
    {
        engineSFX.Stop();
        driftSFX.Stop();
    }


    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        // Zona muerta
        if (input.magnitude < deadzone)
        {
            input = Vector2.zero;
        }
        else
        {
            input = input.normalized * ((input.magnitude - deadzone) / (1f - deadzone));
        }

        
        steerInput = input.x;
    }

    public void OnAccelerate(InputValue value)
    {
        accelInput = value.Get<float>();
    }

    public void OnBrake(InputValue value)
    {
        brakeInput = value.Get<float>();
    }

    void FixedUpdate()
    {
        moveInput = accelInput - brakeInput;   // This input will always be between 1 and -1

        if (Mathf.Abs(moveInput) > 0.01f)
            rb.AddForce(transform.up * moveInput * acceleration);

        // Limits max speed
        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;

        float speed = rb.linearVelocity.magnitude;


        // Apply rotation to the vehicle, but only when moving above the speed of minSpeedToSteer
        if (Mathf.Abs(steerInput) > 0.01f && speed > minSpeedToSteer)
        {
            float turnAmount = steerInput * steering * Time.fixedDeltaTime;
            rb.rotation -= turnAmount;
        }

        if (speed > 0.001f)
        {
            Vector2 forwardVel = transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
            Vector2 sideVel = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);
            rb.linearVelocity = forwardVel + sideVel * (1 - grip);

            isDrifting = sideVel.magnitude > skidThreshold;

            leftTrail.emitting = isDrifting;
            rightTrail.emitting = isDrifting;
            if (isDrifting && driftSFX.isPlaying == false)
            {
                driftSFX.Play();
            }
            else if (!isDrifting && driftSFX.isPlaying == true)
            {
                driftSFX.Stop();
            }
        }

        // If the player's car has an engine SFX
        if (engineSFX)
        {
            if (speed < 0.1f)
            {
                // Lowers the volume to it's minimum setting when it's stopped.
                engineSFX.volume = Mathf.Lerp(engineSFX.volume, 0.1f, 5f * Time.fixedDeltaTime);
            }
            else
            {
                // Normalizes the speed according to the maxSpeed setting
                float t = Mathf.InverseLerp(0f, maxSpeed, speed);

                // The volume increases alongside velocity
                engineSFX.volume = Mathf.Lerp(0.1f, engineVolume, t);

                // So does the pitch of the engine
                engineSFX.pitch = Mathf.Lerp(minEnginePitch, maxEnginePitch, t);
            }
        }

        visual.localRotation = Quaternion.Euler(0, 0, rb.rotation + rotationOffset);
    }

}
