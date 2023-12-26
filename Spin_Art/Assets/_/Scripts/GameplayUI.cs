using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public GameObject auctionUIPanel;

    public void Start()
    {
        table = FindObjectOfType<Table>();
        paintBrush = FindObjectOfType<PaintBrush>();
        tableSpeedSlider.value = 0;
        SetTableSpeed(0);
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

    public void ToggleHardBrush(bool isOn)
    {
        FindObjectOfType<PaintBrush>().useHardBrush = isOn;
    }

    public void Sell()
    {
        TimeTracker timeTracker = FindObjectOfType<TimeTracker>();
        timeTracker.paintDone = true;
        auctionUIPanel.SetActive(true);
        AuctionSystem auctionSystem = FindObjectOfType<AuctionSystem>();
        auctionSystem.timeSpentOnBoard = timeTracker.timeSpentOnBoard;
        auctionSystem.timeSpentOffBoard = timeTracker.timeSpentOffBoard;
        timeTracker.timeSpentIdle = timeTracker.totalTime - timeTracker.timeSpentOffBoard - timeTracker.timeSpentOnBoard;
        auctionSystem.timeSpentIdle = timeTracker.timeSpentIdle;
        auctionSystem.Buy();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
