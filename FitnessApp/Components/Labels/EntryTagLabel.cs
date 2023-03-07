namespace FitnessApp.Components.Labels
{
    public class EntryTagLabel : Label
    {
        public EntryTagLabel() 
        {
            FontSize = 18;
            TextColor = Colors.Black;
            HorizontalOptions = LayoutOptions.Fill;
            VerticalOptions = LayoutOptions.Center;
            HorizontalTextAlignment = TextAlignment.End;
            VerticalTextAlignment = TextAlignment.Center;
        }
    }
}
