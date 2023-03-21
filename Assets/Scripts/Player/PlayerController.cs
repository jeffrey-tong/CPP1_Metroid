using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    //Components
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    BoxCollider2D collider;
    AudioSourceManager asm;

    public float speed;
    public float jumpForce;

    public bool isGrounded;
    public Transform groundCheck;
    public LayerMask isGroundLayer;
    public float groundCheckRadius;

    public bool canJumpAttack = false;
    public bool canCrouch = false;
    public bool isCrouching;
    public Vector2 standingOffset;
    public Vector2 standingSize;
    public Vector2 crouchingOffset;
    public Vector2 crouchingSize;

    Coroutine jumpForceChange;

    //soundclips
    public AudioClip jumpSound;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        asm = GetComponent<AudioSourceManager>();

        if(speed <= 0)
        {
            speed = 6.0f;
            Debug.Log("Speed was set incorrectly, defaulting to " + speed.ToString());
        }
        if (jumpForce <= 0)
        {
            jumpForce = 300.0f;
            Debug.Log("Jump Force was set incorrectly, defaulting to " + jumpForce.ToString());
        }
        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.2f;
            Debug.Log("Ground Check Radius was set incorrectly, defaulting to " + groundCheckRadius.ToString());
        }

        if (!groundCheck)
        {
            groundCheck = GameObject.FindGameObjectWithTag("GroundCheck").transform;
            Debug.Log("Ground Check not set, finding it manually");
        }
    }

    void Update()
    {
        AnimatorClipInfo[] curPlayingClip = anim.GetCurrentAnimatorClipInfo(0);
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);

        if (curPlayingClip.Length > 0)
        {
            if (Input.GetButtonDown("Fire1") && curPlayingClip[0].clip.name != "Shoot")
            {
                anim.SetTrigger("shoot");
            }
            else if (Input.GetButtonDown("Fire2") && curPlayingClip[0].clip.name != "Missile" && GameManager.instance.numMissiles > 0)
            {
                anim.SetTrigger("missile");
                GameManager.instance.numMissiles--;
            }
            else
            {
                Vector2 moveDirection = new Vector2(hInput * speed, rb.velocity.y);
                rb.velocity = moveDirection;
            }
        }
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce);
            canJumpAttack = true;
            asm.PlayOneShot(jumpSound, false);
        }
        if (!isGrounded && Input.GetButtonDown("Jump") && canJumpAttack)
        {
            anim.SetTrigger("JumpAttack");
            rb.AddForce(Vector2.up * jumpForce);
            canJumpAttack = false;
            asm.PlayOneShot(jumpSound, false);
        }

        if (canCrouch && vInput < -0.1)
        {
            isCrouching = true;
            collider.size = crouchingSize;
            collider.offset = crouchingOffset;
        }
        if (canCrouch && vInput > -0.1)
        {
            isCrouching = false;
            collider.size = standingSize;
            collider.offset = standingOffset;
        }

        anim.SetFloat("hInput", Mathf.Abs(hInput));
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isCrouching", isCrouching);

        //check for flipped and create an algorithm to flip character
        if (hInput != 0)
        {
            sr.flipX = hInput < 0;
        }
    }
    public void StartJumpForceChange()
    {
        if (jumpForceChange == null)
        {
            jumpForceChange = StartCoroutine(JumpForceChange());
        }
        else
        {
            StopCoroutine(jumpForceChange);
            jumpForceChange = null;
            jumpForce /= 2;
            jumpForceChange = StartCoroutine(JumpForceChange());
        }
    }

    IEnumerator JumpForceChange()
    {
        jumpForce *= 2;

        yield return new WaitForSeconds(5.0f);

        jumpForce /= 2;
        jumpForceChange = null;
    }

}
