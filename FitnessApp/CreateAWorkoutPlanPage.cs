using FitnessApp.Components.Labels;
using FitnessApp.Models;
using Microsoft.Maui.Controls.Shapes;

namespace FitnessApp
{
    public class CreateAWorkoutPlanPage : ContentPage
    {
        private readonly CreateAWorkoutPlanPageViewModel _vm;
        public CreateAWorkoutPlanPage()
        {
            Title = "Create A Workout";

            BindingContext = _vm = new CreateAWorkoutPlanPageViewModel();

            Grid grid = new Grid()
            {
                RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition(new GridLength(60, GridUnitType.Absolute)),
                    new RowDefinition(new GridLength(40, GridUnitType.Absolute)),
                    new RowDefinition(new GridLength(1, GridUnitType.Star)),
                    new RowDefinition(new GridLength(40, GridUnitType.Absolute)),
                },

                ColumnDefinitions = new ColumnDefinitionCollection()
                {
                    new ColumnDefinition(new GridLength(1, GridUnitType.Auto)),
                    new ColumnDefinition(new GridLength(1, GridUnitType.Star)),
                },
                Padding = new Thickness(15),
                RowSpacing = 5,
                ColumnSpacing = 5
            };


            EntryTagLabel nameLabel = new EntryTagLabel()
            {
                Text = "Name: ",
            };
            Grid.SetColumn(nameLabel, 0);
            Grid.SetRow(nameLabel, 0);
            grid.Add(nameLabel);



            Border entryElement = new Border()
            {
                StrokeShape = new RoundRectangle()
                {
                    CornerRadius = new CornerRadius(10),
                },
                StrokeThickness = 0,
                BackgroundColor = Palette.PrimaryBackground,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
            };
            Entry nameEntry = new Entry()
            {
                Placeholder = "Enter the name of the workout plan",
                PlaceholderColor = Colors.DarkGray,
                FontSize = 18,
                TextColor = Colors.Black,
                Keyboard = Keyboard.Plain,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                ClearButtonVisibility = ClearButtonVisibility.Never
            };
            entryElement.Content = nameEntry;
            Grid.SetColumn(entryElement, 1);
            Grid.SetRow(entryElement, 0);
            nameEntry.SetBinding(Entry.TextProperty, new Binding(nameof(_vm.WorkoutName), BindingMode.TwoWay));
            grid.Add(entryElement);


            Button newExerciseButton = new Button()
            {
                Text = "+Exercise",
                FontSize = 18,
                TextColor = Colors.White,
                BackgroundColor = Palette.Secondary,
                CornerRadius = 10,
                Margin = new Thickness(30, 0),
            };
            Grid.SetColumn(newExerciseButton, 0);
            Grid.SetRow(newExerciseButton, 1);
            Grid.SetColumnSpan(newExerciseButton, 2);
            newExerciseButton.SetBinding(Button.CommandProperty, new Binding(nameof(_vm.SelectExerciseCommand)));
            grid.Add(newExerciseButton);


            CollectionView exerciseList = new CollectionView
            {
                CanReorderItems= true, 
                ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    ItemSpacing = 10,
                },
                ItemTemplate = new DataTemplate(() =>
                {
                    Border border = new Border
                    {
                        StrokeShape = new RoundRectangle()
                        {
                            CornerRadius = 10
                        },
                        StrokeThickness = 0,
                        BackgroundColor = Palette.PrimaryBackground,
                        Padding = new Thickness(15, 10),
                        HorizontalOptions = LayoutOptions.Center,
                    };

                    Label le = new Label
                    {
                        VerticalOptions = LayoutOptions.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.Fill,
                        HorizontalTextAlignment = TextAlignment.Start,
                        TextColor = Colors.Black,
                        FontSize = 18
                    };
                    le.SetBinding(Label.TextProperty, new Binding(nameof(Exercise.Name)));

                    border.Content = le;

                    return border;
                }),
            };
            Grid.SetColumn(exerciseList, 0);
            Grid.SetRow(exerciseList, 2);
            Grid.SetColumnSpan(exerciseList, 2);
            exerciseList.SetBinding(CollectionView.ItemsSourceProperty, new Binding(nameof(_vm.SelectedExercise), source: _vm));
            grid.Add(exerciseList);

            Button saveWorkoutButton = new Button()
            {
                Text = "Save Workout",
                FontSize = 18,
                TextColor = Colors.Black,
                BackgroundColor = Palette.Primary,
                CornerRadius = 10,
            };
            Grid.SetColumn(saveWorkoutButton, 0);
            Grid.SetRow(saveWorkoutButton, 3);
            Grid.SetColumnSpan(saveWorkoutButton, 2);
            saveWorkoutButton.SetBinding(Button.CommandProperty, nameof(_vm.SaveWorkoutCommand));
            grid.Add(saveWorkoutButton);

            Content = grid;
        }

    }

    
}
