using CommunityToolkit.Maui.Views;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Timer = System.Timers.Timer;


namespace FitnessApp.Components
{
    public class TimerView : Grid//AbsoluteLayout
    {
        public static readonly BindableProperty TimeProperty = BindableProperty.Create(
            propertyName: nameof(Time),
            returnType: typeof(TimeSpan),
            declaringType: typeof(TimerView)
        );
        public TimeSpan Time 
        { 
            get => (TimeSpan)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value); 
        }


        #region OnGoingTime
        private static readonly BindablePropertyKey OnGoingTimePropertyKey = BindableProperty.CreateReadOnly(
            propertyName: nameof(OnGoingTime),
            returnType: typeof(TimeSpan),
            declaringType: typeof(TimerView),
            defaultValue: TimeSpan.FromMinutes(1)
        );
        public static readonly BindableProperty OnGoingTimeProperty = OnGoingTimePropertyKey.BindableProperty;
        public TimeSpan OnGoingTime
        {
            get => (TimeSpan)GetValue(OnGoingTimeProperty);
            private set => SetValue(OnGoingTimePropertyKey, value);
        }
        #endregion


        #region TimesUpCommand BindableProperty
        public static readonly BindableProperty TimesUpCommandProperty = BindableProperty.Create(
            propertyName: nameof(TimesUpCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(TimerView)
        );
        public ICommand TimesUpCommand
        {
            get => (ICommand)GetValue(TimesUpCommandProperty);
            set => SetValue(TimesUpCommandProperty, value);
        }
        #endregion


        #region TimesUpCommandParameter BindableProperty
        public static readonly BindableProperty TimesUpCommandParameterProperty = BindableProperty.Create(
            propertyName: nameof(TimesUpCommandParameter),
            returnType: typeof(object),
            declaringType: typeof(TimerView)
        );
        public object TimesUpCommandParameter
        {
            get => (object)GetValue(TimesUpCommandParameterProperty);
            set => SetValue(TimesUpCommandParameterProperty, value);
        }
        #endregion





        private readonly Timer _timer = new()
        {
            Interval = 1000,
            AutoReset = true,
        };

        private readonly GraphicsView _timerGraphicsView;
        private readonly TimerGraphicsObj _timerGraphicsObj;

        private readonly Button _startPauseButton;
        private readonly Label _timeValue;

        public TimerView()
        {
            BindingContext = this;
            

            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition(){ Height = GridLength.Star},
                new RowDefinition(){ Height = new GridLength(40, GridUnitType.Absolute)},
            };

            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition(),
                new ColumnDefinition(),
            };
            Padding = 20;
            ColumnSpacing = 20;
            RowSpacing = 20;

           

            _timerGraphicsView = new GraphicsView
            {
                BackgroundColor = Colors.Transparent,
                Drawable = _timerGraphicsObj = new TimerGraphicsObj(Time, OnGoingTime),
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
            };
            Grid.SetRow(_timerGraphicsView, 0);
            Grid.SetColumn(_timerGraphicsView, 0);
            Grid.SetColumnSpan(_timerGraphicsView, 2);
            Add(_timerGraphicsView);

            _timeValue = new Label
            {
                FontSize = 20,
                TextColor = Palette.Secondary,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
            };
            _timeValue.SetBinding(Label.TextProperty, new Binding(nameof(OnGoingTime), source: this, converter: new TimeToStringConverter()));
            Grid.SetRow(_timeValue, 0);
            Grid.SetColumn(_timeValue, 0);
            Grid.SetColumnSpan(_timeValue, 2);
            Add(_timeValue);

            

            Button cancelButton = new Button
            {
                Text = "Restart",
                BackgroundColor = Colors.LightGrey,
                TextColor = Colors.Black,
                Command = new Command(() =>
                {
                    Restart();
                })
            };
            Grid.SetRow(cancelButton, 1);
            Grid.SetColumn(cancelButton, 0);
            Add(cancelButton);


            _startPauseButton = new Button
            {
                Text = "Start",
                BackgroundColor = Palette.Secondary,
                TextColor = Colors.White,
                //CornerRadius = 40,
                Command = new Command(() =>
                {
                    if (_timer.Enabled is true)
                    {
                        Pause();
                    }
                    else
                    {
                        Start();
                    }
                })
            };
            Grid.SetRow(_startPauseButton, 1);
            Grid.SetColumn(_startPauseButton, 1);
            Add(_startPauseButton);

            _timer.Elapsed += (s, e) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    OnGoingTime -= TimeSpan.FromSeconds(1);
                    _timerGraphicsObj.StartTime = Time;
                    _timerGraphicsObj.OnGoingTime = OnGoingTime;
                    _timerGraphicsView.Invalidate();
                    if (OnGoingTime <= TimeSpan.Zero)
                    {
                        Pause();
                        TimesUpCommand?.Execute(TimesUpCommandParameter);
                    }
                });
            };

        }

        public void Start()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (OnGoingTime <= TimeSpan.Zero)
                {
                    Restart();
                }
                _timer.Start();
                _startPauseButton.Text = "Pause";
            });
        }

        public void Pause()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _timer.Stop();
                _timer.Interval = 1000;
                _startPauseButton.Text = "Start";
            });
           
        }

        public void Restart()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _timer.Stop();
                _timer.Interval = 1000;
                _startPauseButton.Text = "Start";

                OnGoingTime = Time;

                _timerGraphicsObj.StartTime = Time;
                _timerGraphicsObj.OnGoingTime = OnGoingTime;
                _timerGraphicsView.Invalidate();
            });
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(Time))
            {
                Restart();
            }
        }
    }

    public class TimerGraphicsObj : IDrawable
    {
        public TimeSpan StartTime { get; set; }

        public TimeSpan OnGoingTime { get; set; }
        public TimerGraphicsObj(TimeSpan startTime, TimeSpan onGoingTime)
        {
            StartTime = startTime;
            OnGoingTime = onGoingTime;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Palette.Primary;
            canvas.StrokeSize = 4;
            canvas.StrokeLineCap = LineCap.Round;
            float radius = Math.Min(dirtyRect.Width, dirtyRect.Height) * 0.5f;
            if (StartTime == OnGoingTime)
            {
                canvas.DrawCircle(dirtyRect.Center, radius);
            }
            else if (OnGoingTime <= TimeSpan.Zero)
            {
                canvas.StrokeColor = Colors.White;
                canvas.DrawLine(0, 0, 0, 0);
            }
            else
            {
                float percentageToGo = (float)(OnGoingTime.TotalSeconds / StartTime.TotalSeconds);
                float endAngle = 90f - 360f * (1 - percentageToGo);


                canvas.DrawArc(dirtyRect.Center.X - radius, dirtyRect.Center.Y - radius , radius * 2, radius * 2, endAngle, 90, true, false);
            }
        }
    }


    public class TimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan time = (TimeSpan)value;
            return string.Format("{0:mm\\:ss}", time);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
