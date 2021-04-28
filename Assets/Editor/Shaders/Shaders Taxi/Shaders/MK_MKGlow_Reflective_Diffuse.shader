Shader "MK/MKGlow/Reflective/Diffuse"
{
  Properties
  {
    _Color ("Main Color", Color) = (1,1,1,1)
    _ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
    _MainTex ("Base (RGB) RefStrength (A)", 2D) = "white" {}
    _Cube ("Reflection Cubemap", Cube) = "_Skybox" {}
    _BumpMap ("Normalmap", 2D) = "bump" {}
    _MKGlowColor ("Glow Color", Color) = (1,1,1,1)
    _MKGlowPower ("Glow Power", Range(0, 5)) = 2.5
    _MKGlowTex ("Glow Texture", 2D) = "black" {}
    _MKGlowTexColor ("Glow Texture Color", Color) = (1,1,1,1)
    _MKGlowTexStrength ("Glow Texture Strength ", Range(0, 1)) = 1
  }
  SubShader
  {
    Tags
    { 
      "RenderType" = "MKGlow"
    }
    LOD 300
    Pass // ind: 1, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "LIGHTMODE" = "FORWARDBASE"
        "RenderType" = "MKGlow"
        "SHADOWSUPPORT" = "true"
      }
      LOD 300
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile DIRECTIONAL
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4 unity_WorldTransformParams;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      uniform float4 _MKGlowTex_ST;
      uniform float4 _BumpMap_ST;
      //uniform float3 _WorldSpaceCameraPos;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform float4 _Color;
      uniform float4 _ReflectColor;
      uniform float _MKGlowTexStrength;
      uniform float4 _MKGlowTexColor;
      uniform sampler2D _MainTex;
      uniform sampler2D _MKGlowTex;
      uniform sampler2D _BumpMap;
      uniform samplerCUBE _Cube;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float4 tangent :TANGENT0;
          float3 normal :NORMAL0;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
          float4 texcoord4 :TEXCOORD4;
          float4 texcoord7 :TEXCOORD7;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
          float4 texcoord4 :TEXCOORD4;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float4 u_xlat2;
      float3 u_xlat3;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0 = (in_v.vertex.yyyy * conv_mxt4x4_1(unity_ObjectToWorld));
          u_xlat0 = ((conv_mxt4x4_0(unity_ObjectToWorld) * in_v.vertex.xxxx) + u_xlat0);
          u_xlat0 = ((conv_mxt4x4_2(unity_ObjectToWorld) * in_v.vertex.zzzz) + u_xlat0);
          u_xlat1 = (u_xlat0 + conv_mxt4x4_3(unity_ObjectToWorld));
          u_xlat0.xyz = float3(((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz));
          out_v.vertex = mul(unity_MatrixVP, u_xlat1);
          out_v.texcoord.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _MainTex));
          out_v.texcoord.zw = TRANSFORM_TEX(in_v.texcoord.xy, _MKGlowTex);
          out_v.texcoord1.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _BumpMap));
          out_v.texcoord2.w = u_xlat0.x;
          u_xlat1.y = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat1.z = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat1.x = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          u_xlat0.x = dot(u_xlat1.xyz, u_xlat1.xyz);
          u_xlat0.x = rsqrt(u_xlat0.x);
          u_xlat1.xyz = float3((u_xlat0.xxx * u_xlat1.xyz));
          u_xlat2.xyz = float3((in_v.tangent.yyy * conv_mxt4x4_1(unity_ObjectToWorld).yzx));
          u_xlat2.xyz = float3(((conv_mxt4x4_0(unity_ObjectToWorld).yzx * in_v.tangent.xxx) + u_xlat2.xyz));
          u_xlat2.xyz = float3(((conv_mxt4x4_2(unity_ObjectToWorld).yzx * in_v.tangent.zzz) + u_xlat2.xyz));
          u_xlat0.x = dot(u_xlat2.xyz, u_xlat2.xyz);
          u_xlat0.x = rsqrt(u_xlat0.x);
          u_xlat2.xyz = float3((u_xlat0.xxx * u_xlat2.xyz));
          u_xlat3.xyz = float3((u_xlat1.xyz * u_xlat2.xyz));
          u_xlat3.xyz = float3(((u_xlat1.zxy * u_xlat2.yzx) + (-u_xlat3.xyz)));
          u_xlat0.x = (in_v.tangent.w * unity_WorldTransformParams.w);
          u_xlat3.xyz = float3((u_xlat0.xxx * u_xlat3.xyz));
          out_v.texcoord2.y = u_xlat3.x;
          out_v.texcoord2.x = u_xlat2.z;
          out_v.texcoord2.z = u_xlat1.y;
          out_v.texcoord3.x = u_xlat2.x;
          out_v.texcoord4.x = u_xlat2.y;
          out_v.texcoord3.z = u_xlat1.z;
          out_v.texcoord4.z = u_xlat1.x;
          out_v.texcoord3.w = u_xlat0.y;
          out_v.texcoord4.w = u_xlat0.z;
          out_v.texcoord3.y = u_xlat3.y;
          out_v.texcoord4.y = u_xlat3.z;
          out_v.texcoord7 = float4(0, 0, 0, 0);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat0_d;
      float3 u_xlat16_0;
      float4 u_xlat10_0;
      float3 u_xlat16_1;
      float u_xlat2_d;
      float3 u_xlat10_2;
      float3 u_xlat16_3;
      float3 u_xlat16_4;
      float u_xlat16_16;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0.xyz = tex2D(_MKGlowTex, in_f.texcoord.zw).xyz.xyz;
          u_xlat16_0.xyz = float3((u_xlat10_0.xyz * _MKGlowTexColor.xyz));
          u_xlat16_1.xyz = float3((u_xlat16_0.xyz * float3(_MKGlowTexStrength, _MKGlowTexStrength, _MKGlowTexStrength)));
          u_xlat10_0 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat16_1.xyz = float3(((u_xlat10_0.xyz * _Color.xyz) + u_xlat16_1.xyz));
          u_xlat16_1.xyz = float3((u_xlat16_1.xyz * _LightColor0.xyz));
          u_xlat0_d.x = in_f.texcoord2.w;
          u_xlat0_d.y = in_f.texcoord3.w;
          u_xlat0_d.z = in_f.texcoord4.w;
          u_xlat0_d.xyz = float3(((-u_xlat0_d.xyz) + _WorldSpaceCameraPos.xyz));
          u_xlat2_d = dot(u_xlat0_d.xyz, u_xlat0_d.xyz);
          u_xlat2_d = rsqrt(u_xlat2_d);
          u_xlat0_d.xyz = float3((u_xlat0_d.xyz * float3(u_xlat2_d, u_xlat2_d, u_xlat2_d)));
          u_xlat10_2.xyz = tex2D(_BumpMap, in_f.texcoord1.xy).xyz.xyz;
          u_xlat16_3.xyz = float3(((u_xlat10_2.xyz * float3(2, 2, 2)) + float3(-1, (-1), (-1))));
          u_xlat16_4.x = dot(in_f.texcoord2.xyz, u_xlat16_3.xyz);
          u_xlat16_4.y = dot(in_f.texcoord3.xyz, u_xlat16_3.xyz);
          u_xlat16_4.z = dot(in_f.texcoord4.xyz, u_xlat16_3.xyz);
          u_xlat2_d = dot((-u_xlat0_d.xyz), u_xlat16_4.xyz);
          u_xlat2_d = (u_xlat2_d + u_xlat2_d);
          u_xlat0_d.xyz = float3(((u_xlat16_4.xyz * (-float3(u_xlat2_d, u_xlat2_d, u_xlat2_d))) + (-u_xlat0_d.xyz)));
          u_xlat10_0.xyz = float3(texCUBE(_Cube, u_xlat0_d.xyz).xyz);
          u_xlat16_3.xyz = float3((u_xlat10_0.www * u_xlat10_0.xyz));
          u_xlat16_3.xyz = float3((u_xlat16_3.xyz * _ReflectColor.xyz));
          u_xlat16_0.x = dot(u_xlat16_4.xyz, u_xlat16_4.xyz);
          u_xlat16_0.x = rsqrt(u_xlat16_0.x);
          u_xlat16_0.xyz = float3((u_xlat16_0.xxx * u_xlat16_4.xyz));
          u_xlat16_16 = dot(u_xlat16_0.xyz, _WorldSpaceLightPos0.xyz);
          u_xlat16_16 = max(u_xlat16_16, 0);
          out_f.color.xyz = float3(((u_xlat16_1.xyz * float3(u_xlat16_16, u_xlat16_16, u_xlat16_16)) + u_xlat16_3.xyz));
          out_f.color.w = 1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 2, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "LIGHTMODE" = "FORWARDADD"
        "RenderType" = "MKGlow"
      }
      LOD 300
      ZWrite Off
      Blend One One
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile POINT
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4 unity_WorldTransformParams;
      //uniform float4x4 unity_MatrixVP;
      uniform float4x4 unity_WorldToLight;
      uniform float4 _MainTex_ST;
      uniform float4 _MKGlowTex_ST;
      uniform float4 _BumpMap_ST;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform float4 _Color;
      uniform float _MKGlowTexStrength;
      uniform float4 _MKGlowTexColor;
      uniform sampler2D _MainTex;
      uniform sampler2D _MKGlowTex;
      uniform sampler2D _BumpMap;
      uniform sampler2D _LightTexture0;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float4 tangent :TANGENT0;
          float3 normal :NORMAL0;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
          float4 texcoord4 :TEXCOORD4;
          float3 texcoord5 :TEXCOORD5;
          float4 texcoord6 :TEXCOORD6;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
          float4 texcoord4 :TEXCOORD4;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float4 u_xlat2;
      float3 u_xlat3;
      float3 u_xlat4;
      float u_xlat16;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          out_v.texcoord.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _MainTex));
          out_v.texcoord.zw = TRANSFORM_TEX(in_v.texcoord.xy, _MKGlowTex);
          out_v.texcoord1.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _BumpMap));
          u_xlat1.y = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat1.z = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat1.x = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          u_xlat1.xyz = float3(normalize(u_xlat1.xyz));
          u_xlat2.xyz = float3((in_v.tangent.yyy * conv_mxt4x4_1(unity_ObjectToWorld).yzx));
          u_xlat2.xyz = float3(((conv_mxt4x4_0(unity_ObjectToWorld).yzx * in_v.tangent.xxx) + u_xlat2.xyz));
          u_xlat2.xyz = float3(((conv_mxt4x4_2(unity_ObjectToWorld).yzx * in_v.tangent.zzz) + u_xlat2.xyz));
          u_xlat2.xyz = float3(normalize(u_xlat2.xyz));
          u_xlat3.xyz = float3((u_xlat1.xyz * u_xlat2.xyz));
          u_xlat3.xyz = float3(((u_xlat1.zxy * u_xlat2.yzx) + (-u_xlat3.xyz)));
          u_xlat16 = (in_v.tangent.w * unity_WorldTransformParams.w);
          u_xlat3.xyz = float3((float3(u_xlat16, u_xlat16, u_xlat16) * u_xlat3.xyz));
          out_v.texcoord2.y = u_xlat3.x;
          u_xlat4.xyz = float3(((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz));
          u_xlat0 = ((conv_mxt4x4_3(unity_ObjectToWorld) * in_v.vertex.wwww) + u_xlat0);
          out_v.texcoord2.w = u_xlat4.x;
          out_v.texcoord2.x = u_xlat2.z;
          out_v.texcoord2.z = u_xlat1.y;
          out_v.texcoord3.x = u_xlat2.x;
          out_v.texcoord4.x = u_xlat2.y;
          out_v.texcoord3.z = u_xlat1.z;
          out_v.texcoord4.z = u_xlat1.x;
          out_v.texcoord3.w = u_xlat4.y;
          out_v.texcoord4.w = u_xlat4.z;
          out_v.texcoord3.y = u_xlat3.y;
          out_v.texcoord4.y = u_xlat3.z;
          u_xlat1.xyz = float3((u_xlat0.yyy * conv_mxt4x4_1(unity_WorldToLight).xyz));
          u_xlat1.xyz = float3(((conv_mxt4x4_0(unity_WorldToLight).xyz * u_xlat0.xxx) + u_xlat1.xyz));
          u_xlat0.xyz = float3(((conv_mxt4x4_2(unity_WorldToLight).xyz * u_xlat0.zzz) + u_xlat1.xyz));
          out_v.texcoord5.xyz = float3(((conv_mxt4x4_3(unity_WorldToLight).xyz * u_xlat0.www) + u_xlat0.xyz));
          out_v.texcoord6 = float4(0, 0, 0, 0);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat0_d;
      float3 u_xlat16_0;
      float3 u_xlat10_0;
      float3 u_xlat16_1;
      float3 u_xlat16_2;
      float3 u_xlat3_d;
      float3 u_xlat16_5;
      float u_xlat12;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0.xyz = tex2D(_BumpMap, in_f.texcoord1.xy).xyz.xyz;
          u_xlat16_1.xyz = float3(((u_xlat10_0.xyz * float3(2, 2, 2)) + float3(-1, (-1), (-1))));
          u_xlat16_2.x = dot(in_f.texcoord2.xyz, u_xlat16_1.xyz);
          u_xlat16_2.y = dot(in_f.texcoord3.xyz, u_xlat16_1.xyz);
          u_xlat16_2.z = dot(in_f.texcoord4.xyz, u_xlat16_1.xyz);
          u_xlat16_0.x = dot(u_xlat16_2.xyz, u_xlat16_2.xyz);
          u_xlat16_0.x = rsqrt(u_xlat16_0.x);
          u_xlat16_0.xyz = float3((u_xlat16_0.xxx * u_xlat16_2.xyz));
          u_xlat3_d.x = in_f.texcoord2.w;
          u_xlat3_d.y = in_f.texcoord3.w;
          u_xlat3_d.z = in_f.texcoord4.w;
          u_xlat3_d.xyz = float3(((-u_xlat3_d.xyz) + _WorldSpaceLightPos0.xyz));
          u_xlat3_d.xyz = float3(normalize(u_xlat3_d.xyz));
          u_xlat16_1.x = dot(u_xlat16_0.xyz, u_xlat3_d.xyz);
          u_xlat16_1.x = max(u_xlat16_1.x, 0);
          u_xlat0_d.xyz = float3((in_f.texcoord3.www * conv_mxt4x4_1(unity_WorldToLight).xyz));
          u_xlat0_d.xyz = float3(((conv_mxt4x4_0(unity_WorldToLight).xyz * in_f.texcoord2.www) + u_xlat0_d.xyz));
          u_xlat0_d.xyz = float3(((conv_mxt4x4_2(unity_WorldToLight).xyz * in_f.texcoord4.www) + u_xlat0_d.xyz));
          u_xlat0_d.xyz = float3((u_xlat0_d.xyz + conv_mxt4x4_3(unity_WorldToLight).xyz));
          u_xlat0_d.x = dot(u_xlat0_d.xyz, u_xlat0_d.xyz);
          u_xlat0_d.x = tex2D(_LightTexture0, u_xlat0_d.xx).x;
          u_xlat16_5.xyz = float3((u_xlat0_d.xxx * _LightColor0.xyz));
          u_xlat10_0.xyz = tex2D(_MKGlowTex, in_f.texcoord.zw).xyz.xyz;
          u_xlat16_0.xyz = float3((u_xlat10_0.xyz * _MKGlowTexColor.xyz));
          u_xlat16_2.xyz = float3((u_xlat16_0.xyz * float3(_MKGlowTexStrength, _MKGlowTexStrength, _MKGlowTexStrength)));
          u_xlat10_0.xyz = tex2D(_MainTex, in_f.texcoord.xy).xyz.xyz;
          u_xlat16_2.xyz = float3(((u_xlat10_0.xyz * _Color.xyz) + u_xlat16_2.xyz));
          u_xlat16_5.xyz = float3((u_xlat16_5.xyz * u_xlat16_2.xyz));
          out_f.color.xyz = float3((u_xlat16_1.xxx * u_xlat16_5.xyz));
          out_f.color.w = 1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 3, name: PREPASS
    {
      Name "PREPASS"
      Tags
      { 
        "LIGHTMODE" = "PREPASSBASE"
        "RenderType" = "MKGlow"
      }
      LOD 300
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4 unity_WorldTransformParams;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _BumpMap_ST;
      uniform sampler2D _BumpMap;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float4 tangent :TANGENT0;
          float3 normal :NORMAL0;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float4 u_xlat2;
      float3 u_xlat3;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0 = (in_v.vertex.yyyy * conv_mxt4x4_1(unity_ObjectToWorld));
          u_xlat0 = ((conv_mxt4x4_0(unity_ObjectToWorld) * in_v.vertex.xxxx) + u_xlat0);
          u_xlat0 = ((conv_mxt4x4_2(unity_ObjectToWorld) * in_v.vertex.zzzz) + u_xlat0);
          u_xlat1 = (u_xlat0 + conv_mxt4x4_3(unity_ObjectToWorld));
          u_xlat0.xyz = float3(((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz));
          out_v.vertex = mul(unity_MatrixVP, u_xlat1);
          out_v.texcoord.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _BumpMap));
          out_v.texcoord1.w = u_xlat0.x;
          u_xlat1.y = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat1.z = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat1.x = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          u_xlat0.x = dot(u_xlat1.xyz, u_xlat1.xyz);
          u_xlat0.x = rsqrt(u_xlat0.x);
          u_xlat1.xyz = float3((u_xlat0.xxx * u_xlat1.xyz));
          u_xlat2.xyz = float3((in_v.tangent.yyy * conv_mxt4x4_1(unity_ObjectToWorld).yzx));
          u_xlat2.xyz = float3(((conv_mxt4x4_0(unity_ObjectToWorld).yzx * in_v.tangent.xxx) + u_xlat2.xyz));
          u_xlat2.xyz = float3(((conv_mxt4x4_2(unity_ObjectToWorld).yzx * in_v.tangent.zzz) + u_xlat2.xyz));
          u_xlat0.x = dot(u_xlat2.xyz, u_xlat2.xyz);
          u_xlat0.x = rsqrt(u_xlat0.x);
          u_xlat2.xyz = float3((u_xlat0.xxx * u_xlat2.xyz));
          u_xlat3.xyz = float3((u_xlat1.xyz * u_xlat2.xyz));
          u_xlat3.xyz = float3(((u_xlat1.zxy * u_xlat2.yzx) + (-u_xlat3.xyz)));
          u_xlat0.x = (in_v.tangent.w * unity_WorldTransformParams.w);
          u_xlat3.xyz = float3((u_xlat0.xxx * u_xlat3.xyz));
          out_v.texcoord1.y = u_xlat3.x;
          out_v.texcoord1.x = u_xlat2.z;
          out_v.texcoord1.z = u_xlat1.y;
          out_v.texcoord2.x = u_xlat2.x;
          out_v.texcoord3.x = u_xlat2.y;
          out_v.texcoord2.z = u_xlat1.z;
          out_v.texcoord3.z = u_xlat1.x;
          out_v.texcoord2.w = u_xlat0.y;
          out_v.texcoord3.w = u_xlat0.z;
          out_v.texcoord2.y = u_xlat3.y;
          out_v.texcoord3.y = u_xlat3.z;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat16_0;
      float3 u_xlat10_0;
      float3 u_xlat16_1;
      float3 u_xlat16_2;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0.xyz = tex2D(_BumpMap, in_f.texcoord.xy).xyz.xyz;
          u_xlat16_1.xyz = float3(((u_xlat10_0.xyz * float3(2, 2, 2)) + float3(-1, (-1), (-1))));
          u_xlat16_2.x = dot(in_f.texcoord1.xyz, u_xlat16_1.xyz);
          u_xlat16_2.y = dot(in_f.texcoord2.xyz, u_xlat16_1.xyz);
          u_xlat16_2.z = dot(in_f.texcoord3.xyz, u_xlat16_1.xyz);
          u_xlat16_0.x = dot(u_xlat16_2.xyz, u_xlat16_2.xyz);
          u_xlat16_0.x = rsqrt(u_xlat16_0.x);
          u_xlat16_0.xyz = float3((u_xlat16_0.xxx * u_xlat16_2.xyz));
          out_f.color.xyz = float3(((u_xlat16_0.xyz * float3(0.5, 0.5, 0.5)) + float3(0.5, 0.5, 0.5)));
          out_f.color.w = 0;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 4, name: PREPASS
    {
      Name "PREPASS"
      Tags
      { 
        "LIGHTMODE" = "PREPASSFINAL"
        "RenderType" = "MKGlow"
      }
      LOD 300
      ZWrite Off
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4 _ProjectionParams;
      //uniform float4 unity_SHAr;
      //uniform float4 unity_SHAg;
      //uniform float4 unity_SHAb;
      //uniform float4 unity_SHBr;
      //uniform float4 unity_SHBg;
      //uniform float4 unity_SHBb;
      //uniform float4 unity_SHC;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4 unity_WorldTransformParams;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      uniform float4 _MKGlowTex_ST;
      uniform float4 _BumpMap_ST;
      //uniform float3 _WorldSpaceCameraPos;
      uniform float4 _Color;
      uniform float4 _ReflectColor;
      uniform float _MKGlowTexStrength;
      uniform float4 _MKGlowTexColor;
      uniform sampler2D _MainTex;
      uniform sampler2D _MKGlowTex;
      uniform sampler2D _BumpMap;
      uniform samplerCUBE _Cube;
      uniform sampler2D _LightBuffer;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float4 tangent :TANGENT0;
          float3 normal :NORMAL0;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
          float4 texcoord4 :TEXCOORD4;
          float4 texcoord5 :TEXCOORD5;
          float4 texcoord6 :TEXCOORD6;
          float3 texcoord7 :TEXCOORD7;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
          float4 texcoord4 :TEXCOORD4;
          float4 texcoord5 :TEXCOORD5;
          float3 texcoord7 :TEXCOORD7;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat16_0;
      float4 u_xlat1;
      float4 u_xlat2;
      float4 u_xlat3;
      float3 u_xlat4;
      float3 u_xlat16_5;
      float3 u_xlat16_6;
      float u_xlat21;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0 = (in_v.vertex.yyyy * conv_mxt4x4_1(unity_ObjectToWorld));
          u_xlat0 = ((conv_mxt4x4_0(unity_ObjectToWorld) * in_v.vertex.xxxx) + u_xlat0);
          u_xlat0 = ((conv_mxt4x4_2(unity_ObjectToWorld) * in_v.vertex.zzzz) + u_xlat0);
          u_xlat1 = (u_xlat0 + conv_mxt4x4_3(unity_ObjectToWorld));
          u_xlat0.xyz = float3(((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz));
          u_xlat1 = mul(unity_MatrixVP, u_xlat1);
          out_v.vertex = u_xlat1;
          out_v.texcoord.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _MainTex));
          out_v.texcoord.zw = TRANSFORM_TEX(in_v.texcoord.xy, _MKGlowTex);
          out_v.texcoord1.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _BumpMap));
          out_v.texcoord2.w = u_xlat0.x;
          u_xlat2.xyz = float3((in_v.tangent.yyy * conv_mxt4x4_1(unity_ObjectToWorld).yzx));
          u_xlat2.xyz = float3(((conv_mxt4x4_0(unity_ObjectToWorld).yzx * in_v.tangent.xxx) + u_xlat2.xyz));
          u_xlat2.xyz = float3(((conv_mxt4x4_2(unity_ObjectToWorld).yzx * in_v.tangent.zzz) + u_xlat2.xyz));
          u_xlat0.x = dot(u_xlat2.xyz, u_xlat2.xyz);
          u_xlat0.x = rsqrt(u_xlat0.x);
          u_xlat2.xyz = float3((u_xlat0.xxx * u_xlat2.xyz));
          out_v.texcoord2.x = u_xlat2.z;
          u_xlat0.x = (in_v.tangent.w * unity_WorldTransformParams.w);
          u_xlat3.x = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat3.y = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat3.z = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          u_xlat3.xyz = float3(normalize(u_xlat3.xyz));
          u_xlat4.xyz = float3((u_xlat2.xyz * u_xlat3.zxy));
          u_xlat4.xyz = float3(((u_xlat3.yzx * u_xlat2.yzx) + (-u_xlat4.xyz)));
          u_xlat4.xyz = float3((u_xlat0.xxx * u_xlat4.xyz));
          out_v.texcoord2.y = u_xlat4.x;
          out_v.texcoord2.z = u_xlat3.x;
          out_v.texcoord3.x = u_xlat2.x;
          out_v.texcoord4.x = u_xlat2.y;
          out_v.texcoord3.w = u_xlat0.y;
          out_v.texcoord4.w = u_xlat0.z;
          out_v.texcoord3.y = u_xlat4.y;
          out_v.texcoord4.y = u_xlat4.z;
          out_v.texcoord3.z = u_xlat3.y;
          out_v.texcoord4.z = u_xlat3.z;
          u_xlat0.x = (u_xlat1.y * _ProjectionParams.x);
          u_xlat0.w = (u_xlat0.x * 0.5);
          u_xlat0.xz = (u_xlat1.xw * float2(0.5, 0.5));
          out_v.texcoord5.zw = u_xlat1.zw;
          out_v.texcoord5.xy = float2((u_xlat0.zz + u_xlat0.xw));
          out_v.texcoord6 = float4(0, 0, 0, 0);
          u_xlat16_5.x = (u_xlat3.y * u_xlat3.y);
          u_xlat16_5.x = ((u_xlat3.x * u_xlat3.x) + (-u_xlat16_5.x));
          u_xlat16_0 = (u_xlat3.yzzx * u_xlat3.xyzz);
          u_xlat16_6.x = dot(unity_SHBr, u_xlat16_0);
          u_xlat16_6.y = dot(unity_SHBg, u_xlat16_0);
          u_xlat16_6.z = dot(unity_SHBb, u_xlat16_0);
          u_xlat16_5.xyz = float3(((unity_SHC.xyz * u_xlat16_5.xxx) + u_xlat16_6.xyz));
          u_xlat3.w = 1;
          u_xlat16_6.x = dot(unity_SHAr, u_xlat3);
          u_xlat16_6.y = dot(unity_SHAg, u_xlat3);
          u_xlat16_6.z = dot(unity_SHAb, u_xlat3);
          u_xlat16_5.xyz = float3((u_xlat16_5.xyz + u_xlat16_6.xyz));
          u_xlat16_5.xyz = float3(max(u_xlat16_5.xyz, float3(0, 0, 0)));
          u_xlat1.xyz = float3(log2(u_xlat16_5.xyz));
          u_xlat1.xyz = float3((u_xlat1.xyz * float3(0.416666657, 0.416666657, 0.416666657)));
          u_xlat1.xyz = float3(exp2(u_xlat1.xyz));
          u_xlat1.xyz = float3(((u_xlat1.xyz * float3(1.05499995, 1.05499995, 1.05499995)) + float3(-0.0549999997, (-0.0549999997), (-0.0549999997))));
          out_v.texcoord7.xyz = float3(max(u_xlat1.xyz, float3(0, 0, 0)));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat0_d;
      float3 u_xlat10_0;
      float4 u_xlat10_1;
      float3 u_xlat16_2;
      float3 u_xlat16_3;
      float3 u_xlat16_4;
      float3 u_xlat10_4;
      float u_xlat15;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = in_f.texcoord2.w;
          u_xlat0_d.y = in_f.texcoord3.w;
          u_xlat0_d.z = in_f.texcoord4.w;
          u_xlat0_d.xyz = float3(((-u_xlat0_d.xyz) + _WorldSpaceCameraPos.xyz));
          u_xlat0_d.xyz = float3(normalize(u_xlat0_d.xyz));
          u_xlat10_1.xyz = tex2D(_BumpMap, in_f.texcoord1.xy).xyz.xyz;
          u_xlat16_2.xyz = float3(((u_xlat10_1.xyz * float3(2, 2, 2)) + float3(-1, (-1), (-1))));
          u_xlat16_3.x = dot(in_f.texcoord2.xyz, u_xlat16_2.xyz);
          u_xlat16_3.y = dot(in_f.texcoord3.xyz, u_xlat16_2.xyz);
          u_xlat16_3.z = dot(in_f.texcoord4.xyz, u_xlat16_2.xyz);
          u_xlat15 = dot((-u_xlat0_d.xyz), u_xlat16_3.xyz);
          u_xlat15 = (u_xlat15 + u_xlat15);
          u_xlat0_d.xyz = float3(((u_xlat16_3.xyz * (-float3(u_xlat15, u_xlat15, u_xlat15))) + (-u_xlat0_d.xyz)));
          u_xlat10_0.xyz = float3(texCUBE(_Cube, u_xlat0_d.xyz).xyz);
          u_xlat10_1 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat16_2.xyz = float3((u_xlat10_0.xyz * u_xlat10_1.www));
          u_xlat16_2.xyz = float3((u_xlat16_2.xyz * _ReflectColor.xyz));
          u_xlat0_d.xy = float2((in_f.texcoord5.xy / in_f.texcoord5.ww));
          u_xlat10_0.xyz = tex2D(_LightBuffer, u_xlat0_d.xy).xyz.xyz;
          u_xlat16_3.xyz = float3(max(u_xlat10_0.xyz, float3(0.00100000005, 0.00100000005, 0.00100000005)));
          u_xlat16_3.xyz = float3(log2(u_xlat16_3.xyz));
          u_xlat0_d.xyz = float3(((-u_xlat16_3.xyz) + in_f.texcoord7.xyz));
          u_xlat10_4.xyz = tex2D(_MKGlowTex, in_f.texcoord.zw).xyz.xyz;
          u_xlat16_4.xyz = float3((u_xlat10_4.xyz * _MKGlowTexColor.xyz));
          u_xlat16_3.xyz = float3((u_xlat16_4.xyz * float3(_MKGlowTexStrength, _MKGlowTexStrength, _MKGlowTexStrength)));
          u_xlat16_3.xyz = float3(((u_xlat10_1.xyz * _Color.xyz) + u_xlat16_3.xyz));
          out_f.color.xyz = float3(((u_xlat16_3.xyz * u_xlat0_d.xyz) + u_xlat16_2.xyz));
          out_f.color.w = 1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 5, name: DEFERRED
    {
      Name "DEFERRED"
      Tags
      { 
        "LIGHTMODE" = "DEFERRED"
        "RenderType" = "MKGlow"
      }
      LOD 300
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4 unity_WorldTransformParams;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      uniform float4 _MKGlowTex_ST;
      uniform float4 _BumpMap_ST;
      //uniform float3 _WorldSpaceCameraPos;
      uniform float4 _Color;
      uniform float4 _ReflectColor;
      uniform float _MKGlowTexStrength;
      uniform float4 _MKGlowTexColor;
      uniform sampler2D _MainTex;
      uniform sampler2D _MKGlowTex;
      uniform sampler2D _BumpMap;
      uniform samplerCUBE _Cube;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float4 tangent :TANGENT0;
          float3 normal :NORMAL0;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
          float4 texcoord4 :TEXCOORD4;
          float4 texcoord5 :TEXCOORD5;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
          float4 texcoord4 :TEXCOORD4;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
          float4 color1 :SV_Target1;
          float4 color2 :SV_Target2;
          float4 color3 :SV_Target3;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float4 u_xlat2;
      float3 u_xlat3;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0 = (in_v.vertex.yyyy * conv_mxt4x4_1(unity_ObjectToWorld));
          u_xlat0 = ((conv_mxt4x4_0(unity_ObjectToWorld) * in_v.vertex.xxxx) + u_xlat0);
          u_xlat0 = ((conv_mxt4x4_2(unity_ObjectToWorld) * in_v.vertex.zzzz) + u_xlat0);
          u_xlat1 = (u_xlat0 + conv_mxt4x4_3(unity_ObjectToWorld));
          u_xlat0.xyz = float3(((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz));
          out_v.vertex = mul(unity_MatrixVP, u_xlat1);
          out_v.texcoord.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _MainTex));
          out_v.texcoord.zw = TRANSFORM_TEX(in_v.texcoord.xy, _MKGlowTex);
          out_v.texcoord1.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _BumpMap));
          out_v.texcoord2.w = u_xlat0.x;
          u_xlat1.y = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat1.z = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat1.x = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          u_xlat0.x = dot(u_xlat1.xyz, u_xlat1.xyz);
          u_xlat0.x = rsqrt(u_xlat0.x);
          u_xlat1.xyz = float3((u_xlat0.xxx * u_xlat1.xyz));
          u_xlat2.xyz = float3((in_v.tangent.yyy * conv_mxt4x4_1(unity_ObjectToWorld).yzx));
          u_xlat2.xyz = float3(((conv_mxt4x4_0(unity_ObjectToWorld).yzx * in_v.tangent.xxx) + u_xlat2.xyz));
          u_xlat2.xyz = float3(((conv_mxt4x4_2(unity_ObjectToWorld).yzx * in_v.tangent.zzz) + u_xlat2.xyz));
          u_xlat0.x = dot(u_xlat2.xyz, u_xlat2.xyz);
          u_xlat0.x = rsqrt(u_xlat0.x);
          u_xlat2.xyz = float3((u_xlat0.xxx * u_xlat2.xyz));
          u_xlat3.xyz = float3((u_xlat1.xyz * u_xlat2.xyz));
          u_xlat3.xyz = float3(((u_xlat1.zxy * u_xlat2.yzx) + (-u_xlat3.xyz)));
          u_xlat0.x = (in_v.tangent.w * unity_WorldTransformParams.w);
          u_xlat3.xyz = float3((u_xlat0.xxx * u_xlat3.xyz));
          out_v.texcoord2.y = u_xlat3.x;
          out_v.texcoord2.x = u_xlat2.z;
          out_v.texcoord2.z = u_xlat1.y;
          out_v.texcoord3.x = u_xlat2.x;
          out_v.texcoord4.x = u_xlat2.y;
          out_v.texcoord3.z = u_xlat1.z;
          out_v.texcoord4.z = u_xlat1.x;
          out_v.texcoord3.w = u_xlat0.y;
          out_v.texcoord4.w = u_xlat0.z;
          out_v.texcoord3.y = u_xlat3.y;
          out_v.texcoord4.y = u_xlat3.z;
          out_v.texcoord5 = float4(0, 0, 0, 0);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat0_d;
      float3 u_xlat16_0;
      float4 u_xlat10_0;
      float3 u_xlat16_1;
      float3 u_xlat16_2;
      float u_xlat3_d;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0.xyz = tex2D(_MKGlowTex, in_f.texcoord.zw).xyz.xyz;
          u_xlat16_0.xyz = float3((u_xlat10_0.xyz * _MKGlowTexColor.xyz));
          u_xlat16_1.xyz = float3((u_xlat16_0.xyz * float3(_MKGlowTexStrength, _MKGlowTexStrength, _MKGlowTexStrength)));
          u_xlat10_0 = tex2D(_MainTex, in_f.texcoord.xy);
          out_f.color.xyz = float3(((u_xlat10_0.xyz * _Color.xyz) + u_xlat16_1.xyz));
          out_f.color.w = 1;
          out_f.color1 = float4(0, 0, 0, 0);
          out_f.color2.w = 1;
          u_xlat10_0.xyz = tex2D(_BumpMap, in_f.texcoord1.xy).xyz.xyz;
          u_xlat16_1.xyz = float3(((u_xlat10_0.xyz * float3(2, 2, 2)) + float3(-1, (-1), (-1))));
          u_xlat16_2.x = dot(in_f.texcoord2.xyz, u_xlat16_1.xyz);
          u_xlat16_2.y = dot(in_f.texcoord3.xyz, u_xlat16_1.xyz);
          u_xlat16_2.z = dot(in_f.texcoord4.xyz, u_xlat16_1.xyz);
          u_xlat16_0.x = dot(u_xlat16_2.xyz, u_xlat16_2.xyz);
          u_xlat16_0.x = rsqrt(u_xlat16_0.x);
          u_xlat16_0.xyz = float3((u_xlat16_0.xxx * u_xlat16_2.xyz));
          u_xlat16_0.xyz = float3(((u_xlat16_0.xyz * float3(0.5, 0.5, 0.5)) + float3(0.5, 0.5, 0.5)));
          out_f.color2.xyz = float3(u_xlat16_0.xyz);
          u_xlat0_d.x = in_f.texcoord2.w;
          u_xlat0_d.y = in_f.texcoord3.w;
          u_xlat0_d.z = in_f.texcoord4.w;
          u_xlat0_d.xyz = float3(((-u_xlat0_d.xyz) + _WorldSpaceCameraPos.xyz));
          u_xlat3_d = dot(u_xlat0_d.xyz, u_xlat0_d.xyz);
          u_xlat3_d = rsqrt(u_xlat3_d);
          u_xlat0_d.xyz = float3((u_xlat0_d.xyz * float3(u_xlat3_d, u_xlat3_d, u_xlat3_d)));
          u_xlat3_d = dot((-u_xlat0_d.xyz), u_xlat16_2.xyz);
          u_xlat3_d = (u_xlat3_d + u_xlat3_d);
          u_xlat0_d.xyz = float3(((u_xlat16_2.xyz * (-float3(u_xlat3_d, u_xlat3_d, u_xlat3_d))) + (-u_xlat0_d.xyz)));
          u_xlat10_0.xyz = float3(texCUBE(_Cube, u_xlat0_d.xyz).xyz);
          u_xlat16_1.xyz = float3((u_xlat10_0.www * u_xlat10_0.xyz));
          u_xlat16_1.xyz = float3((u_xlat16_1.xyz * _ReflectColor.xyz));
          out_f.color3.xyz = float3(exp2((-u_xlat16_1.xyz)));
          out_f.color3.w = 1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Reflective/VertexLit"
}
