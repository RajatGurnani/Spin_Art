using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    public TMP_Text tableSpeedText;
    public Slider tableSpeedSlider;

    public TMP_Text brushSizeText;
    public Slider brushSizeSlider;

    public Table table;
    public PaintBrush paintBrush;

    public float brushSize;

    public GameManager gameManager;
    public void Start()
    {
        table = FindObjectOfType<Table>();
        paintBrush = FindObjectOfType<PaintBrush>();
        tableSpeedSlider.value = 0;
        SetTableSpeed(1);
    }

    public void SetTableSpeed(float speed)
    {
        table.rotateSpeed = speed;
        tableSpeedText.text = $"Table Speed: {speed}rot/s";
    }

    public void ChangeBrushSize(float size)
    {
        brushSize = size;
        brushSizeText.text = $"Brush Size: {size}px";
    }

    public void SetBrushSize()
    {
        paintBrush.GenerateBrush(brushSize);
    }

    public void ClearCanvas()
    {
        paintBrush.ResetCanvasTexture(Color.white);
    }
}
