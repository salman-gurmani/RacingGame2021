Shader "PIDI Shaders Collection/Planar Reflection/Generic/Simple"
{
  Properties
  {
    [Space(12)] [Header(PBR Properties)] _Color ("Color", Color) = (1,1,1,1)
    _SpecColor ("Specular Color", Color) = (0.1,0.1,0.1,1)
    _MainTex ("Main Tex", 2D) = "white" {}
    _BumpMap ("Normal map", 2D) = "bump" {}
    _GSOHMap ("Gloss(R) Specular (G) Occlusion (B) Heightmap (A)", 2D) = "white" {}
    [Space(8)] _Parallax ("Parallax Height", Range(0, 0.1)) = 0
    _Glossiness ("Smoothness", Range(0, 1)) = 0
    [Space(12)] [Header(Reflection Properties)] _ReflectionTint ("Reflection Tint", Color) = (1,1,1,1)
    _RefDistortion ("Bump Reflection Distortion", Range(0, 0.1)) = 0.25
    _NormalDist ("Surface Distortion", Range(0, 1)) = 0
    _ReflectionTex ("Reflection Texture", 2D) = "white" {}
    [HideInInspector] _ReflectionTexRight ("Reflection Texture", 2D) = "white" {}
    [Space(12)] [Header(Material Emission)] [Enum(Additive,0,Masked,1)] _EmissionMode ("Emission Mode", float) = 0
    [Enum(Disabled,2,Enabled,50,Overbright,100)] _HDROverride ("HDR Mode", float) = 2
    [Space(8)] _EmissionColor ("Emission Color (RGB) Intensity (2*Alpha)", Color) = (0,0,0,0.5)
    _EmissionMap ("Emission Map (RGB) Mask (A)", 2D) = "white" {}
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
      //uniform float4 _ProjectionParams;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4 unity_WorldTransformParams;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      uniform float4 _BumpMap_ST;
      uniform float4 _GSOHMap_ST;
      uniform float4 _EmissionMap_ST;
      //uniform float3 _WorldSpaceCameraPos;
      //uniform float4 _WorldSpaceLightPos0;
      //uniform float4 unity_SpecCube0_HDR;
      uniform float4 _LightColor0;
      uniform float4 _SpecColor;
      uniform float _NormalDist;
      uniform float4 _ReflectionTint;
      uniform float4 _Color;
      uniform float4 _EmissionColor;
      uniform float _Glossiness;
      uniform float _RefDistortion;
      uniform float _Parallax;
      uniform float _EmissionMode;
      uniform float _HDROverride;
      uniform sampler2D _GSOHMap;
      uniform sampler2D _BumpMap;
      uniform sampler2D _MainTex;
      uniform sampler2D _ReflectionTex;
      uniform sampler2D _EmissionMap;
      uniform sampler2D unity_NHxRoughness;
      //uniform samplerCUBE unity_SpecCube0;
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
          float4 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
          float4 texcoord4 :TEXCOORD4;
          float4 texcoord5 :TEXCOORD5;
          float4 texcoord8 :TEXCOORD8;
          float4 texcoord9 :TEXCOORD9;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
          float4 texcoord4 :TEXCOORD4;
          float4 texcoord5 :TEXCOORD5;
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
          out_v.texcoord.zw = TRANSFORM_TEX(in_v.texcoord.xy, _BumpMap);
          out_v.texcoord1.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _GSOHMap));
          out_v.texcoord1.zw = TRANSFORM_TEX(in_v.texcoord.xy, _EmissionMap);
          out_v.texcoord2.w = u_xlat0.x;
          u_xlat2.y = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat2.z = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat2.x = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          u_xlat0.x = dot(u_xlat2.xyz, u_xlat2.xyz);
          u_xlat0.x = rsqrt(u_xlat0.x);
          u_xlat2.xyz = float3((u_xlat0.xxx * u_xlat2.xyz));
          u_xlat3.xyz = float3((in_v.tangent.yyy * conv_mxt4x4_1(unity_ObjectToWorld).yzx));
          u_xlat3.xyz = float3(((conv_mxt4x4_0(unity_ObjectToWorld).yzx * in_v.tangent.xxx) + u_xlat3.xyz));
          u_xlat3.xyz = float3(((conv_mxt4x4_2(unity_ObjectToWorld).yzx * in_v.tangent.zzz) + u_xlat3.xyz));
          u_xlat0.x = dot(u_xlat3.xyz, u_xlat3.xyz);
          u_xlat0.x = rsqrt(u_xlat0.x);
          u_xlat3.xyz = float3((u_xlat0.xxx * u_xlat3.xyz));
          u_xlat4.xyz = float3((u_xlat2.xyz * u_xlat3.xyz));
          u_xlat4.xyz = float3(((u_xlat2.zxy * u_xlat3.yzx) + (-u_xlat4.xyz)));
          u_xlat0.x = (in_v.tangent.w * unity_WorldTransformParams.w);
          u_xlat4.xyz = float3((u_xlat0.xxx * u_xlat4.xyz));
          out_v.texcoord2.y = u_xlat4.x;
          out_v.texcoord2.x = u_xlat3.z;
          out_v.texcoord2.z = u_xlat2.y;
          out_v.texcoord3.x = u_xlat3.x;
          out_v.texcoord4.x = u_xlat3.y;
          out_v.texcoord3.z = u_xlat2.z;
          out_v.texcoord4.z = u_xlat2.x;
          out_v.texcoord3.w = u_xlat0.y;
          out_v.texcoord4.w = u_xlat0.z;
          out_v.texcoord3.y = u_xlat4.y;
          out_v.texcoord4.y = u_xlat4.z;
          u_xlat0.x = (u_xlat1.y * _ProjectionParams.x);
          u_xlat0.w = (u_xlat0.x * 0.5);
          u_xlat0.xz = (u_xlat1.xw * float2(0.5, 0.5));
          out_v.texcoord5.zw = u_xlat1.zw;
          out_v.texcoord5.xy = float2((u_xlat0.zz + u_xlat0.xw));
          out_v.texcoord8 = float4(0, 0, 0, 0);
          out_v.texcoord9 = float4(0, 0, 0, 0);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat0_d;
      float u_xlat16_0;
      float3 u_xlat10_0;
      int u_xlati0;
      float3 u_xlat1_d;
      float4 u_xlat10_1;
      float4 u_xlat2_d;
      float u_xlat16_2;
      float4 u_xlat10_2;
      float3 u_xlat10_3;
      float3 u_xlat16_4;
      float3 u_xlat5;
      float3 u_xlat10_5;
      float3 u_xlat6;
      float3 u_xlat10_6;
      float4 u_xlat16_7;
      float3 u_xlat16_8;
      float3 u_xlat16_9;
      float3 u_xlat16_10;
      float3 u_xlat16_11;
      float3 u_xlat16_12;
      float2 u_xlat13;
      float2 u_xlat16_15;
      int u_xlati26;
      float u_xlat16_33;
      float u_xlat39;
      float u_xlat40;
      float u_xlat16_43;
      float u_xlat16_46;
      int op_shl(int a, int b)
      {
          return int(floor((float(a) * pow(2, float(b)))));
      }
      
      int2 op_shl(int2 a, int2 b)
      {
          a.x = op_shl(a.x, b.x);
          a.y = op_shl(a.y, b.y);
          return a;
      }
      
      int3 op_shl(int3 a, int3 b)
      {
          a.x = op_shl(a.x, b.x);
          a.y = op_shl(a.y, b.y);
          a.z = op_shl(a.z, b.z);
          return a;
      }
      
      int4 op_shl(int4 a, int4 b)
      {
          a.x = op_shl(a.x, b.x);
          a.y = op_shl(a.y, b.y);
          a.z = op_shl(a.z, b.z);
          a.w = op_shl(a.w, b.w);
          return a;
      }
      
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = in_f.texcoord2.w;
          u_xlat0_d.y = in_f.texcoord3.w;
          u_xlat0_d.z = in_f.texcoord4.w;
          u_xlat0_d.xyz = float3(((-u_xlat0_d.xyz) + _WorldSpaceCameraPos.xyz));
          u_xlat0_d.xyz = float3(normalize(u_xlat0_d.xyz));
          u_xlat1_d.xyz = float3((u_xlat0_d.yyy * in_f.texcoord3.xyz));
          u_xlat1_d.xyz = float3(((in_f.texcoord2.xyz * u_xlat0_d.xxx) + u_xlat1_d.xyz));
          u_xlat1_d.xyz = float3(((in_f.texcoord4.xyz * u_xlat0_d.zzz) + u_xlat1_d.xyz));
          u_xlat16_2 = dot(u_xlat1_d.xyz, u_xlat1_d.xyz);
          u_xlat16_2 = rsqrt(u_xlat16_2);
          u_xlat16_15.xy = float2((u_xlat1_d.xy * float2(u_xlat16_2, u_xlat16_2)));
          u_xlat39 = ((u_xlat1_d.z * u_xlat16_2) + 0.419999987);
          u_xlat2_d = (u_xlat16_15.xyxy / float4(u_xlat39, u_xlat39, u_xlat39, u_xlat39));
          u_xlat10_3.xyz = tex2D(_GSOHMap, in_f.texcoord1.xy).yzw.xyz;
          u_xlat16_4.x = (u_xlat10_3.z * _Parallax);
          u_xlat16_4.x = (((-_Parallax) * 0.5) + u_xlat16_4.x);
          u_xlat2_d = ((u_xlat16_4.xxxx * u_xlat2_d) + in_f.texcoord.zwxy);
          u_xlat10_5.xyz = tex2D(_BumpMap, u_xlat2_d.xy).xyz.xyz;
          u_xlat10_6.xyz = tex2D(_MainTex, u_xlat2_d.zw).xyz.xyz;
          u_xlat16_4.xyz = float3((u_xlat10_6.xyz * _Color.xyz));
          u_xlat16_7.xyz = float3(((u_xlat10_5.xyz * float3(2, 2, 2)) + float3(-1, (-1), (-1))));
          u_xlat5.x = dot(in_f.texcoord2.xyz, u_xlat16_7.xyz);
          u_xlat5.y = dot(in_f.texcoord3.xyz, u_xlat16_7.xyz);
          u_xlat5.z = dot(in_f.texcoord4.xyz, u_xlat16_7.xyz);
          u_xlat5.xyz = float3(normalize(u_xlat5.xyz));
          u_xlat16_43 = dot((-u_xlat0_d.xyz), u_xlat5.xyz);
          u_xlat16_43 = (u_xlat16_43 + u_xlat16_43);
          u_xlat16_8.xyz = float3(((u_xlat5.xyz * (-float3(u_xlat16_43, u_xlat16_43, u_xlat16_43))) + (-u_xlat0_d.xyz)));
          u_xlat6.z = (((-u_xlat10_3.x) * _Glossiness) + 1);
          u_xlat16_43 = (((-u_xlat6.z) * 0.699999988) + 1.70000005);
          u_xlat16_43 = (u_xlat16_43 * u_xlat6.z);
          u_xlat16_43 = (u_xlat16_43 * 6);
          u_xlat10_2 = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, float4(u_xlat16_8.xyz, u_xlat16_43));
          u_xlat16_43 = (u_xlat10_2.w + (-1));
          u_xlat16_43 = ((unity_SpecCube0_HDR.w * u_xlat16_43) + 1);
          u_xlat16_43 = (u_xlat16_43 * unity_SpecCube0_HDR.x);
          u_xlat16_8.xyz = float3((u_xlat10_2.xyz * float3(u_xlat16_43, u_xlat16_43, u_xlat16_43)));
          u_xlat16_8.xyz = float3((u_xlat10_3.yyy * u_xlat16_8.xyz));
          u_xlat16_9.xyz = float3((u_xlat10_3.xxx * _SpecColor.xyz));
          u_xlat16_43 = max(u_xlat16_9.y, u_xlat16_9.x);
          u_xlat16_43 = max(u_xlat16_9.z, u_xlat16_43);
          u_xlat16_43 = ((-u_xlat16_43) + 1);
          u_xlat16_46 = ((u_xlat10_3.x * _Glossiness) + (-u_xlat16_43));
          u_xlat16_46 = (u_xlat16_46 + 1);
          u_xlat16_46 = clamp(u_xlat16_46, 0, 1);
          u_xlat16_10.xyz = float3((((-_SpecColor.xyz) * u_xlat10_3.xxx) + float3(u_xlat16_46, u_xlat16_46, u_xlat16_46)));
          u_xlat39 = dot(u_xlat0_d.xyz, u_xlat5.xyz);
          u_xlat40 = u_xlat39;
          u_xlat40 = clamp(u_xlat40, 0, 1);
          u_xlat39 = (u_xlat39 + u_xlat39);
          u_xlat0_d.xyz = float3(((u_xlat5.xyz * (-float3(u_xlat39, u_xlat39, u_xlat39))) + u_xlat0_d.xyz));
          u_xlat39 = dot(u_xlat5.xyz, _WorldSpaceLightPos0.xyz);
          u_xlat39 = clamp(u_xlat39, 0, 1);
          u_xlat16_11.xyz = float3((float3(u_xlat39, u_xlat39, u_xlat39) * _LightColor0.xyz));
          u_xlat0_d.x = dot(u_xlat0_d.xyz, _WorldSpaceLightPos0.xyz);
          u_xlat0_d.x = (u_xlat0_d.x * u_xlat0_d.x);
          u_xlat6.x = (u_xlat0_d.x * u_xlat0_d.x);
          u_xlat0_d.x = tex2D(unity_NHxRoughness, u_xlat6.xz).x;
          u_xlat0_d.x = (u_xlat0_d.x * 16);
          u_xlat16_12.xyz = float3((u_xlat16_9.xyz * u_xlat0_d.xxx));
          u_xlat16_12.xyz = float3(((u_xlat16_4.xyz * float3(u_xlat16_43, u_xlat16_43, u_xlat16_43)) + u_xlat16_12.xyz));
          u_xlat16_4.xyz = float3(max(u_xlat16_4.xyz, float3(0.100000001, 0.100000001, 0.100000001)));
          u_xlat16_43 = ((-u_xlat40) + 1);
          u_xlat16_0 = (u_xlat16_43 * u_xlat16_43);
          u_xlat16_0 = (u_xlat16_43 * u_xlat16_0);
          u_xlat16_0 = (u_xlat16_43 * u_xlat16_0);
          u_xlat16_9.xyz = float3(((float3(u_xlat16_0, u_xlat16_0, u_xlat16_0) * u_xlat16_10.xyz) + u_xlat16_9.xyz));
          u_xlat16_8.xyz = float3((u_xlat16_8.xyz * u_xlat16_9.xyz));
          u_xlat16_8.xyz = float3(((u_xlat16_12.xyz * u_xlat16_11.xyz) + u_xlat16_8.xyz));
          u_xlat0_d.x = dot(u_xlat16_7.xyz, u_xlat1_d.xyz);
          u_xlat0_d.x = clamp(u_xlat0_d.x, 0, 1);
          u_xlat13.x = (u_xlat1_d.z + (-0.5));
          u_xlat16_43 = ((-u_xlat0_d.x) + 1);
          u_xlat16_33 = (_ReflectionTint.w * 0.5);
          u_xlat16_46 = (((-_ReflectionTint.w) * 0.5) + 1);
          u_xlat16_33 = ((u_xlat16_43 * u_xlat16_46) + u_xlat16_33);
          if(int(((0<u_xlat13.x))?((-1)):(0)))
          {
              u_xlati0 = 1;
          }
          else
          {
              u_xlati0 = 0;
          }
          if(int(((u_xlat13.x<0))?((-1)):(0)))
          {
              u_xlati26 = 1;
          }
          else
          {
              u_xlati26 = 0;
          }
          u_xlati0 = ((-u_xlati0) + u_xlati26);
          u_xlati0 = op_shl(u_xlati0, 1);
          u_xlat0_d.x = float(u_xlati0);
          u_xlat0_d.x = (u_xlat13.x * u_xlat0_d.x);
          u_xlat13.x = max(in_f.texcoord5.w, 0.00100000005);
          u_xlat13.xy = float2((in_f.texcoord5.xy / u_xlat13.xx));
          u_xlat0_d.xy = float2((((-u_xlat0_d.xx) * float2(_NormalDist, _NormalDist)) + u_xlat13.xy));
          u_xlat0_d.xy = float2(((u_xlat16_7.xy * float2(float2(_RefDistortion, _RefDistortion))) + u_xlat0_d.xy));
          u_xlat10_0.xyz = tex2D(_ReflectionTex, u_xlat0_d.xy).xyz.xyz;
          u_xlat10_1 = tex2D(_EmissionMap, in_f.texcoord1.zw);
          u_xlat16_7.x = ((_EmissionMode * (-u_xlat10_1.w)) + 1);
          u_xlat16_7.xyw = (u_xlat10_0.xyz * u_xlat16_7.xxx);
          u_xlat16_7.xyw = (u_xlat16_7.xyw * _ReflectionTint.xyz);
          u_xlat16_7.xyz = float3((float3(u_xlat16_33, u_xlat16_33, u_xlat16_33) * u_xlat16_7.xyw));
          u_xlat16_9.xyz = float3(((-u_xlat16_4.xyz) + float3(1, 1, 1)));
          u_xlat16_4.xyz = float3(((float3(u_xlat16_43, u_xlat16_43, u_xlat16_43) * u_xlat16_9.xyz) + u_xlat16_4.xyz));
          u_xlat16_4.xyz = float3((u_xlat16_4.xyz * u_xlat16_7.xyz));
          u_xlat16_7.xyz = float3((u_xlat10_1.www * _EmissionColor.xyz));
          u_xlat16_43 = (_EmissionColor.w * _HDROverride);
          u_xlat16_7.xyz = float3((float3(u_xlat16_43, u_xlat16_43, u_xlat16_43) * u_xlat16_7.xyz));
          u_xlat16_4.xyz = float3(((u_xlat10_1.xyz * u_xlat16_7.xyz) + u_xlat16_4.xyz));
          out_f.color.xyz = float3((u_xlat16_4.xyz + u_xlat16_8.xyz));
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
        "SHADOWSUPPORT" = "true"
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
      //uniform float4 unity_WorldTransformParams;
      //uniform float4x4 unity_MatrixVP;
      uniform float4x4 unity_WorldToLight;
      uniform float4 _MainTex_ST;
      uniform float4 _BumpMap_ST;
      uniform float4 _GSOHMap_ST;
      //uniform float3 _WorldSpaceCameraPos;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform float4 _SpecColor;
      uniform float4 _Color;
      uniform float _Glossiness;
      uniform float _Parallax;
      uniform sampler2D _GSOHMap;
      uniform sampler2D _BumpMap;
      uniform sampler2D _MainTex;
      uniform sampler2D _LightTexture0;
      uniform sampler2D unity_NHxRoughness;
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
          float3 texcoord2 :TEXCOORD2;
          float3 texcoord3 :TEXCOORD3;
          float3 texcoord4 :TEXCOORD4;
          float3 texcoord5 :TEXCOORD5;
          float3 texcoord6 :TEXCOORD6;
          float4 texcoord7 :TEXCOORD7;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
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
      float u_xlat13;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          out_v.texcoord.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _MainTex));
          out_v.texcoord.zw = TRANSFORM_TEX(in_v.texcoord.xy, _BumpMap);
          out_v.texcoord1.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _GSOHMap));
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
          out_v.texcoord6.xyz = float3(((conv_mxt4x4_3(unity_WorldToLight).xyz * u_xlat0.www) + u_xlat0.xyz));
          out_v.texcoord7 = float4(0, 0, 0, 0);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat0_d;
      float4 u_xlat1_d;
      float3 u_xlat10_1;
      float3 u_xlat16_2;
      float2 u_xlat10_3;
      float3 u_xlat16_4;
      float3 u_xlat16_5;
      float u_xlat6;
      float2 u_xlat16_8;
      float3 u_xlat9;
      float3 u_xlat10_9;
      float u_xlat18;
      float u_xlat16_20;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.xyz = float3(((-in_f.texcoord5.xyz) + _WorldSpaceCameraPos.xyz));
          u_xlat0_d.xyz = float3(normalize(u_xlat0_d.xyz));
          u_xlat1_d.xyz = float3((u_xlat0_d.yyy * in_f.texcoord3.xyz));
          u_xlat1_d.xyz = float3(((in_f.texcoord2.xyz * u_xlat0_d.xxx) + u_xlat1_d.xyz));
          u_xlat1_d.xyz = float3(((in_f.texcoord4.xyz * u_xlat0_d.zzz) + u_xlat1_d.xyz));
          u_xlat16_2.x = dot(u_xlat1_d.xyz, u_xlat1_d.xyz);
          u_xlat16_2.x = rsqrt(u_xlat16_2.x);
          u_xlat16_8.xy = float2((u_xlat1_d.xy * u_xlat16_2.xx));
          u_xlat18 = ((u_xlat1_d.z * u_xlat16_2.x) + 0.419999987);
          u_xlat1_d = (u_xlat16_8.xyxy / float4(u_xlat18, u_xlat18, u_xlat18, u_xlat18));
          u_xlat10_3.xy = tex2D(_GSOHMap, in_f.texcoord1.xy).yw.xy;
          u_xlat16_2.x = (u_xlat10_3.y * _Parallax);
          u_xlat16_2.x = (((-_Parallax) * 0.5) + u_xlat16_2.x);
          u_xlat1_d = ((u_xlat16_2.xxxx * u_xlat1_d) + in_f.texcoord.zwxy);
          u_xlat10_9.xyz = tex2D(_BumpMap, u_xlat1_d.xy).xyz.xyz;
          u_xlat10_1.xyz = tex2D(_MainTex, u_xlat1_d.zw).xyz.xyz;
          u_xlat16_2.xyz = float3((u_xlat10_1.xyz * _Color.xyz));
          u_xlat16_4.xyz = float3(((u_xlat10_9.xyz * float3(2, 2, 2)) + float3(-1, (-1), (-1))));
          u_xlat1_d.x = dot(in_f.texcoord2.xyz, u_xlat16_4.xyz);
          u_xlat1_d.y = dot(in_f.texcoord3.xyz, u_xlat16_4.xyz);
          u_xlat1_d.z = dot(in_f.texcoord4.xyz, u_xlat16_4.xyz);
          u_xlat1_d.xyz = float3(normalize(u_xlat1_d.xyz));
          u_xlat18 = dot(u_xlat0_d.xyz, u_xlat1_d.xyz);
          u_xlat18 = (u_xlat18 + u_xlat18);
          u_xlat0_d.xyz = float3(((u_xlat1_d.xyz * (-float3(u_xlat18, u_xlat18, u_xlat18))) + u_xlat0_d.xyz));
          u_xlat9.xyz = float3(((-in_f.texcoord5.xyz) + _WorldSpaceLightPos0.xyz));
          u_xlat9.xyz = float3(normalize(u_xlat9.xyz));
          u_xlat0_d.x = dot(u_xlat0_d.xyz, u_xlat9.xyz);
          u_xlat6 = dot(u_xlat1_d.xyz, u_xlat9.xyz);
          u_xlat6 = clamp(u_xlat6, 0, 1);
          u_xlat0_d.x = (u_xlat0_d.x * u_xlat0_d.x);
          u_xlat1_d.x = (u_xlat0_d.x * u_xlat0_d.x);
          u_xlat1_d.y = (((-u_xlat10_3.x) * _Glossiness) + 1);
          u_xlat16_4.xyz = float3((u_xlat10_3.xxx * _SpecColor.xyz));
          u_xlat0_d.x = tex2D(unity_NHxRoughness, u_xlat1_d.xy).x;
          u_xlat0_d.x = (u_xlat0_d.x * 16);
          u_xlat16_5.xyz = float3((u_xlat16_4.xyz * u_xlat0_d.xxx));
          u_xlat16_20 = max(u_xlat16_4.y, u_xlat16_4.x);
          u_xlat16_20 = max(u_xlat16_4.z, u_xlat16_20);
          u_xlat16_20 = ((-u_xlat16_20) + 1);
          u_xlat16_2.xyz = float3(((u_xlat16_2.xyz * float3(u_xlat16_20, u_xlat16_20, u_xlat16_20)) + u_xlat16_5.xyz));
          u_xlat0_d.xzw = (in_f.texcoord5.yyy * conv_mxt4x4_1(unity_WorldToLight).xyz);
          u_xlat0_d.xzw = ((conv_mxt4x4_0(unity_WorldToLight).xyz * in_f.texcoord5.xxx) + u_xlat0_d.xzw);
          u_xlat0_d.xzw = ((conv_mxt4x4_2(unity_WorldToLight).xyz * in_f.texcoord5.zzz) + u_xlat0_d.xzw);
          u_xlat0_d.xzw = (u_xlat0_d.xzw + conv_mxt4x4_3(unity_WorldToLight).xyz);
          u_xlat0_d.x = dot(u_xlat0_d.xzw, u_xlat0_d.xzw);
          u_xlat0_d.x = tex2D(_LightTexture0, u_xlat0_d.xx).x;
          u_xlat16_4.xyz = float3((u_xlat0_d.xxx * _LightColor0.xyz));
          u_xlat16_4.xyz = float3((float3(u_xlat6, u_xlat6, u_xlat6) * u_xlat16_4.xyz));
          out_f.color.xyz = float3((u_xlat16_2.xyz * u_xlat16_4.xyz));
          out_f.color.w = 1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 3, name: DEFERRED
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
      //uniform float3 _WorldSpaceCameraPos;
      //uniform float4 _ProjectionParams;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4 unity_WorldTransformParams;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      uniform float4 _BumpMap_ST;
      uniform float4 _GSOHMap_ST;
      uniform float4 _EmissionMap_ST;
      uniform float4 _SpecColor;
      uniform float _NormalDist;
      uniform float4 _ReflectionTint;
      uniform float4 _Color;
      uniform float4 _EmissionColor;
      uniform float _Glossiness;
      uniform float _RefDistortion;
      uniform float _Parallax;
      uniform float _EmissionMode;
      uniform float _HDROverride;
      uniform sampler2D _GSOHMap;
      uniform sampler2D _BumpMap;
      uniform sampler2D _MainTex;
      uniform sampler2D _ReflectionTex;
      uniform sampler2D _EmissionMap;
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
          float4 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
          float4 texcoord4 :TEXCOORD4;
          float4 texcoord5 :TEXCOORD5;
          float3 texcoord6 :TEXCOORD6;
          float4 texcoord7 :TEXCOORD7;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
          float4 texcoord4 :TEXCOORD4;
          float4 texcoord5 :TEXCOORD5;
          float3 texcoord6 :TEXCOORD6;
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
      float3 u_xlat4;
      float4 u_xlat5;
      float u_xlat18;
      float u_xlat20;
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
          out_v.texcoord.zw = TRANSFORM_TEX(in_v.texcoord.xy, _BumpMap);
          out_v.texcoord1.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _GSOHMap));
          out_v.texcoord1.zw = TRANSFORM_TEX(in_v.texcoord.xy, _EmissionMap);
          out_v.texcoord2.w = u_xlat0.x;
          u_xlat18 = (in_v.tangent.w * unity_WorldTransformParams.w);
          u_xlat2.x = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat2.y = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat2.z = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          u_xlat2.xyz = float3(normalize(u_xlat2.xyz));
          u_xlat3.xyz = float3((in_v.tangent.yyy * conv_mxt4x4_1(unity_ObjectToWorld).xyz));
          u_xlat3.xyz = float3(((conv_mxt4x4_0(unity_ObjectToWorld).xyz * in_v.tangent.xxx) + u_xlat3.xyz));
          u_xlat3.xyz = float3(((conv_mxt4x4_2(unity_ObjectToWorld).xyz * in_v.tangent.zzz) + u_xlat3.xyz));
          u_xlat3.xyz = float3(normalize(u_xlat3.xyz));
          u_xlat4.xyz = float3((u_xlat2.zxy * u_xlat3.yzx));
          u_xlat4.xyz = float3(((u_xlat2.yzx * u_xlat3.zxy) + (-u_xlat4.xyz)));
          u_xlat4.xyz = float3((float3(u_xlat18, u_xlat18, u_xlat18) * u_xlat4.xyz));
          out_v.texcoord2.y = u_xlat4.x;
          out_v.texcoord2.x = u_xlat3.x;
          out_v.texcoord2.z = u_xlat2.x;
          out_v.texcoord3.x = u_xlat3.y;
          out_v.texcoord3.z = u_xlat2.y;
          out_v.texcoord3.w = u_xlat0.y;
          out_v.texcoord3.y = u_xlat4.y;
          out_v.texcoord4.x = u_xlat3.z;
          out_v.texcoord4.z = u_xlat2.z;
          out_v.texcoord4.w = u_xlat0.z;
          u_xlat0.xyz = float3(((-u_xlat0.xyz) + _WorldSpaceCameraPos.xyz));
          out_v.texcoord4.y = u_xlat4.z;
          u_xlat4.y = dot(u_xlat0.xyz, u_xlat4.xyz);
          u_xlat18 = (u_xlat1.y * _ProjectionParams.x);
          u_xlat5.w = (u_xlat18 * 0.5);
          u_xlat5.xz = (u_xlat1.xw * float2(0.5, 0.5));
          out_v.texcoord5.zw = u_xlat1.zw;
          out_v.texcoord5.xy = float2((u_xlat5.zz + u_xlat5.xw));
          u_xlat4.x = dot(u_xlat0.xyz, u_xlat3.xyz);
          u_xlat4.z = dot(u_xlat0.xyz, u_xlat2.xyz);
          out_v.texcoord6.xyz = float3(u_xlat4.xyz);
          out_v.texcoord7 = float4(0, 0, 0, 0);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat0_d;
      float2 u_xlat16_0;
      float4 u_xlat10_0;
      int u_xlati0;
      float3 u_xlat16_1;
      float3 u_xlat16_2;
      float4 u_xlat10_2;
      float4 u_xlat3_d;
      float4 u_xlat16_3;
      float3 u_xlat10_3;
      float3 u_xlat16_4;
      float3 u_xlat16_5;
      float2 u_xlat6;
      float3 u_xlat16_7;
      float u_xlat16_12;
      int u_xlati12;
      float u_xlat16_13;
      float u_xlat16_19;
      float u_xlat16_22;
      int op_shl(int a, int b)
      {
          return int(floor((float(a) * pow(2, float(b)))));
      }
      
      int2 op_shl(int2 a, int2 b)
      {
          a.x = op_shl(a.x, b.x);
          a.y = op_shl(a.y, b.y);
          return a;
      }
      
      int3 op_shl(int3 a, int3 b)
      {
          a.x = op_shl(a.x, b.x);
          a.y = op_shl(a.y, b.y);
          a.z = op_shl(a.z, b.z);
          return a;
      }
      
      int4 op_shl(int4 a, int4 b)
      {
          a.x = op_shl(a.x, b.x);
          a.y = op_shl(a.y, b.y);
          a.z = op_shl(a.z, b.z);
          a.w = op_shl(a.w, b.w);
          return a;
      }
      
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0.xyz = tex2D(_GSOHMap, in_f.texcoord1.xy).yzw.xyz;
          u_xlat16_1.xyz = float3((u_xlat10_0.xxx * _SpecColor.xyz));
          u_xlat16_19 = max(u_xlat16_1.y, u_xlat16_1.x);
          u_xlat16_19 = max(u_xlat16_1.z, u_xlat16_19);
          out_f.color1.xyz = float3(u_xlat16_1.xyz);
          u_xlat16_1.x = ((-u_xlat16_19) + 1);
          u_xlat16_7.xy = float2((u_xlat10_0.zx * float2(_Parallax, _Glossiness)));
          out_f.color.w = u_xlat10_0.y;
          u_xlat16_7.x = (((-_Parallax) * 0.5) + u_xlat16_7.x);
          out_f.color1.w = u_xlat16_7.y;
          u_xlat16_13 = dot(in_f.texcoord6.xyz, in_f.texcoord6.xyz);
          u_xlat16_13 = rsqrt(u_xlat16_13);
          u_xlat16_2.xyz = float3((float3(u_xlat16_13, u_xlat16_13, u_xlat16_13) * in_f.texcoord6.xyz));
          u_xlat16_0.xy = float2(((in_f.texcoord6.zz * float2(u_xlat16_13, u_xlat16_13)) + float2(0.419999987, (-0.5))));
          u_xlat16_3 = (u_xlat16_2.xyxy / u_xlat16_0.xxxx);
          u_xlat3_d = ((u_xlat16_7.xxxx * u_xlat16_3) + in_f.texcoord.zwxy);
          u_xlat10_0.xzw = tex2D(_MainTex, u_xlat3_d.zw).xyz.xyz;
          u_xlat10_3.xyz = tex2D(_BumpMap, u_xlat3_d.xy).xyz.xyz;
          u_xlat16_7.xyz = float3(((u_xlat10_3.xyz * float3(2, 2, 2)) + float3(-1, (-1), (-1))));
          u_xlat16_4.xyz = float3((u_xlat10_0.xzw * _Color.xyz));
          out_f.color.xyz = float3((u_xlat16_1.xxx * u_xlat16_4.xyz));
          u_xlat16_4.xyz = float3(max(u_xlat16_4.xyz, float3(0.100000001, 0.100000001, 0.100000001)));
          u_xlat3_d.x = dot(in_f.texcoord2.xyz, u_xlat16_7.xyz);
          u_xlat3_d.y = dot(in_f.texcoord3.xyz, u_xlat16_7.xyz);
          u_xlat3_d.z = dot(in_f.texcoord4.xyz, u_xlat16_7.xyz);
          u_xlat0_d.x = dot(u_xlat3_d.xyz, u_xlat3_d.xyz);
          u_xlat0_d.x = rsqrt(u_xlat0_d.x);
          u_xlat0_d.xzw = (u_xlat0_d.xxx * u_xlat3_d.xyz);
          u_xlat0_d.xzw = ((u_xlat0_d.xzw * float3(0.5, 0.5, 0.5)) + float3(0.5, 0.5, 0.5));
          out_f.color2.xyz = float3(u_xlat0_d.xzw);
          out_f.color2.w = 1;
          if(int(((0<u_xlat16_0.y))?((-1)):(0)))
          {
              u_xlati0 = 1;
          }
          else
          {
              u_xlati0 = 0;
          }
          if(int(((u_xlat16_0.y<0))?((-1)):(0)))
          {
              u_xlati12 = 1;
          }
          else
          {
              u_xlati12 = 0;
          }
          u_xlati0 = ((-u_xlati0) + u_xlati12);
          u_xlati0 = op_shl(u_xlati0, 1);
          u_xlat0_d.x = float(u_xlati0);
          u_xlat0_d.x = (u_xlat16_0.y * u_xlat0_d.x);
          u_xlat6.x = max(in_f.texcoord5.w, 0.00100000005);
          u_xlat6.xy = float2((in_f.texcoord5.xy / u_xlat6.xx));
          u_xlat0_d.xy = float2((((-u_xlat0_d.xx) * float2(_NormalDist, _NormalDist)) + u_xlat6.xy));
          u_xlat0_d.xy = float2(((u_xlat16_7.xy * float2(float2(_RefDistortion, _RefDistortion))) + u_xlat0_d.xy));
          u_xlat16_12 = dot(u_xlat16_7.xyz, u_xlat16_2.xyz);
          u_xlat16_12 = clamp(u_xlat16_12, 0, 1);
          u_xlat16_1.x = ((-u_xlat16_12) + 1);
          u_xlat10_0.xyz = tex2D(_ReflectionTex, u_xlat0_d.xy).xyz.xyz;
          u_xlat10_2 = tex2D(_EmissionMap, in_f.texcoord1.zw);
          u_xlat16_7.x = ((_EmissionMode * (-u_xlat10_2.w)) + 1);
          u_xlat16_7.xyz = float3((u_xlat10_0.xyz * u_xlat16_7.xxx));
          u_xlat16_7.xyz = float3((u_xlat16_7.xyz * _ReflectionTint.xyz));
          u_xlat16_22 = (_ReflectionTint.w * 0.5);
          u_xlat16_5.x = (((-_ReflectionTint.w) * 0.5) + 1);
          u_xlat16_22 = ((u_xlat16_1.x * u_xlat16_5.x) + u_xlat16_22);
          u_xlat16_7.xyz = float3((u_xlat16_7.xyz * float3(u_xlat16_22, u_xlat16_22, u_xlat16_22)));
          u_xlat16_5.xyz = float3(((-u_xlat16_4.xyz) + float3(1, 1, 1)));
          u_xlat16_4.xyz = float3(((u_xlat16_1.xxx * u_xlat16_5.xyz) + u_xlat16_4.xyz));
          u_xlat16_1.xyz = float3((u_xlat16_7.xyz * u_xlat16_4.xyz));
          u_xlat16_4.xyz = float3((u_xlat10_2.www * _EmissionColor.xyz));
          u_xlat16_19 = (_EmissionColor.w * _HDROverride);
          u_xlat16_4.xyz = float3((float3(u_xlat16_19, u_xlat16_19, u_xlat16_19) * u_xlat16_4.xyz));
          u_xlat16_1.xyz = float3(((u_xlat10_2.xyz * u_xlat16_4.xyz) + u_xlat16_1.xyz));
          out_f.color3.xyz = float3(exp2((-u_xlat16_1.xyz)));
          out_f.color3.w = 1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Diffuse"
}
