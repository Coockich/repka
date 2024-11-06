using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts._4
{
    public class movingplatform : MonoBehaviour
    {
        [SerializeField] private Transform targetPoint;
        [SerializeField] private float speed;
        private Vector2 originalPoint;
        private Vector2 currentTargetPoint;
        private Rigidbody2D body;
        private void Awake()
        {
            originalPoint = transform.position;
            currentTargetPoint = originalPoint;
            body = GetComponent<Rigidbody2D>();
        }
        public void FixedUpdate()
        {
            if (Vector2.Distance(body.position, currentTargetPoint) < 0.1f)
            {
                if (currentTargetPoint == originalPoint)
                {
                    currentTargetPoint = targetPoint.position;
                }
                else
                {
                    currentTargetPoint = originalPoint;
                }
                var direction = (currentTargetPoint - body.position).normalized;
                body.velocity = direction * speed;
            }
        }
    }
}


