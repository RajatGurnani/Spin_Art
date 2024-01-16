using System.Collections;
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
    public RectTransform brushRT;

    public Table table;
    public PaintBrush paintBrush;

    public float brushSize;

    public GameManager gameManager;
    public GameObject auctionUIPanel;

    Coroutine delayBrushSize;

    public RectTransform minSizeRT, maxSizeRT;

    public float startSize = 0.2f;

    public GameObject tutorial;

    public int cleanAmount = 0;

    public void Start()
    {
        table = FindObjectOfType<Table>();
        paintBrush = FindObjectOfType<PaintBrush>();
        tableSpeedSlider.SetValueWithoutNotify(tableSpeedSlider.value = 2.564791f);
        SetTableSpeed(tableSpeedSlider.value);
        brushSizeSlider.value = (int)Mathf.Lerp(brushSizeSlider.minValue, brushSizeSlider.maxValue, startSize);
        tutorial.SetActive(true);
    }

    public void SetTableSpeed(float speed)
    {
        table.rotateSpeed = speed;

        if (tableSpeedText != null)
        {
            tableSpeedText.text = $"Table Speed: {speed}rot/s";
        }
    }

    public void ChangeBrushSize(float size)
    {
        brushSize = size;
        brushRT.sizeDelta = Vector2.Lerp(minSizeRT.sizeDelta, maxSizeRT.sizeDelta, brushSizeSlider.value / brushSizeSlider.maxValue);

        if (brushSizeText != null)
        {
            brushSizeText.text = $"Brush Size: {size}px";
        }
    }

    public void SetBrushSize(float size)
    {
        brushSize = size;
        brushRT.sizeDelta = Vector2.Lerp(minSizeRT.sizeDelta, maxSizeRT.sizeDelta, brushSizeSlider.value / brushSizeSlider.maxValue);
        if (delayBrushSize != null)
        {
            StopCoroutine(delayBrushSize);
            delayBrushSize = null;
        }
        delayBrushSize = StartCoroutine(SetDelayedBrushSize());
        //paintBrush.GenerateBrush(brushSize);
    }

    IEnumerator SetDelayedBrushSize()
    {
        yield return new WaitForSeconds(1f);
        paintBrush.GenerateBrush(brushSize);
        delayBrushSize = null;
    }

    public void ClearCanvas()
    {
        if (++cleanAmount % 3 == 0)
        {
            AdmobController.Instance.ShowInterstitial();
        }
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
        Camera.main.gameObject.SetActive(false);
        AuctionSystem auctionSystem = FindObjectOfType<AuctionSystem>();
        auctionSystem.timeSpentOnBoard = timeTracker.timeSpentOnBoard;
        auctionSystem.timeSpentOffBoard = timeTracker.timeSpentOffBoard;
        timeTracker.timeSpentIdle = timeTracker.totalTime - timeTracker.timeSpentOffBoard - timeTracker.timeSpentOnBoard;
        auctionSystem.timeSpentIdle = timeTracker.timeSpentIdle;
        auctionSystem.Buy();
        gameObject.SetActive(false);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RemoveTutorial()
    {
        tutorial.SetActive(false);
    }

    private void Update()
    {
        if (paintBrush.timeTracker.paintStarted && tutorial.activeSelf)
        {
            tutorial.SetActive(false);
        }
    }
}
