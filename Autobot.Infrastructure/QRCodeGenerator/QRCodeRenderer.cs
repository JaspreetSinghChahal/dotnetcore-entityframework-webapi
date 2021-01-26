using System;
using System.Linq;

namespace SkiaSharp.QrCode
{
    public class QRCodeRenderer : IDisposable
    {
        /// <summary>
        /// Gets the paint.
        /// </summary>
        /// <value>The paint.</value>
        public SKPaint Paint { get; } = new SKPaint();

        /// <summary>
        /// Render the specified data into the given area of the target canvas.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="area">The area.</param>
        /// <param name="data">The data.</param>
        /// Return cordinates of box
        public float[] Render(SKCanvas canvas, SKRect area, QRCodeData data, string color = null)
        {
            if (data != null)
            {
                if (!string.IsNullOrEmpty(color))
                {
                    this.Paint.Color = SKColor.Parse(color);
                }
                var rows = data.ModuleMatrix.Count;
                var columns = data.ModuleMatrix.Select(x => x.Length).Max();
                var cellHeight = area.Height / rows;
                var cellWidth = area.Width / columns;

                for (int y = 0; y < rows; y++)
                {
                    var row = data.ModuleMatrix.ElementAt(y);
                    for (int x = 0; x < row.Length; x++)
                    {
                        if (row[x])
                        {
                            var rect = SKRect.Create(area.Left + x * cellWidth, area.Top + y * cellHeight, cellWidth, cellHeight);
                            canvas.DrawRect(rect, this.Paint);
                        }
                    }
                }
                return GetDimensions(data, area);
            }
            return null;
        }

        private float[] GetDimensions(QRCodeData data, SKRect area)
        {
            float[] dimensions = new float[4];
            float x0 = 0, y0 = 0, x1 = 0, y1 = 0;
            var rows = data.ModuleMatrix.Count;
            var columns = data.ModuleMatrix.Select(x => x.Length).Max();
            var cellHeight = area.Height / rows;
            var cellWidth = area.Width / columns;
            //0-3 is quiet zone
            var firstRow = data.ModuleMatrix.ElementAt(4);
            // first true
            for (int x = 0; x < firstRow.Length; x++)
            {
                if (firstRow[x])
                {
                    x0 = area.Left + x * cellWidth;
                    y0 = area.Top + 4 * cellHeight;
                    break;
                }
            }
            //last true
            for (int x = firstRow.Length - 1; x > 0; x--)
            {
                if (firstRow[x])
                {
                    x1 = area.Left + x * cellWidth + cellWidth;
                    break;
                }
            }
            //Last row
            var lastRow = data.ModuleMatrix.ElementAt(data.ModuleMatrix.Count - 5);
            //last true
            for (int x = lastRow.Length - 1; x > 0; x--)
            {
                if (lastRow[x])
                {
                    y1 = area.Top + (data.ModuleMatrix.Count - 5) * cellHeight;
                    break;
                }
            }
            dimensions[0] = x0;
            dimensions[1] = x1;
            dimensions[2] = y0;
            dimensions[3] = y1;
            return dimensions;
        }

        /// <summary>
        /// Releases all resource used by the <see cref="T:SkiaSharp.QRCodeGeneration.QRCodeRenderer"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the
        /// <see cref="T:SkiaSharp.QRCodeGeneration.QRCodeRenderer"/>. The <see cref="Dispose"/> method leaves the
        /// <see cref="T:SkiaSharp.QRCodeGeneration.QRCodeRenderer"/> in an unusable state. After calling
        /// <see cref="Dispose"/>, you must release all references to the
        /// <see cref="T:SkiaSharp.QRCodeGeneration.QRCodeRenderer"/> so the garbage collector can reclaim the memory
        /// that the <see cref="T:SkiaSharp.QRCodeGeneration.QRCodeRenderer"/> was occupying.</remarks>
        public void Dispose()
        {
            this.Paint.Dispose();
        }
    }
}
