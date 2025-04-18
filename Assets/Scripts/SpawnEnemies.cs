using System.Collections;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;   
    [SerializeField] private GameObject[] bosses;    
    [SerializeField] private float velocidadSpawnBase = 6f;    
    [SerializeField] private float velocidadSpawnMinima = 2f;    
    
    private float velocidadSpawn;

    private float temporizador = 0;

    private Vector3 spawnPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnEnemyCoroutine());
    }

    // Update is called once per frame
    void Update()
    {

        // aumenta el nivel de juego cada 10 segundos
        temporizador += Time.deltaTime;
        if(temporizador > 10)
        {
            GameManager.instancia.aumentarNivel();
            temporizador = 0;
        }
        
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        int i = 0;

        while (true)
        {
            // 10 enemigos
            for (i = 0; i < 10; i++)
            {
                // Randomly select an enemy prefab from the array
                GameObject enemyPrefab = enemies[Random.Range(0, enemies.Length)];
                
                // Set the spawn position to a random point within the specified range
                spawnPosition = new Vector3(transform.position.x, Random.Range(-3, 3), 0);
                // Instantiate the enemy prefab at the current position of the spawner
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                
                // Wait for the specified spawn interval before spawning the next enemy
                velocidadSpawn = velocidadSpawnBase - GameManager.instancia.velocidad;
                if (velocidadSpawn < velocidadSpawnMinima) { velocidadSpawn = velocidadSpawnMinima; }
                yield return new WaitForSeconds(velocidadSpawn);
            }


            // 1 boss
            // Randomly select an enemy prefab from the array
            GameObject bossPrefab = bosses[Random.Range(0, bosses.Length)];
            
            // Instantiate the enemy prefab at the current position of the spawner
            Instantiate(bossPrefab, transform.position, Quaternion.identity);
            
            // Wait for the specified spawn interval before spawning the next enemy
            yield return new WaitForSeconds(15f);

        }
    }
}
