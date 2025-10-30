using BeebopNoteApp.Models;

namespace BeebopNoteApp.Views;

public partial class TestPage : ContentPage
{
	public TestPage()
	{
		InitializeComponent();
	}

	private Color ReturnAvailablePriorityColors()
	{
		var items = new Todoitem();
		return items.PriorityColor;
    }
}