using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float timeBetweenBombs = 5f;
    public float moveSpeed = 2f;
    private float bombTimer;
    private Vector2 moveDirection;
    private Animator _anim;
    private Rigidbody2D _rb;
    private EnemySpawner enemySpawner;
    public Transform player;
    void Start()
    {
        
        ChooseNewDirection();
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();

        enemySpawner = FindObjectOfType<EnemySpawner>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            _rb.velocity = direction * moveSpeed;

            if (direction != Vector2.zero)
            {
                _anim.SetFloat("moveX", direction.x);
                _anim.SetFloat("moveY", direction.y);
                _anim.SetBool("run", true);
            }
            else
            {
                _anim.SetBool("run", false);
            }

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.5f, LayerMask.GetMask("Obstacle"));
            if (hit.collider != null)
            {
                ChooseNewDirection();
            }
        }
    }

    void ChooseNewDirection()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            moveDirection = direction;
        }
        else
        {
            int direction = Random.Range(0, 4);
            switch (direction)
            {
                case 0:
                    moveDirection = Vector2.up;
                    break;
                case 1:
                    moveDirection = Vector2.down;
                    break;
                case 2:
                    moveDirection = Vector2.left;
                    break;
                case 3:
                    moveDirection = Vector2.right;
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            Destroy(gameObject);
            if (enemySpawner != null)
            {
                enemySpawner.EnemyDestroyed();
            }
        }

    }
    
}
