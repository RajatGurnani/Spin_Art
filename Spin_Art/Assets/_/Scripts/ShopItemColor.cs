using UnityEngine;

[CreateAssetMenu(menuName = "Shop Item/Color", fileName = "New Color")]
public class ShopItemColor : ScriptableObject
{
    public Color32 color = Color.white;
    public int cost = 100;
}
