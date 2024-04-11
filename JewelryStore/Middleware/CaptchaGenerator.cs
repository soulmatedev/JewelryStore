using System;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Label = System.Windows.Controls.Label;

namespace JewelryStore.Middleware
{
    internal class CaptchaGenerator
    {
        private readonly int LABEL_WIDTH = 50;
        private readonly int LABEL_HEIGHT = 50;
        private readonly string CAPTCHA_SYMBOLS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        private readonly Random random;
        private string captchaCode;

        public CaptchaGenerator()
        {
            random = new Random(Environment.TickCount);
        }

        public string GenerateCaptcha(Canvas canvas)
        {
            canvas.Children.Clear();
            captchaCode = GetNewCaptchaCode();

            for (int i = 0; i < captchaCode.Length; i++)
            {
                AddCharToCanvas(i, captchaCode[i], canvas);
            }

            GenerateNoise(canvas);
            return captchaCode;
        }

        private string GetNewCaptchaCode()
        {
            captchaCode = "";

            for (int i = 0; i < 4; i++)
            {
                int index = random.Next(CAPTCHA_SYMBOLS.Length);
                captchaCode += CAPTCHA_SYMBOLS[index];
            }

            return captchaCode;
        }

        private Label AddCharToCanvas(int index, char ch, Canvas canvas)
        {
            Label label = new Label();
            label.Content = ch.ToString();
            label.FontSize = random.Next(24, 36);
            label.Width = LABEL_WIDTH;
            label.Height = LABEL_HEIGHT;
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.VerticalContentAlignment = VerticalAlignment.Center;
            label.RenderTransformOrigin = new Point(0.5, 0.5);
            label.RenderTransform = new RotateTransform(random.Next(-20, 20));
            label.FontFamily = new FontFamily("Comic Sans MS");
            label.Foreground = new SolidColorBrush(Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)));

            canvas.Children.Add(label);

            double startPosition = (canvas.ActualWidth / 2) - (LABEL_WIDTH * 2);

            Canvas.SetLeft(label, startPosition + (index * LABEL_WIDTH));
            Canvas.SetTop(label, random.Next(0, 10));

            return label;
        }

        private void GenerateNoise(Canvas canvas)
        {
            int ellipseCount = random.Next(50, 150);
            for (int i = 1; i < ellipseCount; i++)
            {
                double x = random.NextDouble() * 400;
                double y = random.NextDouble() * 50;

                int radius = random.Next(2, 6);
                Ellipse ellipse = new Ellipse
                {
                    Width = radius,
                    Height = radius,
                    Fill = new SolidColorBrush(Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256))),
                    Stroke = Brushes.Transparent,
                };
                canvas.Children.Add(ellipse);
                Canvas.SetLeft(ellipse, x);
                Canvas.SetTop(ellipse, y);
            }

            int lineCount = random.Next(1, 4);
            for (int i = 0; i < lineCount; i++)
            {
                Line line = new Line
                {
                    X1 = random.Next(80, 150),
                    Y1 = random.Next(10, 70),
                    X2 = random.Next(240, 280),
                    Y2 = random.Next(10, 70),
                    Stroke = new SolidColorBrush(Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256))),
                    StrokeThickness = 1,
                };

                canvas.Children.Add(line);
            }
        }
    }

}
