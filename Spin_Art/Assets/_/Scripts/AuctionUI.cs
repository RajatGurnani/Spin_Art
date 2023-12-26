using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class AuctionUI : MonoBehaviour
{
    public TMP_Text amountText;
    float hammerDropDelay = 0.6f;
    public GameObject[] hammer;
    Sequence sequence;

    public void Awake()
    {
        FindObjectOfType<AuctionSystem>().HammerDrop += HammerDrop;
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
                sequence.Append(amountText.rectTransform.DOPunchScale(Vector3.one * 1.3f, 0.2f));
                sequence.AppendInterval(delayTime-0.2f);
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
                sequence.Append(amountText.rectTransform.DOPunchScale(Vector3.one * 1.3f, 0.2f));
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
                sequence.AppendCallback(()=> hammer[1].SetActive(true));
                sequence.AppendCallback(() => amountText.text = price.ToString());
                sequence.Append(amountText.rectTransform.DOPunchScale(Vector3.one * 1.3f, 0.2f));
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
                //sequence.AppendCallback(() => amountText.text = price.ToString());
                //sequence.Append(amountText.rectTransform.DOPunchScale(Vector3.one * 1.3f, 0.2f));
                sequence.Play();
                break;
        }
    }
}
