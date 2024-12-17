using UnityEngine;

public class LaserBall : MonoBehaviour
{
    public int damage = 30;  // Dégâts de la boue laser

    void OnTriggerEnter(Collider other)
    {
        // Vérifie si la boule laser entre en collision avec un ennemi
        if (other.CompareTag("Enemy"))
        {
            // Applique les dégâts à l'ennemi (assume qu'il a un script "EnemyHealth")
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            // Détruire la balle après collision
            Destroy(gameObject);
        }
    }
}
