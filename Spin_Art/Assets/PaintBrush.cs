using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    Camera cam;
    public LayerMask layerMask;
    public Texture2D texture2D;

    public MeshRenderer meshRenderer;

    public int textureSize = 512;
    public Color color;
    public Vector2 coordinates;


    [Range(1, 100)]
    public int brushRadius = 3;

    private void Start()
    {
        cam = Camera.main;
        texture2D = new Texture2D(textureSize, textureSize);
        Color[] colors = new Color[texture2D.width * texture2D.height];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.white;
        }
        texture2D.SetPixels(colors);
        //texture2D.SetPixels(((Texture2D)(meshRenderer.material.GetTexture("_MainTex"))).GetPixels());
        //texture2D.Apply();
        // texture2D.SetPixels((meshRenderer.material.GetTexture("MainTex") as Texture2D).GetPixels());
    }

    private void Update()
    {
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity, layerMask))
        {
            coordinates = hitInfo.textureCoord;
            if (Input.GetMouseButton(0))
            {
                Debug.Log(hitInfo.textureCoord);
                ChangeColor(hitInfo.textureCoord);
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
                texture2D.SetPixel(Mathf.Clamp(width + i, 0, texture2D.width), Mathf.Clamp(height + j, 0, texture2D.height), color);
            }
        }
        //texture2D.SetPixel(width, height, color);
        texture2D.Apply();
        meshRenderer.material.SetTexture("_MainTex", texture2D);
    }

    public void SetBrushColorR(float value)
    {
        color.r = value;
    }
    public void SetBrushColorG(float value)
    {
        color.g = value;
    }
    public void SetBrushColorB(float value)
    {
        color.b = value;
    }
}
