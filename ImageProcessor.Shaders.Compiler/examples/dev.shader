# This is a comment

shader: 1.0		# always use 1.0
name: "Test shader"

let ambientColor = |0,0,0|
let baseColor = |0,0,0|
let myValue = 0.7
let anotherConst = |1, 1, 1|
let diffuse = input[0]		# reference the first image
let clamp = yes

.start(pixel, tu, tv)

	# Return a pixel colour here
	#a = 1 + 2

	let output = pixel

	output.r = (pixel.r * 0.5)
	#output.g = saturate(pixel.g)
	#output.b = normalize(pixel.r * pixel.b)
	output.a = 0

	return output
.end