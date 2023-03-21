using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemySkree : Enemy
{
    Rigidbody2D rb;
    public float speed;
    public int skreeRange;
    float timeSinceLastChase;
    float chaseRate = 1.0f;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();

        if (speed <= 0)
        {
            speed = 4.0f;
        }
        if (skreeRange <= 0)
        {
            skreeRange = 6;
            Debug.Log("Skree range not set, setting it to 6");
        }

        timeSinceLastChase = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.playerInstance) return;
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        if(Time.time >= timeSinceLastChase + chaseRate) { 
            if (Mathf.Sqrt(Mathf.Pow(GameManager.instance.playerInstance.transform.position.x - rb.transform.position.x, 2) + Mathf.Pow(GameManager.instance.playerInstance.transform.position.y - rb.transform.position.y, 2)) < skreeRange)
            {
                anim.SetTrigger("Chase");
                Vector2 direction = (Vector2)GameManager.instance.playerInstance.transform.position - rb.position;
                direction.Normalize();
                rb.velocity = direction * speed;
                timeSinceLastChase = Time.time;
            }
        }
        
    }
}
