using FitnessApp.Components;
using FitnessApp.Models;
using System.Globalization;

namespace FitnessApp
{
    public class WorkOutPage : ContentPage
    {
        private readonly WorkOutPageViewModel _vm;
        public WorkOutPage() 
        {
            Title = "My Workout";

            BindingContext = _vm = new WorkOutPageViewModel();

            RowDefinition variableRow = new RowDefinition(new GridLength(1, GridUnitType.Star));
            variableRow.SetBinding(RowDefinition.HeightProperty, new Binding(nameof(_vm.AllWorkout), converter: new ListToRowHeightConverter()));
            
            Grid container = new Grid()
            {
                Padding = 40,
                RowDefinitions =
                {
                    variableRow,
                    new RowDefinition(new GridLength(40, GridUnitType.Absolute)),
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(new GridLength(40, GridUnitType.Absolute)),
                },
                RowSpacing = 10,
            };

            Label noWorkOutlabel = new Label
            {
                Text = "You have no workout stored in the database",
                FontSize = 20,
                TextColor = Colors.Black,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Fill
            };
            Grid.SetRow(noWorkOutlabel, 0);
            noWorkOutlabel.SetBinding(Label.IsVisibleProperty, new Binding(nameof(_vm.AllWorkout), converter: new ListToBoolConverter(true)));
            container.Add(noWorkOutlabel);

            RefreshView refreshView = new RefreshView();
            refreshView.CommandParameter = refreshView;
            refreshView.SetBinding(RefreshView.CommandProperty, nameof(_vm.RefreshAllWorkoutCommand));
            
            CollectionView workoutList = new CollectionView
            {
                ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    ItemSpacing = 15,
                },
                ItemTemplate = new DataTemplate(() =>
                {
                    WorkoutCard card = new WorkoutCard();
                    card.StartNowButton.SetBinding(Button.CommandProperty, new Binding(nameof(_vm.StartNowCommand), source: _vm));
                    card.StartNowButton.SetBinding(Button.CommandParameterProperty, new Binding("."));

                    card.RemoveButton.SetBinding(Button.CommandProperty, new Binding(nameof(_vm.RemoveWorkoutCommand), source: _vm));
                    card.RemoveButton.SetBinding(Button.CommandParameterProperty, new Binding("."));

                    return card;
                })
            };

            refreshView.Content = workoutList;
            Grid.SetRow(refreshView, 0);
            workoutList.SetBinding(CollectionView.ItemsSourceProperty, new Binding(nameof(_vm.AllWorkout)));
            refreshView.SetBinding(RefreshView.IsVisibleProperty, new Binding(nameof(_vm.AllWorkout), converter: new ListToBoolConverter(false)));
            container.Add(refreshView);

            Button createARecipeButton = new Button
            {
                Text = "Create a workout",
                FontSize = 18,
                TextColor = Colors.White,
                BackgroundColor = Palette.Secondary,
                CornerRadius = 10,
                Command = new Command(async () =>
                {
                    await Shell.Current.GoToAsync(nameof(CreateAWorkoutPlanPage));
                })
            };
            Grid.SetRow(createARecipeButton, 1);
            container.Add(createARecipeButton);


            Label orLabel = new Label
            {
                Text = "or",
                FontSize = 20,
                TextColor = Colors.Grey,
                Padding = 35,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
            };
            Grid.SetRow(orLabel, 2);
            orLabel.SetBinding(Label.IsVisibleProperty, new Binding(nameof(_vm.AllWorkout), converter: new ListToBoolConverter(true)));
            container.Add(orLabel);


            Button createExerciseButton = new Button
            {
                Text = "Create an exercise",
                FontSize = 18,
                TextColor = Colors.White,
                BackgroundColor = Palette.Secondary,
                CornerRadius = 10,
                Command = new Command(async () =>
                {
                    await Shell.Current.GoToAsync(nameof(CreateAnExercisePage));
                })
            };
            Grid.SetRow(createExerciseButton, 3);
            container.Add(createExerciseButton);

           
            Content = container;

        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            //refresh all items
            await _vm.LoadAlWorkout();
        }
    }

    public class ListToBoolConverter : IValueConverter
    {
        private bool _isInverted;
        public ListToBoolConverter(bool isInverted)
        {
            _isInverted = isInverted;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IList<Workout> list = value as IList<Workout>;
            if (list is not null && list.Count > 0)
            {
                if (_isInverted)
                {
                    return false;
                }
                else
                {
                    return true;
                }
               
            }

            if (_isInverted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ListToRowHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IList<Workout> list = value as IList<Workout>;
            if (list is not null && list.Count > 0)
            {
                return GridLength.Star;

            }
            return GridLength.Auto;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
