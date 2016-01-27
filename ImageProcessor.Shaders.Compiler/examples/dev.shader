# This is a comment

shader: 1.0		# always use 1.0
name: "Test shader"

def ambientColor
def baseColor
def myValue = 0.7
def anotherConst = |1, 1, 1|
def diffuse = input:0		# reference the first image

.start(pixel, tu, tv)

	# Return a pixel colour here
	#a = 1 + 2

	a = baseColor * 0.5 + 1

	return b
.end