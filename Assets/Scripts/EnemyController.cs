using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float timeBetweenBombs = 5f;
    public float moveSpeed = 2f;
    private float bombTimer;
    private Vector2 moveDirection;
    private Animator _anim;
    private Rigidbody2D _rb;
    private EnemySpawner enemySpawner;

    void Start()
    {
        bombTimer = timeBetweenBombs;
        ChooseNewDirection();
        StartCoroutine(ChangeDirectionRoutine());
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();

        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    void Update()
    {


        Move();
    }

    void Move()
    {
        _rb.velocity = moveDirection * moveSpeed;

        if (moveDirection != Vector2.zero)
        {
            _anim.SetFloat("moveX", moveDirection.x);
            _anim.SetFloat("moveY", moveDirection.y);
            _anim.SetBool("run", true);
        }
        else
        {
            _anim.SetBool("run", false);
        }


        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, 0.5f, LayerMask.GetMask("Obstacle"));
        if (hit.collider != null)
        {
            ChooseNewDirection();
        }
    }

    void ChooseNewDirection()
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

    IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            ChooseNewDirection();
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
