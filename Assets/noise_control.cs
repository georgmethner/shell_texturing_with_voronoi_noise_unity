using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class noise_control : MonoBehaviour
{
    public ComputeShader shader;
    public Vector2Int resolution;
    public float size;

    RenderTexture result;
    
    static readonly int result_id = Shader.PropertyToID("result");
    
    public RenderTexture generate_texture()
    {
        create_texture();
        shader.SetFloat("size", size);
        shader.SetTexture(0, result_id, result);

        shader.Dispatch(0, Mathf.CeilToInt(result.width / 8.0f), Mathf.CeilToInt(result.height / 8.0f), 1);

        return result;
        
    }
    
    void create_texture()
    {
        if (result != null)
            return;

        result = new RenderTexture(resolution.x, resolution.y, 0, RenderTextureFormat.RG16);
        result.wrapMode = TextureWrapMode.Repeat; 
        result.enableRandomWrite = true;
    }

}
