using Shimakaze.Struct.Ini;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BDLauncherCSharp.Controls;
using System.Text.RegularExpressions;

namespace BDLauncherCSharp
{
    /// <summary>
    /// UserInterface.xaml 的交互逻辑
    /// </summary>
    public partial class UserInterface : GDialog
    {
        public UserInterface()
        {
            InitializeComponent();
        }

        private void cbSize_TextChanged(object sender, KeyEventArgs e)
        {
            Regex r = new Regex(@"\d\*\d |\d +?");

        }

        private void Difficulty_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            (sender as Slider).Value = Math.Ceiling((sender as Slider).Value);
        }

        private void GDialog_SecondButtonClick(Button sender, RoutedEventArgs e)
        {

        }

        private void GDialog_PrimaryButtonClick(Button sender, RoutedEventArgs e)
        {

        }
    }
}
