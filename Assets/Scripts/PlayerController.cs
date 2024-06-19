using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ItemPickUp;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 5f;
    private Rigidbody2D _rb;
    private Animator _anim;
    public GhostEffect _effect;
    public float health, maxHealth;
    public HealthHeartBar healthHeartBar;
    private bool isInvulnerable = false;
    private int speedIncreaseCount = 0;
    public float initialSpeed;
    public float currentSpeed;
    public RestartGameUI restartGameUI;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        health = maxHealth;
        healthHeartBar.UpdateHearts();
        initialSpeed = speed; 
        currentSpeed = speed; 
    }
    void Update()
    {

        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        _rb.velocity = movement * currentSpeed;
        if (movement != Vector2.zero)
        {
            _anim.SetFloat("moveX", movement.x);
            _anim.SetFloat("moveY", movement.y);
            _anim.SetBool("run", true);

        }
        else
        {
            _anim.SetBool("run", false);
        }
    }

    private void ResetHitAnimation()
    {
        _anim.SetBool("hit", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isInvulnerable)
        {
            if (collision.CompareTag("Explosion"))
            {
                _anim.SetBool("hit", true);
                Invoke("ResetHitAnimation", 1.0f);

                health -= 1f;

                if (health <= 0)
                {
                    _anim.SetTrigger("isDead");
                    restartGameUI.ShowRestartPanel();
                }

                if (healthHeartBar != null)
                {
                    healthHeartBar.UpdateHearts();
                }
                StartCoroutine(InvulnerabilityCoroutine(1.0f));
            }
        }
        else
        {
            currentSpeed = initialSpeed;
            _effect.enabled = false;
        }

        if (collision.CompareTag("SpeedIncrease"))
        {
            speedIncreaseCount++;
            currentSpeed += 2f;
            if (speedIncreaseCount >= 2)
            {
                _effect.enabled = true;
                speedIncreaseCount = 0;
            }
        }
    }
    private IEnumerator InvulnerabilityCoroutine(float duration)
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(duration);
        isInvulnerable = false;
    }
    public void IncreaseHealth(int amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        if (healthHeartBar != null)
        {
            healthHeartBar.UpdateHearts();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isInvulnerable)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                _anim.SetBool("hit", true);
                Invoke("ResetHitAnimation", 1.0f);

                health -= 1f;

                if (health <= 0)
                {
                    _anim.SetTrigger("isDead");
                    restartGameUI.ShowRestartPanel();
                }

                if (healthHeartBar != null)
                {
                    healthHeartBar.UpdateHearts();
                }
                StartCoroutine(InvulnerabilityCoroutine(1.0f));
            }
        }
        else
        {
            currentSpeed = initialSpeed;
            _effect.enabled = false;
        }

        if (collision.gameObject.CompareTag("SpeedIncrease"))
        {
            speedIncreaseCount++;
            currentSpeed += 2f;
            if (speedIncreaseCount >= 2)
            {
                _effect.enabled = true;
                speedIncreaseCount = 0;
            }
        }
    }
}