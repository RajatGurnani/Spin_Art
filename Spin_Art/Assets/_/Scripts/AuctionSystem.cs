using System.Collections;
using UnityEngine;

public class AuctionSystem : MonoBehaviour
{
    public int startPrice = 0;
    public int sellingPrice = 0;
    public int basePrice = 10;
    public int currentPrice = 0;

    public byte hammerCount = 0;
    public int zeroHammerThreshold;
    public int firstHammerThreshold;
    public int secondHammerThreshold;

    public System.Action<int, int, float> HammerDrop;

    public float onBoardMultiplier = 1.5f;
    public float offBoardMultiplier = 0.5f;
    public float idleMultiplier = 0.25f;

    public float timeSpentOnBoard = 0;
    public float timeSpentOffBoard = 0;
    public float timeSpentIdle = 0;
    public float auctionTime = 0f;

    public void FinalPriceCalculation()
    {
        sellingPrice = basePrice + (int)(timeSpentOnBoard * onBoardMultiplier + timeSpentOffBoard * offBoardMultiplier + timeSpentIdle * idleMultiplier);
        zeroHammerThreshold = Mathf.Max((int)(0.1f * sellingPrice), 5);
        firstHammerThreshold = (int)(0.25f * sellingPrice);
        secondHammerThreshold = (int)(0.5f * sellingPrice);
        startPrice = (int)Mathf.Clamp(sellingPrice * 0.05f, zeroHammerThreshold * 0.5f, zeroHammerThreshold * 2);
    }

    public void Buy()
    {
        Debug.ClearDeveloperConsole();
        FinalPriceCalculation();
        StartCoroutine(StartAuction());
    }

    IEnumerator StartAuction()
    {
        auctionTime = Time.timeSinceLevelLoad;
        currentPrice = startPrice;

        while (currentPrice <= sellingPrice)
        {
            //if (currentPrice < zeroHammerThreshold)
            //{
            //    // Bid fast
            //    yield return StartCoroutine(Bid(0));
            //    continue;
            //}
            //else if (currentPrice < firstHammerThreshold)
            //{
            //    // bid slow
            //    yield return StartCoroutine(Bid(1));
            //    continue;
            //}
            //else if (currentPrice < secondHammerThreshold)
            //{
            //    // bid u
            //    yield return StartCoroutine(Bid(2));
            //    continue;
            //}
            //else
            //{
            //    // sold
            //    break;
            //}

            if (currentPrice == sellingPrice)
            {

                HammerDrop?.Invoke(3, currentPrice, 2f);
                break;
            }
            else if (currentPrice > secondHammerThreshold)
            {
                yield return StartCoroutine(Bid(2));
                continue;
            }
            else if (currentPrice > firstHammerThreshold)
            {
                yield return StartCoroutine(Bid(1));
                continue;
            }
            else
            {
                yield return StartCoroutine(Bid(0));
                continue;
            }
        }
        auctionTime = Time.timeSinceLevelLoad - auctionTime;
        Debug.Log(auctionTime);
    }

    public IEnumerator Bid(int level)
    {
        //currentPrice += (int)Random.Range(Mathf.Clamp(Mathf.Pow(startPrice, level - 2), 1, startPrice * 2), Mathf.Clamp(Mathf.Pow(startPrice, level + 1), 1, startPrice * 5));

        int startValue = (int)Mathf.Clamp(Mathf.Pow(startPrice, level - 2), 1, zeroHammerThreshold);
        int endValue = (int)Mathf.Clamp(Mathf.Pow(startPrice, level + 1), 1, zeroHammerThreshold * 2);

        currentPrice += (int)Random.Range(startValue, endValue);
        //currentPrice += (int)Random.Range(Mathf.Clamp(Mathf.Pow(startPrice, level - 2), 1, startPrice * 2), Mathf.Pow(startPrice, level + 2));
        currentPrice = Mathf.Clamp(currentPrice, 0, sellingPrice);
        float delayTime = 0.3f + Random.Range(Mathf.Pow(2, level - 2), Mathf.Pow(2, level) * 0.5f);
        Debug.Log($"Auction Level-{level} currentPrice-{currentPrice} + delayTime- {delayTime:F1}");
        //yield return null;
        HammerDrop?.Invoke(level,currentPrice,delayTime);
        yield return new WaitForSeconds(delayTime);
    }
}
