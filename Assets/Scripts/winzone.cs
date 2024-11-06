using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts._4
{
    internal class Winzone : MonoBehaviour
    {
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<player>(out var player))
            {
                Time.timeScale = 0;
            }
        }
    }
}
