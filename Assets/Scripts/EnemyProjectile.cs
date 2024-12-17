using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage = 20;  // Dégâts infligés par le projectile

    void OnTriggerEnter(Collider other)
    {
        // Vérifie si le projectile touche le joueur
        if (other.CompareTag("Player"))
        {
            // Accède au script PlayerHealth et inflige des dégâts
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);  // Infliger les dégâts
            }

            // Détruire le projectile après la collision
            Destroy(gameObject);
        }
    }
}
