using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSpawner : MonoBehaviour
{
    public ColorButton colorButtonPrefab;
    ToggleGroup toggleGroup;

    PlayerData playerData;

    private void Awake()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        playerData = GameManager.Instance.playerData;
    }

    public void Start()
    {
        foreach (Color32 color in playerData.colors)
        {
            ColorButton colorButton = Instantiate(colorButtonPrefab, transform);
            colorButton.SetButton(color);
        }
    }
}
