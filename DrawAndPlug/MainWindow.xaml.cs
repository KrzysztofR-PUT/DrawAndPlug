﻿using System;
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

namespace DrawAndPlug
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Ink.StrokeCollection _added;
        System.Windows.Ink.StrokeCollection _removed;
        private bool handle = true;

        public MainWindow()
        {
            InitializeComponent();
            inkCanvas.Strokes.StrokesChanged += Strokes_StrokesChanged;
        }

        private void Strokes_StrokesChanged(object sender, System.Windows.Ink.StrokeCollectionChangedEventArgs e)
        {
            if (handle)
            {
                _added = e.Added;
                _removed = e.Removed;
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

        private void AddPlugin_Clicked(object sender, RoutedEventArgs e)
        {
            
        }

        private void GreenStroke_Clicked(object sender, RoutedEventArgs e)
        {
            inkCanvas.DefaultDrawingAttributes.Color = Colors.Green;
        }

        private void BlackStroke_Clicked(object sender, RoutedEventArgs e)
        {
            inkCanvas.DefaultDrawingAttributes.Color = Colors.Black;
        }

        private void BlueStroke_Clicked(object sender, RoutedEventArgs e)
        {
            inkCanvas.DefaultDrawingAttributes.Color = Colors.Blue;
        }

        private void RedStroke_Clicked(object sender, RoutedEventArgs e)
        {
            inkCanvas.DefaultDrawingAttributes.Color = Colors.Red;
        }

        private void Clear_Clicked(object sender, RoutedEventArgs e)
        {
            inkCanvas.Strokes.Clear();
        }
    }
}
