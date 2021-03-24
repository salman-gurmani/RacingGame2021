Shader "MatCap/Vertex/Textured Lit"
{
  Properties
  {
    _Color ("Color", Color) = (0.5,0.5,0.5,1)
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _MatCap ("MatCap (RGB)", 2D) = "white" {}
  }
  SubShader
  {
    Tags
    { 
      "RenderType" = "Opaque"
    }
    LOD 200
    Pass // ind: 1, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "LIGHTMODE" = "FORWARDBASE"
        "RenderType" = "Opaque"
        "SHADOWSUPPORT" = "true"
      }
      LOD 200
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
      //uniform float4x4 unity_MatrixV;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform float4 _Color;
      uniform sampler2D _MainTex;
      uniform sampler2D _MatCap;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float3 normal :NORMAL0;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
          float2 texcoord3 :TEXCOORD3;
          float3 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
          float4 texcoord6 :TEXCOORD6;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
          float2 texcoord3 :TEXCOORD3;
          float3 texcoord1 :TEXCOORD1;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float3 u_xlat2;
      float u_xlat6;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0 = (in_v.vertex.yyyy * conv_mxt4x4_1(unity_ObjectToWorld));
          u_xlat0 = ((conv_mxt4x4_0(unity_ObjectToWorld) * in_v.vertex.xxxx) + u_xlat0);
          u_xlat0 = ((conv_mxt4x4_2(unity_ObjectToWorld) * in_v.vertex.zzzz) + u_xlat0);
          u_xlat1 = (u_xlat0 + conv_mxt4x4_3(unity_ObjectToWorld));
          out_v.texcoord2.xyz = float3(((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz));
          out_v.vertex = mul(unity_MatrixVP, u_xlat1);
          u_xlat0.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _MainTex));
          out_v.texcoord.xy = float2(u_xlat0.xy);
          u_xlat0.x = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat0.y = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat0.z = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          out_v.texcoord1.xyz = float3(normalize(u_xlat0.xyz));
          out_v.texcoord2.w = 0;
          u_xlat0.x = conv_mxt4x4_0(unity_WorldToObject).y.x;
          u_xlat0.y = conv_mxt4x4_1(unity_WorldToObject).y.x;
          u_xlat0.z = conv_mxt4x4_2(unity_WorldToObject).y.x;
          u_xlat0.xyz = float3((u_xlat0.xyz * in_v.normal.yyy));
          u_xlat1.x = conv_mxt4x4_0(unity_WorldToObject).x;
          u_xlat1.y = conv_mxt4x4_1(unity_WorldToObject).x;
          u_xlat1.z = conv_mxt4x4_2(unity_WorldToObject).x;
          u_xlat0.xyz = float3(((u_xlat1.xyz * in_v.normal.xxx) + u_xlat0.xyz));
          u_xlat1.x = conv_mxt4x4_0(unity_WorldToObject).z.x;
          u_xlat1.y = conv_mxt4x4_1(unity_WorldToObject).z.x;
          u_xlat1.z = conv_mxt4x4_2(unity_WorldToObject).z.x;
          u_xlat0.xyz = float3(((u_xlat1.xyz * in_v.normal.zzz) + u_xlat0.xyz));
          u_xlat0.xyz = float3(normalize(u_xlat0.xyz));
          u_xlat2.xz = (u_xlat0.yy * conv_mxt4x4_1(unity_MatrixV).xy);
          u_xlat0.xy = ((conv_mxt4x4_0(unity_MatrixV).xy * u_xlat0.xx) + u_xlat2.xz).xy;
          u_xlat0.xy = ((conv_mxt4x4_2(unity_MatrixV).xy * u_xlat0.zz) + u_xlat0.xy).xy;
          out_v.texcoord3.xy = float2(((u_xlat0.xy * float2(0.5, 0.5)) + float2(0.5, 0.5)));
          out_v.texcoord6 = float4(0, 0, 0, 0);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat0_d;
      float3 u_xlat10_0;
      float3 u_xlat10_1;
      float3 u_xlat16_2;
      float u_xlat16_11;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0.xyz = tex2D(_MainTex, in_f.texcoord.xy).xyz.xyz;
          u_xlat10_1.xyz = tex2D(_MatCap, in_f.texcoord3.xy).xyz.xyz;
          u_xlat16_2.xyz = float3((u_xlat10_0.xyz * u_xlat10_1.xyz));
          u_xlat0_d.xyz = float3((u_xlat16_2.xyz * _Color.xyz));
          u_xlat0_d.xyz = float3((u_xlat0_d.xyz + u_xlat0_d.xyz));
          u_xlat16_2.xyz = float3((u_xlat0_d.xyz * _LightColor0.xyz));
          u_xlat16_11 = dot(in_f.texcoord1.xyz, _WorldSpaceLightPos0.xyz);
          u_xlat16_11 = max(u_xlat16_11, 0);
          out_f.color.xyz = float3((float3(u_xlat16_11, u_xlat16_11, u_xlat16_11) * u_xlat16_2.xyz));
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
        "RenderType" = "Opaque"
      }
      LOD 200
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
      //uniform float4x4 unity_MatrixV;
      //uniform float4x4 unity_MatrixVP;
      uniform float4x4 unity_WorldToLight;
      uniform float4 _MainTex_ST;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform float4 _Color;
      uniform sampler2D _MainTex;
      uniform sampler2D _MatCap;
      uniform sampler2D _LightTexture0;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float3 normal :NORMAL0;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
          float2 texcoord3 :TEXCOORD3;
          float3 texcoord1 :TEXCOORD1;
          float3 texcoord2 :TEXCOORD2;
          float3 texcoord4 :TEXCOORD4;
          float4 texcoord5 :TEXCOORD5;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
          float2 texcoord3 :TEXCOORD3;
          float3 texcoord1 :TEXCOORD1;
          float3 texcoord2 :TEXCOORD2;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float4 u_xlat2;
      float3 u_xlat4;
      float u_xlat10;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat1 = mul(unity_ObjectToWorld, float4(in_v.vertex.xyz,1.0));
          out_v.vertex = mul(unity_MatrixVP, u_xlat1);
          u_xlat1.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _MainTex));
          out_v.texcoord.xy = float2(u_xlat1.xy);
          u_xlat1.x = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat1.y = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat1.z = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          out_v.texcoord1.xyz = float3(normalize(u_xlat1.xyz));
          out_v.texcoord2.xyz = float3(((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz));
          u_xlat0 = ((conv_mxt4x4_3(unity_ObjectToWorld) * in_v.vertex.wwww) + u_xlat0);
          u_xlat1.x = conv_mxt4x4_0(unity_WorldToObject).y.x;
          u_xlat1.y = conv_mxt4x4_1(unity_WorldToObject).y.x;
          u_xlat1.z = conv_mxt4x4_2(unity_WorldToObject).y.x;
          u_xlat1.xyz = float3((u_xlat1.xyz * in_v.normal.yyy));
          u_xlat2.x = conv_mxt4x4_0(unity_WorldToObject).x;
          u_xlat2.y = conv_mxt4x4_1(unity_WorldToObject).x;
          u_xlat2.z = conv_mxt4x4_2(unity_WorldToObject).x;
          u_xlat1.xyz = float3(((u_xlat2.xyz * in_v.normal.xxx) + u_xlat1.xyz));
          u_xlat2.x = conv_mxt4x4_0(unity_WorldToObject).z.x;
          u_xlat2.y = conv_mxt4x4_1(unity_WorldToObject).z.x;
          u_xlat2.z = conv_mxt4x4_2(unity_WorldToObject).z.x;
          u_xlat1.xyz = float3(((u_xlat2.xyz * in_v.normal.zzz) + u_xlat1.xyz));
          u_xlat1.xyz = float3(normalize(u_xlat1.xyz));
          u_xlat4.xz = (u_xlat1.yy * conv_mxt4x4_1(unity_MatrixV).xy);
          u_xlat1.xy = ((conv_mxt4x4_0(unity_MatrixV).xy * u_xlat1.xx) + u_xlat4.xz).xy;
          u_xlat1.xy = ((conv_mxt4x4_2(unity_MatrixV).xy * u_xlat1.zz) + u_xlat1.xy).xy;
          out_v.texcoord3.xy = float2(((u_xlat1.xy * float2(0.5, 0.5)) + float2(0.5, 0.5)));
          u_xlat1.xyz = float3((u_xlat0.yyy * conv_mxt4x4_1(unity_WorldToLight).xyz));
          u_xlat1.xyz = float3(((conv_mxt4x4_0(unity_WorldToLight).xyz * u_xlat0.xxx) + u_xlat1.xyz));
          u_xlat0.xyz = float3(((conv_mxt4x4_2(unity_WorldToLight).xyz * u_xlat0.zzz) + u_xlat1.xyz));
          out_v.texcoord4.xyz = float3(((conv_mxt4x4_3(unity_WorldToLight).xyz * u_xlat0.www) + u_xlat0.xyz));
          out_v.texcoord5 = float4(0, 0, 0, 0);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat0_d;
      float3 u_xlat10_0;
      float3 u_xlat16_1;
      float3 u_xlat10_2;
      float3 u_xlat16_3;
      float u_xlat12;
      float u_xlat16_13;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.xyz = float3((in_f.texcoord2.yyy * conv_mxt4x4_1(unity_WorldToLight).xyz));
          u_xlat0_d.xyz = float3(((conv_mxt4x4_0(unity_WorldToLight).xyz * in_f.texcoord2.xxx) + u_xlat0_d.xyz));
          u_xlat0_d.xyz = float3(((conv_mxt4x4_2(unity_WorldToLight).xyz * in_f.texcoord2.zzz) + u_xlat0_d.xyz));
          u_xlat0_d.xyz = float3((u_xlat0_d.xyz + conv_mxt4x4_3(unity_WorldToLight).xyz));
          u_xlat0_d.x = dot(u_xlat0_d.xyz, u_xlat0_d.xyz);
          u_xlat0_d.x = tex2D(_LightTexture0, u_xlat0_d.xx).x;
          u_xlat16_1.xyz = float3((u_xlat0_d.xxx * _LightColor0.xyz));
          u_xlat10_0.xyz = tex2D(_MainTex, in_f.texcoord.xy).xyz.xyz;
          u_xlat10_2.xyz = tex2D(_MatCap, in_f.texcoord3.xy).xyz.xyz;
          u_xlat16_3.xyz = float3((u_xlat10_0.xyz * u_xlat10_2.xyz));
          u_xlat0_d.xyz = float3((u_xlat16_3.xyz * _Color.xyz));
          u_xlat0_d.xyz = float3((u_xlat0_d.xyz + u_xlat0_d.xyz));
          u_xlat16_1.xyz = float3((u_xlat16_1.xyz * u_xlat0_d.xyz));
          u_xlat0_d.xyz = float3(((-in_f.texcoord2.xyz) + _WorldSpaceLightPos0.xyz));
          u_xlat0_d.xyz = float3(normalize(u_xlat0_d.xyz));
          u_xlat16_13 = dot(in_f.texcoord1.xyz, u_xlat0_d.xyz);
          u_xlat16_13 = max(u_xlat16_13, 0);
          out_f.color.xyz = float3((float3(u_xlat16_13, u_xlat16_13, u_xlat16_13) * u_xlat16_1.xyz));
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
        "RenderType" = "Opaque"
      }
      LOD 200
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
      //uniform float4x4 unity_MatrixV;
      //uniform float4x4 unity_MatrixVP;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float3 normal :NORMAL0;
      };
      
      struct OUT_Data_Vert
      {
          float3 texcoord :TEXCOORD0;
          float3 texcoord1 :TEXCOORD1;
          float2 texcoord2 :TEXCOORD2;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float3 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float3 u_xlat2;
      float u_xlat6;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0 = (in_v.vertex.yyyy * conv_mxt4x4_1(unity_ObjectToWorld));
          u_xlat0 = ((conv_mxt4x4_0(unity_ObjectToWorld) * in_v.vertex.xxxx) + u_xlat0);
          u_xlat0 = ((conv_mxt4x4_2(unity_ObjectToWorld) * in_v.vertex.zzzz) + u_xlat0);
          u_xlat1 = (u_xlat0 + conv_mxt4x4_3(unity_ObjectToWorld));
          out_v.texcoord1.xyz = float3(((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz));
          out_v.vertex = mul(unity_MatrixVP, u_xlat1);
          u_xlat0.x = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat0.y = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat0.z = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          out_v.texcoord.xyz = float3(normalize(u_xlat0.xyz));
          u_xlat0.x = conv_mxt4x4_0(unity_WorldToObject).y.x;
          u_xlat0.y = conv_mxt4x4_1(unity_WorldToObject).y.x;
          u_xlat0.z = conv_mxt4x4_2(unity_WorldToObject).y.x;
          u_xlat0.xyz = float3((u_xlat0.xyz * in_v.normal.yyy));
          u_xlat1.x = conv_mxt4x4_0(unity_WorldToObject).x;
          u_xlat1.y = conv_mxt4x4_1(unity_WorldToObject).x;
          u_xlat1.z = conv_mxt4x4_2(unity_WorldToObject).x;
          u_xlat0.xyz = float3(((u_xlat1.xyz * in_v.normal.xxx) + u_xlat0.xyz));
          u_xlat1.x = conv_mxt4x4_0(unity_WorldToObject).z.x;
          u_xlat1.y = conv_mxt4x4_1(unity_WorldToObject).z.x;
          u_xlat1.z = conv_mxt4x4_2(unity_WorldToObject).z.x;
          u_xlat0.xyz = float3(((u_xlat1.xyz * in_v.normal.zzz) + u_xlat0.xyz));
          u_xlat0.xyz = float3(normalize(u_xlat0.xyz));
          u_xlat2.xz = (u_xlat0.yy * conv_mxt4x4_1(unity_MatrixV).xy);
          u_xlat0.xy = ((conv_mxt4x4_0(unity_MatrixV).xy * u_xlat0.xx) + u_xlat2.xz).xy;
          u_xlat0.xy = ((conv_mxt4x4_2(unity_MatrixV).xy * u_xlat0.zz) + u_xlat0.xy).xy;
          out_v.texcoord2.xy = float2(((u_xlat0.xy * float2(0.5, 0.5)) + float2(0.5, 0.5)));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          out_f.color.xyz = float3(((in_f.texcoord.xyz * float3(0.5, 0.5, 0.5)) + float3(0.5, 0.5, 0.5)));
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
        "RenderType" = "Opaque"
      }
      LOD 200
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
      //uniform float4x4 unity_MatrixV;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      uniform float4 _Color;
      uniform sampler2D _MainTex;
      uniform sampler2D _MatCap;
      uniform sampler2D _LightBuffer;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float3 normal :NORMAL0;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
          float2 texcoord2 :TEXCOORD2;
          float3 texcoord1 :TEXCOORD1;
          float4 texcoord3 :TEXCOORD3;
          float4 texcoord4 :TEXCOORD4;
          float3 texcoord5 :TEXCOORD5;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
          float2 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
          float3 texcoord5 :TEXCOORD5;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float4 u_xlat16_1;
      float3 u_xlat2;
      float3 u_xlat16_3;
      float3 u_xlat16_4;
      float3 u_xlat6;
      float u_xlat15;
      float u_xlat16;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0 = (in_v.vertex.yyyy * conv_mxt4x4_1(unity_ObjectToWorld));
          u_xlat0 = ((conv_mxt4x4_0(unity_ObjectToWorld) * in_v.vertex.xxxx) + u_xlat0);
          u_xlat0 = ((conv_mxt4x4_2(unity_ObjectToWorld) * in_v.vertex.zzzz) + u_xlat0);
          u_xlat1 = (u_xlat0 + conv_mxt4x4_3(unity_ObjectToWorld));
          out_v.texcoord1.xyz = float3(((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz));
          u_xlat0 = mul(unity_MatrixVP, u_xlat1);
          out_v.vertex = u_xlat0;
          u_xlat1.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _MainTex));
          out_v.texcoord.xy = float2(u_xlat1.xy);
          u_xlat1.x = conv_mxt4x4_0(unity_WorldToObject).y.x;
          u_xlat1.y = conv_mxt4x4_1(unity_WorldToObject).y.x;
          u_xlat1.z = conv_mxt4x4_2(unity_WorldToObject).y.x;
          u_xlat1.xyz = float3((u_xlat1.xyz * in_v.normal.yyy));
          u_xlat2.x = conv_mxt4x4_0(unity_WorldToObject).x;
          u_xlat2.y = conv_mxt4x4_1(unity_WorldToObject).x;
          u_xlat2.z = conv_mxt4x4_2(unity_WorldToObject).x;
          u_xlat1.xyz = float3(((u_xlat2.xyz * in_v.normal.xxx) + u_xlat1.xyz));
          u_xlat2.x = conv_mxt4x4_0(unity_WorldToObject).z.x;
          u_xlat2.y = conv_mxt4x4_1(unity_WorldToObject).z.x;
          u_xlat2.z = conv_mxt4x4_2(unity_WorldToObject).z.x;
          u_xlat1.xyz = float3(((u_xlat2.xyz * in_v.normal.zzz) + u_xlat1.xyz));
          u_xlat1.xyz = float3(normalize(u_xlat1.xyz));
          u_xlat6.xz = (u_xlat1.yy * conv_mxt4x4_1(unity_MatrixV).xy);
          u_xlat1.xy = ((conv_mxt4x4_0(unity_MatrixV).xy * u_xlat1.xx) + u_xlat6.xz).xy;
          u_xlat1.xy = ((conv_mxt4x4_2(unity_MatrixV).xy * u_xlat1.zz) + u_xlat1.xy).xy;
          out_v.texcoord2.xy = float2(((u_xlat1.xy * float2(0.5, 0.5)) + float2(0.5, 0.5)));
          u_xlat0.y = (u_xlat0.y * _ProjectionParams.x);
          u_xlat1.xzw = (u_xlat0.xwy * float3(0.5, 0.5, 0.5));
          out_v.texcoord3.zw = u_xlat0.zw;
          out_v.texcoord3.xy = float2((u_xlat1.zz + u_xlat1.xw));
          out_v.texcoord4 = float4(0, 0, 0, 0);
          u_xlat0.x = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat0.y = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat0.z = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          u_xlat0.xyz = float3(normalize(u_xlat0.xyz));
          u_xlat16_3.x = (u_xlat0.y * u_xlat0.y);
          u_xlat16_3.x = ((u_xlat0.x * u_xlat0.x) + (-u_xlat16_3.x));
          u_xlat16_1 = (u_xlat0.yzzx * u_xlat0.xyzz);
          u_xlat16_4.x = dot(unity_SHBr, u_xlat16_1);
          u_xlat16_4.y = dot(unity_SHBg, u_xlat16_1);
          u_xlat16_4.z = dot(unity_SHBb, u_xlat16_1);
          u_xlat16_3.xyz = float3(((unity_SHC.xyz * u_xlat16_3.xxx) + u_xlat16_4.xyz));
          u_xlat0.w = 1;
          u_xlat16_4.x = dot(unity_SHAr, u_xlat0);
          u_xlat16_4.y = dot(unity_SHAg, u_xlat0);
          u_xlat16_4.z = dot(unity_SHAb, u_xlat0);
          u_xlat16_3.xyz = float3((u_xlat16_3.xyz + u_xlat16_4.xyz));
          u_xlat16_3.xyz = float3(max(u_xlat16_3.xyz, float3(0, 0, 0)));
          u_xlat0.xyz = float3(log2(u_xlat16_3.xyz));
          u_xlat0.xyz = float3((u_xlat0.xyz * float3(0.416666657, 0.416666657, 0.416666657)));
          u_xlat0.xyz = float3(exp2(u_xlat0.xyz));
          u_xlat0.xyz = float3(((u_xlat0.xyz * float3(1.05499995, 1.05499995, 1.05499995)) + float3(-0.0549999997, (-0.0549999997), (-0.0549999997))));
          out_v.texcoord5.xyz = float3(max(u_xlat0.xyz, float3(0, 0, 0)));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat0_d;
      float3 u_xlat10_0;
      float3 u_xlat1_d;
      float3 u_xlat10_1;
      float3 u_xlat16_2;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0.xyz = tex2D(_MainTex, in_f.texcoord.xy).xyz.xyz;
          u_xlat10_1.xyz = tex2D(_MatCap, in_f.texcoord2.xy).xyz.xyz;
          u_xlat16_2.xyz = float3((u_xlat10_0.xyz * u_xlat10_1.xyz));
          u_xlat0_d.xyz = float3((u_xlat16_2.xyz * _Color.xyz));
          u_xlat0_d.xyz = float3((u_xlat0_d.xyz + u_xlat0_d.xyz));
          u_xlat1_d.xy = float2((in_f.texcoord3.xy / in_f.texcoord3.ww));
          u_xlat10_1.xyz = tex2D(_LightBuffer, u_xlat1_d.xy).xyz.xyz;
          u_xlat16_2.xyz = float3(max(u_xlat10_1.xyz, float3(0.00100000005, 0.00100000005, 0.00100000005)));
          u_xlat16_2.xyz = float3(log2(u_xlat16_2.xyz));
          u_xlat1_d.xyz = float3(((-u_xlat16_2.xyz) + in_f.texcoord5.xyz));
          out_f.color.xyz = float3((u_xlat0_d.xyz * u_xlat1_d.xyz));
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
        "RenderType" = "Opaque"
      }
      LOD 200
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
      //uniform float4x4 unity_MatrixV;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      uniform float4 _Color;
      uniform sampler2D _MainTex;
      uniform sampler2D _MatCap;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float3 normal :NORMAL0;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
          float2 texcoord3 :TEXCOORD3;
          float3 texcoord1 :TEXCOORD1;
          float3 texcoord2 :TEXCOORD2;
          float4 texcoord4 :TEXCOORD4;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
          float2 texcoord3 :TEXCOORD3;
          float3 texcoord1 :TEXCOORD1;
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
      float3 u_xlat2;
      float u_xlat6;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0 = (in_v.vertex.yyyy * conv_mxt4x4_1(unity_ObjectToWorld));
          u_xlat0 = ((conv_mxt4x4_0(unity_ObjectToWorld) * in_v.vertex.xxxx) + u_xlat0);
          u_xlat0 = ((conv_mxt4x4_2(unity_ObjectToWorld) * in_v.vertex.zzzz) + u_xlat0);
          u_xlat1 = (u_xlat0 + conv_mxt4x4_3(unity_ObjectToWorld));
          out_v.texcoord2.xyz = float3(((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz));
          out_v.vertex = mul(unity_MatrixVP, u_xlat1);
          u_xlat0.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _MainTex));
          out_v.texcoord.xy = float2(u_xlat0.xy);
          u_xlat0.x = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat0.y = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat0.z = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          out_v.texcoord1.xyz = float3(normalize(u_xlat0.xyz));
          u_xlat0.x = conv_mxt4x4_0(unity_WorldToObject).y.x;
          u_xlat0.y = conv_mxt4x4_1(unity_WorldToObject).y.x;
          u_xlat0.z = conv_mxt4x4_2(unity_WorldToObject).y.x;
          u_xlat0.xyz = float3((u_xlat0.xyz * in_v.normal.yyy));
          u_xlat1.x = conv_mxt4x4_0(unity_WorldToObject).x;
          u_xlat1.y = conv_mxt4x4_1(unity_WorldToObject).x;
          u_xlat1.z = conv_mxt4x4_2(unity_WorldToObject).x;
          u_xlat0.xyz = float3(((u_xlat1.xyz * in_v.normal.xxx) + u_xlat0.xyz));
          u_xlat1.x = conv_mxt4x4_0(unity_WorldToObject).z.x;
          u_xlat1.y = conv_mxt4x4_1(unity_WorldToObject).z.x;
          u_xlat1.z = conv_mxt4x4_2(unity_WorldToObject).z.x;
          u_xlat0.xyz = float3(((u_xlat1.xyz * in_v.normal.zzz) + u_xlat0.xyz));
          u_xlat0.xyz = float3(normalize(u_xlat0.xyz));
          u_xlat2.xz = (u_xlat0.yy * conv_mxt4x4_1(unity_MatrixV).xy);
          u_xlat0.xy = ((conv_mxt4x4_0(unity_MatrixV).xy * u_xlat0.xx) + u_xlat2.xz).xy;
          u_xlat0.xy = ((conv_mxt4x4_2(unity_MatrixV).xy * u_xlat0.zz) + u_xlat0.xy).xy;
          out_v.texcoord3.xy = float2(((u_xlat0.xy * float2(0.5, 0.5)) + float2(0.5, 0.5)));
          out_v.texcoord4 = float4(0, 0, 0, 0);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat0_d;
      float3 u_xlat10_0;
      float3 u_xlat10_1;
      float3 u_xlat16_2;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0.xyz = tex2D(_MainTex, in_f.texcoord.xy).xyz.xyz;
          u_xlat10_1.xyz = tex2D(_MatCap, in_f.texcoord3.xy).xyz.xyz;
          u_xlat16_2.xyz = float3((u_xlat10_0.xyz * u_xlat10_1.xyz));
          u_xlat0_d.xyz = float3((u_xlat16_2.xyz * _Color.xyz));
          u_xlat0_d.xyz = float3((u_xlat0_d.xyz + u_xlat0_d.xyz));
          out_f.color.xyz = float3(u_xlat0_d.xyz);
          out_f.color.w = 1;
          out_f.color1 = float4(0, 0, 0, 0);
          u_xlat0_d.xyz = float3(((in_f.texcoord1.xyz * float3(0.5, 0.5, 0.5)) + float3(0.5, 0.5, 0.5)));
          u_xlat0_d.w = 1;
          out_f.color2 = u_xlat0_d;
          out_f.color3 = float4(1, 1, 1, 1);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "VertexLit"
}
