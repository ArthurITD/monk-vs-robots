using System.Collections;
using System.Collections.Generic;
using TMPro;
using TotemEntities;
using UnityEngine;
using UnityEngine.UI;

public class AssetsChooser : MonoBehaviour
{
    public Image avatarPreview;
    public Image spearPreview;
    public Button nextAvatarButton;
    public Button previousAvatarButton;
    public Button nextSpearButton;
    public Button previousSpearButton;
    public Button playButton;
    public TMP_Text avatarDataTMP;
    public TMP_Text spearDataTMP;

    private int spearPreviewIndex = 0;
    private int avatarPreviewIndex = 0;

    private List<TotemSpear> userSpears = new List<TotemSpear>();
    private List<TotemAvatar> userAvatars = new List<TotemAvatar>();

    public void OnEnable()
    {
        playButton.interactable = true;
        InitializeAssets();
    }

    public void OnDisable()
    {
        if (CheckAvaliableAssets(userSpears))
        {
            spearPreviewIndex = 0;
            ChangeSpear();
        }
        if (CheckAvaliableAssets(userAvatars))
        {
            avatarPreviewIndex = 0;
            ChangeAvatar();
        }
    }

    public void OnAvatarPreviewChange(int indexStep)
    {
        avatarPreviewIndex += indexStep;
        ChangeAvatar();
    }

    public void OnSpearPreviewChange(int indexStep)
    {
        spearPreviewIndex += indexStep;
        ChangeSpear();
    }

    public void ClearUserData()
    {
        userSpears = new List<TotemSpear>();
        userAvatars = new List<TotemAvatar>();
    }

    public void SetChoosedAssets()
    {
        TotemManager.Instance.currentAvatar = userAvatars[avatarPreviewIndex];
        TotemManager.Instance.currentSpear = userSpears[spearPreviewIndex];
    }

    private void ChangeAvatar()
    {
        //To do: change to  userAvatars[avatarPreviewIndex].avatarPreview
        var newAvatarPreview = userAvatars[avatarPreviewIndex];
        string avatarData = $"Body fat: {newAvatarPreview.bodyFat}\n" +
            $"Body muscles: {newAvatarPreview.bodyMuscles}\n" +
            $"Eyes color: {newAvatarPreview.eyeColor}\n" +
            $"Hair color: {newAvatarPreview.hairColor}\n" +
            $"Hair style: {newAvatarPreview.hairStyle}\n" +
            $"Sex: {newAvatarPreview.sex}" +
            $"Skin color: {newAvatarPreview.skinColor}";
        avatarDataTMP.text = avatarData;
        CheckPreviewButtonsState(avatarPreviewIndex, userAvatars.Count, previousAvatarButton, nextAvatarButton);
    }

    private void ChangeSpear()
    {
        //To do: change to  userSpears[spearPreviewIndex].spearPreview
        var newSpearPreview = userSpears[spearPreviewIndex];
        string spearData = $"Damage: {newSpearPreview.damage}\n" +
            $"Element: {newSpearPreview.element}\n" +
            $"Range: {newSpearPreview.range}\n" +
            $"Shaft color: {newSpearPreview.shaftColor}\n" +
            $"Tip material: {newSpearPreview.tipMaterial}";
        spearDataTMP.text = spearData;
        CheckPreviewButtonsState(spearPreviewIndex, userSpears.Count, previousSpearButton, nextSpearButton);
    }

    private void CheckPreviewButtonsState(int index, int totalItems, Button previousButton, Button nextButton)
    {
        nextButton.interactable = index + 1 >= totalItems ? false : true;
        previousButton.interactable = index == 0 ? false : true;
    }

    private void InitializeAssets()
    {
        userSpears = TotemManager.Instance.currentUser.GetOwnedSpears();
        userAvatars = TotemManager.Instance.currentUser.GetOwnedAvatars();
        if (CheckAvaliableAssets(userSpears))
        {
            ChangeSpear();
        }
        if (CheckAvaliableAssets(userAvatars))
        {
            ChangeAvatar();
        }
    }

    private bool CheckAvaliableAssets<T>(List<T> asstetsList)
    {
        if (asstetsList == null || asstetsList.Count == 0)
        {
            if(playButton.interactable)
            {
                playButton.interactable = false;
            }
            return false;
        }
        return true;
    }
}
