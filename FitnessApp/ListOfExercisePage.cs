using FitnessApp.Components;
using FitnessApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FitnessApp
{
    public class ListOfExercisePage : ContentPage
    {
        private readonly ListOfExercisePageViewModel _vm;
        public ListOfExercisePage(/*string destinationWorkout, ObservableCollection<Exercise> destinationExercise*/) 
        {
            BindingContext = _vm = new ListOfExercisePageViewModel();
            Title = "Exercise Database";

            Grid grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition {Height = GridLength.Star},
                    new RowDefinition {Height = GridLength.Auto},
                    new RowDefinition {Height = GridLength.Auto},
                },

                Padding = 20,
                RowSpacing = 10,
            };

           
            
            CollectionView list = new CollectionView
            {
                SelectionMode = SelectionMode.Multiple,
                ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    ItemSpacing = 10,
                },
                ItemTemplate = new DataTemplate(() =>
                {
                    return new ExerciseCard();
                }),
            };
            //TODO: selected items not working correctly
            list.SetBinding(CollectionView.ItemsSourceProperty, new Binding(nameof(_vm.AllExercise)));
            list.SetBinding(CollectionView.SelectedItemsProperty, new Binding(nameof(_vm.SelectedExercise), BindingMode.TwoWay));


            RefreshView refreshView = new RefreshView
            {
                Content = list
            };
            refreshView.CommandParameter = refreshView;
            refreshView.SetBinding(RefreshView.CommandProperty, nameof(_vm.RefreshCommand));
            Grid.SetRow(refreshView, 0);
            grid.Add(refreshView);


            Button AddToExerciseButton = new Button()
            {
                Text = "+ Add To Workout",
                FontSize = 18,
                TextColor = Colors.Black,
                BackgroundColor = Palette.Secondary,
                CornerRadius = 10,
            };
            AddToExerciseButton.SetBinding(Button.CommandProperty, nameof(_vm.AddToWorkoutCommand));
            Grid.SetRow(AddToExerciseButton, 1);
            grid.Add(AddToExerciseButton);


            Button CreateExerciseButton = new Button()
            {
                Text = "Create An Exercise",
                FontSize = 18,
                TextColor = Colors.Black,
                BackgroundColor = Palette.Primary,
                CornerRadius = 10,
                Command = new Command(async () =>
                {
                    await Shell.Current.GoToAsync(nameof(CreateAnExercisePage));
                })
            };
            Grid.SetRow(CreateExerciseButton, 2);
            grid.Add(CreateExerciseButton);

            Content = grid;
        }
    }
}
