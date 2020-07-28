using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Zegogo.connections;
using Syncfusion.XForms.Buttons;
using Syncfusion.XForms.ComboBox;
using Syncfusion.XForms.TextInputLayout;
using Syncfusion.XForms.Pickers;
using System.Diagnostics;
using Zegogo.Resources;

namespace Zegogo.pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class options : ContentPage
	{
		List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();
		Filters filters = new Filters();
		int id = 0;
		public options(Filters filters)
		{
			InitializeComponent();
			this.filters = filters;
			foreach(var item in filters.filters)
			{
				if(item.type == "checkboxes")
				{
					StackLayout tempStack = new StackLayout();
					tempStack.Padding = new Thickness(10);
					Label tempLabel = new Label { Text = item.name};
					tempLabel.Padding = new Thickness(5, 0, 0, 0);
					tempStack.Children.Add(tempLabel);
					foreach(var item2 in item.options)
					{
						SfCheckBox checkBox = new SfCheckBox();
						checkBox.StateChanged += CheckBox_StateChanged;
						checkBox.HeightRequest = 35;
						checkBox.WidthRequest = 35;
						checkBox.Text = item2.name;
						tempStack.Children.Add(checkBox);
					}
					mainStack.Children.Add(tempStack);
				}
				else if(item.type == "select")
				{
					StackLayout tempStack = new StackLayout();
					tempStack.Padding = new Thickness(10,2,10,2);
					Label tempLabel = new Label { Text = item.name };
					tempLabel.Padding = new Thickness(5, 0, 0, 0);
					tempStack.Children.Add(tempLabel);
					SfComboBox comboBox = new SfComboBox();
					comboBox.Text = AppResources.choose;
					comboBox.HeightRequest = 40;
					comboBox.SelectionChanged += ComboBox_SelectionChanged;
					List<string> resolutionList = new List<string>();

					foreach (var item2 in item.options)
					{
						resolutionList.Add(item2.name);
					}
					comboBox.ComboBoxSource = resolutionList;
					tempStack.Children.Add(comboBox);
					mainStack.Children.Add(tempStack);
				}
				else if(item.type == "price_period")
				{
					StackLayout tempStack = new StackLayout();
					tempStack.Padding = new Thickness(10, -12, 10, -12);
					Label tempLabel = new Label { Text = item.name };
					tempLabel.Padding = new Thickness(5, 0, 0, 0);
					tempStack.Children.Add(tempLabel);
					SfDatePicker sfDatePickerFrom = new SfDatePicker();
					sfDatePickerFrom.Format = DateFormat.dd_MMM_yyyy;
					sfDatePickerFrom.DateSelected += SfDatePickerFrom_DateSelected;
					sfDatePickerFrom.PickerMode = PickerMode.Dialog;
					sfDatePickerFrom.ShowHeader = false;
					sfDatePickerFrom.ShowFooter = true;
					SfDatePicker sfDatePickerTo = new SfDatePicker();
					sfDatePickerTo.Format = DateFormat.dd_MMM_yyyy;
					sfDatePickerTo.DateSelected += SfDatePickerTo_DateSelected;
					sfDatePickerTo.PickerMode = PickerMode.Dialog;
					sfDatePickerTo.ShowHeader = false;
					sfDatePickerTo.ShowFooter = true;
					Grid grid = new Grid()
					{
						RowDefinitions =
						{
							new RowDefinition {Height = 0},
							new RowDefinition()
						},
						ColumnDefinitions =
						{
							new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) },
							new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) },
							new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) },
							new ColumnDefinition { Width = new GridLength(1.5, GridUnitType.Star) }
						}
						
					};
					grid.ColumnSpacing = 20;
					grid.HeightRequest = 61;
					SfButton sfButtonFrom = new SfButton();
					sfButtonFrom.Text = "From";
					sfButtonFrom.TextColor = Color.FromHex("#8D8D8D");
					sfButtonFrom.FontSize = 14;
					sfButtonFrom.BackgroundColor = Color.Transparent;
					sfButtonFrom.CornerRadius = 5;
					sfButtonFrom.BorderWidth = 0.5;
					sfButtonFrom.BorderColor = Color.FromHex("#8D8D8D");
					sfButtonFrom.Margin = new Thickness(0, 0, 0, 0);
					sfButtonFrom.Padding = new Thickness(15, 0, 0, 0);
					sfButtonFrom.HorizontalTextAlignment = TextAlignment.Start;
					sfButtonFrom.Clicked += SfButtonFrom_Clicked;
					SfButton sfButtonTo = new SfButton();
					sfButtonTo.Text = "To";
					sfButtonTo.TextColor = Color.FromHex("#8D8D8D");
					sfButtonTo.FontSize = 14;
					sfButtonTo.BackgroundColor = Color.Transparent;
					sfButtonTo.CornerRadius = 5;
					sfButtonTo.BorderWidth = 0.5;
					sfButtonTo.BorderColor = Color.FromHex("#8D8D8D");
					sfButtonTo.Margin = new Thickness(0, 0, 0, 0);
					sfButtonTo.Padding = new Thickness(15, 0, 0, 0);
					sfButtonTo.HorizontalTextAlignment = TextAlignment.Start;
					sfButtonTo.Clicked += SfButtonTo_Clicked;
					SfTextInputLayout sfTextInputLayout = new SfTextInputLayout();
					sfTextInputLayout.InputView = new Entry();
					sfTextInputLayout.InputView.Unfocused += InputView_Unfocused1;
					(sfTextInputLayout.InputView as Entry).Placeholder = id.ToString();
					(sfTextInputLayout.InputView as Entry).PlaceholderColor = Color.White;
					sfTextInputLayout.ContainerType = ContainerType.Outlined;
					sfTextInputLayout.UnfocusedColor = Color.FromHex("#8D8D8D");
					sfTextInputLayout.Hint = "Price";
					sfTextInputLayout.Padding = new Thickness(0, -8, 0, 0);
					sfTextInputLayout.Opacity = 0.8;
					SfButton sfButtonAdd = new SfButton();
					sfButtonAdd.Text = "+";
					sfButtonAdd.BackgroundColor = Color.Transparent;
					sfButtonAdd.TextColor = Color.FromHex("#BE593D");
					sfButtonAdd.FontSize = 40;
					sfButtonAdd.Padding = new Thickness(-15,0,0,10);
					sfButtonAdd.Clicked += SfButtonAdd_Clicked;
					grid.Children.Add(sfButtonFrom,0,1);
					grid.Children.Add(sfButtonTo,1,1);
					grid.Children.Add(sfTextInputLayout,2,1);
					grid.Children.Add(sfButtonAdd,3,1);
					grid.Children.Add(sfDatePickerFrom, 0, 0);
					grid.Children.Add(sfDatePickerTo, 1, 0);
					grid.Children.Add(new Label { Text = id.ToString(), IsVisible = false, HeightRequest=0 }, 2, 0);
					id++;
					tempStack.Children.Add(grid);
					mainStack.Children.Add(tempStack);
				}
				else
				{
					StackLayout tempStack = new StackLayout();
					tempStack.Padding = new Thickness(10, 2, 10, 2);
					var inputLayout = new SfTextInputLayout() { ContainerType = ContainerType.Outlined};
					
					inputLayout.Hint = item.name;
					inputLayout.HelperText = item.units;
					inputLayout.InputView = new Entry() { Placeholder = item.name, PlaceholderColor = Color.WhiteSmoke};
					inputLayout.InputView.Unfocused += InputView_Unfocused;
					tempStack.Children.Add(inputLayout);
					mainStack.Children.Add(tempStack);
				}
			}
			Button button = new Button();
			button.CornerRadius = 5;
			button.Text = AppResources.save;
			button.BackgroundColor = Color.FromHex("#BE593D");
			button.TextColor = Color.White;
			button.Padding = new Thickness(15);
			button.Margin = new Thickness(45, 15, 45, 15);
			button.Clicked += Button_Clicked;
			mainStack.Children.Add(button);
		}

		private void SfButtonAdd_Clicked(object sender, EventArgs e)
		{
			if((sender as SfButton).Text == "+")
			{
				(sender as SfButton).Text = "–";
				var col = ((sender as SfButton).Parent.Parent as StackLayout).Children;
				foreach (var item in col)
				{
					//DisplayAlert("1", item.ToString(), "1");
				}
				SfDatePicker sfDatePickerFrom = new SfDatePicker();
				sfDatePickerFrom.Format = DateFormat.dd_MMM_yyyy;
				sfDatePickerFrom.DateSelected += SfDatePickerFrom_DateSelected;
				sfDatePickerFrom.PickerMode = PickerMode.Dialog;
				sfDatePickerFrom.ShowHeader = false;
				sfDatePickerFrom.ShowFooter = true;
				SfDatePicker sfDatePickerTo = new SfDatePicker();
				sfDatePickerTo.Format = DateFormat.dd_MMM_yyyy;
				sfDatePickerTo.DateSelected += SfDatePickerTo_DateSelected;
				sfDatePickerTo.PickerMode = PickerMode.Dialog;
				sfDatePickerTo.ShowHeader = false;
				sfDatePickerTo.ShowFooter = true;
				Grid grid = new Grid()
				{
					RowDefinitions =
					{
						new RowDefinition {Height = 0},
						new RowDefinition()
					},
					ColumnDefinitions =
					{
						new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) },
						new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) },
						new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) },
						new ColumnDefinition { Width = new GridLength(1.5, GridUnitType.Star) }
					}
				};
				grid.HeightRequest = 61;
				grid.ColumnSpacing = 20;
				SfButton sfButtonFrom = new SfButton();
				sfButtonFrom.Text = "From";
				sfButtonFrom.TextColor = Color.FromHex("#8D8D8D");
				sfButtonFrom.FontSize = 14;
				sfButtonFrom.BackgroundColor = Color.Transparent;
				sfButtonFrom.CornerRadius = 5;
				sfButtonFrom.BorderWidth = 0.5;
				sfButtonFrom.BorderColor = Color.FromHex("#8D8D8D");
				sfButtonFrom.Margin = new Thickness(0, 0, 0, 0);
				sfButtonFrom.Padding = new Thickness(15, 0, 0, 0);
				sfButtonFrom.HorizontalTextAlignment = TextAlignment.Start;
				sfButtonFrom.Clicked += SfButtonFrom_Clicked;
				SfButton sfButtonTo = new SfButton();
				sfButtonTo.Text = "To";
				sfButtonTo.TextColor = Color.FromHex("#8D8D8D");
				sfButtonTo.FontSize = 14;
				sfButtonTo.BackgroundColor = Color.Transparent;
				sfButtonTo.CornerRadius = 5;
				sfButtonTo.BorderWidth = 0.5;
				sfButtonTo.BorderColor = Color.FromHex("#8D8D8D");
				sfButtonTo.Margin = new Thickness(0, 0, 0, 0);
				sfButtonTo.Padding = new Thickness(15, 0, 0, 0);
				sfButtonTo.HorizontalTextAlignment = TextAlignment.Start;
				sfButtonTo.Clicked += SfButtonTo_Clicked;
				SfTextInputLayout sfTextInputLayout = new SfTextInputLayout();
				sfTextInputLayout.InputView = new Entry ();
				sfTextInputLayout.InputView.Unfocused += InputView_Unfocused1;
				(sfTextInputLayout.InputView as Entry).Placeholder = id.ToString();
				(sfTextInputLayout.InputView as Entry).PlaceholderColor = Color.White;
				sfTextInputLayout.ContainerType = ContainerType.Outlined;
				sfTextInputLayout.UnfocusedColor = Color.FromHex("#8D8D8D");
				sfTextInputLayout.Hint = "Price";
				sfTextInputLayout.Padding = new Thickness(0, -8, 0, 0);
				sfTextInputLayout.Opacity = 0.8;
				SfButton sfButtonAdd = new SfButton();
				sfButtonAdd.Text = "+";
				sfButtonAdd.BackgroundColor = Color.Transparent;
				sfButtonAdd.TextColor = Color.FromHex("#BE593D");
				sfButtonAdd.FontSize = 40;
				sfButtonAdd.Padding = new Thickness(-15, 0, 0, 10);
				sfButtonAdd.Clicked += SfButtonAdd_Clicked;
				grid.Children.Add(sfButtonFrom, 0, 1);
				grid.Children.Add(sfButtonTo, 1, 1);
				grid.Children.Add(sfTextInputLayout, 2, 1);
				grid.Children.Add(sfButtonAdd, 3, 1);
				grid.Children.Add(sfDatePickerFrom, 0, 0);
				grid.Children.Add(sfDatePickerTo, 1, 0);
				grid.Children.Add(new Label { Text = id.ToString(), IsVisible = false, HeightRequest = 0 }, 2, 0);
				id++;
				((sender as SfButton).Parent.Parent as StackLayout).Children.Add(grid);
			}
			else
			{

				var temp = ((sender as SfButton).Parent.Parent) as StackLayout;
				foreach(var item in temp.Children)
				{
					DisplayAlert("1", item.ToString(), "1");
				}
				(((sender as SfButton).Parent.Parent) as StackLayout).Children.Remove((sender as SfButton).Parent as Grid);
				foreach(var item in tuples)
				{
					//if(item.Item1 == )
				}
			}
		}

		private async void InputView_Unfocused1(object sender, FocusEventArgs e)
		{
			var id = (sender as Entry).Placeholder;
			await DisplayAlert("1", id, "1");
			//tuples.Add(new Tuple<string, string>($"options[price_period][{id}][value]", (sender as Entry).Text));

		}

		private void SfDatePickerTo_DateSelected(object sender, Syncfusion.XForms.Pickers.DateChangedEventArgs e)
		{
			string id = (((sender as SfDatePicker).Parent as Grid).Children[6] as Label).Text;
			string date = (sender as SfDatePicker).Date.ToString("MM-dd-yyyy");

			tuples.Add(new Tuple<string, string>($"options[price_period][{id}][max]", date));
			(((sender as SfDatePicker).Parent as Grid).Children[1] as SfButton).Text = date;
			(((sender as SfDatePicker).Parent as Grid).Children[1] as SfButton).TextColor = Color.Black;
		}

		private void SfDatePickerFrom_DateSelected(object sender, Syncfusion.XForms.Pickers.DateChangedEventArgs e)
		{
			string id = (((sender as SfDatePicker).Parent as Grid).Children[6] as Label).Text;
			string date = (sender as SfDatePicker).Date.ToString("MM-dd-yyyy");

			tuples.Add(new Tuple<string, string>($"options[price_period][{id}][min]", date));
			(((sender as SfDatePicker).Parent as Grid).Children[0] as SfButton).Text = date;
			(((sender as SfDatePicker).Parent as Grid).Children[0] as SfButton).TextColor = Color.Black;
		}

		private void SfButtonTo_Clicked(object sender, EventArgs e)
		{
			(((sender as SfButton).Parent as Grid).Children[5] as SfDatePicker).IsOpen = true;
			(((sender as SfButton).Parent as Grid).Children[5] as SfDatePicker).Date = DateTime.Now;
		}

		private void SfButtonFrom_Clicked(object sender, EventArgs e)
		{
			(((sender as SfButton).Parent as Grid).Children[4] as SfDatePicker).IsOpen = true;
			(((sender as SfButton).Parent as Grid).Children[4] as SfDatePicker).Date = DateTime.Now;
		}

		private void InputView_Unfocused(object sender, Xamarin.Forms.FocusEventArgs e)
		{
			foreach (var item in filters.filters)
			{
				if (item.name == (sender as Entry).Placeholder)
				{
					try
					{
						if ((sender as Entry).Text.Length > 0)
						{
							tuples.RemoveAll(p => p.Item1 == $"options[{item.key}]");
							tuples.Add(new Tuple<string, string>($"options[{item.key}]", (sender as Entry).Text));
						}
						else
						{
							tuples.RemoveAll(p => p.Item1 == $"options[{item.key}]");
						}
					}
					catch
					{
						tuples.RemoveAll(p => p.Item1 == $"options[{item.key}]");
					}
				}
			}
		}

		private void ComboBox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
		{
			foreach (var item in filters.filters)
			{
				if (item.type == "select")
				{
					foreach (var item2 in item.options)
					{
						if (item2.name == (sender as SfComboBox).SelectedItem.ToString())
						{		
							tuples.RemoveAll(p => p.Item1 == $"options[{item.key}]");
							tuples.Add(new Tuple<string, string>($"options[{item.key}]", item2.key));
						}
					}
				}
			}
		}
		private void CheckBox_StateChanged(object sender, StateChangedEventArgs e)
		{
			if ((sender as SfCheckBox).IsChecked == true)
			{
				foreach(var item in filters.filters)
				{
					if(item.type == "checkboxes")
					{
						foreach (var item2 in item.options)
						{
							if (item2.name == (sender as SfCheckBox).Text)
							{
								tuples.Add(new Tuple<string, string>($"options[{item.key}][]", item2.key));
							}
						}
					}
					
				}
			}
			else
			{
				foreach (var item in filters.filters)
				{
					if (item.type == "checkboxes")
					{
						foreach (var item2 in item.options)
						{
							if (item2.name == (sender as SfCheckBox).Text)
							{
								tuples.RemoveAll(p => p.Item2 == item2.key);
							}
						}
					}
				}
			}
		}

		private async void Button_Clicked(object sender, EventArgs e)
		{
			
			MessagingCenter.Send(tuples, "sendListOptions");
			await Navigation.PopAsync();
		}

		private async void ImageButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PopAsync();
		}
	}
}