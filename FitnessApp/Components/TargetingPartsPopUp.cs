using CommunityToolkit.Maui.Views;
using FitnessApp.Models;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;

namespace FitnessApp.Components
{
    public class TargetingPartsPopUp : Popup
    {

		#region SelectedParts BindableProperty
		public static readonly BindableProperty SelectedPartsProperty = BindableProperty.Create(
			propertyName: nameof(SelectedParts),
			returnType: typeof(ObservableCollection<object>),
			declaringType: typeof(TargetingPartsPopUp)
		);
		public ObservableCollection<object> SelectedParts
		{
			get => (ObservableCollection<object>)GetValue(SelectedPartsProperty);
			set => SetValue(SelectedPartsProperty, value);
		}
		#endregion

		public TargetingPartsPopUp()
		{
			
			Border border = new Border
			{
				StrokeShape = new RoundRectangle
				{
					CornerRadius = 10,
				},
				StrokeThickness = 0,
				Shadow = new Shadow
				{
					Opacity = 0.2f,
					Offset = new Point(0, 0),
					Radius = 10,
				}
			};

			Grid container = new Grid
			{
				RowDefinitions =
				{
					new RowDefinition(new GridLength(40, GridUnitType.Absolute)),
					new RowDefinition(new GridLength(1, GridUnitType.Star)),
				},
				ColumnDefinitions =
				{
					new ColumnDefinition(new GridLength(40, GridUnitType.Absolute)),
					new ColumnDefinition(new GridLength(1, GridUnitType.Star)),
					new ColumnDefinition(new GridLength(40, GridUnitType.Absolute)),
				},
				ColumnSpacing = 10,
				RowSpacing = 10,
				Padding = new Thickness(0, 0, 0, 15),
			};

			Label title = new Label 
			{ 
				Text = "Select All Targeting Body Parts",
				FontSize = 18,
				HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
			};
			Grid.SetRow(title, 0);
			Grid.SetColumn(title, 1);
            container.Add(title);


			ImageButton closeButton = new ImageButton
			{
				Source = "icon_close",
				Command = new Command(() =>
				{
					Close();
				}),
				HeightRequest = 20,
				WidthRequest = 20,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};
			Grid.SetRow(closeButton, 0);
			Grid.SetColumn(closeButton, 2);
			container.Add(closeButton);

			CollectionView partsList = new CollectionView
			{
				ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
				{
					ItemSpacing = 5,
				},
				SelectionMode = SelectionMode.Multiple,
				ItemsSource = ((TargetableParts[])Enum.GetValues(typeof(TargetableParts))).ToList(),
				HorizontalOptions = LayoutOptions.Fill,
				VerticalOptions = LayoutOptions.Fill,
			};

			partsList.ItemTemplate = new DataTemplate(() =>
			{
				Border buttonContainer = new Border
				{
					StrokeShape = new RoundRectangle()
					{
						CornerRadius = 10
					},
					StrokeThickness = 0,
					BackgroundColor = Palette.PrimaryBackground,
					HorizontalOptions = LayoutOptions.Fill,
					VerticalOptions = LayoutOptions.Center,
					Padding = 5,
					MinimumWidthRequest = 250,
				};

				Label part = new Label
				{
					TextColor = Colors.Black,
					FontSize = 18,
					HorizontalTextAlignment = TextAlignment.Center,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
				};

				part.SetBinding(Label.TextProperty, new Binding(".", converter: new EnumToTargetingPartsConverter()));

				buttonContainer.Content = part;

				return buttonContainer;
			});
			partsList.SetBinding(CollectionView.SelectedItemsProperty, new Binding(nameof(SelectedParts), BindingMode.TwoWay, source: this));

			Grid.SetRow(partsList, 1);
			Grid.SetColumn(partsList, 1);

			container.Add(partsList);
			border.Content = container;
			Content = border;


        }

	}
}
