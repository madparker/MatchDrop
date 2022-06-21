// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "game/Focus Alpha" {
Properties {
    _Vector1 ("Streak1", Vector) = (1,0,0,0)
    _Vector2 ("Streak2", Vector) = (1,0,0,0)
    _Vector3 ("Prev1", Vector) = (1,0,0,0)
    _Prev2   ("Prev2", Vector) = (1,0,0,0)
    _VecAngle ("Angle", Vector) = (1,0,0,0)
    _MainTex ("Texture", 2D) = "white" { }
    _Mode ("Mode", Int) = 0
}

SubShader {
 
    Tags { "RenderType"="Transparent" "Queue"="Transparent" }
    LOD 200
    Blend SrcAlpha OneMinusSrcAlpha
    ZTest Less
 
    Pass {
 
		CGPROGRAM
			// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members srcPos)
			#pragma exclude_renderers d3d11 xbox360
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
	 
			float4 _Vector1;
			float4 _Vector2;
			float4 _Vector3;
			float4 _Vector4;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			int _Mode;
	 
			struct v2f {
			    float4  pos : SV_POSITION;
			    float2  uv : TEXCOORD0;
			    float4 srcPos;
			};
			
			struct vertexInput {
				float4 vertex : POSITION;
			};
			struct vertexOutput {
				float4 pos : SV_POSITION;
				float4 position_in_world_space : TEXCOORD0;
			};
			
			bool sameSide(float4 p1, float4 p2, float4 a1, float4 b1){
	    		float3 cp1 = cross(b1.xyz-a1.xyz, p1.xyz-a1.xyz);
	    		float3 cp2 = cross(b1.xyz-a1.xyz, p2.xyz-a1.xyz);
	    
	    		return dot(cp1, cp2) >= 0;
	    	}

			bool pointInTriangle(float4 p, float4 a1, float4 b1, float4 c1){
	    		if (sameSide(p, a1, b1, c1) && sameSide(p, b1, a1, c1) && sameSide(p, c1, a1, b1)){
	    			return true;
	    		} else {
	    			return false;
	    		}
	    	}
			 

         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output; 
 
            output.pos =  UnityObjectToClipPos(input.vertex);
            output.position_in_world_space = 
               mul(unity_ObjectToWorld, input.vertex);
               // transformation of input.vertex from object 
               // coordinates to world coordinates;
            return output;
         }
 
 		float4 Mode1(vertexOutput input){
	   float4 startPos1 = float4(-1.63, 3.59, 0, 0);
	 		float4 startPos2 = float4(-1.53, 3.59, 0, 0);
	 		float4 endPos1 = _Vector2;
	 		float4 endPos2 = float4(endPos1.x+0.1, endPos1.y, 0, 0);
	 	
	 		if(input.position_in_world_space.x < _Vector1.x &&
		 			input.position_in_world_space.x > _Vector2.x &&
		 			input.position_in_world_space.y > _Vector1.y &&
		 			input.position_in_world_space.y < _Vector2.y){
	 				return float4(0, 0, 0, 0.0);
	 		} else if (input.position_in_world_space.x - 0.1  < _Vector1.x &&
	 			input.position_in_world_space.x + 0.1 > _Vector2.x &&
	 			input.position_in_world_space.y + 0.1 > _Vector1.y &&
	 			input.position_in_world_space.y - 0.1 < _Vector2.y){
	 				return float4(1, 0, 0, 1); 
	 		} else if(pointInTriangle(input.position_in_world_space, startPos1, startPos2, endPos1) ||
	 				pointInTriangle(input.position_in_world_space, startPos2, endPos1, endPos2)){
	 				return float4(1, 0, 0, 1); 
	 		} else {
	 			return float4(0, 0, 0, 0.75); 
	 		}
 		}
 		
 		float4 Mode3(vertexOutput input, float4 p3, float4 p4, float dist3, float circRad, float circRad2, float4 result){
 		
 			float4 r = result;
//			float4 lineCenter = (p1 + p2)/2;
			float4 prevCenter = _Vector3;
				
			if(pointInTriangle(input.position_in_world_space, p3, p4, prevCenter)){
				r = float4(1.0, 1.0, 1.0, 1.0); 
			} 
		
			if(dist3 < circRad2){
				r = float4(1.0, 1.0, 1.0, 1.0); 
			} 
			
			if(dist3 < circRad){
				r = float4(1.0, 1.0, 1.0, 0.0); 
			} 
				
			return r;
 		}
 
         float4 frag(vertexOutput input) : COLOR 
         {
         	float4 result = float4(0, 0, 0, 0.75);
         
			float lineDist = 0.35;
			float lineDist2 = lineDist * 1.1;
			float circRad = 1.457;
			float circRad2 = circRad * 1.005;
         
         	if(_Mode == 1){
         		result =  Mode1(input);
         	} else {
          
	         
				float4 slope = normalize(_Vector1 - _Vector2);
				float4 revSlope = float4(slope.y, -slope.x, slope.z, slope.a);

				float4 p1 = _Vector1 - revSlope * lineDist;
				float4 p2 = _Vector2 - revSlope * lineDist;
				float4 p3 = _Vector1 + revSlope * lineDist;
				float4 p4 = _Vector2 + revSlope * lineDist;
				
				float4 p1_1 = _Vector1 - revSlope * lineDist2;
				float4 p2_1 = _Vector2 - revSlope * lineDist2;
				float4 p3_1 = _Vector1 + revSlope * lineDist2;
				float4 p4_1 = _Vector2 + revSlope * lineDist2;

				float d1 = dot(normalize(input.position_in_world_space), slope);

				float dist1 = distance(input.position_in_world_space, _Vector1);
				float dist2 = distance(input.position_in_world_space, _Vector2);
				float dist3 = distance(input.position_in_world_space, _Vector3);

	 			if((pointInTriangle(input.position_in_world_space, p1_1, p2_1, p3_1) || 
						pointInTriangle(input.position_in_world_space, p2_1, p3_1, p4_1)) ||
						(dist1 < circRad2) || (dist2 < circRad2)){
					result = float4(1.0, 1.0, 1.0, 1.0); 
				} 
	 
	 			if((pointInTriangle(input.position_in_world_space, p1, p2, p3) || 
						pointInTriangle(input.position_in_world_space, p2, p3, p4)) ||
						(dist1 < circRad) || (dist2 < circRad)){
					result =  float4(1.0, 1.0, 1.0, 0.0); 
				} 
				
				if(_Mode == 3){
					result = Mode3(input, p3, p4, dist3, circRad, circRad2, result);
				}
				if(_Mode == 4){
					result = Mode3(input, p3, p4, dist3, circRad * 1.25, circRad2 * 1.25, result);
				}
			}
			
			return result; 
         }
		ENDCG
 
	    }
	}

Fallback "VertexLit"
} 