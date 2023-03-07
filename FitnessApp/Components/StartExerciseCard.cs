using FitnessApp.Models;
using Microsoft.Maui.Controls.Shapes;
using System.Globalization;

namespace FitnessApp.Components
{
    public class StartExerciseCard : Border
    {

        public readonly Button StartWorkoutTimerButton;

        public readonly Button StartRestTimerButton;

        public StartExerciseCard()
        {
            StrokeShape = new RoundRectangle()
            {
                CornerRadius = 10
            };
            StrokeThickness = 0;
            BackgroundColor = Palette.PrimaryBackground;
            Padding = new Thickness(15, 10);

            Grid container = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(new GridLength(40, GridUnitType.Absolute)),
                },

                ColumnDefinitions =
                {
                    new ColumnDefinition(new GridLength(1, GridUnitType.Star)),
                    new ColumnDefinition(new GridLength(1, GridUnitType.Star)),
                },
                RowSpacing = 5,
                ColumnSpacing = 10,
            };

            Label exerciseName = new Label
            {
                FontSize = 18,
                TextColor = Colors.Black,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
            };
            exerciseName.SetBinding(Label.TextProperty, nameof(Exercise.Name));
            Grid.SetRow(exerciseName, 0);
            Grid.SetColumn(exerciseName, 0);
            Grid.SetColumnSpan(exerciseName, 2);
            container.Add(exerciseName);

            HorizontalStackLayout exerciseTypeSection = new HorizontalStackLayout
            {
                Spacing = 10,
            };


            Label weightedLabel = new Label
            {
                Text = "- Weighted",
                FontSize = 15,
                TextColor = Colors.Black,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center, 
            };
            weightedLabel.SetBinding(CheckBox.IsVisibleProperty, nameof(Exercise.IsWeighted));
            exerciseTypeSection.Add(weightedLabel);



            Label timedLabel = new Label
            {
                Text = "- Timed",
                FontSize = 15,
                TextColor = Colors.Black,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
            };
            timedLabel.SetBinding(CheckBox.IsVisibleProperty, nameof(Exercise.IsTimed));
            exerciseTypeSection.Add(timedLabel);
            

            Grid.SetRow(exerciseTypeSection, 1);
            Grid.SetColumn(exerciseTypeSection, 0);
            Grid.SetColumnSpan(exerciseTypeSection, 2);
            container.Add(exerciseTypeSection);
            


            Label setsInfo = new Label
            {
                FontSize = 15,
                TextColor = Colors.Black,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
            };
            setsInfo.SetBinding(Label.TextProperty, new MultiBinding
            {
                Bindings =
                {
                    new Binding(nameof(Exercise.Sets)),
                    new Binding(nameof(Exercise.Repetitions)),
                    new Binding(nameof(Exercise.IsTimed)),
                    new Binding(nameof(Exercise.TimePerSet))
                },
                Converter = new SetsRepsAndTimeToLabelConverter()
            });
            Grid.SetRow(setsInfo, 2);
            Grid.SetColumn(setsInfo, 0);
            Grid.SetColumnSpan(setsInfo, 2);
            container.Add(setsInfo);

            StartWorkoutTimerButton = new Button
            {
                Text = "Time Exercise",
                FontSize = 18,
                TextColor = Colors.White,
                BackgroundColor = Palette.Primary,
            };
            StartWorkoutTimerButton.SetBinding(Button.IsVisibleProperty, nameof(Exercise.IsTimed));
            Grid.SetRow(StartWorkoutTimerButton, 3);
            Grid.SetColumn(StartWorkoutTimerButton, 0);
            container.Add(StartWorkoutTimerButton);

            StartRestTimerButton = new Button
            {
                Text = "Time Rest",
                FontSize = 18,
                TextColor = Colors.White,
                BackgroundColor = Colors.Orange,
            };
            StartRestTimerButton.SetBinding(Grid.ColumnProperty, new Binding(nameof(Exercise.IsTimed), converter: new BoolToIntConverter(), converterParameter: (1, 0)));
            StartRestTimerButton.SetBinding(Grid.ColumnSpanProperty, new Binding(nameof(Exercise.IsTimed), converter: new BoolToIntConverter(), converterParameter: (1, 2)));
            Grid.SetRow(StartRestTimerButton, 3);
             container.Add(StartRestTimerButton);

            Content = container;
        }
    }

    public class BoolToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            (int t, int f)? twoStates = parameter as (int, int)?;

            if(twoStates is not null && value is true)
            {
                return twoStates?.t;
            }
            return twoStates?.f;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
