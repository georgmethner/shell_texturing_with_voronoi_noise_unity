using System;
using UnityEditor.UIElements;
using UnityEngine;

public class grass_control : MonoBehaviour
{
    public Shader shader;

    [Header("LAYER ATTRIBUTES")]
    public Mesh layer_mesh;
    public Material grass_material;

    public int layer_count;

    public float grass_length;
    
    public AnimationCurve thickness_curve;
    public float debug_x;


    void OnEnable()
    {
        RenderTexture temp_tex = GetComponent<noise_control>().generate_texture();
        grass_material.SetTexture("noise_tex", temp_tex);
        grass_material.SetVector("grass_info", new Vector4(grass_length, layer_count, transform.localScale.x, 0));

        //Set thickness from curve
        var thickness_values = new float[layer_count];
        for (int i = 0; i < layer_count; i++)
        {
            thickness_values[i] = thickness_curve.Evaluate(i / (float) layer_count);
            print(thickness_values[i]);
        }
        grass_material.SetFloatArray("thickness_values", thickness_values);
        
        // create all children
        for (int i = 0; i < layer_count; i++)
        {
            GameObject child = new GameObject("grass" + (i + 1));

            child.transform.position = this.transform.position;
            child.transform.SetParent(this.transform);
            child.transform.localScale = Vector3.one;

            child.AddComponent<MeshFilter>().mesh = layer_mesh;
            var mesh_r = child.AddComponent<MeshRenderer>();

            mesh_r.material = grass_material;
            mesh_r.material.SetFloat("layer_index", i+1);
        }
    }

    void OnDisable()
    {
        // del all children
        if (transform.childCount != 0)
            foreach (Transform child_trans in transform)
                Destroy(child_trans.gameObject);
    }

}
