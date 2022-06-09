using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject avatarChooserPanel;
    public GameObject mainMenuPanel;
    public GameObject volumeSettingsPanel;
    public GameObject gameTitleObject;

    public Slider uiVolumeSlider;
    public Slider effectsVolumeSlider;
    public Slider musicVolumeSlider;

    public AssetsChooser assetsChooser;

    public ArenaPreviewManager arenaPreviewManager;

    private AuthenticationManager authenticationManager;
    
    public TMP_Text VolumeSliderValueText { get; set; }

    void Awake()
    {
        authenticationManager = GetComponent<AuthenticationManager>();
        if (TotemManager.Instance.currentUser == null)
        {
            authenticationManager.logInPanel.SetActive(true);
        }
        else
        {
            mainMenuPanel.SetActive(true);
        }

        uiVolumeSlider.value = InitVolumeSlider(VolumeType.UI.ToString());
        //effectsVolumeSlider.value = PlayerPrefs.GetFloat(VolumeType.Effects.ToString());
        musicVolumeSlider.value = InitVolumeSlider(VolumeType.Music.ToString());

        AudioManager.Instance.SetMusic(MusicType.Menu);
    }

    public void OnChooseAvatarClick()
    {
        gameTitleObject.SetActive(false);
        mainMenuPanel.SetActive(false);
        arenaPreviewManager.SwitchCameraToAvatarChooser().OnComplete(() =>
        {
            avatarChooserPanel.SetActive(true);
        });
    }

    public void OnLogOutClick()
    {
        assetsChooser.ClearUserData();
        mainMenuPanel.SetActive(false);
        
        arenaPreviewManager.SwitchCameraToShrine().OnComplete(() =>
        {
            authenticationManager.LogOut();
        });
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

    public void OnPlayClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void OnAvatarChooserCancelClick()
    {
        avatarChooserPanel.SetActive(false);
        arenaPreviewManager.SwitchCameraToShrine().OnComplete(() =>
        {
            mainMenuPanel.SetActive(true);
            gameTitleObject.SetActive(true);
        });
    }

    public void ShowHideVolumeSettings(bool isActive)
    {
        volumeSettingsPanel.SetActive(isActive);
        mainMenuPanel.SetActive(!isActive);
    }

    public void SetVolumeSliderText(float value)
    {
        VolumeSliderValueText.text = Math.Round(value * Constants.VOLUME_RECALCULATION_COEF, 1).ToString();
    }

    public void OnSetDefaultVolumeClick()
    {
        uiVolumeSlider.value = Constants.VOLUME_DEFAULT_VALUE;
        musicVolumeSlider.value = Constants.VOLUME_DEFAULT_VALUE;
    }

    private float InitVolumeSlider(string volumeTypeKey)
    {
        return PlayerPrefs.HasKey(volumeTypeKey) ?
            PlayerPrefs.GetFloat(volumeTypeKey) : Constants.VOLUME_DEFAULT_VALUE; 
    }
}
