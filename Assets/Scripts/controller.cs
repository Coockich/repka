using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Assets.Scripts
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private float speed = 10;
        [SerializeField] private float jump = 10;

        [Header("Gravity Settings")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private CircleCollider2D groundCollider;
        [SerializeField] private float gravity;
        [SerializeField] private float maxSlopeAngle = 60f;
        
        private Rigidbody2D body;
        private ContactFilter2D groundFilter;

        private bool isJump;
        private Vector2 movementInput;
        private bool isGrounded;

        private Vector2 gravityVelocity;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }
        private void Start()
        {
            groundFilter.SetLayerMask(groundLayer);
            groundFilter.useLayerMask = true;
            groundFilter.useTriggers = false;
        }
        public void OnJump()
        {
            isJump = true;
        }
        public void OnMove(InputValue input)
        {
            movementInput = input.Get<Vector2>();
        }
        private void FixedUpdate()
        {
            GroundCheck();

            if (!isGrounded)
            {
                gravityVelocity = gravityVelocity - Vector2.down * Physics2D.gravity * gravityVelocity * Time.fixedDeltaTime;
            } else
            {
                gravityVelocity = Vector2.Max(Vector2.zero, gravityVelocity);
            }
            if (isJump && isGrounded)
            {
                gravityVelocity += jump * Vector2.up;
            }
            isJump = false;
            var deltaPosition = movementInput * Vector2.right * speed * Time.fixedDeltaTime;
            var deltaGravity = gravityVelocity * Time.fixedDeltaTime;
            body.MovePosition(body.position + deltaPosition);
        }
        private void GroundCheck()
        {
            Collider2D[] collides = new Collider2D[16];
            var count = groundCollider.OverlapCollider(groundFilter, collides);
            isGrounded = count > 0;
        }
    }
}