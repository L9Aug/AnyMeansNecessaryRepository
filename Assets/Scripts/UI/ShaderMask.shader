Shader "Custom/ShaderMask" {
	Properties {
		
		_MainTex ("MainTexture", 2D) = "white" {}
		_Mask("MaskTexture",2D) = "white" {}
		_SecondTex("SecondTexture",2D) = "white" {}

		
		
	}
	SubShader{

		Tags {"Queue" = "Transparent"}
	ZWrite Off
	Lighting On

	Blend SrcAlpha OneMinusSrcAlpha

			Pass{

					SetTexture[_MainTex]
					{
						Combine texture
					}

					SetTexture[_SecondTex]
					{
						Combine previous,texture
					}

					SetTexture[_Mask]
					{
						Combine previous,texture
					}
	
	

				}

	

			}
		
	
}
