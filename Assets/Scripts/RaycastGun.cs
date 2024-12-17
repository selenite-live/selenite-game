using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(LineRenderer))]
public class RaycastGun : MonoBehaviour
{
    public Camera playerCamera;
    public Transform laserOrigin1; // Canon 1
    public Transform laserOrigin2; // Canon 2
    public float gunRange = 50f;
    public float fireRate = 0.2f;
    public float laserDuration = 0.05f;

    // Son de tir
    public AudioSource shootSound; // Assigner via l'inspecteur

    // Dégâts infligés par l'arme
    public int damage = 10; // Dégâts de l'arme laser
    public int alternateWeaponDamage = 30; // Dégâts de la boule laser

    // TextMeshPro pour afficher l'arme actuelle et la charge
    public TextMeshPro textWeapon;  // TextMeshPro - arme actuelle
    public TextMeshPro textCharge;  // TextMeshPro - charge actuelle

    // Zone de charge
    private float charge = 100f;  // Charge du gun (100% au départ)
    private bool isCharging = false;  // Pour savoir si l'arme est en train de charger

    // Nouveau booléen pour désactiver la perte de charge
    public bool chargeSystemEnabled = true;  // Activer/Désactiver la perte de charge
    public float rechargeRate = 5f;  // Vitesse de recharge configurable

    // Prefab de la boule laser
    public GameObject laserBallPrefab;  // Prefab de la boule laser
    public Transform laserBallSpawnPoint; // Point de départ de la boule laser

    // Vitesse de la boule laser
    public float laserBallSpeed = 20f; // Vitesse modifiable via l'inspecteur

    LineRenderer laserLine;
    float fireTimer;
    private int currentWeapon = 0; // 0 pour le laser, 1 pour la boule laser

    void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
        laserLine.positionCount = 4; // 2 points pour chaque canon
        UpdateWeaponText();
    }

    void Update()
    {
        fireTimer += Time.deltaTime;

        // Vérifier si l'arme peut tirer (charge > 0 et pas en train de recharger ou si le système de charge est désactivé)
        bool canFire = (charge > 0 || !chargeSystemEnabled) && fireTimer > fireRate;

        // Détection de l'appui du bouton de tir
        if (Input.GetButton("Fire1") && canFire)
        {
            fireTimer = 0;

            if (currentWeapon == 0)  // Si l'arme actuelle est le laser
            {
                // Tir du canon 1
                RaycastAndShoot(laserOrigin1, 0, 1); // Les positions 0 et 1 dans le LineRenderer sont pour le canon 1

                // Tir du canon 2
                RaycastAndShoot(laserOrigin2, 2, 3); // Les positions 2 et 3 dans le LineRenderer sont pour le canon 2

                StartCoroutine(ShootLaser());
                PlayShootSound();  // Jouer le son de tir seulement lorsqu'un laser est tiré
            }
            else if (currentWeapon == 1)  // Si l'arme actuelle est la boule laser
            {
                ShootLaserBall();
                PlayShootSound();  // Jouer le son pour la boule laser
            }

            // Réduire la charge après un tir si le système de charge est activé
            if (chargeSystemEnabled)
            {
                charge -= 10f;  // Réduit de 10% par tir
                charge = Mathf.Max(charge, 0);  // Empêche la charge de descendre en dessous de 0
            }
            UpdateChargeText();
        }

        // Détection du relâchement du bouton de tir
        if (Input.GetButtonUp("Fire1"))
        {
            // Arrêter le son si le tir est relâché ou si la charge est épuisée
            if (charge <= 0 || fireTimer < fireRate)
            {
                shootSound.Stop();        // Arrêter le son immédiatement
            }
        }

        // Recharge de l'arme si la charge est inférieure à 100% et le système de charge est activé
        if (charge < 100f && !isCharging && chargeSystemEnabled)
        {
            StartCoroutine(RechargeWeapon());
        }

        // Si le système de charge est désactivé, maintenir la charge à 100%
        if (!chargeSystemEnabled)
        {
            charge = 100f;
            UpdateChargeText();
        }

        // Changer d'arme (touche E)
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeWeapon();
        }
    }

    void RaycastAndShoot(Transform laserOrigin, int startIndex, int endIndex)
    {
        laserLine.SetPosition(startIndex, laserOrigin.position);

        Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, gunRange))
        {
            laserLine.SetPosition(endIndex, hit.point);

            // Si l'arme touche un objet avec un script de santé, infliger des dégâts
            if (hit.transform.GetComponent<EnemyHealth>() != null)
            {
                hit.transform.GetComponent<EnemyHealth>().TakeDamage(damage); // Inflige les dégâts
            }
        }
        else
        {
            laserLine.SetPosition(endIndex, rayOrigin + (playerCamera.transform.forward * gunRange));
        }
    }

    IEnumerator ShootLaser()
    {
        laserLine.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        laserLine.enabled = false;
    }

    void UpdateWeaponText()
    {
        // Afficher l'arme actuelle dans le TextMeshPro
        if (currentWeapon == 0)
        {
            textWeapon.text = "Weapon: Laser";
        }
        else if (currentWeapon == 1)
        {
            textWeapon.text = "Weapon: Laser Ball";
        }
    }

    void UpdateChargeText()
    {
        // Afficher la charge actuelle dans le TextMeshPro
        textCharge.text = "Charge: " + Mathf.Round(charge) + "%";
    }

    IEnumerator RechargeWeapon()
    {
        isCharging = true;
        while (charge < 100f)
        {
            charge += rechargeRate * Time.deltaTime;  // Recharge l'arme lentement selon la vitesse configurée
            charge = Mathf.Min(charge, 100f);  // Empêche la charge de dépasser 100%
            UpdateChargeText();
            yield return null;
        }
        isCharging = false;
    }

    void ChangeWeapon()
    {
        // Alterne entre les armes (Laser et Boule Laser)
        currentWeapon = (currentWeapon + 1) % 2;  // Alterne entre 0 et 1
        damage = (currentWeapon == 0) ? 10 : alternateWeaponDamage;  // Change les dégâts
        UpdateWeaponText();  // Mise à jour du texte affichant l'arme actuelle
    }

    void ShootLaserBall()
    {
        // Crée une boule laser à partir du point d'origine et la propulse dans la direction du tir
        GameObject laserBall = Instantiate(laserBallPrefab, laserBallSpawnPoint.position, laserBallSpawnPoint.rotation);
        Rigidbody rb = laserBall.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(playerCamera.transform.forward * laserBallSpeed, ForceMode.VelocityChange);  // Vitesse configurable via l'inspecteur
        }

        // Inflige des dégâts si le projectile touche un ennemi (le script du projectile peut inclure cette logique)
        laserBall.GetComponent<LaserBall>().damage = alternateWeaponDamage;  // Assigne les dégâts de la boule laser
    }

    void PlayShootSound()
    {
        // Joue le son de tir
        if (!shootSound.isPlaying && (charge > 0 || !chargeSystemEnabled))
        {
            shootSound.Play();  // Joue le son
        }
    }
}
