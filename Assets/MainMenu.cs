
using System;
using Managers;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject exitGamePanel;
    [SerializeField] private GameObject recordsPanel;
    [Header("Buttons")] [SerializeField] 
    private GameObject exitButton;
    [SerializeField] private GameObject playButton;

    private void Start()
    {
        exitGamePanel.gameObject.SetActive(false);
        recordsPanel.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        LevelManger.Instance.LoadNewGame();
    }

    public void ShowRecords()
    {
        recordsPanel.SetActive(true);
    }

    public void HideRecords()
    {
        recordsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        exitGamePanel.gameObject.SetActive(true);
    }

    public void ConfirmExit()
    {
        Application.Quit();
    }

    public void DenyExit()
    {
        exitGamePanel.gameObject.SetActive(false);
        Vector2 exitPos = exitButton.transform.position;
        Vector2 playPos = playButton.transform.position;

        exitButton.transform.position = playPos;
        playButton.transform.position = exitPos;
    }
}
