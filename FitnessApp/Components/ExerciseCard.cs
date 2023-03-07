using FitnessApp.Models;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using System.Globalization;

namespace FitnessApp.Components
{
    public class ExerciseCard : Border
    {
        
        public readonly Label Name;
        public ExerciseCard()
        {
            StrokeShape = new RoundRectangle()
            {
                CornerRadius = 10
            };
            StrokeThickness = 0;
            BackgroundColor = Palette.PrimaryBackground;
            HorizontalOptions = LayoutOptions.Fill;
            VerticalOptions = LayoutOptions.Center;
            Padding = new Thickness(15, 10);

            VerticalStackLayout container = new VerticalStackLayout
            {
                Spacing = 3,
            };

            Name = new Label
            {
                FontSize = 18,
                TextColor = Colors.Black,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
            };
            Name.SetBinding(Label.TextProperty, nameof(Exercise.Name));
            container.Add(Name);




            FlexLayout targetingParts = new FlexLayout
            {
                AlignContent = FlexAlignContent.Start,
                AlignItems = FlexAlignItems.Start,
                Direction = FlexDirection.Row,
                JustifyContent = FlexJustify.Start,
                Wrap = FlexWrap.Wrap,

            };
            targetingParts.SetBinding(BindableLayout.ItemsSourceProperty, nameof(Exercise.TargetingParts));
            BindableLayout.SetItemTemplate(targetingParts, new DataTemplate(() =>
            {
                Label part = new Label
                {
                    FontSize = 15,
                    TextColor = Colors.Grey,
                    Margin = new Thickness(0,0,5,0)
                };
                part.SetBinding(Label.TextProperty, new Binding(".", converter: new EnumToTargetingPartsConverter()));
                return part;
            }));
            container.Add(targetingParts);

            Label sets = new Label
            {
                FontSize = 15,
                TextColor = Colors.Black,
            };
            sets.SetBinding(Label.TextProperty, new MultiBinding
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
            container.Add(sets);

            Content = container;
        }
    }

    public class EnumToTargetingPartsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not null)
            {
                int idx = (int)value;

                return Exercise.AllTargetableParts[idx];
            }

            return string.Empty;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SetsRepsAndTimeToLabelConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int? sets = values[0] as int?;
            int? repetitions = values[1] as int?;
            bool? isTimed = values[2] as bool?;
            TimeSpan? timePerSet = values[3] as TimeSpan?;

            if (sets is null || repetitions is null || isTimed is null || timePerSet is null)
            {
                return "";
            }

            if (repetitions != 0)
            {
                return $"{sets} sets of {repetitions} reps";
            }
            else if (isTimed is true)
            {
                return $"{sets} sets of {((TimeSpan)timePerSet).Minutes} minute {((TimeSpan)timePerSet).Seconds} second reps";
            }
            else
            {
                return $"{sets}";
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
