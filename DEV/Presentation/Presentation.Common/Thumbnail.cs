using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


namespace SOMAX.G4.Presentation.Common
{
    public class Thumbnail
    {
        public Image CreateThumbnail(Image image, Size thumbnailSize)
        {
            float scalingRatio = CalculateScalingRatio(image.Size, thumbnailSize);
            int scaledWidth = (int)Math.Round((float)image.Size.Width / scalingRatio);
            int scaledHeight = (int)Math.Round((float)image.Size.Height / scalingRatio);
            int scaledLeft =0;
            int scaledTop = 0;
            Rectangle cropArea = new Rectangle(scaledLeft, scaledTop, scaledWidth, scaledHeight);
            System.Drawing.Image thumbnail = new Bitmap(scaledWidth, scaledHeight);
            using (Graphics thumbnailGraphics = Graphics.FromImage(thumbnail))
            {
                thumbnailGraphics.CompositingQuality = CompositingQuality.HighQuality;
                thumbnailGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                thumbnailGraphics.SmoothingMode = SmoothingMode.HighQuality;
                thumbnailGraphics.DrawImage(image, cropArea, new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            }
            return thumbnail;
        }
        public Image CreateLarge(Image image, Size thumbnailSize)
        {
            float scalingRatio = CalculateScalingRatio(image.Size, thumbnailSize);
            int scaledWidth = (int)Math.Round((float)image.Size.Width * scalingRatio);
            int scaledHeight = (int)Math.Round((float)image.Size.Height * scalingRatio);
            int scaledLeft = 0; 
            int scaledTop = 0;
            Rectangle cropArea = new Rectangle(scaledLeft, scaledTop, scaledWidth, scaledHeight);
            System.Drawing.Image thumbnail = new Bitmap(scaledWidth, scaledHeight);
            using (Graphics thumbnailGraphics = Graphics.FromImage(thumbnail))
            {
                thumbnailGraphics.CompositingQuality = CompositingQuality.HighQuality;
                thumbnailGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                thumbnailGraphics.SmoothingMode = SmoothingMode.HighQuality;
                thumbnailGraphics.DrawImage(image, cropArea);
            }
            return thumbnail;
        }
        private float CalculateScalingRatio(Size originalSize, Size targetSize)
        {
            //float originalAspectRatio = (float)originalSize.Width / (float)originalSize.Height;
            //float targetAspectRatio = (float)targetSize.Width / (float)targetSize.Height;
            float scalingRatio =1;

            if (originalSize.Width > targetSize.Width && originalSize.Width >= originalSize.Height)// resize to thumb
            {
                scalingRatio = (float)originalSize.Width / (float)targetSize.Width;
            }
            else if (originalSize.Width < targetSize.Width) //resize to large
            {
                scalingRatio = (float)targetSize.Width / (float)originalSize.Width;
            }
            else if (originalSize.Height > targetSize.Height && originalSize.Height >= originalSize.Width)// resize to thumb
            {
                scalingRatio = (float)originalSize.Height / (float)targetSize.Height;
            }
            else if (originalSize.Height < targetSize.Height)//resize to large
            {
                scalingRatio = (float)targetSize.Height / (float)originalSize.Height;
            }
            return scalingRatio;
        }
      
    }

}
