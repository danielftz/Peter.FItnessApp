using Microsoft.Maui.Controls.Shapes;

namespace FitnessApp.Components.Entries
{
    public class BorderedEntry : Border
    {
        public readonly Entry Entry;

        public BorderedEntry() 
        {
            StrokeShape = new RoundRectangle()
            {
                CornerRadius = 10
            };
            StrokeThickness = 0;
            BackgroundColor = Palette.PrimaryBackground;
            HorizontalOptions = LayoutOptions.Fill;
            VerticalOptions = LayoutOptions.Center;

            Content = Entry = new Entry
            {
                PlaceholderColor = Colors.DarkGray,
                FontSize = 18,
                TextColor = Colors.Black,
                Keyboard = Keyboard.Plain,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                ClearButtonVisibility = ClearButtonVisibility.Never
            };
        }
    }
}
