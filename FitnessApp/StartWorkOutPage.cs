using FitnessApp.Components;
using FitnessApp.Models;

namespace FitnessApp
{
    public class StartWorkOutPage : ContentPage
    {
        private StartWorkOutPageViewModel _vm;
        public StartWorkOutPage()
        {
            BindingContext = _vm = new StartWorkOutPageViewModel(this);

            this.SetBinding(ContentPage.TitleProperty, nameof(_vm.Name));

            VerticalStackLayout container = new VerticalStackLayout
            {
                Spacing = 30,
                Padding = 30,
            };

            VerticalStackLayout allWorkout = new VerticalStackLayout
            {
                Spacing = 10,
            };
            allWorkout.SetBinding(BindableLayout.ItemsSourceProperty, nameof(_vm.Exercise));


            BindableLayout.SetItemTemplate(allWorkout, new DataTemplate(() =>
            {
                StartExerciseCard s = new();
                s.StartWorkoutTimerButton.SetBinding(Button.CommandProperty, new Binding(nameof(_vm.StartTimerCommand), source: _vm));
                s.StartWorkoutTimerButton.SetBinding(Button.CommandParameterProperty, nameof(Exercise.TimePerSet));

                s.StartRestTimerButton.SetBinding(Button.CommandProperty, new Binding(nameof(_vm.StartTimerCommand), source: _vm));
                s.StartRestTimerButton.SetBinding(Button.CommandParameterProperty, nameof(Exercise.RestPeriod));
                return s; 
            }));
            container.Add(allWorkout);


            Button allDoneButton = new Button
            {
                Text = "ALL DONE!",
                FontSize = 25,
                TextColor = Colors.White,
                BackgroundColor = Palette.Primary,
                Padding = 25,
                Margin = new Thickness(25, 0),
            };
            allDoneButton.SetBinding(Button.CommandProperty, nameof(_vm.AllDoneCommand));
            container.Add(allDoneButton);

            Content = new ScrollView
            {
                Content = container
            };
             
        }
    }
}
