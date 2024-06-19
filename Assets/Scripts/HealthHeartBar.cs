    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class HealthHeartBar : MonoBehaviour
    {
    public GameObject heartPrefab;
    public PlayerController playerHealth;
    List<HealthHeart> hearts = new List<HealthHeart>();

    public void Start()
    {
        DrawHearts();
    }

    public void DrawHearts()
    {
        ClearHearts();
        int heartCount = Mathf.CeilToInt(playerHealth.maxHealth / 2f);

        for (int i = 0; i < heartCount; i++)
        {
            CreateHeart();
        }

        UpdateHearts();
    }

    public void CreateHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);
        HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
        heartComponent.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heartComponent);
    }

    public void ClearHearts()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts.Clear();
    }

    public void UpdateHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            int heartIndex = i * 2;
            if (playerHealth.health >= heartIndex + 2)
            {
                hearts[i].SetHeartImage(HeartStatus.Full);
            }
            else if (playerHealth.health == heartIndex + 1)
            {
                hearts[i].SetHeartImage(HeartStatus.Half);
            }
            else
            {
                hearts[i].SetHeartImage(HeartStatus.Empty);
            }
        }
    }
}
