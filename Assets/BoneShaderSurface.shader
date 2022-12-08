
Shader "Custom/BoneShaderSurface"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows Lambert vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            
        };
        

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float4x4 MyXformMat[22];
        

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void vert (inout appdata_full v) {
            float4x4 temp = MyXformMat[v.color.x];     //Get the matrix of the bone that should be applied. 

            temp[0][3] *= v.color.y;                   //Multiplies the last row of the matrix by the weight. My theory is this would be equivalent to applying the weight to only the translation, but I don't think that's the case
            temp[1][3] *= v.color.y;                   //It seems to also apply to scale and rotation. From experimenting the weight applied to the scale is correct, but I'm not sure about the rotation
            temp[2][3] *= v.color.y;                   //If we want it to only apply to the translation, might need to first multiply the bone matrix and the object matrix and then multiply the result of that by the weight
            temp = mul(temp, unity_ObjectToWorld);      //Multiplies the bone matrix times the object's transformation matrix          
            v.vertex = mul(temp, v.vertex);  
            v.vertex = mul(unity_WorldToObject, v.vertex);           //Transofrms the vertex by the concatentation of the object's transformation matrix
            //v.vertex = mul(UNITY_MATRIX_VP, v.vertex);  //View and projection matrix multiplication
            //v.vertex = mul(v.vertex);
      }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            
        }
        ENDCG
    }
    FallBack "Diffuse"
}

/*
Shader "Custom/BoneShaderSurface"
{
    Properties
    {
        _Albedo ("Albedo (RGB), Alpha (A)", 2D) = "white" {}
        [NoScaleOffset]
        _Metallic ("Metallic (R), Occlusion (G), Emission (B), Smoothness (A)", 2D) = "black" {}
        [NoScaleOffset]
        _Normal ("Normal (RGB)", 2D) = "bump" {}
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Geometry"
            "RenderType" = "Opaque"
        }
        
        CGINCLUDE
        #define _GLOSSYENV 1
        ENDCG
        
        CGPROGRAM
        #pragma target 3.0
        #include "UnityPBSLighting.cginc"
        #pragma surface surf Standard Lambert vertex:vert
        #pragma exclude_renderers gles
        
        struct Input
        {
            float2 uv_Albedo;
        };
        sampler2D _Albedo;
        sampler2D _Normal;
        sampler2D _Metallic;
        float4x4 MyXformMat[22];

        void vert (inout appdata_full v) {
            float4x4 temp = MyXformMat[v.color.x];     //Get the matrix of the bone that should be applied. 

            temp[0][3] *= v.color.y;                   //Multiplies the last row of the matrix by the weight. My theory is this would be equivalent to applying the weight to only the translation, but I don't think that's the case
            temp[1][3] *= v.color.y;                   //It seems to also apply to scale and rotation. From experimenting the weight applied to the scale is correct, but I'm not sure about the rotation
            temp[2][3] *= v.color.y;                   //If we want it to only apply to the translation, might need to first multiply the bone matrix and the object matrix and then multiply the result of that by the weight
            temp = mul(temp, unity_ObjectToWorld);      //Multiplies the bone matrix times the object's transformation matrix          
            v.vertex = mul(temp, v.vertex);  
            v.vertex = mul(unity_WorldToObject, v.vertex);           //Transofrms the vertex by the concatentation of the object's transformation matrix
            //v.vertex = mul(UNITY_MATRIX_VP, v.vertex);  //View and projection matrix multiplication
            //v.vertex = mul(v.vertex);
        }
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 albedo = tex2D(_Albedo, IN.uv_Albedo);
            fixed4 metallic = tex2D(_Metallic, IN.uv_Albedo);
            fixed3 normal = UnpackScaleNormal(tex2D(_Normal, IN.uv_Albedo), 1);
        
            o.Albedo = albedo.rgb;
            o.Alpha = albedo.a;
            o.Normal = normal;
            o.Smoothness = metallic.a;
            o.Occlusion = metallic.g;
            o.Emission = metallic.b;
            o.Metallic = metallic.r;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
*/

