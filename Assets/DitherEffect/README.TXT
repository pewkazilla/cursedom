This effect lets you force the vertical screen resolution & apply a low-colour ‘dither’ effect.
To use, add the DitheringEffect script to your camera. Hook up the dithering effect shader to the script, and one of the matrix textures.
Each matrix has a particular visual style, blue noise is the most natural and bayer is most like mid-90s 3D graphics.
Make sure the texture you use as the matrix is set to use point filtering rather than bilinear, and is set to 16 bit or uncompressed, rather than compressed.

I hope you find neat things to do with this!

By Maya Violet ~ @VioletMayMay on Twitter