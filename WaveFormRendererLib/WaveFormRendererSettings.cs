using System.Drawing;
using System.Drawing.Drawing2D;

namespace NAudio.WaveFormRenderer
{
    public class WaveFormRendererSettings
    {
        protected WaveFormRendererSettings()
        {
            Width = 800;
            TopHeight = 50;
            BottomHeight = 50;
            PixelsPerPeak = 1;
            SpacerPixels = 0;
            BackgroundColor = Color.Beige;
        }

        // for display purposes only
        public string Name { get; set; }

        public int Width { get; set; }

        /// <summary>
        /// Extra space to put above the top height. This leaves room for custom line effects.
        /// </summary>
        public int TopMargin { get; set; }
        public int TopHeight { get; set; }
        /// <summary>
        /// Extra space to put above the top height. This leaves room for custom line effects.
        /// </summary>
        public int BottomMargin { get; set; }
        public int BottomHeight { get; set; }
        public int PixelsPerPeak { get; set; }

        /// <summary>
        /// If set to true, uses a single line for peaks. Make sure that the width
        /// is set accordingly.
        /// </summary>
        public bool UseSingleStrokesForPeak { get; set; } = false;
        public int SpacerPixels { get; set; }
        /// <summary>
        /// If set to true, uses a single line for spacers. Make sure that the width
        /// is set accordingly.
        /// </summary>
        public bool UseSingleStrokesForSpacer { get; set; } = false;
        public virtual Pen TopPeakPen { get; set; }
        public virtual Pen TopSpacerPen { get; set; }
        public virtual Pen BottomPeakPen { get; set; }
        public virtual Pen BottomSpacerPen { get; set; }
        public bool DecibelScale { get; set; }
        public Color BackgroundColor { get; set; }
        public Image BackgroundImage { get; set; }
        public Brush BackgroundBrush {
            get
            {
                if (BackgroundImage == null) return new SolidBrush(BackgroundColor);
                return new TextureBrush(BackgroundImage,WrapMode.Clamp);
            }
        }

        /// <summary>
        /// If set to true, use anti-aliasing when drawing lines.
        /// </summary>
        public bool AntiAlias { get; set; }

        protected static Pen CreateGradientPen(int height, Color startColor, Color endColor)
        {
            var brush = new LinearGradientBrush(new Point(0, 0), new Point(0, height), startColor, endColor);
            return new Pen(brush);
        }
    }
}