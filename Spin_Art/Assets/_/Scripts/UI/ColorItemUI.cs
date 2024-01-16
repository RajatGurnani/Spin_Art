using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorItemUI : MonoBehaviour
{
    public Image noMoneyImage;
    public Image boughtImage;
    public Image colorImage;
    public TMP_Text costText;
    public Button buyButton;
    public ShopItemColor shopItemColor;
    PlayerData playerData;

    private void Start()
    {
        playerData = GameManager.Instance.playerData;
        buyButton.onClick.AddListener(Buy);
    }

    public void SetButton(ShopItemColor shopItemColor)
    {
        this.shopItemColor = shopItemColor;
        colorImage.color = shopItemColor.color;
        costText.text = shopItemColor.cost.ToString();
        playerData ??= GameManager.Instance.playerData;

        //if (playerData.HasColor(shopItemColor.color))
        //{
        //    buyButton.gameObject.SetActive(false);
        //    boughtImage.gameObject.SetActive(true);
        //}
    }

    private void Update()
    {
        if (playerData.HasColor(shopItemColor.color))
        {
            buyButton.interactable = false;
            noMoneyImage.gameObject.SetActive(false);
            boughtImage.gameObject.SetActive(true);
        }
        else
        {
            boughtImage.gameObject.SetActive(false);
            if (GameManager.Instance.playerData.Currency <= shopItemColor.cost)
            {
                noMoneyImage.gameObject.SetActive(true);
                buyButton.interactable = false;
            }
            else
            {
                noMoneyImage.gameObject.SetActive(false);
                buyButton.interactable = true;
            }
        }
    }

    public void Buy()
    {
        if (GameManager.Instance.playerData.Currency >= shopItemColor.cost)
        {
            GameManager.Instance.playerData.Currency -= shopItemColor.cost;
            GameManager.Instance.playerData.AddColor(shopItemColor.color);
            SaveSystem.SavePlayerData(GameManager.Instance.playerData);

            buyButton.interactable = false;
            boughtImage.gameObject.SetActive(true);
        }
    }
}
