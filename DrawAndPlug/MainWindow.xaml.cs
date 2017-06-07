using System;
using System.Collections.Generic;
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
using Microsoft.Win32;
using System.IO;
using System.Reflection;
using PluginInterface;

namespace DrawAndPlug
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string pluginDir = @".\plugins\";
        System.Windows.Ink.StrokeCollection _added;
        System.Windows.Ink.StrokeCollection _removed;
        private bool handle = true;
        private bool rectangleMode = false;

        private Point startingPosition;
        private Point endingPosition;

        private Color selectedColor = Colors.Black;

        public MainWindow()
        {
            InitializeComponent();
            LoadPlugins();
            inkCanvas.Strokes.StrokesChanged += Strokes_StrokesChanged;
            //inkCanvas.UseCustomCursor = true;                     //zmienia kursor
            //inkCanvas.EditingMode = InkCanvasEditingMode.None;    //zabrania rysowania

            /*Ellipse myEllipse = new Ellipse();                    //rysowanie elipsy
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);
            myEllipse.Fill = mySolidColorBrush;
            myEllipse.StrokeThickness = 2;
            myEllipse.Stroke = Brushes.Black;
            myEllipse.Width = 200;
            myEllipse.Height = 100;
            inkCanvas.Children.Add(myEllipse);*/
        }



        private void AddPlugin_Clicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Plugins (*.dll)|*dll";

            if (openFileDialog.ShowDialog() == true)
            {
                if (!Directory.Exists(pluginDir))
                    Directory.CreateDirectory(pluginDir);

                File.Copy(openFileDialog.FileName, pluginDir + openFileDialog.SafeFileName);
            }
        }

        private void LoadPlugins()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(pluginDir);
            if (directoryInfo.Exists) {
                foreach (var file in directoryInfo.GetFiles("*.dll"))
                {
                    var assembly = Assembly.LoadFrom(file.FullName);
                    var types = assembly.GetTypes();

                    foreach (var type in types)
                    {
                        if (type.IsClass && type.IsPublic && typeof(IPlugin).IsAssignableFrom(type))
                        {
                            var obj = (IPlugin)Activator.CreateInstance(type);
                            Button button = new Button();
                            button.Content = obj.GetOperationName();
                            Separator separator = new Separator();
                            pluginBar.Items.Add(separator);
                            pluginBar.Items.Add(button);
                        }
                    }
                }
            }
        }

        private void Save_Clicked(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG file (*.png)|*.png";
            if (saveFileDialog.ShowDialog() == true)
            {
                BitmapFrame frame = InkOperation.CreateBitmapSource(inkCanvas);

                FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create);
                var encoder = new PngBitmapEncoder();
                encoder.Interlace = PngInterlaceOption.On;
                encoder.Frames.Add(frame);
                encoder.Save(stream);
                stream.Close();
            }
        }

        private void Undo_Clicked(object sender, RoutedEventArgs e)
        {
            handle = false;
            inkCanvas.Strokes.Remove(_added);
            inkCanvas.Strokes.Add(_removed);
            handle = true;
        }

        private void Redo_Clicked(object sender, RoutedEventArgs e)
        {
            handle = false;
            inkCanvas.Strokes.Add(_added);
            inkCanvas.Strokes.Remove(_removed);
            handle = true;
        }
        
        private void Strokes_StrokesChanged(object sender, System.Windows.Ink.StrokeCollectionChangedEventArgs e)
        {
            if (handle)
            {
                _added = e.Added;
                _removed = e.Removed;
            }
        }

        private void GreenStroke_Clicked(object sender, RoutedEventArgs e)
        {
            selectedColor = Colors.Green;
            inkCanvas.DefaultDrawingAttributes.Color = selectedColor;
        }

        private void BlackStroke_Clicked(object sender, RoutedEventArgs e)
        {
            selectedColor = Colors.Black;
            inkCanvas.DefaultDrawingAttributes.Color = selectedColor;
        }

        private void BlueStroke_Clicked(object sender, RoutedEventArgs e)
        {
            selectedColor = Colors.Blue;
            inkCanvas.DefaultDrawingAttributes.Color = selectedColor;
        }

        private void RedStroke_Clicked(object sender, RoutedEventArgs e)
        {
            selectedColor = Colors.Red;
            inkCanvas.DefaultDrawingAttributes.Color = selectedColor;
        }

        private void Clear_Clicked(object sender, RoutedEventArgs e)
        {
            inkCanvas.Strokes.Clear();
            inkCanvas.Children.Clear();
        }

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (rectangleMode)
            {
                startingPosition = e.GetPosition(inkCanvas);
            }
        }

        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (rectangleMode)
            {
                endingPosition = e.GetPosition(inkCanvas);
                DrawRectangle();
            }
        }

        private void PenTool_Clicked(object sender, RoutedEventArgs e)
        {
            rectangleMode = false;
            inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            inkCanvas.UseCustomCursor = false;
        }

        private void RectangleTool_Clicked(object sender, RoutedEventArgs e)
        {
            rectangleMode = true;
            inkCanvas.EditingMode = InkCanvasEditingMode.None;
            inkCanvas.UseCustomCursor = true;
        }

        private void DrawRectangle()
        {
            double width = Math.Abs(startingPosition.X - endingPosition.X);
            double heigth = Math.Abs(startingPosition.Y - endingPosition.Y);

            Rectangle rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(selectedColor);
            rect.Fill = new SolidColorBrush(selectedColor);
            rect.Width = width;
            rect.Height = heigth;

            if (startingPosition.X < endingPosition.X)
                InkCanvas.SetLeft(rect, startingPosition.X);
            else
                InkCanvas.SetLeft(rect, endingPosition.X);

            if (startingPosition.Y < endingPosition.Y)
                InkCanvas.SetTop(rect, startingPosition.Y);
            else
                InkCanvas.SetTop(rect, endingPosition.Y);

            inkCanvas.Children.Add(rect);
        }
    }
}
