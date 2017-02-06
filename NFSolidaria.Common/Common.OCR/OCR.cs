using System.Drawing;
using System.Web;
using Tesseract;

namespace Common.OCR
{
    public class OCR
    {
        private float confidence;
        private string text;

        public OCR(Image image)
        {
            this.ReadImage(image);
        }

        public string GetText()
        {
            return this.text;
        }

        public float GetConfidence()
        {
            return this.confidence;
        }

        private void ReadImage(Image image)
        {
            var path = HttpContext.Current.Server.MapPath(@"~/tessdata");

            using (var engine = new TesseractEngine(path, "por", EngineMode.TesseractOnly))
            {
                using (var item = new Bitmap(image))
                {
                    using (var pix = PixConverter.ToPix(item))
                    {
                        using (var page = engine.Process(pix))
                        {
                            confidence = page.GetMeanConfidence();
                            text = page.GetText();
                            page.Dispose();
                        }

                        pix.Dispose();
                    }

                    item.Dispose();
                }

                engine.Dispose();
            }
        }

    }
}
