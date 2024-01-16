using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    public Color color;
    PaintBrush paintBrush;
    Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.group = GetComponentInParent<ToggleGroup>();
        SetButton(color);
        paintBrush = FindObjectOfType<PaintBrush>();
        toggle.onValueChanged.AddListener(ToggleColor);
    }

    public void SetButton(Color color)
    {
        this.color = color;
        GetComponentInChildren<Image>().color = color;
    }

    public void ToggleColor(bool value)
    {
        if (paintBrush!=null)
        {
            if (value)
            {
                paintBrush.brushColor = color;
            }
            return;
            if (value)
            {
                if (!paintBrush.colors.Contains(color))
                {
                    paintBrush.colors.Add(color);
                }
            }
            else
            {
                if (paintBrush.colors.Contains(color))
                {
                    paintBrush.colors.Remove(color);
                }
            }
        }
    }
}
