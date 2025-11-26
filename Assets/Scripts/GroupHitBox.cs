using System.Collections.Generic;
using UnityEngine;

public class GroupHitBox : MonoBehaviour
{
    public List<EnemyHealth> enemiesDetected;

    private void Awake()
    {
        enemiesDetected = new List<EnemyHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();

        if(enemy)
        {
            enemiesDetected.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        enemiesDetected.Remove(other.gameObject.GetComponent<EnemyHealth>());
    }
}
