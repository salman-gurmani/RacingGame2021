Shader "MatCap/Vertex/Plain Additive Z"
{
  Properties
  {
    _Color ("Main Color", Color) = (0.5,0.5,0.5,1)
    _MatCap ("MatCap (RGB)", 2D) = "white" {}
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      Fog
      { 
        Mode  Off
      } 
      ColorMask 0
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      struct appdata_t
      {
          float3 vertex :POSITION0;
          float4 color :COLOR0;
      };
      
      struct OUT_Data_Vert
      {
          float4 color :COLOR0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 color :COLOR0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          out_v.color = in_v.color;
          out_v.color = clamp(out_v.color, 0, 1);
          out_v.vertex = UnityObjectToClipPos(float4(in_v.vertex, 0));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          out_f.color = in_f.color;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 2, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      ZWrite Off
      Cull Off
      Blend One OneMinusSrcColor
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 unity_MatrixV;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _Color;
      uniform sampler2D _MatCap;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float3 normal :NORMAL0;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
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
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          u_xlat0.x = (in_v.normal.x * conv_mxt4x4_0(unity_WorldToObject).x).x;
          u_xlat0.y = (in_v.normal.x * conv_mxt4x4_1(unity_WorldToObject).x).x;
          u_xlat0.z = (in_v.normal.x * conv_mxt4x4_2(unity_WorldToObject).x).x;
          u_xlat1.x = (in_v.normal.y * conv_mxt4x4_0(unity_WorldToObject).y).x;
          u_xlat1.y = (in_v.normal.y * conv_mxt4x4_1(unity_WorldToObject).y).x;
          u_xlat1.z = (in_v.normal.y * conv_mxt4x4_2(unity_WorldToObject).y).x;
          u_xlat0.xyz = float3((u_xlat0.xyz + u_xlat1.xyz));
          u_xlat1.x = (in_v.normal.z * conv_mxt4x4_0(unity_WorldToObject).z).x;
          u_xlat1.y = (in_v.normal.z * conv_mxt4x4_1(unity_WorldToObject).z).x;
          u_xlat1.z = (in_v.normal.z * conv_mxt4x4_2(unity_WorldToObject).z).x;
          u_xlat0.xyz = float3((u_xlat0.xyz + u_xlat1.xyz));
          u_xlat0.xyz = float3(normalize(u_xlat0.xyz));
          u_xlat2.xz = (u_xlat0.yy * conv_mxt4x4_1(unity_MatrixV).xy);
          u_xlat0.xy = ((conv_mxt4x4_0(unity_MatrixV).xy * u_xlat0.xx) + u_xlat2.xz).xy;
          u_xlat0.xy = ((conv_mxt4x4_2(unity_MatrixV).xy * u_xlat0.zz) + u_xlat0.xy).xy;
          out_v.texcoord.xy = float2(((u_xlat0.xy * float2(0.5, 0.5)) + float2(0.5, 0.5)));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat0_d;
      float4 u_xlat10_0;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2D(_MatCap, in_f.texcoord.xy);
          u_xlat0_d = (u_xlat10_0 * _Color);
          out_f.color = (u_xlat0_d + u_xlat0_d);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "VertexLit"
}
