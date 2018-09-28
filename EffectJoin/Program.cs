using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace EffectJoin
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFolder = @".\a";

            int widthMax = 0;
            int heightMax = 0;

            List<Image> readImgs = new List<Image>();
            foreach (var fileName in Directory.GetFiles(inputFolder))
            {
                var fileInfo = new FileInfo(fileName);
                if (fileInfo.Extension != ".png")
                    continue;

                Console.WriteLine(fileName + " begin! ");
                var testImage = Image.FromFile(fileName);
                readImgs.Add(testImage);

                widthMax = Math.Max(widthMax, testImage.Width);
                heightMax = Math.Max(heightMax, testImage.Height);
            }

            widthMax = GetBoundSize(widthMax);
            heightMax = GetBoundSize(heightMax);

            Bitmap finalBitmap = new Bitmap(widthMax*readImgs.Count, heightMax);
            Graphics g = Graphics.FromImage(finalBitmap);
            for (int i = 0; i < readImgs.Count; i++)
            {
                int xOff = i*widthMax + widthMax/2-readImgs[i].Width/2;
                int yOff = heightMax / 2 - readImgs[i].Height / 2;
                g.DrawImageUnscaled(readImgs[i], xOff, yOff);
            }
            g.Dispose();
            finalBitmap.Save("b.png");
        }

        static int GetBoundSize(int val)
        {
            for (int i = 16; i <= 1024; i*=2)
            {
                if (val < i)
                    return i;
            }
            return 2048;
        }
    }
}
