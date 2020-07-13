#ifndef SHARED_FOO
// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
#pragma exclude_renderers d3d11 gles
#define SHARED_FOO

// uniform sampler2D _MainTex;
// uniform float4 _MainTex_TexelSize;

// struct v2f {
// 		float4 pos : SV_POSITION;
// 		float2 uv : TEXCOORD0;
// };

#define KERNEL_PHYSICAL_SIZE 20

void makeGaussianKernel(float sigma, int kernelSize, inout float kernel[KERNEL_PHYSICAL_SIZE])
{
	//const double PI = 3.14159265;       // PI

	int i, center;
	float sum = 0;                      // used for normalization
	float result = 0;                  // result of gaussian func

	// compute kernel elements normal distribution equation(Gaussian)
	// do only half(positive area) and mirror to negative side
	// because Gaussian is even function, symmetric to Y-axis.
	center = kernelSize / 2;   // center value of n-array(0 ~ n-1)

	if (sigma == 0)
	{
		for (i = 0; i <= center; ++i)
			kernel[center + i] = kernel[center - i] = 0;

		kernel[center] = 1.0;
	}
	else
	{
		for (i = 0; i <= center; ++i)
		{
			//result = exp(-(i*i)/(double)(2*sigma*sigma)) / (sqrt(2*PI)*sigma);
			// NOTE: dividing (sqrt(2*PI)*sigma) is not needed because normalizing result later
			result = exp(-(i*i) / (float)(2 * sigma*sigma));
			kernel[center + i] = kernel[center - i] = (float)result;
			sum += (float)result;
			if (i != 0) sum += (float)result;
		}

		// normalize kernel
		// make sum of all elements in kernel to 1
		for (i = 0; i <= center; ++i)
			kernel[center + i] = kernel[center - i] /= sum;
	}
}

#endif // SHARED_FOO