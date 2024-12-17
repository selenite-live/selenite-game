using System.Collections;
using UnityEngine;
using TMPro;  // Import de TextMeshPro

public class WaveManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;          // Tableau de préfabriqués d'ennemis (vaisseaux)
    public Transform[] spawnPoints;            // Liste des points de spawn pour les ennemis
    public int totalWaves = 10;                // Nombre total de vagues
    public float timeBetweenWaves = 5f;        // Temps entre les vagues
    public TextMeshProUGUI waveMessage;        // Le texte pour afficher les messages de vague (TextMeshPro)

    private int currentWave = 0;               // Numéro de la vague actuelle
    private int enemiesPerWave;                // Nombre d'ennemis dans chaque vague
    private int enemiesRemaining;              // Compteur d'ennemis restants

    void Start()
    {
        StartCoroutine(StartNextWave());
    }

    // Lancer une nouvelle vague
    IEnumerator StartNextWave()
    {
        currentWave++;
        if (currentWave > totalWaves)
        {
            waveMessage.text = "Vous avez gagné !";
            yield break;  // Arrête le jeu une fois que toutes les vagues sont terminées
        }

        waveMessage.text = "Vague " + currentWave;
        enemiesPerWave = currentWave * 2;  // Le nombre d'ennemis augmente à chaque vague
        enemiesRemaining = enemiesPerWave;

        // Délai avant la prochaine vague
        yield return new WaitForSeconds(timeBetweenWaves);

        // Lancer la vague
        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f); // Délai entre les apparitions d'ennemis
        }

        waveMessage.text = "";  // Efface le message après l'apparition des ennemis
    }

    // Fonction pour faire apparaître les ennemis
    void SpawnEnemy()
    {
        // Choisir un point de spawn aléatoire parmi les spawnPoints
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        // Choisir un type d'ennemi aléatoire parmi les préfabriqués
        int enemyIndex = GetEnemyIndexForWave();
        
        // Instancier l'ennemi au point de spawn choisi
        Instantiate(enemyPrefabs[enemyIndex], spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
    }

    // Fonction pour déterminer quel type d'ennemi doit apparaître selon la vague actuelle
    int GetEnemyIndexForWave()
    {
        // Plus la vague est élevée, plus les ennemis de types différents peuvent apparaître
        // Si c'est la première vague, on ne voit que le premier type d'ennemi, puis plus de variété apparaît
        if (currentWave <= 3)
        {
            return 0;  // Premier type d'ennemi
        }
        else if (currentWave <= 6)
        {
            return Random.Range(0, 2);  // Choisir entre les premiers deux types
        }
        else if (currentWave <= 9)
        {
            return Random.Range(0, 3);  // Choisir parmi les trois premiers types
        }
        else
        {
            return Random.Range(0, enemyPrefabs.Length);  // Choisir parmi tous les types d'ennemis
        }
    }

    // Appelée quand un ennemi est tué
    public void EnemyKilled()
    {
        enemiesRemaining--;

        if (enemiesRemaining <= 0)
        {
            StartCoroutine(StartNextWave());  // Passer à la vague suivante
        }
    }
}
