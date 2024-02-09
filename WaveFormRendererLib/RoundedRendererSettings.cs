using System.Drawing;
using System.Drawing.Drawing2D;

namespace NAudio.WaveFormRenderer
{
    public class RoundedRendererSettings : WaveFormRendererSettings
    {
        public RoundedRendererSettings(Color topPeakColor, Color bottomPeakColor, int width)
        {
            PixelsPerPeak = width;
            UseSingleStrokesForPeak = true;
            topPeakPen = new Pen(topPeakColor) { EndCap = LineCap.Round, Width = width };
            bottomPeakPen = new Pen(bottomPeakColor) { EndCap = LineCap.Round, Width = width };
            TopMargin = width;
            BottomMargin = TopMargin;
            AntiAlias = true;
        }

        private Pen topPeakPen;
        public override Pen TopPeakPen
        {
            get { return topPeakPen; }
            set { topPeakPen = value; }
        }

        private Pen bottomPeakPen;
        public override Pen BottomPeakPen
        {
            get { return bottomPeakPen; }
            set { bottomPeakPen = value; }
        }
    }
}