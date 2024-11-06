using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts._4
{
    internal class Coins : MonoBehaviour
    {
        [SerializeField] private int score = 1;

        private manager levelManager;

        public void SetLevelManager(manager levelManager)
        {
            this.levelManager = levelManager;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<player>(out var player))
            {
                player.AddScore(score);
                gameObject.SetActive(false);
                //levelManager.UpdateScore(score);
            }
        }
    }
}
