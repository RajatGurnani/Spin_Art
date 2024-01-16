using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DisplayImage : MonoBehaviour
{
    public Table table;
    public PaintBrush brush;
    public RawImage displayImage;

    public Button restartButton;

    private void Start()
    {
        table = FindObjectOfType<Table>(true);
        brush = FindObjectOfType<PaintBrush>(true);
        displayImage.texture = brush.texture2D;
        //Sprite sprite = Sprite.Create(brush.texture2D, displayImage.rectTransform.rect, Vector2.one * 0.5f);
        //displayImage.sprite = sprite;
    }

    private void Update()
    {
        displayImage.rectTransform.Rotate(Vector3.forward, 360 * table.rotateSpeed * Time.deltaTime, Space.Self);
    }

    public void Restart()
    {
        AdmobController.Instance.ShowInterstitial();
        StartCoroutine(DelayedRestart());
    }

    IEnumerator DelayedRestart(float delay = 0.1f)
    {
        //yield return new WaitForSeconds(delay);
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
