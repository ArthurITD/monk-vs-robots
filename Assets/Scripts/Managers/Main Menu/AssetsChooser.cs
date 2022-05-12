using System.Collections;
using System.Collections.Generic;
using TMPro;
using TotemEntities;
using UnityEngine;
using UnityEngine.UI;

public class AssetsChooser : MonoBehaviour
{
    [SerializeField] private Image avatarPreview;
    [SerializeField] private Image spearPreview;
    [SerializeField] private Button nextAvatarButton;
    [SerializeField] private Button previousAvatarButton;
    [SerializeField] private Button nextSpearButton;
    [SerializeField] private Button previousSpearButton;
    [SerializeField] private Button playButton;
    [SerializeField] private TMP_Text avatarDataTMP;
    [SerializeField] private TMP_Text spearDataTMP;
    [SerializeField] private TMP_Text currentSpearIndexTMP;
    [SerializeField] private TMP_Text maxSpearsCountTMP;
    [SerializeField] private TMP_Text currentAvatarIndexTMP;
    [SerializeField] private TMP_Text maxAvatarsCountTMP;

    private int spearPreviewIndex = 0;
    private int avatarPreviewIndex = 0;

    private List<TotemSpear> userSpears = new List<TotemSpear>();
    private List<TotemAvatar> userAvatars = new List<TotemAvatar>();

    public void OnEnable()
    {
        playButton.interactable = true;
        InitializeAssets();
        VerifyAssets();
    }

    public void OnDisable()
    {
        avatarDataTMP.text = string.Empty;
        spearDataTMP.text = string.Empty;
        avatarPreviewIndex = 0;
        spearPreviewIndex = 0;
    }

    public void OnAvatarPreviewChange(int indexStep)
    {
        avatarPreviewIndex += indexStep;
        avatarPreviewIndex = CheckPreviewIndex(avatarPreviewIndex, userAvatars.Count);
        currentAvatarIndexTMP.text = (avatarPreviewIndex+1).ToString();
        ChangeAvatar();
    }

    public void OnSpearPreviewChange(int indexStep)
    {
        spearPreviewIndex += indexStep;
        spearPreviewIndex = CheckPreviewIndex(spearPreviewIndex, userSpears.Count);
        currentSpearIndexTMP.text = (spearPreviewIndex + 1).ToString();
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

    private int CheckPreviewIndex(int currentValue, int maxValue)
    {
        if (currentValue >= maxValue)
        {
            return 0;
        }
        else if (currentValue < 0)
        {
            return maxValue - 1;
        }

        return currentValue;
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
    }

    private void InitializeAssets()
    {
        userSpears = TotemManager.Instance.currentUser.GetOwnedSpears();
        userAvatars = TotemManager.Instance.currentUser.GetOwnedAvatars();
        maxSpearsCountTMP.text = userSpears.Count.ToString();
        maxAvatarsCountTMP.text = userAvatars.Count.ToString();
    }

    private bool CheckAvaliableAssets<T>(List<T> asstetsList, Button nextButton, Button previousButton)
    {
        if (asstetsList == null || asstetsList.Count == 0)
        {
            if(playButton.interactable)
            {
                playButton.interactable = false;
            }
            nextButton.interactable = false;
            previousButton.interactable = false;
            return false;
        }
        return true;
    }

    private void VerifyAssets()
    {
        if (CheckAvaliableAssets(userSpears, nextSpearButton, previousSpearButton))
        {
            ChangeSpear();
            currentSpearIndexTMP.text = "1";
        }
        else
        {
            currentSpearIndexTMP.text = "0";
        }

        if (CheckAvaliableAssets(userAvatars, nextAvatarButton, previousAvatarButton))
        {
            ChangeAvatar();
            currentAvatarIndexTMP.text = "1";
        }
        else
        {
            currentAvatarIndexTMP.text = "0";
        }
    }
}
