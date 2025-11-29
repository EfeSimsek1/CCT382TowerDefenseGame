using UnityEngine;

[CreateAssetMenu(fileName = "BombCard", menuName = "Card/Ability/AOE/Bomb")]
public class BombCard : AOEAbilityCard
{
    public int damage;
    public DamageType damageType;
    public LayerMask enemyMask;
    public GameObject explosionPrefab;

    public override void Activate()
    {
        #region Damage Enemies

        Collider[] enemyColliders = Physics.OverlapSphere(UIInputManager.groundPos, radius, enemyMask);

        foreach(Collider enemy in enemyColliders)
        {
            EnemyHealth enemyHealth = enemy.gameObject.GetComponent<EnemyHealth>();

            if(enemyHealth)
            {
                enemyHealth.Damage(damage, damageType);
            }
        }

        #endregion

        GameObject explosionEffect = Instantiate(explosionPrefab, UIInputManager.groundPos, Quaternion.identity);
        explosionEffect.transform.localScale = new Vector3(radius, radius, radius);
    }
}
