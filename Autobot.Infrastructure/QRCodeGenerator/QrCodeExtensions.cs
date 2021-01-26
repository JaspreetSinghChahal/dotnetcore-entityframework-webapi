
namespace SkiaSharp.QrCode
{
    public static class QrCodeExtensions
    {
        public static void Render(this SKCanvas canvas, QRCodeData data, int width, int hight)
        {
            canvas.Clear(SKColors.Transparent);

            using (var renderer = new QRCodeRenderer())
            {
                var area = SKRect.Create(0, 0, width, hight);
                renderer.Render(canvas, area, data);
            }
        }

        public static void Render(this SKCanvas canvas, QRCodeData data, int width, int hight, SKColor clearColor)
        {
            canvas.Clear(clearColor);

            using (var renderer = new QRCodeRenderer())
            {
                var area = SKRect.Create(0, 15, width, hight);
                renderer.Render(canvas, area, data);
            }
        }

        public static void Render(this SKCanvas canvas, QRCodeData data, SKRect area, SKColor clearColor)
        {
            canvas.Clear(clearColor);

            using (var renderer = new QRCodeRenderer())
            {
                renderer.Render(canvas, area, data);
            }
        }

        public static void Render(this SKCanvas canvas, QRCodeData data, int width, int height, string color, string points, string codeNumber)
        {
            canvas.Clear(SKColors.Transparent);
            using (var renderer = new QRCodeRenderer())
            {
                var area = SKRect.Create(0, 0, width, height);
                var dimensions = renderer.Render(canvas, area, data, color);
                RenderText(canvas, dimensions, points, codeNumber, color);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="dimensions"></param>
        /// <param name="points"></param>
        /// <param name="codeNumber"></param>
        private static void RenderText(SKCanvas canvas, float[] dimensions, string points, string codeNumber, string color)
        {
            float textHeight = ((dimensions[1] - dimensions[0]) * 10) / 100;
            float textGap = ((dimensions[1] - dimensions[0]) * 2) / 100;
            if (dimensions == null)
            {
                return;
            }

            var paint = new SKPaint
            {
                TextSize = textHeight,
                Color = string.IsNullOrEmpty(color) ? SKColors.Black : SKColor.Parse(color),
                Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright)
            };

            if (!string.IsNullOrEmpty(points))
            {
                //x0,y0 
                float xPos = dimensions[0];
                float yPos = dimensions[2] - textGap;
                canvas.DrawText(points, xPos, yPos, paint);
            }

            if (!string.IsNullOrEmpty(codeNumber))
            {
                //(x0-x1)/2,y1 
                float textWidth = paint.MeasureText(codeNumber);
                float xPos = (dimensions[1] - dimensions[0]) / 2 + dimensions[0] - (textWidth / 2);
                float yPos = dimensions[3] + textHeight + textGap;
                canvas.DrawText(codeNumber, xPos, yPos, paint);
            }
        }
    }
}
