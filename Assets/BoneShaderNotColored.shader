Shader "Unlit/BoneShaderNotColored"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 mColor : COLOR0; //Holds the weights and the vertex
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4x4 MyXformMat[22];

            v2f vert (appdata v)
            {
                v2f o;
                
                float4x4 temp = MyXformMat[v.mColor.x];     //Get the matrix of the bone that should be applied. 

                temp[0][3] *= v.mColor.y;                   //Multiplies the last row of the matrix by the weight. My theory is this would be equivalent to applying the weight to only the translation, but I don't think that's the case
                temp[1][3] *= v.mColor.y;                   //It seems to also apply to scale and rotation. From experimenting the weight applied to the scale is correct, but I'm not sure about the rotation
                temp[2][3] *= v.mColor.y;                   //If we want it to only apply to the translation, might need to first multiply the bone matrix and the object matrix and then multiply the result of that by the weight
                temp = mul(temp, unity_ObjectToWorld);      //Multiplies the bone matrix times the object's transformation matrix          
                o.vertex = mul(temp, v.vertex);             //Transofrms the vertex by the concatentation of the object's transformation matrix
                o.vertex = mul(UNITY_MATRIX_VP, o.vertex);  //View and projection matrix multiplication
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);       //Default tex and fog stuff
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
