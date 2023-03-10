using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<int> strokeThicknesses = new List<int>() { 1, 5, 8, 10, 15, 20, 50 };

        public MainWindow()
        {
            InitializeComponent();
            foreach (int x in strokeThicknesses)
            {
                StrokeThicknesessComboBox.Items.Add(x);
            }
        }

        private void SetDrawingLine(object sender, RoutedEventArgs e)
        {
            drawLine = !drawLine;
        }

        private void Canvas_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                currentPoint = e.GetPosition(this);
        }

        private void Canvas_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Line line = new Line();
                line.Stroke = newColor;
                line.StrokeThickness = Convert.ToDouble(StrokeThicknesessComboBox.SelectedItem.ToString());
                line.X1 = currentPoint.X;
                line.Y1 = currentPoint.Y;
                line.X2 = e.GetPosition(this).X;
                line.Y2 = e.GetPosition(this).Y;
                currentPoint = e.GetPosition(this);
                paintSurface.Children.Add(line);
            }
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            newColor = new SolidColorBrush(_colorPicker.SelectedColor.Value);
        }

        private void CleanCanvas(object sender, RoutedEventArgs e)
        {
            paintSurface.Children.Clear();
        }
    }
}