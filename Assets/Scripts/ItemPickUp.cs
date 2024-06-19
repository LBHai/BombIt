using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public float speedIncrease;
    public enum ItemType
    {
        ExtraBomb,
        BlastRadius,
        SpeedIncrease,
        Health,
    }
    public ItemType type;
    private void OnItemPickUp(GameObject player)
    {
       
        switch (type)
        {
            case ItemType.ExtraBomb:
                player.GetComponent<BoomController>().IncreaseMaxBombs();
                break;
            case ItemType.BlastRadius:
                player.GetComponent<BoomController>().explosionRadius++;
                break;
            case ItemType.SpeedIncrease:
                player.GetComponent<PlayerController>().speed += speedIncrease;
                break;
            case ItemType.Health:
                player.GetComponent<PlayerController>().IncreaseHealth(1);
                break;
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnItemPickUp(collision.gameObject);
        }
    }
}
