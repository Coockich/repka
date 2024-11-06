using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 1f;
    public float jumpforce = 5f;
    float horizontalMove = 0f;
    public Animator animator;
    Rigidbody2D rb;
    SpriteRenderer sr;

    [Header("Event")]
    [Space]

    public UnityEvent OnLandEvent;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        float movement = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position += new Vector3(movement, 0, 0) * (speed + 2) * Time.deltaTime;
        }
        else
        {
            transform.position += new Vector3(movement, 0, 0) * speed * Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) && Mathf.Abs(rb.velocity.y) < 0.05f)
        {
            rb.AddForce(new Vector2(0, jumpforce), ForceMode2D.Impulse);
            animator.SetBool("isjumping", true);
        }
        else
        {
            animator.SetBool("isjumping",false);
        }
            
            
        sr.flipX = movement < 0 ? true : false;

        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        
    }
    public void OnLanding()
    {
        animator.SetBool("isjumping",false);
    }
    
}