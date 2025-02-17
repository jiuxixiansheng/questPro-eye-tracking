using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class drawTrail : MonoBehaviour
{
    private Texture tex;//这张图是板子的原图
    public RenderTexture cacheTex;//缓存上一帧的图
    RenderTexture currentTex;//当前帧操作的图


    public float brushSize = 0.01f;
    public Color brushCol = Color.red;

    private Material effectMat;//用来处理图像的材质

    private Material renderMat;//原始面板的材质

    private Vector2 lastuv;

    private bool isDown;
    // Start is called before the first frame update

    public Laser laser;
    public RawImage board;//视频所在平面
    void Start()
    {
        Initialized();
    }

    // Update is called once per frame
    void Update()
    {
        if (laser.isOnBoard)
        {

            if (!isDown)
            {
                isDown = true;
                lastuv = laser.getRaycast().textureCoord;
            }
            RenderBrushToBoard(laser.getRaycast());
            lastuv = laser.getRaycast().textureCoord;
        }
        else
        {
            isDown = false;
        }

    }

    private void RenderBrushToBoard(RaycastHit hit)
    {
        Debug.Log("1");
        Vector2 dir = hit.textureCoord - lastuv;

        if (Vector3.SqrMagnitude(dir) > brushSize * brushSize)
        {
            int length = Mathf.CeilToInt(dir.magnitude / brushSize);

            for (int i = 0; i < length; i++)
            {
                RenderToMatTex(lastuv + dir.normalized * i * brushSize);

            }
        }
        else
        {
            RenderToMatTex(hit.textureCoord);
        }
        Debug.Log("2");
    }

    /*    private void RenderBrushToBoard(RaycastHit hit)
        {
            RenderToMatTex(hit.textureCoord2);
        }*/

    private void RenderToMatTex(Vector2 uv)
    {
        effectMat.SetVector("_BrushPos", new Vector4(uv.x, uv.y, lastuv.x, lastuv.y));
        effectMat.SetColor("_BrushColor", brushCol);
        effectMat.SetFloat("_BrushSize", brushSize);
        Graphics.Blit(cacheTex, currentTex, effectMat);
        //renderMat.SetTexture("_MainTex", currentTex);
        renderMat.SetTexture("_BaseMap", currentTex);
        Graphics.Blit(currentTex, cacheTex);
    }

    private void Initialized()
    {
        effectMat = new Material(Shader.Find("Brush/MarkPenEffect"));
        Material boardMat = board.material;

        tex = boardMat.mainTexture;

        renderMat = boardMat;

        cacheTex = new RenderTexture(tex.width, tex.height, 0, RenderTextureFormat.ARGB32);
        Graphics.Blit(tex, cacheTex);
        //renderMat.SetTexture("_MainTex", cacheTex);
        renderMat.SetTexture("_BaseMap", cacheTex);
        currentTex = new RenderTexture(tex.width, tex.height, 0, RenderTextureFormat.ARGB32);

    }
}
