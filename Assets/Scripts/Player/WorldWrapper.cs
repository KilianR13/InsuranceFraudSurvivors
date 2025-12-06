using System;
using UnityEngine;

public class WorldWrapper : MonoBehaviour
{
    [Header("Mapa (tamaño total)")]
    public float mapWidth = 864f;   // ancho total del mundo
    public float mapHeight = 756f;  // alto total del mundo

    [Header("Opciones")]
    [Tooltip("Si > 0 evita el teletransporte justo en el borde (útil si el jugador tiene tamaño)")]
    public float padding = 0f;

    [Tooltip("Si true usará Rigidbody2D para mover al jugador (más limpio para física). Si false usa transform.position.")]
    public bool useRigidbody2D = true;

    private float halfWidth;
    private float halfHeight;
    private Rigidbody2D rb2d;

    public event Action changedSides;
    private bool isOnRightSide = true; // o false según empiece
    private bool isOnTopSide = true; // o false según empiece

    private bool changedThisFrame = false;


    void Awake()
    {
        halfWidth = mapWidth * 0.5f;
        halfHeight = mapHeight * 0.5f;

        if (useRigidbody2D)
            rb2d = GetComponent<Rigidbody2D>();
    }

    void LateUpdate()
    {
        // Usamos LateUpdate para que la posición ya esté actualizada por la física/inputs
        WrapPosition();
        changedThisFrame = false;
    }

    private void WrapPosition()
    {
        bool newSide = transform.position.x > 0; // ejemplo: lado derecho si x > 0

        if (newSide != isOnRightSide && !changedThisFrame)
        {
            isOnRightSide = newSide;
            changedThisFrame = true;
            changedSides?.Invoke(); // se dispara solo 1 vez
        }

        bool newSideY = transform.position.y > 0; // lado superior si y > 0
        if (newSideY != isOnTopSide && !changedThisFrame)
        {
            isOnTopSide = newSideY;
            changedThisFrame = true;
            changedSides?.Invoke(); // se dispara solo 1 vez
        }

        Vector3 pos = transform.position;
        bool changed = false;

        float minX = -halfWidth + padding;
        float maxX =  halfWidth - padding;
        float minY = -halfHeight + padding;
        float maxY =  halfHeight - padding;

        if (pos.x < minX)
        {
            pos.x += mapWidth; // saltamos al lado derecho
            changed = true;
        }
        else if (pos.x > maxX)
        {
            pos.x -= mapWidth; // saltamos al lado izquierdo
            changed = true;
        }

        if (pos.y < minY)
        {
            pos.y += mapHeight; // saltamos arriba
            changed = true;
        }
        else if (pos.y > maxY)
        {
            pos.y -= mapHeight; // saltamos abajo
            changed = true;
        }

        if (changed)
        {
            if (useRigidbody2D && rb2d != null)
            {
                // Usa Rigidbody2D para evitar conflictos con la física
                rb2d.position = new Vector2(pos.x, pos.y);
                rb2d.linearVelocity = rb2d.linearVelocity; // deja la velocidad intacta (puedes modificar si quieres)
            }
            else
            {
                transform.position = pos;
            }
        }
    }
}
