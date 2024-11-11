using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using System.Threading.Tasks;
using System.Threading;

public class NewBehaviorScript : MonoBehaviour
{
    public CharacterController2D controller;
    public float speed = 1f;
    public float jumpforce = 3f;
    private float jumpcooldown = 1.8f;
    float horizontalMove = 0f;
    public Animator animator;
    public Rigidbody2D rb;
    SpriteRenderer sr;
    private HingeJoint2D hj;
    private Vector3 inputVector;

    public float pushForce = 10f;

    public bool attached = false;
    public bool running = false;
    private bool faceRight = true;
    public Transform attachedTo;
    private GameObject disregard;

    bool isDead = false;

    //[Header("Event")]
    //[Space]

    //public UnityEvent OnLandEvent;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        hj = gameObject.GetComponent<HingeJoint2D>();
    }
    private async void Update()
    {
        CheckKeyboardInputs();

        if (isDead)
            return;

        inputVector.x = Input.GetAxis("Horizontal");

        ReflectPlayer();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position += inputVector * (speed + 4) * Time.deltaTime;

            animator.SetBool("running", true);
        }
        else
        {
            transform.position += inputVector * speed * Time.deltaTime;

            animator.SetBool("running", false);
        }

        if (jumpcooldown > 0f)
        {
            jumpcooldown -= Time.deltaTime;
        }

        if ((Input.GetKey(KeyCode.UpArrow)||Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.W)) && Mathf.Abs(rb.velocity.y) < 0.05f && jumpcooldown <= 0f)
        {
            animator.SetBool("isjumping", true);
            jumpcooldown = 1.8f;
            await Task.Delay(500);
            rb.AddForce(new Vector2(0, jumpforce), ForceMode2D.Impulse);
        }
        else
        {
            OnLanding();
            //animator.SetBool("isjumping", false);
        }
            
            
        sr.flipX = inputVector.x < 0 ? true : false;

        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;

        if (horizontalMove != 0 || 
            Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) 
            animator.SetBool("ismoving", true);
        else
            animator.SetBool("ismoving", false);
    }

    void ReflectPlayer() 
    {
        if (inputVector.x < 0 && faceRight ||
            inputVector.x > 0 && !faceRight)
        {
            Vector3 temp = transform.localScale;
            temp.x *= -1;
            transform.localScale = temp;

            faceRight = !faceRight;
        }
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
        hj.connectedBody.gameObject.GetComponent<RopeSegment>().isPlayerAttached = false;
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
                if (attachedTo != col.gameObject.transform.parent)
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