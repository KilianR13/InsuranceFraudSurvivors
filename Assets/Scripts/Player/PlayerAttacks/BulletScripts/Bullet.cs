using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const float MAX_LIFE_TIME = 4f;
    private float _lifeTime = 0f;
    public Vector2 Velocity;

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)Velocity * Time.deltaTime;
        _lifeTime += Time.deltaTime;

        if (_lifeTime > MAX_LIFE_TIME)
        {
            Disable();
        }
    }

    private void Disable()
    {
        _lifeTime = 0f;
        gameObject.SetActive(false);
    }
}
