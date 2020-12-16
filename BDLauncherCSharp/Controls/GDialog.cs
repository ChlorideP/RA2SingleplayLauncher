using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace BDLauncherCSharp.Controls
{
    public enum GDialogResult
    {
        PrimaryButton, SecondButton, CloseButton, FaildOpen
    }

    [ContentProperty(nameof(Content))]
    public class GDialog : Border
    {
        private ManualResetEvent notify;

        private class DialogButtonVisiblityConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is null || value is string s && string.IsNullOrEmpty(s)) return Visibility.Collapsed;
                else return Visibility.Visible;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
        }

        protected virtual void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Result = GDialogResult.CloseButton;
            CloseButtonClick?.Invoke(sender as Button, e);
        }

        protected override void OnInitialized(EventArgs e)
        {
            MinHeight = 300;
            MinWidth = 460;
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
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
                    title.SetBinding(TextBlock.TextProperty, new Binding(nameof(Title)) { Source = this });
                    title.SetBinding(TextBlock.ForegroundProperty, new Binding(nameof(TitleForeground)) { Source = this });

                    var content = new ContentControl
                    {
                        Margin = new Thickness(30, 5, 30, 5),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    content.SetBinding(ContentControl.ContentProperty, new Binding(nameof(Content)) { Source = this });
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
                        primaryButton.SetBinding(Button.ContentProperty, new Binding(nameof(PrimaryButtonContent)) { Source = this });
                        primaryButton.SetBinding(Button.CommandProperty, new Binding(nameof(PrimaryButtonCommand)) { Source = this });
                        primaryButton.SetBinding(Button.CommandParameterProperty, new Binding(nameof(PrimaryButtonCommandParameter)) { Source = this });
                        primaryButton.SetBinding(Button.CommandTargetProperty, new Binding(nameof(PrimaryButtonCommandTarget)) { Source = this });
                        primaryButton.CommandTarget = this;
                        primaryButton.Click += PrimaryButton_Click;

                        var secondButton = new Button
                        {
                            MinWidth = 120,
                            Margin = new Thickness(10, 0, 10, 0)
                        };
                        secondButton.SetBinding(Button.VisibilityProperty, new Binding(nameof(secondButton.Content)) { Source = secondButton, Converter = cvtr });
                        secondButton.SetBinding(Button.ContentProperty, new Binding(nameof(SecondButtonContent)) { Source = this });
                        secondButton.SetBinding(Button.CommandProperty, new Binding(nameof(SecondButtonCommand)) { Source = this });
                        secondButton.SetBinding(Button.CommandParameterProperty, new Binding(nameof(SecondButtonCommandParameter)) { Source = this });
                        secondButton.SetBinding(Button.CommandTargetProperty, new Binding(nameof(SecondButtonCommandTarget)) { Source = this });
                        secondButton.CommandTarget = this;
                        secondButton.Click += SecondButton_Click;

                        var closeButton = new Button
                        {
                            MinWidth = 120
                        };
                        closeButton.SetBinding(Button.VisibilityProperty, new Binding(nameof(closeButton.Content)) { Source = closeButton, Converter = cvtr });
                        closeButton.SetBinding(Button.ContentProperty, new Binding(nameof(CloseButtonContent)) { Source = this });
                        closeButton.SetBinding(Button.CommandProperty, new Binding(nameof(CloseButtonCommand)) { Source = this });
                        closeButton.SetBinding(Button.CommandParameterProperty, new Binding(nameof(CloseButtonCommandParameter)) { Source = this });
                        closeButton.SetBinding(Button.CommandTargetProperty, new Binding(nameof(CloseButtonCommandTarget)) { Source = this });
                        closeButton.CommandTarget = this;
                        closeButton.Click += CloseButton_Click;

                        buttonPanel.Children.Add(primaryButton);
                        buttonPanel.Children.Add(secondButton);
                        buttonPanel.Children.Add(closeButton);
                    }
                    root.Children.Add(title);
                    root.Children.Add(content);
                    root.Children.Add(buttonPanel);
                }
                Child = root;
            }
            base.OnInitialized(e);
        }

        protected virtual void PrimaryButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Result = GDialogResult.PrimaryButton;
            PrimaryButtonClick?.Invoke(sender as Button, e);
        }

        protected virtual void SecondButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Result = GDialogResult.SecondButton;
            SecondButtonClick?.Invoke(sender as Button, e);
        }

        public static readonly DependencyProperty CloseButtonCommandParameterProperty =
            DependencyProperty.Register("CloseButtonCommandParameter", typeof(object), typeof(GDialog), new PropertyMetadata(null));

        public static readonly DependencyProperty CloseButtonCommandProperty =
            DependencyProperty.Register("CloseButtonCommand", typeof(ICommand), typeof(GDialog), new PropertyMetadata(null));

        public static readonly DependencyProperty CloseButtonCommandTargetProperty =
            DependencyProperty.Register("CloseButtonCommandTarget", typeof(IInputElement), typeof(GDialog), new PropertyMetadata(null));

        public static readonly DependencyProperty CloseButtonContentProperty =
            DependencyProperty.Register("CloseButtonContent", typeof(object), typeof(GDialog), new PropertyMetadata("Cancel"));

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(GDialog), new PropertyMetadata(null));

        public static readonly DependencyProperty PrimaryButtonCommandParameterProperty =
            DependencyProperty.Register("PrimaryButtonCommandParameter", typeof(object), typeof(GDialog), new PropertyMetadata(null));

        public static readonly DependencyProperty PrimaryButtonCommandProperty =
            DependencyProperty.Register("PrimaryButtonCommand", typeof(ICommand), typeof(GDialog), new PropertyMetadata(null));

        public static readonly DependencyProperty PrimaryButtonCommandTargetProperty =
            DependencyProperty.Register("PrimaryButtonCommandTarget", typeof(IInputElement), typeof(GDialog), new PropertyMetadata(null));

        public static readonly DependencyProperty PrimaryButtonContentProperty =
            DependencyProperty.Register("PrimaryButtonContent", typeof(object), typeof(GDialog), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty SecondButtonCommandParameterProperty =
            DependencyProperty.Register("SecondButtonCommandParameter", typeof(object), typeof(GDialog), new PropertyMetadata(null));

        public static readonly DependencyProperty SecondButtonCommandProperty =
            DependencyProperty.Register("SecondButtonCommand", typeof(ICommand), typeof(GDialog), new PropertyMetadata(null));

        public static readonly DependencyProperty SecondButtonCommandTargetProperty =
            DependencyProperty.Register("SecondButtonCommandTarget", typeof(IInputElement), typeof(GDialog), new PropertyMetadata(null));

        public static readonly DependencyProperty SecondButtonContentProperty =
            DependencyProperty.Register("SecondButtonContent", typeof(object), typeof(GDialog), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty TitleForegroundProperty =
            DependencyProperty.Register("TitleForeground", typeof(SolidColorBrush), typeof(GDialog), new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(GDialog), new PropertyMetadata(string.Empty));

        public ICommand CloseButtonCommand
        {
            get => (ICommand)GetValue(CloseButtonCommandProperty);
            set => SetValue(CloseButtonCommandProperty, value);
        }

        public object CloseButtonCommandParameter
        {
            get => GetValue(CloseButtonCommandParameterProperty);
            set => SetValue(CloseButtonCommandParameterProperty, value);
        }

        public IInputElement CloseButtonCommandTarget
        {
            get => (IInputElement)GetValue(CloseButtonCommandTargetProperty);
            set => SetValue(CloseButtonCommandTargetProperty, value);
        }

        public object CloseButtonContent
        {
            get => GetValue(CloseButtonContentProperty);
            set => SetValue(CloseButtonContentProperty, value);
        }

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public ICommand PrimaryButtonCommand
        {
            get => (ICommand)GetValue(PrimaryButtonCommandProperty);
            set => SetValue(PrimaryButtonCommandProperty, value);
        }

        public object PrimaryButtonCommandParameter
        {
            get => GetValue(PrimaryButtonCommandParameterProperty);
            set => SetValue(PrimaryButtonCommandParameterProperty, value);
        }

        public IInputElement PrimaryButtonCommandTarget
        {
            get => (IInputElement)GetValue(PrimaryButtonCommandTargetProperty);
            set => SetValue(PrimaryButtonCommandTargetProperty, value);
        }

        public object PrimaryButtonContent
        {
            get => GetValue(PrimaryButtonContentProperty);
            set => SetValue(PrimaryButtonContentProperty, value);
        }

        public GDialogResult Result { get; private set; }

        public ICommand SecondButtonCommand
        {
            get => (ICommand)GetValue(SecondButtonCommandProperty);
            set => SetValue(SecondButtonCommandProperty, value);
        }

        public object SecondButtonCommandParameter
        {
            get => GetValue(SecondButtonCommandParameterProperty);
            set => SetValue(SecondButtonCommandParameterProperty, value);
        }

        public IInputElement SecondButtonCommandTarget
        {
            get => (IInputElement)GetValue(SecondButtonCommandTargetProperty);
            set => SetValue(SecondButtonCommandTargetProperty, value);
        }

        public object SecondButtonContent
        {
            get => GetValue(SecondButtonContentProperty);
            set => SetValue(SecondButtonContentProperty, value);
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public SolidColorBrush TitleForeground
        {
            get => (SolidColorBrush)GetValue(TitleForegroundProperty);
            set => SetValue(TitleForegroundProperty, value);
        }

        public delegate void DialogButtonRoutedEventHandler(Button sender, RoutedEventArgs e);

        public event DialogButtonRoutedEventHandler CloseButtonClick;

        public event DialogButtonRoutedEventHandler PrimaryButtonClick;

        public event DialogButtonRoutedEventHandler SecondButtonClick;

        public void Hide() => notify.Set();
        public void Show(ManualResetEvent notify) => (this.notify = notify).Reset();
    }
}
