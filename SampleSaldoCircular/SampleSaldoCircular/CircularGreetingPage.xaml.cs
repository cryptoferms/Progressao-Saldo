using SampleSaldoCircular.Helper;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleSaldoCircular
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CircularGreetingPage : ContentPage
    {
        //https://medium.com/@bertuzzi/xamarin-rocket-8-indicador-de-progresso-fe512e8812da

        //definindo valores absolutos para testar a solução
        const int LIMITE_DISPONIVEL = 500;
        const int LIMITE_TOTAL = 1000;

        SKPaintSurfaceEventArgs args;

        //chamando a classe para ajudar com a elaboração do grafico circular.
        ProgressHelpers progressHelpers = new ProgressHelpers();

        public CircularGreetingPage()
        {
            InitializeComponent();
            //chamada do método que ira inicializar nossa barra de progressão
            InitiateProgressUpdate();

            var total = LIMITE_TOTAL;
            var disponivel = LIMITE_DISPONIVEL;
            saldototal.Text = "R$" + total.ToString();
            saldodisponivel.Text = "R$" + disponivel.ToString();
        }
        void canvas_PaintSurface(object sender, SKPaintSurfaceEventArgs args1)
        {
            args = args1;
            DrawGaugeAsync();
        }
        async void SwitchToggledAsync(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            await InitiateProgressUpdate();
        }
        void SliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (canvas != null)
            {
                canvas.InvalidateSurface();
            }
        }
        async Task AnimateProgress(int progress)
        {
            sw_listToggle.IsEnabled = false;
            sweepAngleSlider.Value = 1;

            for (int i = 0; i < progress; i = i + 5)
            {
                sweepAngleSlider.Value = i;
                await Task.Delay(3);
            }

            sweepAngleSlider.Value = progress;
            sw_listToggle.IsEnabled = true;
        }

        async Task InitiateProgressUpdate()
        {
            if (sw_listToggle.IsToggled)
            {
                await AnimateProgress(progressHelpers.GetSweepAngle(LIMITE_TOTAL, LIMITE_DISPONIVEL));
            }
            else
            {
                await AnimateProgress(progressHelpers.GetSweepAngle(LIMITE_TOTAL / 30, LIMITE_DISPONIVEL));
            }
        }
        public void DrawGaugeAsync()
        {
            int uPadding = 150;
            int side = 500;
            int radialGaugeWidth = 25;

            int lineSize1 = 220;
            int lineSize2 = 70;
            int lineSize3 = 80;

            int lineHeight1 = 100;
            int lineHeight2 = 200;
            int lineHeight3 = 300;

            float startAngle = -220;
            float sweepAngle = 260;

            try
            {

                SKImageInfo info = args.Info;
                SKSurface surface = args.Surface;
                SKCanvas canvas = surface.Canvas;
                progressHelpers.SetDevice(info.Height, info.Width);
                canvas.Clear();

                float upperPading = progressHelpers.GetFactoredHeight(uPadding);

                int Xc = info.Width / 2;
                float Yc = progressHelpers.GetFactoredHeight(side);


                int X1 = (int)(Xc - Yc);
                int Y1 = (int)(Yc - Yc + upperPading);


                int X2 = (int)(Xc + Yc);
                int Y2 = (int)(Yc + Yc + upperPading);

                SKPaint paint1 = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = Color.FromHex("#fc0b03").ToSKColor(),
                    StrokeWidth = progressHelpers.GetFactoredWidth(radialGaugeWidth),
                    StrokeCap = SKStrokeCap.Round
                };

                SKPaint paint2 = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = Color.FromHex("#05c782").ToSKColor(),
                    StrokeWidth = progressHelpers.GetFactoredWidth(radialGaugeWidth),
                    StrokeCap = SKStrokeCap.Round
                };

                SKRect rect = new SKRect(X1, Y1, X2, Y2);

                SKPath path1 = new SKPath();
                path1.AddArc(rect, startAngle, sweepAngle);
                canvas.DrawPath(path1, paint1);

                SKPath path2 = new SKPath();
                path2.AddArc(rect, startAngle, (float)sweepAngleSlider.Value);
                canvas.DrawPath(path2, paint2);

                using (SKPaint sKPaint = new SKPaint())
                {
                    sKPaint.Style = SKPaintStyle.Fill;
                    sKPaint.IsAntialias = true;
                    sKPaint.Color = SKColor.Parse("#0000");
                    sKPaint.TextAlign = SKTextAlign.Center;
                    sKPaint.TextSize = progressHelpers.GetFactoredHeight(lineSize3);
                    canvas.DrawText("Limite Disponível", Xc, Yc + progressHelpers.GetFactoredHeight(lineHeight1), sKPaint);
                }

                using (SKPaint skPaint = new SKPaint())
                {
                    skPaint.Style = SKPaintStyle.Fill;
                    skPaint.IsAntialias = true;
                    skPaint.Color = SKColor.Parse("#171717");
                    skPaint.TextAlign = SKTextAlign.Center;
                    skPaint.TextSize = progressHelpers.GetFactoredHeight(lineHeight2);
                    skPaint.TextSize = progressHelpers.GetFactoredHeight(lineSize1);
                    skPaint.Typeface = SKTypeface.FromFamilyName(
                                        "Arial",
                                        SKFontStyleWeight.Bold,
                                        SKFontStyleWidth.Normal,
                                        SKFontStyleSlant.Upright);
                    canvas.DrawText($"R${LIMITE_DISPONIVEL} ", Xc, Yc + progressHelpers.GetFactoredHeight(lineHeight1), skPaint);
                }

                using (SKPaint skPaint = new SKPaint())
                {
                    skPaint.Style = SKPaintStyle.Fill;
                    skPaint.IsAntialias = true;
                    skPaint.Color = SKColor.Parse("#676a69");
                    skPaint.TextAlign = SKTextAlign.Center;
                    skPaint.TextSize = progressHelpers.GetFactoredHeight(lineSize2);
                    canvas.DrawText("Limite", Xc, Yc + progressHelpers.GetFactoredHeight(lineHeight2), skPaint);
                }

                using (SKPaint skPaint = new SKPaint())
                {
                    skPaint.Style = SKPaintStyle.Fill;
                    skPaint.IsAntialias = true;
                    skPaint.Color = SKColor.Parse("#e2797a");
                    skPaint.TextAlign = SKTextAlign.Center;
                    skPaint.TextSize = progressHelpers.GetFactoredHeight(lineSize3);

                    if (sw_listToggle.IsToggled)
                    {
                        canvas.DrawText($"Total R$ {LIMITE_TOTAL} ", Xc, Yc + progressHelpers.GetFactoredHeight(lineHeight3), skPaint);
                    }
                    else
                    {
                        canvas.DrawText($"Utilizado R$ {(LIMITE_TOTAL - LIMITE_DISPONIVEL)}", Xc, Yc + progressHelpers.GetFactoredHeight(lineHeight3), skPaint);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }
    }
}