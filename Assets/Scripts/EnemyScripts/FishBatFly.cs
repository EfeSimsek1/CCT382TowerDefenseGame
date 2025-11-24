using UnityEngine;

public class FishBatFly : MonoBehaviour
{
    public float amplitude = 15f; // degrees
    public float speed = 2f; // oscillations per second

    Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * Mathf.PI * 2f * speed) * amplitude;
        transform.localRotation = initialRotation * Quaternion.Euler(0f, 0f, angle);
    }
}
