using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;


// Parallel computation support
using Uk.Org.Adcock.Parallel;
using System;
using System.Runtime.InteropServices;

namespace OpenCVUtils
{
    public class OpenCV
    {
        public static string TestMethod()
        {
            Mat src = new Mat("Assets/pictures/test.png", ImreadModes.GrayScale);
            // Mat src = Cv2.ImRead("lenna.png", ImreadModes.GrayScale);
            Mat dst = new Mat();

            Cv2.Canny(src, dst, 50, 200);
            using (new Window("src image", src))
            using (new Window("dst image", dst))
            {
                Cv2.WaitKey();
            }

            return "Hi";
        }

        public void MatToTexture(Mat image, Texture2D processedTexture, int width, int height)
        {
            processedTexture.SetPixels32(MatToColor(image, width, height));
            // to update the texture, OpenGL manner
            processedTexture.Apply();
        }

        public Color32[] MatToColor(Mat image, int width, int height)
        {
            byte[] imageData = new Byte[width * height];
            // cannyImageData is byte array, because canny image is grayscale
            image.GetArray(0, 0, imageData);
            // create Color32 array that can be assigned to Texture2D directly
            Color32[] c = new Color32[height * width];

            // parallel for loop
            Parallel.For(0, height, i => {
                for (var j = 0; j < width; j++)
                {
                    byte vec = imageData[j + i * width];
                    var color32 = new Color32
                    {
                        r = vec,
                        g = vec,
                        b = vec,
                        a = 0
                    };
                    c[j + i * width] = color32;
                }
            });
            return c;
        }

    }

}


