using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using NAudio.Wave;

namespace NAudio.WaveFormRenderer
{
    public class WaveFormRenderer
    {
        public Image Render(WaveStream waveStream, WaveFormRendererSettings settings)
        {
            return Render(waveStream, new MaxPeakProvider(), settings);
        }        

        public Image Render(WaveStream waveStream, IPeakProvider peakProvider, WaveFormRendererSettings settings)
        {
            int bytesPerSample = (waveStream.WaveFormat.BitsPerSample / 8);
            var samples = waveStream.Length / (bytesPerSample);
            var samplesPerPixel = (int)(samples / settings.Width);
            var stepSize = settings.PixelsPerPeak + settings.SpacerPixels;
            var readSize = Align(samplesPerPixel * stepSize, waveStream.BlockAlign);
            peakProvider.Init(waveStream.ToSampleProvider(), readSize);
            return Render(peakProvider, settings);
        }

        private static int Align(int value, int size)
        {
            return ((value + size - 1) / size) * size;
        }

        private static Image Render(IPeakProvider peakProvider, WaveFormRendererSettings settings)
        {
            if (settings.DecibelScale)
                peakProvider = new DecibelPeakProvider(peakProvider, 48);

            var b = new Bitmap(settings.Width, settings.TopHeight + settings.BottomHeight + settings.TopMargin + settings.BottomMargin);
            if (settings.BackgroundColor == Color.Transparent)
            {
                b.MakeTransparent();
            }
            using (var g = Graphics.FromImage(b))
            {
                g.FillRectangle(settings.BackgroundBrush, 0,0,b.Width,b.Height);
                if ( settings.AntiAlias )
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                var midPoint = settings.TopHeight + settings.TopMargin;

                int x = 0;
                var currentPeak = peakProvider.GetNextPeak();
                while (x < settings.Width)
                {
                    var nextPeak = peakProvider.GetNextPeak();

                    if (settings.UseSingleStrokesForPeak)
                    {
                        var lineHeight = settings.TopHeight * currentPeak.Max;
                        var midX = x + settings.PixelsPerPeak * 0.5f;
                        g.DrawLine(settings.TopPeakPen, midX, midPoint, midX, midPoint - lineHeight);
                        lineHeight = settings.BottomHeight * currentPeak.Min;
                        g.DrawLine(settings.BottomPeakPen, midX, midPoint, midX, midPoint - lineHeight);
                        x += settings.PixelsPerPeak;
                    }
                    else
                    {
                        for (int n = 0; n < settings.PixelsPerPeak; n++)
                        {
                            var lineHeight = settings.TopHeight * currentPeak.Max;
                            g.DrawLine(settings.TopPeakPen, x, midPoint, x, midPoint - lineHeight);
                            lineHeight = settings.BottomHeight * currentPeak.Min;
                            g.DrawLine(settings.BottomPeakPen, x, midPoint, x, midPoint - lineHeight);
                            x++;
                        }
                    }

                    // spacer bars are always the lower of the 
                    var max = Math.Min(currentPeak.Max, nextPeak.Max);
                    var min = Math.Max(currentPeak.Min, nextPeak.Min);

                    if (settings.UseSingleStrokesForSpacer)
                    {
                        var midX = x + settings.SpacerPixels * 0.5f;
                        var lineHeight = settings.TopHeight * max;
                        g.DrawLine(settings.TopSpacerPen, midX, midPoint, midX, midPoint - lineHeight);
                        lineHeight = settings.BottomHeight * min;
                        g.DrawLine(settings.BottomSpacerPen, midX, midPoint, midX, midPoint - lineHeight);
                        x += settings.SpacerPixels;
                    }
                    else
                    {

                        for (int n = 0; n < settings.SpacerPixels; n++)
                        {
                            var lineHeight = settings.TopHeight * max;
                            g.DrawLine(settings.TopSpacerPen, x, midPoint, x, midPoint - lineHeight);
                            lineHeight = settings.BottomHeight * min;
                            g.DrawLine(settings.BottomSpacerPen, x, midPoint, x, midPoint - lineHeight);
                            x++;
                        }
                    }

                    currentPeak = nextPeak;
                }
            }
            return b;
        }


    }
}
