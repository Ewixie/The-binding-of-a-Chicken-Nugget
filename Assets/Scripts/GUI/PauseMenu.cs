using System;
using Managers;
using UnityEngine;

namespace GUI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pausePanel;

        private void Start()
        {
            if (GameManager.Instance is not null)
            {
                pausePanel.gameObject.SetActive(GameManager.Instance.IsPaused);
            }
            else
            {
                pausePanel.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (GameManager.Instance is null) return;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameManager.Instance.IsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }


        public void Pause()
        {
            pausePanel.gameObject.SetActive(true);
            GameManager.Instance.Pause();
        }

        public void Resume()
        {
            pausePanel.gameObject.SetActive(false);
            GameManager.Instance.Unpause();
        }

        public void Restart()
        {
            GameManager.Instance.RestartRun();
        }

        public void Exit()
        {
            LevelManger.Instance.LoadMenu();
        }
    }
}