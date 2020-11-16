using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using BDLauncherCSharp.Controls;
using BDLauncherCSharp.Extensions;

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
            this.I18NInitialize();
        }

        private void cbSize_TextChanged(object sender, KeyEventArgs e)
        {
            Regex r = new Regex(@"\d\*\d |\d +?");

        }

        private void Difficulty_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            (sender as Slider).Value = Math.Ceiling((sender as Slider).Value);
        }

        protected override void PrimaryButton_Click(object sender, RoutedEventArgs e)
        {

            base.PrimaryButton_Click(sender, e);
        }
    }
}
