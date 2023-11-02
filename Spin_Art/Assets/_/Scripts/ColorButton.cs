using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    public Color color;
    PaintBrush paintBrush;
    Toggle toggle;

    private void Start()
    {
        GetComponentInChildren<Image>().color = color;
        paintBrush = FindObjectOfType<PaintBrush>();
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(ToggleColor);

        ToggleColor(toggle.isOn);
    }

    public void ToggleColor(bool value)
    {
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
