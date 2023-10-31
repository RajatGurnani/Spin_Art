using UnityEngine;
using UnityEngine.UI;

public class PaintBrush : MonoBehaviour
{
    Camera cam;
    public LayerMask layerMask;
    public Texture2D texture2D;

    public MeshRenderer meshRenderer;

    public int textureSize = 512;
    public Color brushColor;
    public Vector2 coordinates;


    [Range(1, 100)]
    public int brushRadius = 3;
    public float[,] brushMatrix;
    public Slider brushSlider;

    private void Start()
    {
        cam = Camera.main;
        texture2D = new Texture2D(textureSize, textureSize);
        Color[] colors = new Color[texture2D.width * texture2D.height];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.white;
        }
        texture2D.filterMode = FilterMode.Bilinear;
        texture2D.anisoLevel = 1;
        texture2D.SetPixels(colors);

        brushSlider.SetValueWithoutNotify(brushRadius);
    }

    private void Update()
    {
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity, layerMask))
        {
            coordinates = hitInfo.textureCoord;
            if (Input.GetMouseButton(0))
            {
                Debug.Log(hitInfo.textureCoord);
                ChangeColor2(hitInfo.textureCoord);
            }
        }
    }

    public void ChangeColor(Vector2 textureCoord)
    {
        int width = (int)(textureCoord.x * texture2D.width);
        int height = (int)(textureCoord.y * texture2D.height);

        Vector2Int coordinate = Vector2Int.zero;
        for (int i = 0; i < brushRadius; i++)
        {
            for (int j = 0; j < brushRadius; j++)
            {
                texture2D.SetPixel(Mathf.Clamp(width + i, 0, texture2D.width), Mathf.Clamp(height + j, 0, texture2D.height), brushColor);
            }
        }
        texture2D.Apply();
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetTexture("_Main_Texture", texture2D);
        //meshRenderer.GetComponent<Renderer>().material.SetTexture("_MainTex", texture2D);
        meshRenderer.SetPropertyBlock(propertyBlock);
    }


    public void ChangeColor2(Vector2 textureCoord)
    {
        int width = (int)(textureCoord.x * texture2D.width);
        int height = (int)(textureCoord.y * texture2D.height);
        int length = 2 * brushRadius + 1;

        int offsetX = width - brushRadius;
        int offsetY = height - brushRadius;

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                Color currentColor = texture2D.GetPixel(i + offsetX, j + offsetY);
                Color newColor = Color.Lerp(brushColor, currentColor, brushMatrix[i, j]);
                texture2D.SetPixel(Mathf.Clamp(i + offsetX, 0, texture2D.width), Mathf.Clamp(j + offsetY, 0, texture2D.height), newColor);
            }
        }
        texture2D.Apply();
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetTexture("_Main_Texture", texture2D);
        meshRenderer.SetPropertyBlock(propertyBlock);
    }

    public void SetBrushColorR(float value)
    {
        brushColor.r = value;
    }
    public void SetBrushColorG(float value)
    {
        brushColor.g = value;
    }
    public void SetBrushColorB(float value)
    {
        brushColor.b = value;
    }

    public void GenerateBrush(float size)
    {
        brushRadius = (int)size;
        brushMatrix = new float[2 * brushRadius + 1, 2 * brushRadius + 1];
        int length = 2 * brushRadius + 1;

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                float distance = new Vector2(i - brushRadius, j - brushRadius).magnitude;
                brushMatrix[i, j] = distance / brushRadius;
                Debug.Log(brushMatrix[i, j]);
            }
        }
    }

    public void CircularMask()
    {

    }
}
