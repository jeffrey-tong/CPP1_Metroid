using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Player;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class Enemy : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Animator anim;

    public Pickup[] pickupPrefab;
    protected int _health;
    public int maxHealth;
    public AudioClip deathSound;

    public Transform enemyTransform;
    public int health
    {
        get => _health;
        set
        {
            _health = value;
            if (_health > maxHealth)
            {
                _health = maxHealth;
            }
            if(_health <= 0)
            {
                Death();
            }
        }
    }

    public void Hurt()
    {
        anim.SetTrigger("Hurt");
    }

    public void Death()
    {
        GameManager.instance.playerInstance.GetComponent<AudioSourceManager>().PlayOneShot(deathSound, false);
        int num = Random.Range(0, pickupPrefab.Length);
        switch (num)
        {
            case 0:
                Instantiate(pickupPrefab[0], enemyTransform.position, enemyTransform.rotation);
                break;
            case 1:
                Instantiate(pickupPrefab[1], enemyTransform.position, enemyTransform.rotation);
                break;
            case 2:
                Instantiate(pickupPrefab[2], enemyTransform.position, enemyTransform.rotation);
                break;
            default:
                break;
        }
        Destroy(gameObject.transform.parent.gameObject.transform.parent.gameObject);
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        enemyTransform = GetComponent<Transform>();
        if (maxHealth <= 0)
        {
            maxHealth = 5;
        }
        health = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        Hurt();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.lives -= 8;
        }
    }
}
