using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuctionUI : MonoBehaviour
{
    public TMP_Text amountText;
    float hammerDropDelay = 0.6f;
    public GameObject[] hammer;
    Sequence sequence;

    public AuctionSystem auctionSystem;
    public GameObject restartButton;

    public AudioSource audioSource;
    public void Awake()
    {
        auctionSystem.HammerDrop += HammerDrop;
        //restartButton.GetComponent<Button>().onClick.AddListener(Restart);
    }

    public void HammerDrop(int hammerLevel, int price, float delayTime)
    {
        if (sequence.IsActive())
        {
            sequence.Kill();
        }

        sequence = DOTween.Sequence();
        switch (hammerLevel)
        {
            case 0:
                sequence.AppendCallback(() => amountText.text = price.ToString());
                sequence.Append(amountText.rectTransform.DOPunchScale(Vector3.one * 1.1f, 0.2f));
                sequence.AppendInterval(delayTime - 0.2f);
                sequence.Play().OnComplete(() =>
                {
                    foreach (var item in hammer)
                    {
                        item.SetActive(false);
                    }
                });
                break;
            case 1:
                sequence.AppendInterval(hammerDropDelay);
                sequence.AppendCallback(() => hammer[0].SetActive(true));
                //sequence.AppendInterval(delayTime - hammerDropDelay);
                sequence.AppendCallback(() => amountText.text = price.ToString());
                sequence.Append(amountText.rectTransform.DOPunchScale(Vector3.one * 1.1f, 0.2f));
                sequence.Play().OnComplete(() =>
                {
                    foreach (var item in hammer)
                    {
                        item.SetActive(false);
                    }
                });
                break;
            case 2:
                sequence.AppendInterval(hammerDropDelay);
                sequence.AppendCallback(() => hammer[0].SetActive(true));
                sequence.AppendInterval(hammerDropDelay);
                sequence.AppendCallback(() => hammer[1].SetActive(true));
                sequence.AppendCallback(() => amountText.text = price.ToString());
                sequence.Append(amountText.rectTransform.DOPunchScale(Vector3.one * 1.1f, 0.2f));
                sequence.Play().OnComplete(() =>
                {
                    foreach (var item in hammer)
                    {
                        item.SetActive(false);
                    }
                });
                break;
            case 3:
                sequence.AppendInterval(hammerDropDelay);
                sequence.AppendCallback(() => hammer[0].SetActive(true));
                sequence.AppendInterval(hammerDropDelay);
                sequence.AppendCallback(() => hammer[1].SetActive(true));
                sequence.AppendInterval(hammerDropDelay);
                sequence.AppendCallback(() => hammer[2].SetActive(true));
                sequence.Play().OnComplete(() =>
                {
                    GameManager.Instance.playerData.AddCurrency(auctionSystem.sellingPrice);
                    SaveSystem.SavePlayerData(GameManager.Instance.playerData);
                    restartButton.SetActive(true);
                    if (audioSource != null)
                    {
                        audioSource.Play();
                    }
                });
                break;
        }
    }

    private void OnDestroy()
    {
        auctionSystem.HammerDrop -= HammerDrop;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
