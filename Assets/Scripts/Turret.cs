using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    public float fireRate = 2f;  // Temps entre chaque tir
    public GameObject turretLaserPrefab;  // Prefab du projectile de la tourelle
    public Transform firePoint1;  // Point de tir de la tourelle
    public float bulletSpeed = 30f;  // Vitesse des projectiles

    private float nextFireTime;

    void Update()
    {
        // Si la tourelle peut tirer, elle tire
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + Random.Range(fireRate - 0.5f, fireRate + 0.5f);  // Variation aléatoire du tir
        }
    }

    // Fonction pour tirer des projectiles
    void Shoot()
    {
        // Tirez à partir du point de tir
        ShootFromFirePoint(firePoint1);
    }

    // Fonction pour tirer depuis un point de tir spécifique
    void ShootFromFirePoint(Transform firePoint)
    {
        // Crée une instance du projectile à partir du point de tir
        GameObject laser = Instantiate(turretLaserPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = laser.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Applique une force au projectile dans la direction du point de tir
            rb.AddForce(firePoint.forward * bulletSpeed, ForceMode.VelocityChange);
        }
    }
}
