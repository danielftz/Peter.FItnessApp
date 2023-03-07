using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui.Views;
using FitnessApp.Tool;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace FitnessApp.Components
{
    public class TimerPopUp : Popup
    {

        #region StartTime BindableProperty
        public static readonly BindableProperty StartTimeProperty = BindableProperty.Create(
            propertyName: nameof(StartTime),
            returnType: typeof(TimeSpan),
            declaringType: typeof(TimerPopUp)
        );
        public TimeSpan StartTime
        {
            get => (TimeSpan)GetValue(StartTimeProperty);
            set => SetValue(StartTimeProperty, value);
        }
        #endregion

        public readonly TimerView TimerView;

        //private readonly MediaElement _alarmObj;

        public TimerPopUp()
        {
            
            Border border = new Border
            {
                HeightRequest = 300,
                WidthRequest = 250,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = 10,
                },
                StrokeThickness = 0,
                Shadow = new Shadow
                {
                    Opacity = 0.2f,
                    Offset = new Point(0, 0),
                    Radius = 10,
                }
            };

            AbsoluteLayout container = new();

            TimerView = new TimerView();
            TimerView.TimesUpCommand = new Command(() =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Alarm.Instance.Start();
                });
            });
            TimerView.SetBinding(TimerView.TimeProperty, new Binding(nameof(StartTime), source: this));

            AbsoluteLayout.SetLayoutFlags(TimerView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(TimerView, new Rect(0, 0, 1, 1));
            container.Add(TimerView);

            border.Content = container;
            Content = border;
        }
    }
}
