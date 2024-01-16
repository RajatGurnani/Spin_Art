using UnityEngine;
using UnityEngine.UI;

public class ChangeBackgroundGradient : MonoBehaviour
{
    public Color32 color = Color.white;
    Image bgImage;

    private void Awake()
    {
        bgImage = GetComponent<Image>();
    }

    public void OnEnable()
    {
        bgImage.material.color = color;
    }
}
