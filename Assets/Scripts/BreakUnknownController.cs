
using System.Collections.Generic;
using UnityEngine;


public class BreakUnknownController : MonoBehaviour
{
    public float breakUnknownTime = 1f;
    [Range(0f,1f)]
    public float itemSpawnChance = 0.2f;

    public GameObject[] spawnaleItem;
    void Start()
    {
        Destroy(gameObject,breakUnknownTime);
    }

    private void OnDestroy()
    {
        if(spawnaleItem.Length > 0 && Random.value<itemSpawnChance)
        {
           int randomIndex = Random.Range(0, spawnaleItem.Length);
            Instantiate(spawnaleItem[randomIndex],transform.position,Quaternion.identity);
        }
    }
}
