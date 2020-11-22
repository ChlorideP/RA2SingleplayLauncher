using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using BDLauncherCSharp.Controls;
using static BDLauncherCSharp.Data.DisplayPanelDataSource;
using BDLauncherCSharp.Extensions;

namespace BDLauncherCSharp
{
    /// <summary>
    /// UserInterface.xaml 的交互逻辑
    /// </summary>
    public partial class UserInterface : GDialog
    {
        /*
         * 三层架构
         * 数据访问层: 用于从系统中获取数据
         * 业务逻辑层: 负责绝大多数的数据处理
         * 视图层: 仅负责向用户展示界面
         * 
         * MVC结构(视图层)
         * Model 模型: 数据模型
         * View 视图: 用户界面
         * Controller 控制器: 决定如何将模型显示到界面上和怎样响应用户操作的东西
         * 
         * MVVM结构(视图层)
         * Model 模型: 数据模型
         * View 视图: 用户界面
         * ViewModel 视图模型: 与视图进行数据绑定 所作的操作会直接或间接的反馈到模型
         * 
         * 这代码太乱了.jpg
         * 
         */

        public UserInterface()
        {
            InitializeComponent();
            this.I18NInitialize();
            cbSize.SelectedValue = ScreenSize_Full();
            cbSize.ItemsSource = ScreeSize_ListAll();
            cbRenderer.ItemsSource = Renderers_ListAll();
        }

        /// <Summary>
        /// 数据验证逻辑
        /// </Summary>
        private void ScreenSize_CheckValidVal(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(cbSize.Text))
                return;
            
            if (!Regex.IsMatch(cbSize.Text, @"\d+\*\d+"))
            {
                MessageBox.Show(I18NExtension.I18N("msgSizeNotValidError"), I18NExtension.I18N("msgCaptain"));
                cbSize.Text = string.Empty;
                return;
            }

            string[] size = cbSize.Text.Split('*');
            if (int.Parse(size[0]) > 2560 || int.Parse(size[1]) > 1440) //2k only
            {
                MessageBox.Show(I18NExtension.I18N("msgSizeTooLargeError"), I18NExtension.I18N("msgCaptain"));
                cbSize.Text = string.Empty;
                return;
            }
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
