using FitnessApp.Models;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace FitnessApp.Components
{
    public class WorkoutCard : Border
    {

        public readonly Button StartNowButton;

        public readonly Button RemoveButton;
        public WorkoutCard()
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
                    new RowDefinition(new GridLength(40, GridUnitType.Absolute)),
                },

                ColumnDefinitions =
                {
                    new ColumnDefinition(new GridLength(4, GridUnitType.Star)),
                    new ColumnDefinition(new GridLength(3, GridUnitType.Star)),
                    new ColumnDefinition(new GridLength(1, GridUnitType.Star)),
                },
                RowSpacing = 5,
                ColumnSpacing = 10,
            };

            Label workoutName = new Label
            {
                FontSize = 18,
                TextColor = Colors.Black,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
            };
            workoutName.SetBinding(Label.TextProperty, nameof(Workout.Name));
            Grid.SetRow(workoutName, 0);
            Grid.SetColumn(workoutName, 0);
            Grid.SetColumnSpan(workoutName, 2);
            container.Add(workoutName);

            
            FlexLayout targetingParts = new FlexLayout
            {
                AlignContent = FlexAlignContent.Start,
                AlignItems = FlexAlignItems.Start,
                Direction = FlexDirection.Row,
                JustifyContent = FlexJustify.Start,
                Wrap = FlexWrap.Wrap,
            };
            targetingParts.SetBinding(BindableLayout.ItemsSourceProperty, nameof(Workout.TargetingParts));
            BindableLayout.SetItemTemplate(targetingParts, new DataTemplate(() =>
            {
                Label part = new Label
                {
                    FontSize = 15,
                    TextColor = Colors.Grey,
                    Margin = new Thickness(0, 0, 5, 0)
                };
                part.SetBinding(Label.TextProperty, new Binding(".", converter: new EnumToTargetingPartsConverter()));
                return part;
            }));
            Grid.SetRow(targetingParts, 1);
            Grid.SetColumn(targetingParts, 0);
            Grid.SetColumnSpan(targetingParts, 3);
            container.Add(targetingParts);


            StartNowButton = new Button
            {
                Text = "Start Now",
                FontSize = 18,
                TextColor = Colors.White,
                BackgroundColor = Palette.Primary,
            };

            Grid.SetRow(StartNowButton, 2);
            Grid.SetColumn(StartNowButton, 0);
            container.Add(StartNowButton);

            RemoveButton = new Button
            {
                Text = "Remove",
                FontSize = 18,
                TextColor = Colors.White,
                BackgroundColor = Colors.Orange,
            };

            Grid.SetRow(RemoveButton, 2);
            Grid.SetColumn(RemoveButton, 1);
            Grid.SetColumnSpan(RemoveButton, 2);
            container.Add(RemoveButton);

            Content = container;
        }
    }
}
