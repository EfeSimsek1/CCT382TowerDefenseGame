using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("hit!");
    }
}
