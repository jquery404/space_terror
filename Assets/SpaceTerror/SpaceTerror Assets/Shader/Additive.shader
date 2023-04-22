
Shader "Custom/Additive" {
	
	Properties
	{
		_Color("Color", Color) = (1, 0, 0)
	}
	
	SubShader
	{		
		Color[_Color]
		Pass{}
	}

}
