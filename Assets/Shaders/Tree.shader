// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Tree"
{
    Properties
    {
        _Color0 ("Color X", Color) = (1,1,1,1)
        _Color1 ("Color Y", Color) = (1,1,1,1)
        _Color2 ("Color Z", Color) = (1,1,1,1)
        
        _BumpMap("Normal Map", 2D) = "bump" {}
        _DetailMap("Detail", 2D) = "white" {}
        _OcclusionMap("Occlusion", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 100
        
        // ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f
            {
                half3 tspace0 : TEXCOORD0;
                half3 tspace1 : TEXCOORD1;
                half3 tspace2 : TEXCOORD2;
                float2 uv : TEXCOORD3;
                float4 wPos : TEXCOORD4;
                float4 pos : SV_POSITION;
            };

            v2f vert (float4 vertex : POSITION, float3 normal : NORMAL, float4 tangent : TANGENT, float2 uv : TEXCOORD3)
            {
                v2f o;
                o.wPos = mul(unity_ObjectToWorld, vertex);
                o.pos = UnityObjectToClipPos(vertex);
                half3 wNormal = UnityObjectToWorldNormal(normal);
                half3 wTangent = UnityObjectToWorldDir(tangent.xyz);
                half tangentSign = tangent.w * unity_WorldTransformParams.w;
                half3 wBitangent = cross(wNormal, wTangent) * tangentSign;
                o.tspace0 = half3(wTangent.x, wBitangent.x, wNormal.x);
                o.tspace1 = half3(wTangent.y, wBitangent.y, wNormal.y);
                o.tspace2 = half3(wTangent.z, wBitangent.z, wNormal.z);
                o.uv = uv;
                return o;
            }

            fixed4 _Color0;
            fixed4 _Color1;
            fixed4 _Color2;
            sampler2D _BumpMap;
            sampler2D _DetailMap;
            sampler2D _OcclusionMap;
            
            fixed4 frag (v2f i) : SV_Target
            {
                half3 tnormal = UnpackNormal(tex2D(_BumpMap, i.uv));
                half3 worldNormal;
                worldNormal.x = dot(i.tspace0, tnormal);
                worldNormal.y = dot(i.tspace1, tnormal);
                worldNormal.z = dot(i.tspace2, tnormal);
                half3 blend = abs(worldNormal);
                blend /= dot(blend,1.0);
                fixed4 c = _Color0 * blend.x + _Color1 * blend.y + _Color2 * blend.z;
                c *= tex2D(_OcclusionMap, i.uv);
                c.a = tex2D(_DetailMap, i.uv);
                c.a *= i.wPos.y / 4 + 1;
                //c.a = clamp(c.a, 0, 1);
                return c;
            }
            ENDCG
        }
    }
}
