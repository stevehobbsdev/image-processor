shader 1.0
name "Demo Shader"
author "Steve Hobbs"

# This is a comment

# Declarations
float alpha				# single float value
float3 ambient			# rgb value
float4 ambientAlpha		# rgb value with alpha

float3 temp1 = (0.9, 0.5, 0.1)		# vector with initial value
float val = 0.1			# float constant

image main = input 0		# image type, references first input image
image alpha = input 1		# image type, references second input image

# Pixel processor
.run (pixel, tu, tv)

	# return the pixel we were given
	return pixel

.end