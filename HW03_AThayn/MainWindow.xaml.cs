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

namespace HW03_AThayn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Brush _selectedColor = Brushes.White;

        public MainWindow()
        {
            InitializeComponent();
        }

        public Brush SelectedColor
        {
            get
            {
                return _selectedColor;
            }
            set
            {
                _selectedColor = value;
            }
        }

        public void BtnColorSelected(object sender, RoutedEventArgs e)
        {
            Button btnSelected = (Button)sender;
            SelectedColor = btnSelected.Background;
            RctColorSelected.Fill = SelectedColor;
        }

        public void BtnPageSelected(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Image image = new Image();

            switch(button.Content)
            {
                case "Llama":
                    image.Source = new BitmapImage(new Uri("pack://application:,,,/ColoringPages/Llama.jpg"));                    
                    break;
                case "Bear":
                    image.Source = new BitmapImage(new Uri("pack://application:,,,/ColoringPages/Bear.jpg"));
                    break;
                case "Bird":
                    image.Source = new BitmapImage(new Uri("pack://application:,,,/ColoringPages/Bird.jpg"));
                    break;
                case "Turtle":
                    image.Source = new BitmapImage(new Uri("pack://application:,,,/ColoringPages/Turtle.jpg"));
                    break;
                case "Fish":
                    image.Source = new BitmapImage(new Uri("pack://application:,,,/ColoringPages/Fish.jpg"));
                    break;
                default:
                    image.Source = new BitmapImage(new Uri("pack://application:,,,/ColoringPages/Pig.gif"));
                    break;                   
            }
            image.Stretch = Stretch.Uniform;
            ColoringPage.Source = image.Source;
        }
    }
}
