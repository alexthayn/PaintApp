using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using System.Drawing;
using System.IO;
using System.Windows.Ink;

namespace HW04_AThayn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SolidColorBrush SelectedColor { get; set; } = Brushes.Black;
        public enum SELECTEDTOOL { LARGEBRUSH=0, BRUSH=1, PENCIL=2, ERASER=3 }
        public int SelectedTool = 2;
        Dictionary<TouchDevice, Ellipse> _Followers = new Dictionary<TouchDevice, Ellipse>();
        private StylusPointCollection stylusPoints;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void BtnColorSelected(object sender, RoutedEventArgs e)
        {
            Button btnSelected = (Button)sender;
            SelectedColor = (SolidColorBrush)btnSelected.Background;
            RctColorSelected.Fill = SelectedColor;
            strokeAttr.Color = SelectedColor.Color;
        }

        public void BtnToolSelected(object sender, RoutedEventArgs e)
        {
            Button btnSelected = (Button)sender;
            switch(btnSelected.Name)
            {
                case "Brush":
                    SelectedTool = (int)SELECTEDTOOL.BRUSH;
                    DrawingCanvas.EditingMode = InkCanvasEditingMode.Ink;
                    strokeAttr.Width = 10;
                    strokeAttr.Height = 10;
                    break;
                case "LargeBrush":
                    SelectedTool = (int)SELECTEDTOOL.LARGEBRUSH;
                    DrawingCanvas.EditingMode = InkCanvasEditingMode.Ink;
                    strokeAttr.Width = 20;
                    strokeAttr.Height = 20;
                    break;
                case "Eraser":
                    SelectedTool = (int)SELECTEDTOOL.ERASER;
                    DrawingCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
                    strokeAttr.Width = 10;
                    strokeAttr.Height = 10;
                    break;
                default:
                    SelectedTool = (int)SELECTEDTOOL.PENCIL;
                    DrawingCanvas.EditingMode = InkCanvasEditingMode.Ink;
                    strokeAttr.Width = 3;
                    strokeAttr.Height = 3;
                    break;
            }            
        }

        private void DrawingCanvas_OnTouchMove(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice.Captured == DrawingCanvas)
            {
                Ellipse follower = _Followers[e.TouchDevice];
                TranslateTransform transform = follower.RenderTransform as TranslateTransform;

                TouchPoint point = e.GetTouchPoint(DrawingCanvas);

                transform.X = point.Position.X;
                transform.Y = point.Position.Y;
            }
        }

        private void DrawingCanvas_OnTouchUp(object sender, TouchEventArgs e)
        {
            DrawingCanvas.ReleaseTouchCapture(e.TouchDevice);

            DrawingCanvas.Children.Remove(_Followers[e.TouchDevice]);
            _Followers.Remove(e.TouchDevice);
        }

        private void DrawingCanvas_OnTouchDown(object sender, TouchEventArgs e)
        {
            DrawingCanvas.CaptureTouch(e.TouchDevice);

            Ellipse follower = new Ellipse();
            follower.Width = follower.Height = 50;
            follower.Fill = SelectedColor;
            follower.Stroke = SelectedColor;

            TouchPoint point = e.GetTouchPoint(DrawingCanvas);

            follower.RenderTransform = new TranslateTransform(point.Position.X, point.Position.Y);

            _Followers[e.TouchDevice] = follower;

            DrawingCanvas.Children.Add(follower);
        }

        private void DrawingCanvas_StylusDown(object sender, StylusDownEventArgs e)
        {

            stylusPoints = new StylusPointCollection();
            StylusPointCollection eventPoints =
                e.GetStylusPoints(this, stylusPoints.Description);

            stylusPoints.Add(eventPoints);
        }

        private void DrawingCanvas_StylusUp(object sender, StylusEventArgs e)
        {
            if (stylusPoints == null)
            {
                return;
            }
            
            StylusPointCollection newStylusPoints =
                e.GetStylusPoints(this, stylusPoints.Description);
            stylusPoints.Add(newStylusPoints);
            
            Stroke stroke = new Stroke(stylusPoints);

            stylusPoints = null;
            
            Stylus.Capture(null);
        }

        private void DrawingCanvas_StylusMove(object sender, StylusEventArgs e)
        {
            if (stylusPoints == null)
            {
                return;
            }
            
            StylusPointCollection newStylusPoints =
                e.GetStylusPoints(this, stylusPoints.Description);
            stylusPoints.Add(newStylusPoints);
        }
    }
}
