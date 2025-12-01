using UnityEngine;

public class EasyWaves : MonoBehaviour
{
    [Tooltip("Maximum forward distance (in world units). Object will move from start position to start+distance and back.")]
    [SerializeField] private float distance = 5f;

    [Tooltip("How quickly the object moves back and forth. Larger = faster.")]
    [SerializeField] private float rate = 1f;

    [Tooltip("If true use the object's forward (local Z). If false use world Z (Vector3.forward).")]
    [SerializeField] private bool useLocalForward = false;

    // Optional phase offset (start somewhere between 0..distance)
    [SerializeField] private float phaseOffset = 0f;

    private Vector3 startPosition;
    private Vector3 direction;

    private void Awake()
    {
        startPosition = transform.position;
        // Go backwards instead of forwards:
        direction = useLocalForward ? -transform.forward.normalized : Vector3.back;
    }

    private void Update()
    {
        // PingPong yields a value that goes 0 -> distance -> 0 -> ...
        float t = Mathf.PingPong(Time.time * rate + phaseOffset, distance);
        transform.position = startPosition + direction * t;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (distance < 0f) distance = 0f;
        if (rate < 0f) rate = 0f;
    }
#endif
}