using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public int spawnLimit = 3;
    private int currentEnemyCount = 0;
    public Tilemap groundTilemap;
    public Text totalKillText;
    private int totalKills = 0;

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            if (currentEnemyCount < spawnLimit)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomGroundTilePosition();

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        currentEnemyCount++;
    }

    Vector3 GetRandomGroundTilePosition()
    {
        Vector3Int randomTilePosition = Vector3Int.zero;
        bool isValidSpawnPosition = false;

        while (!isValidSpawnPosition)
        {
            randomTilePosition = new Vector3Int(
                Random.Range(groundTilemap.cellBounds.min.x, groundTilemap.cellBounds.max.x),
                Random.Range(groundTilemap.cellBounds.min.y, groundTilemap.cellBounds.max.y),
                0
            );
            TileBase tile = groundTilemap.GetTile(randomTilePosition);
            if (tile != null)
            {
                isValidSpawnPosition = true;
            }
        }

        Vector3 spawnPosition = groundTilemap.CellToWorld(randomTilePosition) + new Vector3(0.5f, 0.5f, 0f);

        return spawnPosition;
    }

    public void EnemyDestroyed()
    {
        currentEnemyCount--;
        totalKills++; 
        Debug.Log("Current Enemy Count: " + currentEnemyCount);
        Debug.Log("Total Kills: " + totalKills); 
        UpdateTotalKillsText();
    }


    void UpdateTotalKillsText()
    {
        if (totalKillText != null)
        {
            totalKillText.text = "Total Kills: " + totalKills.ToString();
        }
        
    }
}
