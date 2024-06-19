using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoomController : MonoBehaviour
{
    [SerializeField]
    public GameObject BombPrefab;
    public int maxBombs = 2;
    public List<GameObject> poolBomb; 
    public List<float> bombTimers;
    public float timeExplosion = 3f;
    private Animator _anim;
    public float explosionDuration = 1f;
    public ExplosionController explosionPrefabs;
    public int explosionRadius = 1;
    public LayerMask explosionLayerMask;
    public Tilemap breakUnknownTiles;
    public BreakUnknownController breakUnknownPrefabs;

    void Start()
    {
        poolBomb = new List<GameObject>();
        bombTimers = new List<float>();
        _anim = GetComponent<Animator>();

        for (int i = 0; i < maxBombs; i++)
        {
            AddBombToPool();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaceBomb();
        }
        for (int i = 0; i < maxBombs; i++)
        {
            if (poolBomb[i].activeSelf)
            {
                bombTimers[i] += Time.deltaTime;
                if (bombTimers[i] >= timeExplosion)
                {
                    Vector2 bombPosition = poolBomb[i].transform.position;
                    poolBomb[i].SetActive(false);
                    ExplosionController explosion = Instantiate(explosionPrefabs, bombPosition, Quaternion.identity);
                    explosion.SetActiveRenderer(explosion.start);
                    explosion.DestroyAfter(explosionDuration);
                    GameObject explosionGameObject = explosion.gameObject;
                    StartCoroutine(DestroyExplosionAfterDelay(explosionGameObject, explosionDuration));

                    Explore(bombPosition, Vector2.up, explosionRadius);
                    Explore(bombPosition, Vector2.down, explosionRadius);
                    Explore(bombPosition, Vector2.left, explosionRadius);
                    Explore(bombPosition, Vector2.right, explosionRadius);
                }
            }
        }
    }
    void PlaceBomb()
    {
        int index = GetNextAvailableBombIndex();
        if (index != -1)
        {
            GameObject placeBomb = poolBomb[index];
            Vector2 position = transform.position;
            position.x = Mathf.Round(position.x) - 0.5f;
            position.y = Mathf.Round(position.y) - 0.5f;

            if (!IsBombAtPosition(position))
            {
                placeBomb.transform.position = new Vector3(position.x, position.y, transform.position.z);
                placeBomb.SetActive(true);
                placeBomb.GetComponent<Collider2D>().isTrigger = true;
                bombTimers[index] = 0f;
            }
        }
    }
    public int GetNextAvailableBombIndex()
    {
        for (int i = 0; i < maxBombs; i++)
        {
            if (!poolBomb[i].activeSelf)
            {
                return i;
            }
        }
        return -1;
    }

    private void AddBombToPool()
    {
        GameObject newBomb = Instantiate(BombPrefab);
        newBomb.SetActive(false);
        poolBomb.Add(newBomb);
        bombTimers.Add(0f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            collision.isTrigger = false;
        }
    }

    private IEnumerator DestroyExplosionAfterDelay(GameObject explosion, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(explosion);
    }

    void Explore(Vector2 position, Vector2 direction, int length)
    {
        if (length <= 0)
        {
            return;
        }

        position += direction;

        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            BreakUnknown(position);
            return;
        }

        ExplosionController explosion = Instantiate(explosionPrefabs, position, Quaternion.identity);
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionDuration);
        Explore(position, direction, length - 1);
    }

    public bool IsBombAtPosition(Vector2 position)
    {
        foreach (GameObject bomb in poolBomb)
        {
            if (bomb.activeSelf && (Vector2)bomb.transform.position == position)
            {
                return true;
            }
        }
        return false;
    }

    private void BreakUnknown(Vector2 position)
    {
        Vector3Int cell = breakUnknownTiles.WorldToCell(position);
        TileBase tile = breakUnknownTiles.GetTile(cell);
        if (tile != null)
        {
            Instantiate(breakUnknownPrefabs, position, Quaternion.identity);
            breakUnknownTiles.SetTile(cell, null);
        }
    }

    public void IncreaseMaxBombs()
    {
        maxBombs++;
        AddBombToPool();
    }
}
