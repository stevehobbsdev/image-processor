# This is a comment

shader 1.0		# always use 1.0
name "Test shader"

float3 ambientColor
float4 baseColor
float const = 0.7
float3 anotherConst = (1,1,1)

imageref diffuse = image(0)		# reference the first image

.start (pixel, tu, tv)

	# Return a pixel colour here

	return b
.end