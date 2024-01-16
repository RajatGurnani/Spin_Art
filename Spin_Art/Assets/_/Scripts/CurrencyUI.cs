using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    TMP_Text currencyText;
    PlayerData playerData;

    private void Start()
    {
        playerData = GameManager.Instance.playerData;
        currencyText = GetComponentInChildren<TMP_Text>();
        UpdateCurrency();
    }

    public void UpdateCurrency()
    {
        if (currencyText != null)
        {
            currencyText.text = playerData.Currency.ToString();
        }
    }

    private void OnEnable()
    {
        Signal.CurrencyChange += UpdateCurrency;
    }

    private void OnDisable()
    {
        Signal.CurrencyChange -= UpdateCurrency;
    }
}
