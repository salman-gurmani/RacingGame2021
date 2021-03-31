Shader "MK/MKGlow/Unlit/Transparent"
{
  Properties
  {
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
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
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent"
      "RenderType" = "MKGlow"
    }
    LOD 100
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "QUEUE" = "Transparent"
        "RenderType" = "MKGlow"
      }
      LOD 100
      ZWrite Off
      Blend SrcAlpha OneMinusSrcAlpha
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      uniform float _MKGlowTexStrength;
      uniform float4 _MKGlowTexColor;
      uniform sampler2D _MainTex;
      uniform sampler2D _MKGlowTex;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float2 texcoord :TEXCOORD0;
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
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          u_xlat0.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _MainTex));
          out_v.texcoord.xy = float2(u_xlat0.xy);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat16_0;
      float4 u_xlat10_0;
      float4 u_xlat10_1;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2D(_MKGlowTex, in_f.texcoord.xy);
          u_xlat16_0 = (u_xlat10_0 * _MKGlowTexColor);
          u_xlat10_1 = tex2D(_MainTex, in_f.texcoord.xy);
          out_f.color = ((u_xlat16_0 * float4(_MKGlowTexStrength, _MKGlowTexStrength, _MKGlowTexStrength, _MKGlowTexStrength)) + u_xlat10_1);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
