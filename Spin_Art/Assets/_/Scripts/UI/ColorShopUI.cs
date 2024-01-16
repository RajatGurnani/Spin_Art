using UnityEngine;

public class ColorShopUI : MonoBehaviour
{
    public ColorItemUI colorItemPrefab;
    public ShopItemColor[] colors;
    public RectTransform colorHolder;

    public void Start()
    {
        //colorHolder.GetComponent<GridLayout>().cellSize = colorItemPrefab.GetComponent<RectTransform>().sizeDelta;
        foreach (var item in colors)
        {
            var colorItem = Instantiate(colorItemPrefab, colorHolder);
            colorItem.SetButton(item);
        }
    }
}
