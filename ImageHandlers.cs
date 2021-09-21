using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCompiler
{
    public static class ImageHandlers
    {
        public static bool IsLandscape(this Bitmap bitmap)
        {
            if (bitmap.Width > bitmap.Height)
                return true;

            return false;
        }

        public static int margin = 8;

        public static bool IsPortrait(this Bitmap bitmap)
        {
            return !IsLandscape(bitmap);
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(this Image image, double factor)
        {
            int inputWidth = image.Width;
            int inputHeight = image.Height;

            int width = (int)(factor * inputWidth);
            int height = (int)(factor * inputHeight);

            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        // > Arrange images horizontally (center)
        // > Arrange images horizontally (resize)
        // > Arrange images vertically (center)
        // > Arrange images vertically (resize)
        // > ... and then everything else builds on these?

        public static Bitmap AddImageSideCenter(this Bitmap bitmap1, Bitmap bitmap2)
        {
            int outputHeight = MathHandlers.Maximum(new int[] { bitmap1.Height, bitmap2.Height});
            int outputWidth = bitmap1.Width + margin + bitmap2.Width;

            int StartHeightImage1 = (outputHeight - bitmap1.Height) / 2;
            int StartWidthImage1 = 0;
            int StartHeightImage2 = (outputHeight - bitmap2.Height) / 2;
            int StartWidthImage2 = bitmap1.Width+ margin;

            Bitmap output = new Bitmap(outputWidth, outputHeight);

            using (Graphics g = Graphics.FromImage(output))
            {
                //Create the background
                Rectangle ImageSize = new Rectangle(0, 0, output.Width, output.Height);
                g.FillRectangle(Brushes.White, ImageSize);

                //Apply the images
                g.DrawImage(bitmap1, new Point(StartWidthImage1, StartHeightImage1));
                g.DrawImage(bitmap2, new Point(StartWidthImage2, StartHeightImage2));
            }

            return output;
        }

        public static Bitmap AddImageSideShrink(this Bitmap bitmap1, Bitmap bitmap2)
        {
            int LargeImageHeight = MathHandlers.Maximum(new int[] { bitmap1.Height, bitmap2.Height });
            int SmallImageHeight = MathHandlers.Minimum(new int[] { bitmap1.Height, bitmap2.Height });
            double ratio = (double)SmallImageHeight / (double)LargeImageHeight;

            if (bitmap1.Height > bitmap2.Height)
                bitmap1 = bitmap1.ResizeImage(ratio);
            else
                bitmap2 = bitmap2.ResizeImage(ratio);

            int outputWidth = bitmap1.Width + margin + bitmap2.Width;
            int outputHeight = MathHandlers.Maximum(new int[] { bitmap1.Height, bitmap2.Height });

            int StartHeightImage1 = (outputHeight - bitmap1.Height) / 2;
            int StartWidthImage1 = 0;
            int StartHeightImage2 = (outputHeight - bitmap2.Height) / 2;
            int StartWidthImage2 = bitmap1.Width + margin;

            Bitmap output = new Bitmap(outputWidth, outputHeight);

            using (Graphics g = Graphics.FromImage(output))
            {
                //Create the background
                Rectangle ImageSize = new Rectangle(0, 0, output.Width, output.Height);
                g.FillRectangle(Brushes.White, ImageSize);

                //Apply the images
                g.DrawImage(bitmap1, new Point(StartWidthImage1, StartHeightImage1));
                g.DrawImage(bitmap2, new Point(StartWidthImage2, StartHeightImage2));
            }

            return output;
        }

        public static Bitmap AddImageBelowCenter(this Bitmap bitmap1, Bitmap bitmap2)
        {
            int outputWidth = MathHandlers.Maximum(new int[] { bitmap1.Width, bitmap2.Width });
            int outputHeight = bitmap1.Height + margin + bitmap2.Height;
            
            int StartWidthImage1 = (outputWidth - bitmap1.Width) / 2;
            int StartHeightImage1 = 0;
            int StartWidthImage2 = (outputWidth - bitmap2.Width) / 2;
            int StartHeightImage2 = bitmap1.Height + margin;

            Bitmap output = new Bitmap(outputWidth, outputHeight);

            using (Graphics g = Graphics.FromImage(output))
            {
                //Create the background
                Rectangle ImageSize = new Rectangle(0, 0, output.Width, output.Height);
                g.FillRectangle(Brushes.White, ImageSize);

                //Apply the images
                g.DrawImage(bitmap1, new Point(StartWidthImage1, StartHeightImage1));
                g.DrawImage(bitmap2, new Point(StartWidthImage2, StartHeightImage2));
            }

            return output;
        }

        public static Bitmap AddImageBelowShrink(this Bitmap bitmap1, Bitmap bitmap2)
        {
            int LargeImageWidth = MathHandlers.Maximum(new int[] { bitmap1.Width, bitmap2.Width });
            int SmallImageWidth = MathHandlers.Minimum(new int[] { bitmap1.Width, bitmap2.Width });
            double ratio = (double)SmallImageWidth / (double)LargeImageWidth;

            if (bitmap1.Width > bitmap2.Width)
                bitmap1 = bitmap1.ResizeImage(ratio);
            else
                bitmap2 = bitmap2.ResizeImage(ratio);

            int outputHeight = bitmap1.Height + margin + bitmap2.Height;
            int outputWidth = MathHandlers.Maximum(new int[] { bitmap1.Width, bitmap2.Width });

            int StartWidthImage1 = (outputWidth - bitmap1.Width) / 2;
            int StartHeightImage1 = 0;
            int StartWidthImage2 = (outputWidth - bitmap2.Width) / 2;
            int StartHeightImage2 = bitmap1.Height + margin;

            Bitmap output = new Bitmap(outputWidth, outputHeight);

            using (Graphics g = Graphics.FromImage(output))
            {
                //Create the background
                Rectangle ImageSize = new Rectangle(0, 0, output.Width, output.Height);
                g.FillRectangle(Brushes.White, ImageSize);

                //Apply the images
                g.DrawImage(bitmap1, new Point(StartWidthImage1, StartHeightImage1));
                g.DrawImage(bitmap2, new Point(StartWidthImage2, StartHeightImage2));
            }

            return output;
        }
    }
}
 