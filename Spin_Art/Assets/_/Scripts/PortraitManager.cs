using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitManager : MonoBehaviour
{
    public Sprite[] images;

    public Image fillupImage;

    public float filluptime = 10f;
    public int fillupSessions = 10;

    PlayerData playerData;

    public Button nextButton;

    private void Awake()
    {
        nextButton.gameObject.SetActive(false);
        playerData = GameManager.Instance.playerData;
    }
    private void Start()
    {
        Fillup();
    }

    public void Fillup()
    {
        int index = (playerData.sessionsCompleted / fillupSessions) % images.Length;
        fillupImage.sprite = images[index];
        Debug.Log(playerData.sessionsCompleted % fillupSessions / (float)fillupSessions);
        fillupImage.fillAmount = playerData.sessionsCompleted % fillupSessions / (float)fillupSessions;
        playerData.sessionsCompleted++;
        SaveSystem.SavePlayerData(playerData);

        if (playerData.sessionsCompleted % fillupSessions == 0)
        {
            fillupImage.DOFillAmount(1f, filluptime).OnComplete(() =>
            {
                nextButton.gameObject.SetActive(true);
            });
        }
        else
        {
            fillupImage.DOFillAmount(playerData.sessionsCompleted % fillupSessions / (float)fillupSessions, filluptime).OnComplete(() =>
            {
                nextButton.gameObject.SetActive(true);
            });
        }
    }
}
