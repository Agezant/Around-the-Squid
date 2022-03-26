using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_control : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    bool isGrounded;
    public float velocity;
    public float jumpHeight; 
    public Transform ground_check;
    public int HealthPoints = -111111111;
    int CurrentHealthPoints;

    
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        CurrentHealthPoints = HealthPoints;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetAxis("Horizontal") == 0 && isGrounded) {
            animator.SetInteger("state", 3);
        }
        else {
            Flip();
            if (isGrounded)
            {
                animator.SetInteger("state", 2);
            }
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
                rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
            }
            GroundCheck();
        }
    }

    void Flip()
    {
        if(Input.GetAxis("Horizontal") > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0); 
        }
        else if(Input.GetAxis("Horizontal") < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * velocity, rb.velocity.y);
    }

    void GroundCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(ground_check.position, 0.2f);
        isGrounded = colliders.Length > 1;
        if (!isGrounded)
        {
            animator.SetInteger("state", 3);
        }
    }

    public void RecountHealthPonts(int deltaHealthPoints)
    {
        CurrentHealthPoints += deltaHealthPoints;
        if (deltaHealthPoints < 0)
        {
            StartCoroutine(OnHit());
        }
        print(CurrentHealthPoints);
        if (CurrentHealthPoints <= 0){
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }

    IEnumerator OnHit()
    {
        SpriteRenderer sr =  GetComponent<SpriteRenderer>();
        sr.color = new Color(1f, sr.color.g - 0.02f, sr.color.b - 0.02f);
        yield return new WaitForSeconds(0.02f);
    }
}
