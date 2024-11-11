using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public CharacterController2D controller;
    public float speed = 1f;
    public float jumpforce = 3f;
    private float jumpcooldown = 0f;
    float horizontalMove = 0f;
    public Animator animator;
    public Rigidbody2D rb;
    SpriteRenderer sr;
    private HingeJoint2D hj;

    public float pushForce = 10f;

    public bool attached = false;
    public Transform attachedTo;
    private GameObject disregard;

    bool isDead = false;

    //[Header("Event")]
    //[Space]

    //public UnityEvent OnLandEvent;

    private void Start()
    {
        rb =gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        hj = gameObject.GetComponent<HingeJoint2D>();
    }
    private void Update()
    {
        CheckKeyboardInputs();

        if (isDead)
            return;

        float movement = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position += new Vector3(movement, 0, 0) * (speed + 4) * Time.deltaTime;
        }
        else
        {
            transform.position += new Vector3(movement, 0, 0) * speed * Time.deltaTime;
        }

        if (jumpcooldown > 0f)
        {
            jumpcooldown -= Time.deltaTime;
        }

        if ((Input.GetKey(KeyCode.UpArrow)||Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.W)) && Mathf.Abs(rb.velocity.y) < 0.05f && jumpcooldown <= 0f)
        {
            rb.AddForce(new Vector2(0, jumpforce), ForceMode2D.Impulse);
            jumpcooldown = 1.2f;
            animator.SetBool("isjumping", true);
        }
        else
        {
            OnLanding();
            //animator.SetBool("isjumping", false);
        }
            
            
        sr.flipX = movement < 0 ? true : false;

        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        
    }

    void CheckKeyboardInputs()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey("left"))
        {
            if (attached)
            {
                rb.AddRelativeForce(new Vector3(-1, 0, 0) * pushForce);
            }
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey("right"))
        {
            if (attached)
            {
                rb.AddRelativeForce(new Vector3(1, 0, 0) * pushForce);
            }
        }
        if ((Input.GetKeyDown("w") || Input.GetKeyDown("up")) && attached)
        {
            Slide(1);
        }
        if ((Input.GetKeyDown("s") || Input.GetKeyDown("down")) && attached)
        {
            Slide(-1);
        }
        if (Input.GetKeyDown(KeyCode.Space) && attached)
        {
            Detach();
        }
    }
    public void Attach(Rigidbody2D ropeBone)
    {
        ropeBone.gameObject.GetComponent<RopeSegment>().isPlayerAttached = true;
        hj.connectedBody= ropeBone;
        hj.enabled = true;
        attached = true;
        attachedTo = ropeBone.gameObject.transform.parent;
    }
    void Detach()
    {
        hj.connectedBody.gameObject.GetComponent<RopeSegment>().isPlayerAttached=false;
        attached = false;
        hj.enabled= false;
        hj.connectedBody = null;
        
        StartCoroutine(AttachedNull());
    }
    IEnumerator AttachedNull()
    {
        yield return new WaitForSeconds(0.5f);
        attachedTo = null;
    }
    public void Slide(int direction)
    {
        RopeSegment myConnection = hj.connectedBody.gameObject.GetComponent<RopeSegment>();
        GameObject newSeg = null;
        if (direction > 0)
        {
            if (myConnection.connectedAbove != null)
            {
                if(myConnection.connectedAbove.gameObject.GetComponent<RopeSegment>() != null)
                {
                    newSeg = myConnection.connectedAbove;
                }
            }
        }
        else
        {
            if(myConnection.connectedBelow != null)
            {
                newSeg = myConnection.connectedBelow;
            }
        }
        if (newSeg != null)
        {
            transform.position = newSeg.transform.position;
            myConnection.isPlayerAttached = false;
            newSeg.GetComponent<RopeSegment>().isPlayerAttached = true;
            hj.connectedBody = newSeg.GetComponent<Rigidbody2D>();
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!attached)
        {
            if (col.gameObject.tag == "Rope")
            {
                if(attachedTo != col.gameObject.transform.parent)
                {
                    if (disregard == null || col.gameObject.transform.parent.gameObject != disregard)
                    {
                        Attach(col.gameObject.GetComponent<Rigidbody2D>());
                    }
                }
            }
        }
    }
    public void OnLanding()
    {
        animator.SetBool("isjumping", false);
    }
    
    public void Die()
    {
        isDead = true;
        FindObjectOfType<levmanag>().Restart();
    }
}