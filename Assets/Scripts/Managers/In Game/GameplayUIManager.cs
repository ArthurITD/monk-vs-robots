using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Opsive.UltimateCharacterController.Traits;
using Opsive.Shared.Events;

public class GameplayUIManager : MonoBehaviour
{
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winPanel;

    void Start()
    {
        EventHandler.RegisterEvent<GameEndedType>("GameEnded", OnGameEnded);
    }

    private void OnGameEnded(GameEndedType gameEndedType)
    {
        switch (gameEndedType)
        {
            case GameEndedType.Win:
                ShowHidePanel(winPanel, true);
                break;
            case GameEndedType.Lose:
                ShowHidePanel(losePanel, true);
                break;
        }
    }

    private void ShowHidePanel(GameObject panel, bool isActive)
    {
        Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isActive;
        panel.SetActive(isActive);
    }

    private void OnDestroy()
    {
        EventHandler.UnregisterEvent<GameEndedType>("GameEnded", OnGameEnded);
    }

    public void OnRestartClicked()
    {
        ShowHidePanel(losePanel, false);
        EventHandler.ExecuteEvent("GameRestarted");
    }

    public void OnExitClick()
    {
        losePanel.SetActive(false);
        winPanel.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
