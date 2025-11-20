using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    [SerializeField] public Vector3 offset;

    public void UpdateHealthbar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    void Update()
    {
        transform.rotation = cam.transform.rotation;
        transform.position = target.position + offset;
    }

    public void Initialize(Transform targetTransform, Camera camera)
    {
        target = targetTransform;
        cam = camera;
    }

}
