using UnityEngine;
using TMPro; // Pour utiliser TextMeshPro

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 200;   // Points de vie max du joueur
    private int currentHealth;     // Points de vie actuels du joueur

    public TextMeshProUGUI hpText; // Référence au TextMeshPro pour afficher les PV du joueur

    void Start()
    {
        currentHealth = maxHealth;  // Initialiser les points de vie à la valeur max
        UpdateHealthDisplay();  // Met à jour l'affichage des PV dès le début
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;   // Réduire les points de vie
        if (currentHealth < 0)
            currentHealth = 0;  // Pour éviter que les PV ne deviennent négatifs

        UpdateHealthDisplay();  // Met à jour l'affichage après avoir pris des dégâts

        if (currentHealth <= 0)
        {
            Die();  // Appeler la fonction de mort si les PV sont à zéro
        }
    }

    void Die()
    {
        // Ici, tu peux gérer la mort du joueur (ex : afficher une explosion, désactiver le vaisseau, etc.)
        Debug.Log("Le vaisseau principal est détruit !");
        Destroy(gameObject);  // Détruire le vaisseau (ou désactiver selon ce que tu veux)
    }

    void UpdateHealthDisplay()
    {
        // Met à jour le texte affichant les PV du joueur
        if (hpText != null)
        {
            hpText.text = "HP: " + currentHealth.ToString();
        }
    }
}
