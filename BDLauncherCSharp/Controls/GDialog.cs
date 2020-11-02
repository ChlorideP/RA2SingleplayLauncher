using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;


namespace BDLauncherCSharp.Controls
{
    public enum GDialogResult
    {
        PrimaryButton, SecondButton, CloseButton
    }

    [ContentProperty(nameof(Content))]
    public class GDialog : Border
    {
        private ManualResetEvent notify;

        // Using a DependencyProperty as the backing store for CloseButtonContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseButtonContentProperty =
            DependencyProperty.Register("CloseButtonContent", typeof(object), typeof(GDialog), new PropertyMetadata("Cancel"));

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(GDialog), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for PrimaryButtonContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PrimaryButtonContentProperty =
            DependencyProperty.Register("PrimaryButtonContent", typeof(object), typeof(GDialog), new PropertyMetadata(string.Empty));

        // Using a DependencyProperty as the backing store for SecondButtonContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondButtonContentProperty =
            DependencyProperty.Register("SecondButtonContent", typeof(object), typeof(GDialog), new PropertyMetadata(string.Empty));

        // Using a DependencyProperty as the backing store for TitleForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleForegroundProperty =
            DependencyProperty.Register("TitleForeground", typeof(SolidColorBrush), typeof(GDialog), new PropertyMetadata(Brushes.White));

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(GDialog), new PropertyMetadata(string.Empty));

        public object CloseButtonContent
        {
            get => this.GetValue(CloseButtonContentProperty);
            set => this.SetValue(CloseButtonContentProperty, value);
        }

        public object Content
        {
            get => this.GetValue(ContentProperty);
            set { this.SetValue(ContentProperty, value); }
        }

        public object PrimaryButtonContent
        {
            get => this.GetValue(PrimaryButtonContentProperty);
            set => this.SetValue(PrimaryButtonContentProperty, value);
        }

        public GDialogResult Result { get; private set; }

        public object SecondButtonContent
        {
            get => this.GetValue(SecondButtonContentProperty);
            set => this.SetValue(SecondButtonContentProperty, value);
        }

        public string Title
        {
            get => (string)this.GetValue(TitleProperty);
            set => this.SetValue(TitleProperty, value);
        }

        public SolidColorBrush TitleForeground
        {
            get { return (SolidColorBrush)GetValue(TitleForegroundProperty); }
            set { SetValue(TitleForegroundProperty, value); }
        }

        public delegate void DialogButtonRoutedEventHandler(Button sender, RoutedEventArgs e);

        public event DialogButtonRoutedEventHandler CloseButtonClick;

        public event DialogButtonRoutedEventHandler PrimaryButtonClick;

        public event DialogButtonRoutedEventHandler SecondButtonClick;
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Result = GDialogResult.CloseButton;
            CloseButtonClick?.Invoke(sender as Button, e);
        }

        private void primaryButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Result = GDialogResult.PrimaryButton;
            PrimaryButtonClick?.Invoke(sender as Button, e);
        }

        private void secondButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Result = GDialogResult.SecondButton;
            SecondButtonClick?.Invoke(sender as Button, e);
        }

        protected void Hide() => notify.Set();

        protected override void OnInitialized(EventArgs e)
        {

            this.MinHeight = 300;
            this.MinWidth = 460;
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Center;
            //this.Background = new SolidColorBrush(Color.FromArgb(0x50, 0, 0, 0));
            //this.BorderBrush = Brushes.Gray;
            //this.BorderThickness = new Thickness(2);

            var cvtr = new DialogButtonVisiblityConverter();

            {
                var root = new Grid();
                root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                root.RowDefinitions.Add(new RowDefinition());
                root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                {
                    var title = new TextBlock
                    {
                        Margin = new Thickness(20, 20, 20, 5),
                        FontWeight = FontWeights.Bold,
                    };
                    title.FontSize *= 2;
                    title.SetBinding(TextBlock.TextProperty, new Binding(nameof(this.Title)) { Source = this });
                    title.SetBinding(TextBlock.ForegroundProperty, new Binding(nameof(this.TitleForeground)) { Source = this });


                    var content = new ContentControl
                    {
                        Margin = new Thickness(30, 5, 30, 5),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    content.SetBinding(ContentControl.ContentProperty, new Binding(nameof(this.Content)) { Source = this });
                    Grid.SetRow(content, 1);

                    var buttonPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Margin = new Thickness(20, 5, 20, 20)
                    };
                    Grid.SetRow(buttonPanel, 2);
                    {
                        var primaryButton = new Button
                        {
                            MinWidth = 120
                        };
                        primaryButton.SetBinding(Button.VisibilityProperty, new Binding(nameof(primaryButton.Content)) { Source = primaryButton, Converter = cvtr });
                        primaryButton.SetBinding(Button.ContentProperty, new Binding(nameof(this.PrimaryButtonContent)) { Source = this });
                        primaryButton.Click += primaryButton_Click;

                        var secondButton = new Button
                        {
                            MinWidth = 120
                        };
                        secondButton.SetBinding(Button.VisibilityProperty, new Binding(nameof(secondButton.Content)) { Source = secondButton, Converter = cvtr });
                        secondButton.SetBinding(Button.ContentProperty, new Binding(nameof(this.SecondButtonContent)) { Source = this });
                        secondButton.Click += secondButton_Click;

                        var closeButton = new Button
                        {
                            MinWidth = 120
                        };
                        closeButton.SetBinding(Button.VisibilityProperty, new Binding(nameof(closeButton.Content)) { Source = closeButton, Converter = cvtr });
                        closeButton.SetBinding(Button.ContentProperty, new Binding(nameof(this.CloseButtonContent)) { Source = this });
                        closeButton.Click += closeButton_Click;

                        buttonPanel.Children.Add(primaryButton);
                        buttonPanel.Children.Add(secondButton);
                        buttonPanel.Children.Add(closeButton);
                    }
                    root.Children.Add(title);
                    root.Children.Add(content);
                    root.Children.Add(buttonPanel);
                }
                this.Child = root;
            }
            base.OnInitialized(e);
        }
        public void Show(ManualResetEvent notify) => (this.notify = notify).Reset();
        private class DialogButtonVisiblityConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is null || value is string s && string.IsNullOrEmpty(s)) return Visibility.Collapsed;
                else return Visibility.Visible;
            }
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
        }
    }
}

