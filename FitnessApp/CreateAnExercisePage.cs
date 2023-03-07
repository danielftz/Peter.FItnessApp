using CommunityToolkit.Maui.Views;
using FitnessApp.Components;
using FitnessApp.Components.Entries;
using FitnessApp.Components.Labels;
using FitnessApp.Models;
using Microsoft.Maui.Layouts;

namespace FitnessApp
{
    public class CreateAnExercisePage : ContentPage
    {
        private readonly CreateAnExercisePageViewModel _vm;
        public CreateAnExercisePage()
        {
            BindingContext = _vm = new CreateAnExercisePageViewModel();

            Shell.SetPresentationMode(this, PresentationMode.Modal);
            Grid container = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition(new GridLength(60, GridUnitType.Absolute)),
                    new RowDefinition(new GridLength(60, GridUnitType.Absolute)),
                    new RowDefinition(new GridLength(60, GridUnitType.Absolute)),
                    new RowDefinition(new GridLength(1, GridUnitType.Auto)),
                    new RowDefinition(new GridLength(1, GridUnitType.Absolute)),
                    new RowDefinition(new GridLength(60, GridUnitType.Absolute)),
                    new RowDefinition(new GridLength(30, GridUnitType.Absolute)),
                    new RowDefinition(new GridLength(60, GridUnitType.Absolute)),
                    new RowDefinition(new GridLength(60, GridUnitType.Absolute)),
                    new RowDefinition(new GridLength(1, GridUnitType.Absolute)),
                    new RowDefinition(new GridLength(40, GridUnitType.Absolute)),
                    new RowDefinition(new GridLength(1, GridUnitType.Auto)),
                    new RowDefinition(new GridLength(40, GridUnitType.Absolute)),
                    new RowDefinition(new GridLength(40, GridUnitType.Absolute)),
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition(new GridLength(1, GridUnitType.Auto)),
                    new ColumnDefinition(new GridLength(3, GridUnitType.Star)),
                },
                Padding = 45,
                RowSpacing = 5,
                ColumnSpacing = 5
            };

            EntryTagLabel nameLabel = new EntryTagLabel
            {
                Text = "Name: ",
            };
            Grid.SetColumn(nameLabel, 0);
            Grid.SetRow(nameLabel, 0);
            container.Add(nameLabel);

            BorderedEntry nameEntry = new BorderedEntry()
            {

            };
            nameEntry.Entry.Placeholder = "Enter the name of the exercise";
            Grid.SetColumn(nameEntry, 1);
            Grid.SetRow(nameEntry, 0);
            container.Add(nameEntry);
            nameEntry.Entry.SetBinding(Entry.TextProperty, new Binding(nameof(_vm.CurrentExercise.Name), BindingMode.TwoWay, source: _vm.CurrentExercise));


            EntryTagLabel weightedLabel = new EntryTagLabel
            {
                Text = "Weighted: ",
            };
            Grid.SetColumn(weightedLabel, 0);
            Grid.SetRow(weightedLabel, 1);
            container.Add(weightedLabel);

            CheckBox weightedCheck = new CheckBox
            {
                Color = Palette.Primary,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
            };
            Grid.SetColumn(weightedCheck, 1);
            Grid.SetRow(weightedCheck, 1);
            container.Add(weightedCheck);
            weightedCheck.SetBinding(CheckBox.IsCheckedProperty, new Binding(nameof(_vm.CurrentExercise.IsWeighted), BindingMode.TwoWay, source: _vm.CurrentExercise));



            EntryTagLabel timedLabel = new EntryTagLabel
            {
                Text = "Timed: ",
            };
            Grid.SetColumn(timedLabel, 0);
            Grid.SetRow(timedLabel, 2);
            container.Add(timedLabel);

            CheckBox timedCheck = new CheckBox
            {
                Color = Palette.Primary,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
            };
            Grid.SetColumn(timedCheck, 1);
            Grid.SetRow(timedCheck, 2);
            container.Add(timedCheck);
            timedCheck.SetBinding(CheckBox.IsCheckedProperty, new Binding(nameof(_vm.IsTimed), BindingMode.TwoWay, source: _vm));



            EntryTagLabel timePerSetLabel = new EntryTagLabel
            {
                Text = "Time\nPer Set:",
                HeightRequest = 60,
            };
            Grid.SetColumn(timePerSetLabel, 0);
            Grid.SetRow(timePerSetLabel, 3);
            timePerSetLabel.SetBinding(EntryTagLabel.IsVisibleProperty, new Binding(nameof(_vm.IsTimed)));
            container.Add(timePerSetLabel);

            Grid timePerSetContainer = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(){Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition(){Width = new GridLength(10, GridUnitType.Absolute)},
                    new ColumnDefinition(){Width = GridLength.Star},
                    new ColumnDefinition(){Width = new GridLength(10, GridUnitType.Absolute)},
                },
                ColumnSpacing = 1,
            };
            timePerSetContainer.SetBinding(Grid.IsVisibleProperty, new Binding(nameof(_vm.IsTimed)));

            BorderedEntry timePerSetMinEntry = new BorderedEntry()
            {

            };
            timePerSetMinEntry.Entry.Placeholder = "minutes";
            timePerSetMinEntry.Entry.Keyboard = Keyboard.Numeric;
            Grid.SetColumn(timePerSetMinEntry, 0);
            Grid.SetRow(timePerSetMinEntry, 0);
            timePerSetContainer.Add(timePerSetMinEntry);
            timePerSetMinEntry.Entry.SetBinding(Entry.TextProperty, new Binding(nameof(_vm.TimePerSetMinutes), BindingMode.TwoWay));


            Label timePerSetMins = new Label
            {
                FontSize = 18,
                TextColor = Colors.Black,
                Text = "'"
            };
            Grid.SetColumn(timePerSetMins, 1);
            Grid.SetRow(timePerSetMins, 0);
            timePerSetContainer.Add(timePerSetMins);


            BorderedEntry timePerSetSecondsEntry = new BorderedEntry()
            {

            };
            timePerSetSecondsEntry.Entry.Placeholder = "seconds";
            timePerSetSecondsEntry.Entry.Keyboard = Keyboard.Numeric;
            Grid.SetColumn(timePerSetSecondsEntry, 2);
            Grid.SetRow(timePerSetSecondsEntry, 0);
            timePerSetContainer.Add(timePerSetSecondsEntry);
            timePerSetSecondsEntry.Entry.SetBinding(Entry.TextProperty, new Binding(nameof(_vm.TimePerSetSeconds), BindingMode.TwoWay));

            Label timePerSetSeconds = new Label
            {
                FontSize = 18,
                TextColor = Colors.Black,
                Text = "''"
            };
            Grid.SetColumn(timePerSetSeconds, 3);
            Grid.SetRow(timePerSetSeconds, 0);
            timePerSetContainer.Add(timePerSetSeconds);

            Grid.SetColumn(timePerSetContainer, 1);
            Grid.SetRow(timePerSetContainer, 3);
            container.Add(timePerSetContainer);


            BoxView line = new BoxView
            {
                Color = Colors.LightGray
            };
            Grid.SetColumn(line, 0);
            Grid.SetColumnSpan(line, 2);
            Grid.SetRow(line, 4);
            container.Add(line);


            EntryTagLabel repsLabel = new EntryTagLabel
            {
                Text = "Reps: ",
            };
            Grid.SetColumn(repsLabel, 0);
            Grid.SetRow(repsLabel, 5);
            container.Add(repsLabel);

            BorderedEntry repsEntry = new BorderedEntry()
            {

            };
            repsEntry.Entry.Keyboard = Keyboard.Numeric;
            repsEntry.Entry.Placeholder = "Enter the number of repetition per set";
            Grid.SetColumn(repsEntry, 1);
            Grid.SetRow(repsEntry, 5);
            container.Add(repsEntry);
            repsEntry.Entry.SetBinding(Entry.TextProperty, new Binding(nameof(_vm.CurrentExercise.Repetitions), BindingMode.TwoWay, source: _vm.CurrentExercise));


            Label x = new Label
            {
                Text = "X",
                FontSize = 18,
                TextColor = Colors.Black,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
            };
            Grid.SetColumn(x, 1);
            Grid.SetRow(x, 6);
            container.Add(x);

            EntryTagLabel setsLabel = new EntryTagLabel
            {
                Text = "Sets: ",
            };
            Grid.SetColumn(setsLabel, 0);
            Grid.SetRow(setsLabel, 7);
            container.Add(setsLabel);

            BorderedEntry setsEntry = new BorderedEntry()
            {

            };
            setsEntry.Entry.Keyboard = Keyboard.Numeric;
            setsEntry.Entry.Placeholder = "Enter the number of sets";
            Grid.SetColumn(setsEntry, 1);
            Grid.SetRow(setsEntry, 7);
            container.Add(setsEntry);
            setsEntry.Entry.SetBinding(Entry.TextProperty, new Binding(nameof(_vm.CurrentExercise.Sets), BindingMode.TwoWay, source: _vm.CurrentExercise));


            EntryTagLabel restLabel = new EntryTagLabel
            {
                Text = "Rest: ",
            };
            Grid.SetColumn(restLabel, 0);
            Grid.SetRow(restLabel, 8);
            container.Add(restLabel);

            Grid restTimeContainer = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(){Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition(){Width = new GridLength(10, GridUnitType.Absolute)},
                    new ColumnDefinition(){Width = GridLength.Star},
                    new ColumnDefinition(){Width = new GridLength(10, GridUnitType.Absolute)},
                },
                ColumnSpacing = 1,
            };
            BorderedEntry restMinEntry = new BorderedEntry()
            {

            };
            restMinEntry.Entry.Placeholder = "minutes";
            restMinEntry.Entry.Keyboard = Keyboard.Numeric;
            Grid.SetColumn(restMinEntry, 0);
            Grid.SetRow(restMinEntry, 0);
            restTimeContainer.Add(restMinEntry);
            restMinEntry.Entry.SetBinding(Entry.TextProperty, new Binding(nameof(_vm.RestMinutes), BindingMode.TwoWay, source: _vm));


            Label mins = new Label
            {
                FontSize = 18,
                TextColor = Colors.Black,
                Text = "'"
            };
            Grid.SetColumn(mins, 1);
            Grid.SetRow(mins, 0);
            restTimeContainer.Add(mins);


            BorderedEntry secondsEntry = new BorderedEntry()
            {

            };
            secondsEntry.Entry.Placeholder = "seconds";
            secondsEntry.Entry.Keyboard = Keyboard.Numeric;
            Grid.SetColumn(secondsEntry, 2);
            Grid.SetRow(secondsEntry, 0);
            restTimeContainer.Add(secondsEntry);
            secondsEntry.Entry.SetBinding(Entry.TextProperty, new Binding(nameof(_vm.RestSeconds), BindingMode.TwoWay, source: _vm));

            Label seconds = new Label
            {
                FontSize = 18,
                TextColor = Colors.Black,
                Text = "''"
            };
            Grid.SetColumn(seconds, 3);
            Grid.SetRow(seconds, 0);
            restTimeContainer.Add(seconds);


            Grid.SetColumn(restTimeContainer, 1);
            Grid.SetRow(restTimeContainer, 8);
            container.Add(restTimeContainer);

            BoxView line2 = new BoxView
            {
                Color = Colors.LightGrey,
            };
            Grid.SetColumn(line2, 0);
            Grid.SetColumnSpan(line2, 2);
            Grid.SetRow(line2, 9);
            container.Add(line2);


            EntryTagLabel partsLabel = new EntryTagLabel
            {
                Text = "Targeting\nBody\nParts: ",
                Margin = new Thickness(0,0,0,5),
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                VerticalTextAlignment = TextAlignment.Start,
                HorizontalTextAlignment = TextAlignment.End,
            };
            Grid.SetColumn(partsLabel, 0);
            Grid.SetRow(partsLabel, 10);
            Grid.SetRowSpan(partsLabel, 2);
            container.Add(partsLabel);

            Button changeTargetingPartsButton = new Button
            {
                Text = "Tap To Change",
                BackgroundColor = Palette.PrimaryBackground,
                TextColor = Palette.Primary,
                CornerRadius = 10,
                HorizontalOptions = LayoutOptions.Fill
            };
            Grid.SetColumn(changeTargetingPartsButton, 1);
            Grid.SetRow(changeTargetingPartsButton, 10);
            container.Add(changeTargetingPartsButton);

            FlexLayout currentTargetingParts = new FlexLayout
            {
                AlignContent = FlexAlignContent.Start,
                AlignItems = FlexAlignItems.Start,
                Direction = FlexDirection.Row,
                JustifyContent = FlexJustify.Start,
                Wrap = FlexWrap.Wrap,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
            };
            currentTargetingParts.SetBinding(BindableLayout.ItemsSourceProperty, nameof(_vm.TargetingParts));
            BindableLayout.SetItemTemplate(currentTargetingParts, new DataTemplate(() =>
            {
                Label part = new Label
                {
                    HeightRequest = 20,
                    FontSize = 16,
                    TextColor = Colors.DarkGrey,
                    Margin = new Thickness(0, 0, 5, 0),
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Start,
                };
                part.SetBinding(Label.TextProperty, new Binding(".", converter: new EnumToTargetingPartsConverter()));
                return part;
            }));
            Grid.SetColumn(currentTargetingParts, 1);
            Grid.SetRow(currentTargetingParts, 11);
            container.Add(currentTargetingParts);

            Button saveButton = new Button()
            {
                Text = "Save",
                FontSize = 18,
                TextColor = Colors.White,
                BackgroundColor = Palette.Secondary,
                CornerRadius = 10,
            };
            Grid.SetColumn(saveButton, 0);
            Grid.SetRow(saveButton, 12);
            Grid.SetColumnSpan(saveButton, 2);
            saveButton.SetBinding(Button.CommandProperty, new Binding(nameof(_vm.SaveCommand), source: _vm));
            container.Add(saveButton);


            Button cancelButton = new Button()
            {
                Text = "Cancel",
                FontSize = 18,
                TextColor = Colors.Black,
                BackgroundColor = Colors.Grey,
                CornerRadius = 10,
            };
            Grid.SetColumn(cancelButton, 0);
            Grid.SetRow(cancelButton, 13);
            Grid.SetColumnSpan(cancelButton, 2);
            cancelButton.SetBinding(Button.CommandProperty, new Binding(nameof(_vm.CancelCommand), source: _vm));
            container.Add(cancelButton);


            Content = container;

            changeTargetingPartsButton.Command = new Command(async() =>
            {
                TargetingPartsPopUp targetingPartsPopUp = new TargetingPartsPopUp
                {
                    CanBeDismissedByTappingOutsideOfPopup = true,
                };
                targetingPartsPopUp.SetBinding(TargetingPartsPopUp.SelectedPartsProperty, new Binding(nameof(_vm.TargetingParts), BindingMode.TwoWay, source: _vm));

                await this.ShowPopupAsync(targetingPartsPopUp);
            });
        }
    }

 
}
