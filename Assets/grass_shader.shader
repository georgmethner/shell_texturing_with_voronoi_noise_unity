Shader "Custom/grass_shader"
{
    Properties
    {
        _grass_color ("Main Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            "LightMode" = "ForwardBase"
        }

        Pass
        {
            Cull Off


            CGPROGRAM
            #pragma fragment frag alpha
            #pragma vertex vert

            #include "UnityPBSLighting.cginc"
            #include "AutoLight.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            Texture2D<float2> noise_tex;
            fixed4 _grass_color;
            float layer_index;
            float thickness_values[256];
            float4 grass_info;   // grass_info.x = grass_length, grass_info.y = layer_count, grass_info.z = field_size

            v2f vert(appdata v)
            {
                v2f o;
                v.vertex.y += sqrt(layer_index) * grass_info.x;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_TARGET
            {
                float2 uv = i.uv;

                float dif = layer_index / grass_info.y;
                float2 n = noise_tex.Load(int3(frac(uv * 5.0) * 2048.0, 0));
                n.x = -n.x + 1.0;
                
                if (dif >= n.y || n.y < -n.x+1.0 || thickness_values[layer_index / n.y] > n.x)
                    discard;

                float4 c = float4(_grass_color.rgb * dif, 1);
                //float4 c = float4(n.yy, 1, 1);

                return c;
            }
            ENDCG
        }
    }

}