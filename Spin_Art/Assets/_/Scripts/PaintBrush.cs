using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    Camera cam;
    public LayerMask layerMask;
    public Texture2D texture2D;

    public MeshRenderer meshRenderer;

    public int textureSize = 512;
    public Color brushColor;
    public List<Color> colors;
    public bool randomColor = false;
    public Vector2 coordinates;


    [Range(1, 100)]
    public int brushRadius = 3;
    public float[,] brushMatrix;

    public ComputeShader brushGeneratorComputeShader;
    public RenderTexture renderTexture;
    public Material material;
    public AnimationCurve hardnessCurve;
    public bool useHardBrush = true;

    public TimeTracker timeTracker;

    public Texture2D sampleTexture;

    public Color[] testColors;

    private void Start()
    {
        timeTracker = FindObjectOfType<TimeTracker>();
        cam = Camera.main;

        ResetCanvasTexture(Color.white);
    }

    private void Update()
    {
        Debug.Log(texture2D.mipmapCount);
        for (int i = 0; i< 100; i++)
        {
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity, layerMask))
            {
                coordinates = hitInfo.textureCoord;
                if (Input.GetMouseButton(0))
                {
                    timeTracker.paintStarted = true;
                    timeTracker.IncrementOnTime();
                    //Debug.Log(hitInfo.textureCoord);
                    if (randomColor)
                    {
                        brushColor = colors[Random.Range(0, colors.Count)];
                        ChangeColor2(hitInfo.textureCoord);//, colors[Random.Range(0, colors.Length)]);
                    }
                    else
                    {
                        ChangeColor2(hitInfo.textureCoord);
                    }
                }
            }
        }
    }

    /*
     * 
     * 
     * Try render texture dumbass
     * https://forum.unity.com/threads/texture-painting-performance.410701/1
     * 
     * 
     */
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
        meshRenderer.SetPropertyBlock(propertyBlock);
    }

    public Vector2Int size = new Vector2Int();
    public void ChangeColor2(Vector2 textureCoord)
    {
        Vector2Int midPoint = new Vector2Int((int)(texture2D.width * textureCoord.x), (int)(texture2D.height * textureCoord.y));


        int width = (int)(textureCoord.x * texture2D.width);
        int height = (int)(textureCoord.y * texture2D.height);
        int length = 2 * brushRadius + 1;

        int offsetX = width - brushRadius;
        int offsetY = height - brushRadius;

        Vector2Int startPoint = new Vector2Int(Mathf.Clamp(midPoint.x - brushRadius, 0, texture2D.width-1), Mathf.Clamp(midPoint.y - brushRadius, 0, texture2D.height-1));
        Vector2Int endPoint = new Vector2Int(Mathf.Clamp(midPoint.x + brushRadius, 0, texture2D.width-1), Mathf.Clamp(midPoint.y + brushRadius, 0, texture2D.height - 1));
        //Debug.Log($"{nameof(midPoint)} {midPoint}\n" +
        //          $"{nameof(startPoint)} {startPoint}\n" +
        //          $"{nameof(endPoint)} {endPoint}\n" +
        //          $"Result {endPoint - startPoint}");

        size = new(endPoint.x - startPoint.x + 1, endPoint.y - startPoint.y + 1);
        //sampleTexture = new Texture2D(endPoint.x-startPoint.x +1, endPoint.y - startPoint.y +1);

        //testColors = new Color[sampleTexture.width * sampleTexture.height];
        //Debug.Log(texture2D.GetPixels(startPoint.x, startPoint.y, size.x, size.y).Length);
        //testColors = texture2D.GetPixels(startPoint.x, startPoint.y, size.x, size.y);
        //sampleTexture.SetPixels(texture2D.GetPixels(offsetX,offsetY,length,length));
        //sampleTexture.Apply();

        //for (int i = 0; i < length; i++)
        //{
        //    for (int j = 0; j < length; j++)
        //    {
        //        Color newColor;
        //        Color currentColor = texture2D.GetPixel(i + offsetX, j + offsetY);
        //        float value = brushMatrix[i, j];
        //        newColor = Color.Lerp(brushColor, currentColor, useHardBrush ? Mathf.Floor(value) : value);
        //        texture2D.SetPixel(Mathf.Clamp(i + offsetX, 0, texture2D.width), Mathf.Clamp(j + offsetY, 0, texture2D.height), newColor);
        //    }
        //}
        ;
        texture2D.SetPixels32(startPoint.x,startPoint.y,size.x,size.y, Enumerable.Repeat(new Color32(0, 0, 0, 1), size.x * size.y).ToArray());
        texture2D.Apply(false);
        return;
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetTexture("_Main_Texture", texture2D);
        meshRenderer.SetPropertyBlock(propertyBlock);

        material.SetFloat("_Rotation", 1f);
        Graphics.Blit(texture2D, renderTexture, material);
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
                float ratio = Mathf.Clamp01(distance / brushRadius);
                brushMatrix[i, j] = ratio;
            }
        }
    }

    public void ResetCanvasTexture(Color color)
    {
        texture2D = new Texture2D(textureSize, textureSize,TextureFormat.RGBA32,1,true);
        Color[] colors = new Color[texture2D.width * texture2D.height];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = color;
        }
        texture2D.filterMode = FilterMode.Bilinear;
        texture2D.anisoLevel = 1;
        texture2D.SetPixels(colors);
        texture2D.Apply(updateMipmaps:false);

        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetTexture("_Main_Texture", texture2D);
        meshRenderer.SetPropertyBlock(propertyBlock);
    }

    public void CircularMask()
    {

    }
}
