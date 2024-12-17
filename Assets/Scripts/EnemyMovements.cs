using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;  // Référence au joueur (initialisé dynamiquement si nécessaire)
    public float moveSpeed = 5f;  // Vitesse de déplacement
    public float stoppingDistance = 10f;  // Distance à laquelle l'ennemi s'arrête de se rapprocher du joueur
    public float fireRate = 2f;  // Temps entre chaque tir
    public GameObject enemyLaserPrefab;  // Prefab du projectile ennemi
    public Transform firePoint1;  // Premier point de tir de l'ennemi
    public Transform firePoint2;  // Deuxième point de tir de l'ennemi
    public float bulletSpeed = 30f;  // Vitesse des projectiles
    public float detectionRange = 50f;  // Distance à laquelle l'ennemi détecte le joueur

    private float nextFireTime;

    void Start()
    {
        // Recherche le joueur dans la scène par son tag
        if (player == null)
        {
            player = GameObject.FindWithTag("Player")?.transform;

            // Si le joueur n'est pas trouvé, affiche une erreur
            if (player == null)
            {
                Debug.LogError("Player not found! Ensure the player has the 'Player' tag.");
            }
        }
    }

    void Update()
    {
        // Si le joueur n'est toujours pas assigné, on ne fait rien
        if (player == null) return;

        // Vérifie la distance entre l'ennemi et le joueur
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Si le joueur est dans la zone de détection
        if (distanceToPlayer <= detectionRange)
        {
            // Si l'ennemi est plus loin que la distance d'arrêt, il se déplace vers le joueur
            if (distanceToPlayer > stoppingDistance)
            {
                MoveTowardsPlayer();
            }

            // Si l'ennemi peut tirer (basé sur le time intervalle), il tire peu importe la distance
            if (Time.time >= nextFireTime)
            {
                ShootAtPlayer();
                nextFireTime = Time.time + Random.Range(fireRate - 0.5f, fireRate + 0.5f);  // Variation aléatoire du tir
            }
        }
    }

    // Fonction pour déplacer l'ennemi vers le joueur
    void MoveTowardsPlayer()
    {
        // Direction vers le joueur
        Vector3 direction = (player.position - transform.position).normalized;

        // Mouvement de l'ennemi
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Rotation fluide pour orienter l'ennemi vers le joueur
        Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 200f * Time.deltaTime);
    }

    // Fonction pour tirer des projectiles des deux points de tir
    void ShootAtPlayer()
    {
        // Tirez à partir du premier point de tir
        ShootFromFirePoint(firePoint1);
        // Tirez à partir du deuxième point de tir
        ShootFromFirePoint(firePoint2);
    }

    // Fonction pour tirer depuis un point de tir spécifique
    void ShootFromFirePoint(Transform firePoint)
    {
        // Crée une instance du projectile à partir du point de tir
        GameObject laser = Instantiate(enemyLaserPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = laser.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Applique une force au projectile pour qu'il aille vers le joueur
            Vector3 direction = (player.position - firePoint.position).normalized;
            rb.AddForce(direction * bulletSpeed, ForceMode.VelocityChange);
        }
    }
}
