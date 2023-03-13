using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Reflection;
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

namespace CanvasApp
{
    public partial class MainWindow : Window
    {
        private Point currentPoint = new Point();
        private Brush newColor = new SolidColorBrush();
        private bool drawLine = false;
        private bool startOfNewLine = false;
        private Line straightLine = new Line();
        private List<int> strokeThicknesses = new List<int>() { 1, 5, 8, 10, 15, 20, 50 };

        public MainWindow()
        {
            InitializeComponent();
            foreach (int x in strokeThicknesses)
            {
                StrokeThicknesessComboBox.Items.Add(x);
            }
            StrokeThicknesessComboBox.SelectedItem = strokeThicknesses[0];
        }

        private void SetDrawingLine(object sender, RoutedEventArgs e)
        {
            drawLine = !drawLine;
            startOfNewLine = false;
        }

        private void Canvas_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                currentPoint = e.GetPosition(this);
            }

            if (drawLine)
            {
                if (e.LeftButton == MouseButtonState.Pressed && !startOfNewLine)
                {
                    straightLine = new Line();
                    straightLine.Stroke = newColor;
                    straightLine.StrokeThickness = Convert.ToDouble(StrokeThicknesessComboBox.SelectedItem.ToString());
                    straightLine.X1 = currentPoint.X;
                    straightLine.Y1 = CorrectCurrentY(currentPoint.Y);
                    startOfNewLine = true;
                }
                else if (e.LeftButton == MouseButtonState.Pressed && startOfNewLine)
                {
                    straightLine.X2 = currentPoint.X;
                    straightLine.Y2 = CorrectCurrentY(currentPoint.Y);
                    startOfNewLine = false;
                    paintSurface.Children.Add(straightLine);
                }
            }
            else
            {
                Ellipse circle = new Ellipse();
                circle.Width = Convert.ToDouble(StrokeThicknesessComboBox.SelectedItem.ToString());
                circle.Height = Convert.ToDouble(StrokeThicknesessComboBox.SelectedItem.ToString());
                Canvas.SetLeft(circle, currentPoint.X - Math.Floor(circle.Width / 2));
                Canvas.SetTop(circle, CorrectCurrentY(currentPoint.Y) - Math.Floor(circle.Height / 2));
                SetShapeProperties(circle);
                circle.Fill = newColor;

                paintSurface.Children.Add(circle);
            }
        }

        private void Canvas_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (!drawLine)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Line line = new Line();
                    SetShapeProperties(line);
                    line.X1 = currentPoint.X;
                    line.Y1 = CorrectCurrentY(currentPoint.Y);
                    line.X2 = e.GetPosition(this).X;
                    line.Y2 = e.GetPosition(this).Y - SettingsPanel.Height - 5; ;
                    currentPoint = e.GetPosition(this);
                    paintSurface.Children.Add(line);
                }
            }
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            newColor = new SolidColorBrush(_colorPicker.SelectedColor!.Value);
        }

        private void CleanCanvas(object sender, RoutedEventArgs e)
        {
            paintSurface.Children.Clear();
        }

        private void SetShapeProperties(Shape shape)
        {
            shape.Stroke = newColor;
            shape.StrokeThickness = Convert.ToDouble(StrokeThicknesessComboBox.SelectedItem.ToString());
        }

        private double CorrectCurrentY(double y)
        {
            return y - SettingsPanel.Height - 5;
        }
    }
}