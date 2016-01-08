using LeratHarborMap.TechnicalCore;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;

namespace LeratHarborMap.DomainModel
{
    public class DrawerName
    {
        /// <summary>
        /// Gets the harbor map image path
        /// </summary>
        public string HarborMapImagePath { get; private set; }

        public Harbor Harbor { get; private set; }

        public DrawerName(string harborMapImagePath, Harbor harbor)
        {
            HarborMapImagePath = harborMapImagePath;
            Harbor = harbor;
        }

        /// <summary>
        /// Draw the name on the map
        /// </summary>
        /// <returns>Return the full path of the generated image</returns>
        public string Draw()
        {
            Logger.Info("Start Drawing names");

            string generatedFileName = "PlanLeratAvecNoms.jpg";

            Bitmap bmp = new Bitmap(HarborMapImagePath);
            Graphics mainGraphic = Graphics.FromImage(bmp);

            mainGraphic.PageUnit = GraphicsUnit.Pixel;
            mainGraphic.SmoothingMode = SmoothingMode.AntiAlias;
            // g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            mainGraphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            mainGraphic.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

            //DrawLineA_HardVersion(mainGraphic);
            DrawLines(mainGraphic);

            mainGraphic.Flush();
            Logger.Info("Saving drawing to file {0}", generatedFileName);
            bmp.Save(generatedFileName);

            return generatedFileName;
        }

        private void DrawLines(Graphics mainGraphic)
        {
            foreach (Line line in Harbor.Lines)
            {
                Logger.Debug("Drawing line {0}", line.Name);
                foreach (var anchor in line.Anchorages)
                {
                    var container = mainGraphic.BeginContainer();
                    mainGraphic.TranslateTransform(anchor.Coord.X, anchor.Coord.Y);
                    mainGraphic.RotateTransform(line.Rotation);
                    RectangleF rectf = new RectangleF(0, 0, 250, 995);
                    mainGraphic.DrawString(anchor.LastName, new Font("Tahoma", 8), Brushes.Red, rectf);
                    mainGraphic.EndContainer(container);
                }
            }
        }
    }
}
