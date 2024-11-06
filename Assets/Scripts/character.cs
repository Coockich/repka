using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent (typeof (Rigidbody2D))]

public class character : MonoBehaviour
{
    private Rigidbody2D body;
    private Vector2 movementInput;
    [SerializeField]
    private float force = 2;
    
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }
    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>().normalized;
    }
    private void FixedUpdate()
    {
        
        body.AddForce(movementInput * force);
        //newPosition = rigidbody.position+velocity*Time.fixedDeltaTime
        //regidbody.MovePosition(newPosition)
        //rigidbody.velocity= velocity;
    }
}
