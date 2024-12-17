using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;   // Points de vie max de l'ennemi
    private int currentHealth;    // Points de vie actuels

    private WaveManager waveManager;  // Référence au WaveManager

    void Start()
    {
        currentHealth = maxHealth;  // Initialiser les points de vie
        waveManager = FindObjectOfType<WaveManager>();  // Trouver le WaveManager dans la scène
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;   // Réduire les points de vie
        if (currentHealth <= 0)
        {
            Die();  // Appeler la fonction de mort si les PV sont à zéro
        }
    }

    void Die()
    {
        Debug.Log("Le vaisseau ennemi est détruit !");
        waveManager.EnemyKilled();  // Notifier le WaveManager que cet ennemi est mort
        Destroy(gameObject);  // Détruire l'ennemi
    }
}
