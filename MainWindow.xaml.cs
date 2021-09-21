using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageCompiler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<Bitmap> images = new List<Bitmap>();
            CurrentIndex = 0;
            images.Add(new Bitmap(@"C:\Users\charl\Documents\Dump\1l.png"));
            images.Add(new Bitmap(@"C:\Users\charl\Documents\Dump\2l.png"));
            images.Add(new Bitmap(@"C:\Users\charl\Documents\Dump\3l.png"));
            images.Add(new Bitmap(@"C:\Users\charl\Documents\Dump\4l.png"));

            List<Bitmap> outputImages = GenerateCompiledImage(images);

            displayImages = outputImages;
            ImageDisplay.Source = BitmapToImageSource(displayImages[CurrentIndex]);
            UpdateDisplayLabel();
        }

        private List<Bitmap> displayImages = new List<Bitmap>();
        int CurrentIndex = 0;

        #region SWITCH
        private List<Bitmap> GenerateCompiledImage(List<Bitmap> images)
        {
            switch (images.Count)
            {
                case 0:
                    return new List<Bitmap> { new Bitmap(10, 10) };
                case 1:
                    return new List<Bitmap> { images[0] };
                case 2:
                    return Combine2Images(images[0], images[1]);
                case 3:
                    return Combine3Images(images[0], images[1], images[2]);
                default:
                    //Pass through first 4 images
                    return Combine4Images(images[0], images[1], images[2], images[3]);
            }
        }
        #endregion

        #region 2
        private List<Bitmap> Combine2Images(Bitmap bitmap1, Bitmap bitmap2)
        {
            if (bitmap1.IsLandscape() && bitmap2.IsLandscape())
                return LandscapeLandscape(bitmap1, bitmap2);

            if (bitmap1.IsPortrait() && bitmap2.IsPortrait())
                return PortraitPortrait(bitmap1, bitmap2);

            if (bitmap1.IsLandscape() && bitmap2.IsPortrait())
                return LandscapePortrait(bitmap1, bitmap2);

            if (bitmap1.IsPortrait() && bitmap2.IsLandscape())
                return PortraitLandscape(bitmap1, bitmap2);

            return new List<Bitmap> { new Bitmap(10, 10) };
        }

        private List<Bitmap> LandscapeLandscape(Bitmap bitmap1, Bitmap bitmap2)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageBelowCenter(bitmap2));
            return images;
        }

        private List<Bitmap> PortraitPortrait(Bitmap bitmap1, Bitmap bitmap2)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageSideCenter(bitmap2));
            return images;
        }
        private List<Bitmap> LandscapePortrait(Bitmap bitmap1, Bitmap bitmap2)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageSideCenter(bitmap2));
            images.Add(bitmap1.AddImageSideShrink(bitmap2));
            return images;
        }
        private List<Bitmap> PortraitLandscape(Bitmap bitmap1, Bitmap bitmap2)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageSideCenter(bitmap2));
            images.Add(bitmap1.AddImageSideShrink(bitmap2));
            return images;
        }
        #endregion

        #region 3
        private List<Bitmap> Combine3Images(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3)
        {
            //{p p p}
            if (bitmap1.IsPortrait() && bitmap2.IsPortrait() && bitmap3.IsPortrait())
                return PortraitPortraitPortrait(bitmap1, bitmap2, bitmap3);

            //{p p l}
            if (bitmap1.IsPortrait() && bitmap2.IsPortrait() && bitmap3.IsLandscape())
                return PortraitPortraitLandscape(bitmap1, bitmap2, bitmap3);

            //{p l p}
            if (bitmap1.IsPortrait() && bitmap2.IsLandscape() && bitmap3.IsPortrait())
                return PortraitLandscapePortrait(bitmap1, bitmap2, bitmap3);

            //{p l l}
            if (bitmap1.IsPortrait() && bitmap2.IsLandscape() && bitmap3.IsLandscape())
                return PortraitLandscapeLandscape(bitmap1, bitmap2, bitmap3);

            //{l p p}
            if (bitmap1.IsLandscape() && bitmap2.IsPortrait() && bitmap3.IsPortrait())
                return LandscapePortraitPortrait(bitmap1, bitmap2, bitmap3);

            //{l p l}
            if (bitmap1.IsLandscape() && bitmap2.IsPortrait() && bitmap3.IsLandscape())
                return LandscapePortraitLandscape(bitmap1, bitmap2, bitmap3);

            //{l l p}
            if (bitmap1.IsLandscape() && bitmap2.IsLandscape() && bitmap3.IsPortrait())
                return LandscapeLandscapePortrait(bitmap1, bitmap2, bitmap3);

            //{l l l}
            if (bitmap1.IsLandscape() && bitmap2.IsLandscape() && bitmap3.IsLandscape())
                return LandscapeLandscapeLandscape(bitmap1, bitmap2, bitmap3);

            return new List<Bitmap> { new Bitmap(10, 10) };
        }

        private List<Bitmap> LandscapeLandscapePortrait(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideShrink(bitmap3));
            images.Add(bitmap1.AddImageBelowCenter(bitmap2).AddImageSideCenter(bitmap3));
            images.Add(bitmap1.AddImageBelowCenter(bitmap2).AddImageSideShrink(bitmap3));
            return images;
        }

        private List<Bitmap> LandscapePortraitLandscape(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageSideShrink(bitmap2).AddImageSideCenter(bitmap3));
            images.Add(bitmap1.AddImageBelowCenter(bitmap3).AddImageSideCenter(bitmap2));
            images.Add(bitmap1.AddImageBelowCenter(bitmap3).AddImageSideShrink(bitmap2));
            return images;
        }

        private List<Bitmap> LandscapePortraitPortrait(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3));
            images.Add(bitmap1.AddImageSideShrink(bitmap2).AddImageSideShrink(bitmap3));
            images.Add(bitmap1.AddImageBelowCenter(bitmap2.AddImageSideCenter(bitmap3)));
            images.Add(bitmap1.AddImageBelowShrink(bitmap2.AddImageSideCenter(bitmap3)));
            return images;
        }

        private List<Bitmap> PortraitLandscapeLandscape(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3));
            images.Add(bitmap1.AddImageSideShrink(bitmap2).AddImageSideShrink(bitmap3));
            images.Add(bitmap1.AddImageSideCenter(bitmap2.AddImageBelowCenter(bitmap3)));
            images.Add(bitmap1.AddImageSideShrink(bitmap2.AddImageBelowCenter(bitmap3)));
            return images;
        }

        private List<Bitmap> PortraitLandscapePortrait(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageSideShrink(bitmap2).AddImageSideShrink(bitmap3));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3));
            images.Add(bitmap1.AddImageSideCenter(bitmap3).AddImageBelowCenter(bitmap2));
            images.Add(bitmap1.AddImageSideCenter(bitmap3).AddImageBelowShrink(bitmap2));
            return images;
        }

        private List<Bitmap> PortraitPortraitLandscape(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3));
            images.Add(bitmap1.AddImageSideShrink(bitmap2).AddImageSideShrink(bitmap3));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageBelowCenter(bitmap3));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageBelowShrink(bitmap3));
            return images;
        }

        private List<Bitmap> PortraitPortraitPortrait(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageBelowCenter(bitmap3));
            return images;
        }

        private List<Bitmap> LandscapeLandscapeLandscape(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageBelowCenter(bitmap2).AddImageBelowCenter(bitmap3));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageBelowCenter(bitmap3));
            images.Add(bitmap1.AddImageBelowCenter(bitmap2).AddImageSideCenter(bitmap3));
            return images;
        }
        #endregion

        #region 4
        private List<Bitmap> Combine4Images(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3, Bitmap bitmap4)
        {
            //{p p p p}
            if (bitmap1.IsPortrait() && bitmap2.IsPortrait() && bitmap3.IsPortrait() && bitmap4.IsPortrait())
                return PortraitPortraitPortraitPortrait(bitmap1, bitmap2, bitmap3, bitmap4);

            //{p p p l}
            if (bitmap1.IsPortrait() && bitmap2.IsPortrait() && bitmap3.IsPortrait() && bitmap4.IsLandscape())
                return PortraitPortraitPortraitLandscape(bitmap1, bitmap2, bitmap3, bitmap4);

            //{p p l p}
            if (bitmap1.IsPortrait() && bitmap2.IsPortrait() && bitmap3.IsLandscape() && bitmap4.IsPortrait())
                return PortraitPortraitLandscapePortrait(bitmap1, bitmap2, bitmap3, bitmap4);

            //{p p l l}
            if (bitmap1.IsPortrait() && bitmap2.IsPortrait() && bitmap3.IsLandscape() && bitmap4.IsLandscape())
                return PortraitPortraitLandscapeLandscape(bitmap1, bitmap2, bitmap3, bitmap4);

            //{p l p p}
            if (bitmap1.IsPortrait() && bitmap2.IsLandscape() && bitmap3.IsPortrait() && bitmap4.IsPortrait())
                return PortraitLandscapePortraitPortrait(bitmap1, bitmap2, bitmap3, bitmap4);

            //{p l p l}
            if (bitmap1.IsPortrait() && bitmap2.IsLandscape() && bitmap3.IsPortrait() && bitmap4.IsLandscape())
                return PortraitLandscapePortraitLandscape(bitmap1, bitmap2, bitmap3, bitmap4);

            //{p l l p}
            if (bitmap1.IsPortrait() && bitmap2.IsLandscape() && bitmap3.IsLandscape() && bitmap4.IsPortrait())
                return PortraitLandscapeLandscapePortrait(bitmap1, bitmap2, bitmap3, bitmap4);

            //{p l l l}
            if (bitmap1.IsPortrait() && bitmap2.IsLandscape() && bitmap3.IsLandscape() && bitmap4.IsLandscape())
                return PortraitLandscapeLandscapeLandscape(bitmap1, bitmap2, bitmap3, bitmap4);

            //{l p p p}
            if (bitmap1.IsLandscape() && bitmap2.IsPortrait() && bitmap3.IsPortrait() && bitmap4.IsPortrait())
                return LandscapePortraitPortraitPortrait(bitmap1, bitmap2, bitmap3, bitmap4);

            //{l p p l}
            if (bitmap1.IsLandscape() && bitmap2.IsPortrait() && bitmap3.IsPortrait() && bitmap4.IsLandscape())
                return LandscapePortraitPortraitLandscape(bitmap1, bitmap2, bitmap3, bitmap4);

            //{l p l p}
            if (bitmap1.IsLandscape() && bitmap2.IsPortrait() && bitmap3.IsLandscape() && bitmap4.IsPortrait())
                return LandscapePortraitLandscapePortrait(bitmap1, bitmap2, bitmap3, bitmap4);

            //{l p l l}
            if (bitmap1.IsLandscape() && bitmap2.IsPortrait() && bitmap3.IsLandscape() && bitmap4.IsLandscape())
                return LandscapePortraitLandscapeLandscape(bitmap1, bitmap2, bitmap3, bitmap4);

            //{l l p p}
            if (bitmap1.IsLandscape() && bitmap2.IsLandscape() && bitmap3.IsPortrait() && bitmap4.IsPortrait())
                return LandscapeLandscapePortraitPortrait(bitmap1, bitmap2, bitmap3, bitmap4);

            //{l l p l}
            if (bitmap1.IsLandscape() && bitmap2.IsLandscape() && bitmap3.IsPortrait() && bitmap4.IsLandscape())
                return LandscapeLandscapePortraitLandscape(bitmap1, bitmap2, bitmap3, bitmap4);

            //{l l l p}
            if (bitmap1.IsLandscape() && bitmap2.IsLandscape() && bitmap3.IsLandscape() && bitmap4.IsPortrait())
                    return LandscapeLandscapeLandscapePortrait(bitmap1, bitmap2, bitmap3, bitmap4);

            //{l l l l}
            if (bitmap1.IsLandscape() && bitmap2.IsLandscape() && bitmap3.IsLandscape() && bitmap4.IsLandscape())
                return LandscapeLandscapeLandscapeLandscape(bitmap1, bitmap2, bitmap3, bitmap4);

            return new List<Bitmap> { new Bitmap(10, 10) };
        }

        private List<Bitmap> PortraitPortraitPortraitPortrait(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3, Bitmap bitmap4)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3).AddImageSideCenter(bitmap4));
            images.Add(bitmap1.AddImageSideShrink(bitmap2).AddImageSideShrink(bitmap3).AddImageSideShrink(bitmap4));
            images.Add(bitmap1.AddImageSideShrink(bitmap2).AddImageBelowShrink(bitmap3.AddImageSideShrink(bitmap4)));
            return images;
        }

        private List<Bitmap> PortraitPortraitPortraitLandscape(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3, Bitmap bitmap4)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageSideShrink(bitmap2).AddImageSideShrink(bitmap3).AddImageSideShrink(bitmap4));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3).AddImageSideCenter(bitmap4));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3).AddImageBelowCenter(bitmap4));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3).AddImageBelowShrink(bitmap4));
            return images;
        }

        private List<Bitmap> PortraitPortraitLandscapePortrait(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3, Bitmap bitmap4)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageSideShrink(bitmap2).AddImageSideShrink(bitmap3).AddImageSideShrink(bitmap4));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3).AddImageSideCenter(bitmap4));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap4).AddImageBelowCenter(bitmap3));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap4).AddImageBelowShrink(bitmap3));
            return images;
        }

        private List<Bitmap> PortraitPortraitLandscapeLandscape(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3, Bitmap bitmap4)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageSideShrink(bitmap2).AddImageSideShrink(bitmap3).AddImageSideShrink(bitmap4));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3).AddImageSideCenter(bitmap4));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3.AddImageBelowCenter(bitmap4)));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideShrink(bitmap3.AddImageBelowCenter(bitmap4)));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideShrink(bitmap3).AddImageBelowShrink(bitmap4));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideShrink(bitmap4).AddImageBelowShrink(bitmap3));
            return images;
        }

        private List<Bitmap> PortraitLandscapePortraitPortrait(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3, Bitmap bitmap4)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageSideShrink(bitmap2).AddImageSideShrink(bitmap3).AddImageSideShrink(bitmap4));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3).AddImageSideCenter(bitmap4));
            images.Add(bitmap1.AddImageSideCenter(bitmap3).AddImageSideCenter(bitmap4).AddImageBelowCenter(bitmap2));
            images.Add(bitmap1.AddImageSideCenter(bitmap3).AddImageSideCenter(bitmap4).AddImageBelowShrink(bitmap2));
            return images;
        }

        private List<Bitmap> PortraitLandscapePortraitLandscape(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3, Bitmap bitmap4)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageSideShrink(bitmap2).AddImageSideShrink(bitmap3).AddImageSideShrink(bitmap4));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3).AddImageSideCenter(bitmap4));
            images.Add(bitmap1.AddImageSideShrink(bitmap3).AddImageSideShrink(bitmap2.AddImageBelowShrink(bitmap4)));
            images.Add(bitmap1.AddImageSideShrink(bitmap3).AddImageSideCenter(bitmap2.AddImageBelowShrink(bitmap4)));
            images.Add(bitmap1.AddImageSideShrink(bitmap2.AddImageBelowShrink(bitmap4)).AddImageSideShrink(bitmap3));
            images.Add(bitmap1.AddImageSideShrink(bitmap3).AddImageBelowShrink(bitmap2.AddImageBelowShrink(bitmap4)));
            images.Add(bitmap2.AddImageBelowShrink(bitmap1.AddImageSideShrink(bitmap3).AddImageBelowShrink(bitmap4)));
            return images;
        }

        private List<Bitmap> PortraitLandscapeLandscapePortrait(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3, Bitmap bitmap4)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageSideShrink(bitmap2.AddImageBelowShrink(bitmap3)).AddImageSideShrink(bitmap4));
            images.Add(bitmap2.AddImageBelowCenter(bitmap3).AddImageSideShrink(bitmap1).AddImageSideShrink(bitmap4));
            images.Add(bitmap1.AddImageSideShrink(bitmap2).AddImageSideShrink(bitmap3).AddImageSideShrink(bitmap4));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3).AddImageSideCenter(bitmap4));
            return images;
        }

        private List<Bitmap> PortraitLandscapeLandscapeLandscape(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3, Bitmap bitmap4)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageSideShrink(bitmap2.AddImageBelowShrink(bitmap3).AddImageBelowShrink(bitmap4)));
            images.Add(bitmap1.AddImageSideShrink(bitmap2).AddImageSideShrink(bitmap3).AddImageSideShrink(bitmap4));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3).AddImageSideCenter(bitmap4));
            images.Add(bitmap1.AddImageSideCenter(bitmap2.AddImageBelowShrink(bitmap3).AddImageBelowShrink(bitmap4)));
            return images;
        }

        private List<Bitmap> LandscapePortraitPortraitPortrait(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3, Bitmap bitmap4)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageBelowShrink(bitmap2.AddImageSideShrink(bitmap3).AddImageSideShrink(bitmap4)));
            images.Add(bitmap1.AddImageSideShrink(bitmap2).AddImageSideShrink(bitmap3).AddImageSideShrink(bitmap4));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3).AddImageSideCenter(bitmap4));
            images.Add(bitmap1.AddImageBelowCenter(bitmap2.AddImageSideShrink(bitmap3).AddImageSideShrink(bitmap4)));
            return images;
        }

        private List<Bitmap> LandscapePortraitPortraitLandscape(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3, Bitmap bitmap4)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageSideShrink(bitmap2).AddImageSideShrink(bitmap3).AddImageSideShrink(bitmap4));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3).AddImageSideCenter(bitmap4));
            images.Add(bitmap1.AddImageBelowShrink(bitmap4).AddImageSideShrink(bitmap2).AddImageSideCenter(bitmap3));
            images.Add(bitmap2.AddImageSideShrink(bitmap3).AddImageSideShrink(bitmap1.AddImageBelowShrink(bitmap4)));
            return images;
        }

        private List<Bitmap> LandscapePortraitLandscapePortrait(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3, Bitmap bitmap4)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageSideShrink(bitmap2).AddImageSideShrink(bitmap3).AddImageSideShrink(bitmap4));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3).AddImageSideCenter(bitmap4));
            images.Add(bitmap1.AddImageBelowShrink(bitmap3).AddImageSideShrink(bitmap2).AddImageSideShrink(bitmap4));
            images.Add(bitmap2.AddImageSideShrink(bitmap1.AddImageBelowShrink(bitmap3)).AddImageSideShrink(bitmap4));
            images.Add(bitmap1.AddImageSideShrink(bitmap2.AddImageBelowShrink(bitmap3)).AddImageSideShrink(bitmap4));
            return images;
        }

        private List<Bitmap> LandscapePortraitLandscapeLandscape(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3, Bitmap bitmap4)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageSideShrink(bitmap2).AddImageSideShrink(bitmap3).AddImageSideShrink(bitmap4));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3).AddImageSideCenter(bitmap4));
            images.Add(bitmap2.AddImageSideShrink(bitmap1.AddImageBelowShrink(bitmap3).AddImageBelowShrink(bitmap4)));
            images.Add(bitmap1.AddImageBelowShrink(bitmap3).AddImageBelowShrink(bitmap4).AddImageSideShrink(bitmap2));
            return images;
        }

        private List<Bitmap> LandscapeLandscapePortraitPortrait(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3, Bitmap bitmap4)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageBelowShrink(bitmap2).AddImageSideShrink(bitmap3).AddImageSideShrink(bitmap4));
            images.Add(bitmap1.AddImageSideShrink(bitmap2).AddImageSideShrink(bitmap3).AddImageSideShrink(bitmap4));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3).AddImageSideCenter(bitmap4));
            return images;
        }

        private List<Bitmap> LandscapeLandscapePortraitLandscape(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3, Bitmap bitmap4)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageBelowShrink(bitmap2).AddImageBelowShrink(bitmap4).AddImageSideShrink(bitmap3));
            images.Add(bitmap1.AddImageBelowShrink(bitmap2).AddImageBelowShrink(bitmap4).AddImageSideCenter(bitmap3));
            images.Add(bitmap1.AddImageSideShrink(bitmap2).AddImageSideShrink(bitmap3).AddImageSideShrink(bitmap4));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3).AddImageSideCenter(bitmap4));
            return images;
        }

        private List<Bitmap> LandscapeLandscapeLandscapePortrait(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3, Bitmap bitmap4)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageBelowShrink(bitmap2).AddImageBelowShrink(bitmap3).AddImageSideShrink(bitmap4));
            images.Add(bitmap1.AddImageBelowShrink(bitmap2).AddImageBelowShrink(bitmap3).AddImageSideCenter(bitmap4));
            images.Add(bitmap1.AddImageSideShrink(bitmap2).AddImageSideShrink(bitmap3).AddImageSideShrink(bitmap4));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3).AddImageSideCenter(bitmap4));
            return images;
        }

        private List<Bitmap> LandscapeLandscapeLandscapeLandscape(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmap3, Bitmap bitmap4)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(bitmap1.AddImageSideShrink(bitmap2).AddImageBelowShrink(bitmap3.AddImageSideShrink(bitmap4)));
            images.Add(bitmap1.AddImageSideShrink(bitmap3).AddImageBelowShrink(bitmap2.AddImageSideShrink(bitmap4)));
            images.Add(bitmap1.AddImageSideShrink(bitmap2).AddImageSideShrink(bitmap3).AddImageSideShrink(bitmap4));
            images.Add(bitmap1.AddImageSideCenter(bitmap2).AddImageSideCenter(bitmap3).AddImageSideCenter(bitmap4));
            return images;
        }
        #endregion

        #region helpers
        //https://stackoverflow.com/questions/22499407/how-to-display-a-bitmap-in-a-wpf-image
        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
        #endregion

        #region GUI
        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            CurrentIndex--;
            if (CurrentIndex < 0)
                CurrentIndex = displayImages.Count - 1;

            ImageDisplay.Source = BitmapToImageSource(displayImages[CurrentIndex]);
            UpdateDisplayLabel();
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            CurrentIndex++;
            if (CurrentIndex >= displayImages.Count)
                CurrentIndex = 0;

            ImageDisplay.Source = BitmapToImageSource(displayImages[CurrentIndex]);
            UpdateDisplayLabel();
        }

        private void UpdateDisplayLabel()
        {
            ImageNumberTextBlock.Text = $"{CurrentIndex + 1}/{displayImages.Count}";
        }
        #endregion
    }
}


