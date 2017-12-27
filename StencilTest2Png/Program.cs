using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;

namespace StencilTest2Png
{
    class Program
    {
        private static int tempWidth;
        private static int tempHeight;
        private static byte[] tempPixelValues;

        static void Main(string[] args)
        {
            var tempImg = (Bitmap) Image.FromFile("./template.png");
            tempWidth = tempImg.Width;
            tempHeight = tempImg.Height;
            CheckTemp(tempImg);

            var inputFolder = @"E:\doc\minister\数据表\ResH5\peopleskill";
            var outFolder = @"E:\doc\minister\数据表\ResH5\peopleskill1\";
            foreach (var fileName in Directory.GetFiles(inputFolder))
            {
                var fileInfo = new FileInfo(fileName);
                if (fileInfo.Extension != ".png")
                    continue;

                var testImage = Image.FromFile(fileName);
                Effect((Bitmap)testImage);
                testImage.Save(outFolder + fileInfo.Name);
                testImage = null;
            }
        }

        public static void CheckTemp(Bitmap source)
        {
            int imageWidth = source.Width;
            int imageHeight = source.Height;

            Rectangle rect = new Rectangle(0, 0, imageWidth, imageHeight);
            System.Drawing.Imaging.BitmapData bmpData = source.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, source.PixelFormat);
            IntPtr ptr = bmpData.Scan0;

            int count = imageWidth * imageHeight * 4;
            tempPixelValues = new byte[count];
            System.Runtime.InteropServices.Marshal.Copy(ptr, tempPixelValues, 0, count);
            source.UnlockBits(bmpData);
        }

        public static void Effect(Bitmap source)
        {
            int imageWidth = source.Width;
            int imageHeight = source.Height;

            Rectangle rect = new Rectangle(0, 0, imageWidth, imageHeight);
            System.Drawing.Imaging.BitmapData bmpData = source.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, source.PixelFormat);
            IntPtr ptr = bmpData.Scan0;

            int count = imageWidth * imageHeight * 4;
            byte[] pixelValues = new byte[count];
            System.Runtime.InteropServices.Marshal.Copy(ptr, pixelValues, 0, count);
            MakeTransparent(pixelValues, imageWidth, imageHeight);
            System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, ptr, count);
            source.UnlockBits(bmpData);

        }

        private static void MakeTransparent(byte[] pixelValues, int imageWidth, int imageHeight)
        {
            int index = 0;
            for (int i = 0; i < imageHeight; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    var tx = j*tempWidth/imageWidth;
                    var ty = i * tempHeight / imageHeight;
                    index++;
                    index++;
                    index++;
                    if (tempPixelValues[(tx+ty* tempWidth)*4+3] == 0)
                    {
                        pixelValues[index] = 0;
                    }
                    index++;
                }
            }
        }
    }
}
