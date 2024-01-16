using RDG;
using System.Collections.Generic;
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

    public AudioSource audioSource;

    private void Start()
    {
        timeTracker = FindObjectOfType<TimeTracker>();
        cam = Camera.main;

        ResetCanvasTexture(Color.white);
    }

    public Vector2 prevMousePosition;
    public Vector2 currentMousePosition;

    private void Update()
    {
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity, layerMask))
        {
            if (Input.GetMouseButton(0))
            {
                timeTracker.paintStarted = true;
                timeTracker.IncrementOnTime();

                Vibration.Vibrate(Mathf.Clamp((int)(Time.deltaTime * 1000), 10, 100), 30 , false);
                ChangeColor2(hitInfo.textureCoord);

                if (audioSource != null)
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                    audioSource.UnPause();
                }
            }
            else
            {
                if (audioSource != null)
                {
                    audioSource.Pause();
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

        for (int i = 0; i < brushRadius; i++)
        {
            for (int j = 0; j < brushRadius; j++)
            {
                texture2D.SetPixel(Mathf.Clamp(width + i, 0, texture2D.width), Mathf.Clamp(height + j, 0, texture2D.height), brushColor);
            }
        }
        texture2D.Apply();
        MaterialPropertyBlock propertyBlock = new();
        meshRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetTexture("_Main_Texture", texture2D);
        meshRenderer.SetPropertyBlock(propertyBlock);
    }

    public Vector2Int size = new();
    public void ChangeColor2(Vector2 textureCoord)
    {
        Vector2Int midPoint = new((int)(texture2D.width * textureCoord.x), (int)(texture2D.height * textureCoord.y));

        int width = (int)(textureCoord.x * texture2D.width);
        int height = (int)(textureCoord.y * texture2D.height);
        int length = 2 * brushRadius + 1;

        int offsetX = width - brushRadius;
        int offsetY = height - brushRadius;

        Vector2Int startPoint = new(Mathf.Clamp(midPoint.x - brushRadius, 0, texture2D.width - 1), Mathf.Clamp(midPoint.y - brushRadius, 0, texture2D.height - 1));
        Vector2Int endPoint = new(Mathf.Clamp(midPoint.x + brushRadius, 0, texture2D.width - 1), Mathf.Clamp(midPoint.y + brushRadius, 0, texture2D.height - 1));
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

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                Color newColor;
                Color currentColor = texture2D.GetPixel(i + offsetX, j + offsetY);
                float value = brushMatrix[i, j];
                newColor = Color.Lerp(brushColor, currentColor, useHardBrush ? Mathf.Floor(value) : value);
                texture2D.SetPixel(Mathf.Clamp(i + offsetX, 0, texture2D.width), Mathf.Clamp(j + offsetY, 0, texture2D.height), newColor);
            }
        }
        ;
        //texture2D.SetPixels32(startPoint.x, startPoint.y, size.x, size.y, Enumerable.Repeat(new Color32(0, 0, 0, 1), size.x * size.y).ToArray());
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
        Debug.Log("generating brush");
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
        texture2D = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, 1, true);
        Color[] colors = new Color[texture2D.width * texture2D.height];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = color;
        }
        texture2D.filterMode = FilterMode.Bilinear;
        texture2D.anisoLevel = 1;
        texture2D.SetPixels(colors);
        texture2D.Apply(updateMipmaps: false);

        MaterialPropertyBlock propertyBlock = new();
        meshRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetTexture("_Main_Texture", texture2D);
        meshRenderer.SetPropertyBlock(propertyBlock);
    }
}
