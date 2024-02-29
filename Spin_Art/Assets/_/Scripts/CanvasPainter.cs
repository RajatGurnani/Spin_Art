using UnityEngine;
using UnityEngine.UI;

public class CanvasPainter : MonoBehaviour
{
    public RawImage table;

    public Texture2D texture2D;
    public int textureSize = 512;

    private void Start()
    {
        texture2D = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, 1, true);
        table = FindObjectOfType<RawImage>();
        //table.sprite = new Sprite();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 localPoint;
            Rect r = table.rectTransform.rect;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(table.rectTransform, Input.mousePosition, Camera.main, out localPoint);

            Debug.Log(localPoint);
            int px = Mathf.Clamp((int)(((localPoint.x - r.x) * texture2D.width) / r.width), 0, texture2D.width);
            int py = Mathf.Clamp((int)(((localPoint.y - r.y) * texture2D.height) / r.height), 0, texture2D.height);

            //Debug.Log($"{px}_{py}");
            Color32 col = texture2D.GetPixel(px, py);
        }
    }
}
