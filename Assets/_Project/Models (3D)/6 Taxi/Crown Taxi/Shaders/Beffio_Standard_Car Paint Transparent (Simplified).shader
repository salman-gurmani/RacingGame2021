Shader "Beffio/Standard/Car Paint Transparent (Simplified)"
{
  Properties
  {
    _BaseColor ("Base Color", Color) = (1,1,1,1)
    _DetailColor ("Detail Color", Color) = (1,1,1,1)
    _DetailMap ("Detail Map", 2D) = "white" {}
    _DetailMapDepthBias ("Detail Map Depth Bias", float) = 1
    _DiffuseColor ("Diffuse Color", Color) = (1,1,1,1)
    _DiffuseMap ("Diffuse Map", 2D) = "white" {}
    _NormalMap ("Normal Map", 2D) = "bump" {}
    _ReflectionColor ("Reflection Color", Color) = (1,1,1,1)
    _ReflectionMap ("Reflection Map", Cube) = "" {}
    _ReflectionStrength ("Reflection Strength", Range(0, 1)) = 0.5
  }
  SubShader
  {
    Tags
    { 
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "LIGHTMODE" = "FORWARDBASE"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      ZWrite Off
      Blend SrcAlpha OneMinusSrcAlpha
      ColorMask RGB
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
      uniform float4 _DetailMap_ST;
      uniform float4 _DiffuseMap_ST;
      uniform float4 _NormalMap_ST;
      //uniform float3 _WorldSpaceCameraPos;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform float4 _BaseColor;
      uniform float4 _DetailColor;
      uniform float _DetailMapDepthBias;
      uniform float4 _DiffuseColor;
      uniform float4 _ReflectionColor;
      uniform float _ReflectionStrength;
      uniform sampler2D _NormalMap;
      uniform samplerCUBE _ReflectionMap;
      uniform sampler2D _DiffuseMap;
      uniform sampler2D _DetailMap;
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
          float texcoord5 :TEXCOORD5;
          float4 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
          float4 texcoord4 :TEXCOORD4;
          float4 texcoord8 :TEXCOORD8;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
          float texcoord5 :TEXCOORD5;
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
      float u_xlat12;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0 = (in_v.vertex.yyyy * conv_mxt4x4_1(unity_ObjectToWorld));
          u_xlat0 = ((conv_mxt4x4_0(unity_ObjectToWorld) * in_v.vertex.xxxx) + u_xlat0);
          u_xlat0 = ((conv_mxt4x4_2(unity_ObjectToWorld) * in_v.vertex.zzzz) + u_xlat0);
          u_xlat1 = (u_xlat0 + conv_mxt4x4_3(unity_ObjectToWorld));
          u_xlat0.xyz = float3(((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz));
          out_v.vertex = mul(unity_MatrixVP, u_xlat1);
          out_v.texcoord.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _DetailMap));
          out_v.texcoord.zw = TRANSFORM_TEX(in_v.texcoord.xy, _DiffuseMap);
          u_xlat12 = (u_xlat1.y * conv_mxt4x4_1(unity_MatrixVP).z).x;
          u_xlat12 = ((conv_mxt4x4_0(unity_MatrixVP).z * u_xlat1.x) + u_xlat12).x;
          u_xlat12 = ((conv_mxt4x4_2(unity_MatrixVP).z * u_xlat1.z) + u_xlat12).x;
          out_v.texcoord5 = ((conv_mxt4x4_3(unity_MatrixVP).z * u_xlat1.w) + u_xlat12);
          out_v.texcoord1.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _NormalMap));
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
          out_v.texcoord8 = float4(0, 0, 0, 0);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat0_d;
      float3 u_xlat16_0;
      float4 u_xlat10_0;
      float3 u_xlat1_d;
      float3 u_xlat10_1;
      float3 u_xlat10_2;
      float3 u_xlat16_3;
      float3 u_xlat16_4;
      float u_xlat15;
      float u_xlat16_18;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2D(_DiffuseMap, in_f.texcoord.zw);
          u_xlat15 = (u_xlat10_0.w * _DiffuseColor.w);
          u_xlat0_d.xyz = float3(((u_xlat10_0.xyz * _DiffuseColor.xyz) + (-_BaseColor.xyz)));
          u_xlat0_d.xyz = float3(((float3(u_xlat15, u_xlat15, u_xlat15) * u_xlat0_d.xyz) + _BaseColor.xyz));
          u_xlat1_d.x = in_f.texcoord2.w;
          u_xlat1_d.y = in_f.texcoord3.w;
          u_xlat1_d.z = in_f.texcoord4.w;
          u_xlat1_d.xyz = float3(((-u_xlat1_d.xyz) + _WorldSpaceCameraPos.xyz));
          u_xlat1_d.xyz = float3(normalize(u_xlat1_d.xyz));
          u_xlat10_2.xyz = tex2D(_NormalMap, in_f.texcoord1.xy).xyz.xyz;
          u_xlat16_3.xyz = float3(((u_xlat10_2.xyz * float3(2, 2, 2)) + float3(-1, (-1), (-1))));
          u_xlat16_4.x = dot(in_f.texcoord2.xyz, u_xlat16_3.xyz);
          u_xlat16_4.y = dot(in_f.texcoord3.xyz, u_xlat16_3.xyz);
          u_xlat16_4.z = dot(in_f.texcoord4.xyz, u_xlat16_3.xyz);
          u_xlat15 = dot((-u_xlat1_d.xyz), u_xlat16_4.xyz);
          u_xlat15 = (u_xlat15 + u_xlat15);
          u_xlat1_d.xyz = float3(((u_xlat16_4.xyz * (-float3(u_xlat15, u_xlat15, u_xlat15))) + (-u_xlat1_d.xyz)));
          u_xlat10_1.xyz = float3(texCUBE(_ReflectionMap, u_xlat1_d.xyz).xyz);
          u_xlat1_d.xyz = float3(((u_xlat10_1.xyz * _ReflectionColor.xyz) + (-u_xlat0_d.xyz)));
          u_xlat0_d.xyz = float3(((float3(_ReflectionStrength, _ReflectionStrength, _ReflectionStrength) * u_xlat1_d.xyz) + u_xlat0_d.xyz));
          u_xlat1_d.xyz = float3((u_xlat0_d.xyz + (-_DetailColor.xyz)));
          u_xlat10_2.xyz = tex2D(_DetailMap, in_f.texcoord.xy).xyz.xyz;
          u_xlat1_d.xyz = float3(((u_xlat10_2.xyz * u_xlat1_d.xyz) + _DetailColor.xyz));
          u_xlat0_d.xyz = float3((u_xlat0_d.xyz + (-u_xlat1_d.xyz)));
          u_xlat15 = (in_f.texcoord5 * _DetailMapDepthBias);
          u_xlat15 = clamp(u_xlat15, 0, 1);
          u_xlat0_d.xyz = float3(((float3(u_xlat15, u_xlat15, u_xlat15) * u_xlat0_d.xyz) + u_xlat1_d.xyz));
          u_xlat16_3.xyz = float3((u_xlat0_d.xyz * _LightColor0.xyz));
          u_xlat16_0.x = dot(u_xlat16_4.xyz, u_xlat16_4.xyz);
          u_xlat16_0.x = rsqrt(u_xlat16_0.x);
          u_xlat16_0.xyz = float3((u_xlat16_0.xxx * u_xlat16_4.xyz));
          u_xlat16_18 = dot(u_xlat16_0.xyz, _WorldSpaceLightPos0.xyz);
          u_xlat16_18 = max(u_xlat16_18, 0);
          out_f.color.xyz = float3((float3(u_xlat16_18, u_xlat16_18, u_xlat16_18) * u_xlat16_3.xyz));
          out_f.color.w = _BaseColor.w;
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
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      ZWrite Off
      Blend SrcAlpha One
      ColorMask RGB
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
      uniform float4 _DetailMap_ST;
      uniform float4 _DiffuseMap_ST;
      uniform float4 _NormalMap_ST;
      //uniform float3 _WorldSpaceCameraPos;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform float4 _BaseColor;
      uniform float4 _DetailColor;
      uniform float _DetailMapDepthBias;
      uniform float4 _DiffuseColor;
      uniform float4 _ReflectionColor;
      uniform float _ReflectionStrength;
      uniform sampler2D _NormalMap;
      uniform samplerCUBE _ReflectionMap;
      uniform sampler2D _DiffuseMap;
      uniform sampler2D _DetailMap;
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
          float texcoord6 :TEXCOORD6;
          float3 texcoord2 :TEXCOORD2;
          float3 texcoord3 :TEXCOORD3;
          float3 texcoord4 :TEXCOORD4;
          float3 texcoord5 :TEXCOORD5;
          float3 texcoord7 :TEXCOORD7;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
          float texcoord6 :TEXCOORD6;
          float3 texcoord2 :TEXCOORD2;
          float3 texcoord3 :TEXCOORD3;
          float3 texcoord4 :TEXCOORD4;
          float3 texcoord5 :TEXCOORD5;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float4 u_xlat2;
      float3 u_xlat3;
      float u_xlat5;
      float u_xlat13;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat1 = mul(unity_ObjectToWorld, float4(in_v.vertex.xyz,1.0));
          out_v.vertex = mul(unity_MatrixVP, u_xlat1);
          out_v.texcoord.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _DetailMap));
          out_v.texcoord.zw = TRANSFORM_TEX(in_v.texcoord.xy, _DiffuseMap);
          u_xlat5 = (u_xlat1.y * conv_mxt4x4_1(unity_MatrixVP).z).x;
          u_xlat1.x = ((conv_mxt4x4_0(unity_MatrixVP).z * u_xlat1.x) + u_xlat5).x;
          u_xlat1.x = ((conv_mxt4x4_2(unity_MatrixVP).z * u_xlat1.z) + u_xlat1.x).x;
          out_v.texcoord6 = ((conv_mxt4x4_3(unity_MatrixVP).z * u_xlat1.w) + u_xlat1.x);
          out_v.texcoord1.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _NormalMap));
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
          u_xlat13 = (in_v.tangent.w * unity_WorldTransformParams.w);
          u_xlat3.xyz = float3((float3(u_xlat13, u_xlat13, u_xlat13) * u_xlat3.xyz));
          out_v.texcoord2.y = u_xlat3.x;
          out_v.texcoord2.x = u_xlat2.z;
          out_v.texcoord2.z = u_xlat1.y;
          out_v.texcoord3.x = u_xlat2.x;
          out_v.texcoord4.x = u_xlat2.y;
          out_v.texcoord3.z = u_xlat1.z;
          out_v.texcoord4.z = u_xlat1.x;
          out_v.texcoord3.y = u_xlat3.y;
          out_v.texcoord4.y = u_xlat3.z;
          out_v.texcoord5.xyz = float3(((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz));
          u_xlat0 = ((conv_mxt4x4_3(unity_ObjectToWorld) * in_v.vertex.wwww) + u_xlat0);
          u_xlat1.xyz = float3((u_xlat0.yyy * conv_mxt4x4_1(unity_WorldToLight).xyz));
          u_xlat1.xyz = float3(((conv_mxt4x4_0(unity_WorldToLight).xyz * u_xlat0.xxx) + u_xlat1.xyz));
          u_xlat0.xyz = float3(((conv_mxt4x4_2(unity_WorldToLight).xyz * u_xlat0.zzz) + u_xlat1.xyz));
          out_v.texcoord7.xyz = float3(((conv_mxt4x4_3(unity_WorldToLight).xyz * u_xlat0.www) + u_xlat0.xyz));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat0_d;
      float3 u_xlat16_0;
      float4 u_xlat10_0;
      float3 u_xlat16_1;
      float3 u_xlat2_d;
      float3 u_xlat10_2;
      float3 u_xlat10_3;
      float3 u_xlat16_4;
      float3 u_xlat16_5;
      float u_xlat18;
      float u_xlat16_19;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.xyz = float3((in_f.texcoord5.yyy * conv_mxt4x4_1(unity_WorldToLight).xyz));
          u_xlat0_d.xyz = float3(((conv_mxt4x4_0(unity_WorldToLight).xyz * in_f.texcoord5.xxx) + u_xlat0_d.xyz));
          u_xlat0_d.xyz = float3(((conv_mxt4x4_2(unity_WorldToLight).xyz * in_f.texcoord5.zzz) + u_xlat0_d.xyz));
          u_xlat0_d.xyz = float3((u_xlat0_d.xyz + conv_mxt4x4_3(unity_WorldToLight).xyz));
          u_xlat0_d.x = dot(u_xlat0_d.xyz, u_xlat0_d.xyz);
          u_xlat0_d.x = tex2D(_LightTexture0, u_xlat0_d.xx).x;
          u_xlat16_1.xyz = float3((u_xlat0_d.xxx * _LightColor0.xyz));
          u_xlat10_0 = tex2D(_DiffuseMap, in_f.texcoord.zw);
          u_xlat18 = (u_xlat10_0.w * _DiffuseColor.w);
          u_xlat0_d.xyz = float3(((u_xlat10_0.xyz * _DiffuseColor.xyz) + (-_BaseColor.xyz)));
          u_xlat0_d.xyz = float3(((float3(u_xlat18, u_xlat18, u_xlat18) * u_xlat0_d.xyz) + _BaseColor.xyz));
          u_xlat2_d.xyz = float3(((-in_f.texcoord5.xyz) + _WorldSpaceCameraPos.xyz));
          u_xlat2_d.xyz = float3(normalize(u_xlat2_d.xyz));
          u_xlat10_3.xyz = tex2D(_NormalMap, in_f.texcoord1.xy).xyz.xyz;
          u_xlat16_4.xyz = float3(((u_xlat10_3.xyz * float3(2, 2, 2)) + float3(-1, (-1), (-1))));
          u_xlat16_5.x = dot(in_f.texcoord2.xyz, u_xlat16_4.xyz);
          u_xlat16_5.y = dot(in_f.texcoord3.xyz, u_xlat16_4.xyz);
          u_xlat16_5.z = dot(in_f.texcoord4.xyz, u_xlat16_4.xyz);
          u_xlat18 = dot((-u_xlat2_d.xyz), u_xlat16_5.xyz);
          u_xlat18 = (u_xlat18 + u_xlat18);
          u_xlat2_d.xyz = float3(((u_xlat16_5.xyz * (-float3(u_xlat18, u_xlat18, u_xlat18))) + (-u_xlat2_d.xyz)));
          u_xlat10_2.xyz = float3(texCUBE(_ReflectionMap, u_xlat2_d.xyz).xyz);
          u_xlat2_d.xyz = float3(((u_xlat10_2.xyz * _ReflectionColor.xyz) + (-u_xlat0_d.xyz)));
          u_xlat0_d.xyz = float3(((float3(_ReflectionStrength, _ReflectionStrength, _ReflectionStrength) * u_xlat2_d.xyz) + u_xlat0_d.xyz));
          u_xlat2_d.xyz = float3((u_xlat0_d.xyz + (-_DetailColor.xyz)));
          u_xlat10_3.xyz = tex2D(_DetailMap, in_f.texcoord.xy).xyz.xyz;
          u_xlat2_d.xyz = float3(((u_xlat10_3.xyz * u_xlat2_d.xyz) + _DetailColor.xyz));
          u_xlat0_d.xyz = float3((u_xlat0_d.xyz + (-u_xlat2_d.xyz)));
          u_xlat18 = (in_f.texcoord6 * _DetailMapDepthBias);
          u_xlat18 = clamp(u_xlat18, 0, 1);
          u_xlat0_d.xyz = float3(((float3(u_xlat18, u_xlat18, u_xlat18) * u_xlat0_d.xyz) + u_xlat2_d.xyz));
          u_xlat16_1.xyz = float3((u_xlat16_1.xyz * u_xlat0_d.xyz));
          u_xlat16_0.x = dot(u_xlat16_5.xyz, u_xlat16_5.xyz);
          u_xlat16_0.x = rsqrt(u_xlat16_0.x);
          u_xlat16_0.xyz = float3((u_xlat16_0.xxx * u_xlat16_5.xyz));
          u_xlat2_d.xyz = float3(((-in_f.texcoord5.xyz) + _WorldSpaceLightPos0.xyz));
          u_xlat2_d.xyz = float3(normalize(u_xlat2_d.xyz));
          u_xlat16_19 = dot(u_xlat16_0.xyz, u_xlat2_d.xyz);
          u_xlat16_19 = max(u_xlat16_19, 0);
          out_f.color.xyz = float3((float3(u_xlat16_19, u_xlat16_19, u_xlat16_19) * u_xlat16_1.xyz));
          out_f.color.w = _BaseColor.w;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "VertexLit"
}
