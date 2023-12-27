using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class NewPaintBrush : MonoBehaviour
{
    public RenderTexture renderTexture;
    public Material material;
    public Vector2 coordinates;

    public Camera cam;
    public LayerMask layerMask;

    public Texture2D texture2D;
    private void Awake()
    {
        texture2D = new Texture2D(512, 512, TextureFormat.RGBA32, 1, true);

        texture2D.SetPixels(Enumerable.Repeat(Color.white,512*512).ToArray());
        texture2D.Apply();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(5);
        cam = Camera.main;
        Debug.Log("asdfas");
        Graphics.Blit(texture2D, renderTexture, material);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity, layerMask))
            {
                coordinates = hitInfo.textureCoord;
            }
        }
    }

    public void WriteToRenderTexture()
    {

    }
}
