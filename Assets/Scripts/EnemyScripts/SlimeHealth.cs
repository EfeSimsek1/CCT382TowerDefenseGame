using UnityEditor;
using UnityEngine;

public class SlimeHealth : EnemyHealth
{


    public override void Die()
    {
        killTrigger = true;
        EnemySpawner.onEnemyDestroy.Invoke(gameObject);
        GameManager.onEnemyDefeated.Invoke(gameObject);
        var src = GetComponent<AudioSource>();
        if (src != null) src.PlayOneShot(deathClip);



        //debug
        Debug.Log("Slime died, attempting to split.");

        //Split into two smaller slimes if possible
        float newSlimeHealth = maxHealth / 2f; 
        EnemySpawner.onEnemyMultiply.Invoke(gameObject, 2);
        if (newSlimeHealth >= 1)
        {
            for (int i = 0; i < 2; i++)
            {
                // Create new slime
                GameObject newSlime = Instantiate(gameObject, transform.position, Quaternion.identity);
                SlimeHealth slimeHealth = newSlime.GetComponent<SlimeHealth>();
                slimeHealth.maxHealth = Mathf.FloorToInt(newSlimeHealth);
                slimeHealth.killTrigger = false;

                // Adjust scale
                newSlime.transform.localScale = transform.localScale * 0.75f; // Scale down to 75%
                newSlime.GetComponent<EnemyBehaviour>().speed *= 1.2f; // Increase speed slightly
                newSlime.GetComponent<EnemyBehaviour>().moneyDroppedOnDeath = Mathf.Max(1, newSlime.GetComponent<EnemyBehaviour>().moneyDroppedOnDeath / 2); // Reduce money dropped
                newSlime.GetComponent<EnemyBehaviour>().damageOnDeath = Mathf.Max(1, newSlime.GetComponent<EnemyBehaviour>().damageOnDeath / 2); // Reduce damage on death
                newSlime.GetComponent<EnemyHealth>().deathClip = deathClip;

            }
        }


        
        
        Destroy(gameObject);
    }
}
