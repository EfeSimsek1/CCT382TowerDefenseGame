using UnityEngine;

public class TadPoleWiggle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public float wiggleAngle = 30f;
    public float wiggleSpeed = 1f;

    float _baseY;
    bool _baseSet;

    void Update()
    {
        if (!_baseSet)
        {
            _baseY = transform.localEulerAngles.y;
            _baseSet = true;
        }

        float y = _baseY + Mathf.Sin(Time.time * wiggleSpeed) * wiggleAngle;
        Vector3 e = transform.localEulerAngles;
        e.y = y;
        transform.localEulerAngles = e;
    }
}
