using FitnessApp.Components;
using Microsoft.Maui.Controls.Shapes;

namespace FitnessApp
{
    public class HomePage : ContentPage
    {
        private readonly HomePageViewModel _vm;
        public HomePage()
        {
            _vm = new HomePageViewModel();


            Image icon = new Image
            {
                Source = "lifting",
                Margin = 50,
                Aspect = Aspect.AspectFit,
                WidthRequest = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density * 0.5,
            };



            Label text = new Label()
            {

                FontSize = 25,
                TextColor = Colors.Black

            };

            text.SetBinding(Label.TextProperty, new Binding(nameof(_vm.DayCounter), source: _vm, stringFormat: "You have worked out for {0} days in a row!"));



            Button button = new Button()
            {
                Text = "+ Check In",
                FontSize = 25,
                TextColor = Colors.White,
                BackgroundColor = Palette.Secondary,
                CornerRadius = 10,
            };
            button.SetBinding(Button.CommandProperty, new Binding(nameof(_vm.DayIncreaseCommand), source: _vm));


            VerticalStackLayout layout = new VerticalStackLayout()
            {
                Padding = 30,
                Spacing = 30,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center
            };

            layout.Add(icon);
            layout.Add(text);
            layout.Add(button);

            Content = layout;
        }
    }
}
