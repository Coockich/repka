using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Platformer
{
    public class hud : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Button reloadButton;
        [SerializeField] private GameObject winText;

        private readonly UICommandQueue commandQueue = new UICommandQueue();

        public void UpdateScore(int a)
        {
            commandQueue.TryEnqueueCommand(new UpdateScoreCommand(a));
        }

        private void Start()
        {
            Time.timeScale = 1;
            reloadButton.onClick.AddListener(() => commandQueue.TryEnqueueCommand(new ReloadCommand()));
            StartCoroutine(UpdateTask());
        }
        private IEnumerator UpdateTask()
        {
            while (true)
            {
                if (commandQueue.TryDequeueCommand(out var command))
                {
                    switch (command)
                    {
                        case ReloadCommand reloadCommand:
                            {
                                var currentScene = SceneManager.GetActiveScene();
                                var newScene = SceneManager.LoadSceneAsync(currentScene.name);
                                newScene.completed += (operation) =>
                                {
                                    Time.timeScale = 1;
                                };
                                yield return newScene;
                                break;
                            }
                        case UpdateScoreCommand update:
                            {
                                scoreText.text = $"Score:{update.NewScore}";
                                break;
                            }
                        case wincommand winCommand:
                            {
                                winText.SetActive(true);
                                break;
                            }
                    }


                }
                yield return null;
            }
        }
        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}
