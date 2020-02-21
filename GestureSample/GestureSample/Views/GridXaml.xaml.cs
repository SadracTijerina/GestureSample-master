using Xamarin.Forms;
using System;
using System.Collections.Generic;
using GestureSample.ViewModels;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestureSample.Views
{
	public partial class GridXaml
	{
		public int finalXCord;
		public int finalYCord;
		public int initXCord;
		public int initYCord;

		public string tappedCord;

		Dictionary<string, List<string>> grid = new Dictionary<string, List<string>>();

		public GridXaml()
		{
			InitializeComponent();
			initializeGrid();
		}

		void tappedGrid(object sender, MR.Gestures.TapEventArgs e)
		{
			tappedCord = TicTacToeViewModel.tapYCord.ToString() + TicTacToeViewModel.tapXCord.ToString();

			updateXaml(grid, true);

		}

		//This function is basically used to do the animation as we are panning
		void Image_Panning(object sender, MR.Gestures.PanEventArgs e)
		{
			initXCord = TicTacToeViewModel.initXCord;
			initYCord = TicTacToeViewModel.inityCord;

			if (initXCord == -1 || initYCord == -1)
			{
				return;
			}

			//Here we are initializing the variable s to the image that we are controlling/panning
			var s = e.Sender as MR.Gestures.Image;

			if (s == null)
			{
				return;
			}

			MainGrid.RaiseChild(s);

			//These two lines is what does the animations. TotalDistance is what made the animation smooth
			s.TranslationX += e.TotalDistance.X;
			s.TranslationY += e.TotalDistance.Y;

			//This sets up once we try to trade images to bring it back to its original spot, due to it going past the grid length which kept causing bugs.
			if (e.ViewPosition.Y > MainGrid.Height)
			{
				TicTacToeViewModel.finalXCord = TicTacToeViewModel.initXCord;
				TicTacToeViewModel.finalYCord = TicTacToeViewModel.inityCord;
				return;
			}

			//TODO: Shuffle the rest of the blocks as we are panning one image if possible
		}

		//This is called after panning is done, to update the grid
		void tradeImages(object sender, MR.Gestures.PanEventArgs e)
		{

			//Here we are getting the coordinates from the view model because for whatever reason we can't accurately get the coordinates through the file behind xaml
			initXCord = TicTacToeViewModel.initXCord;
			initYCord = TicTacToeViewModel.inityCord;

			finalXCord = TicTacToeViewModel.finalXCord;
			finalYCord = TicTacToeViewModel.finalYCord;

			var s = e.Sender as MR.Gestures.Label;

			//If the user doesn't have an image selected we don't want to try and shuffle the blocks
			if (s == null)
			{
				return;
			}

			//if the points are equal just move the block back to its intial block since in some occasions it would be hovering over another grid item since it wasn't fully dragged over there. 
			//As well as check if we went past the grid length we should just go back to its position since it would struggle when dealing with the length, not width
			if ( finalYCord < 0 || finalYCord > 2 || (initXCord == finalXCord && initYCord == finalYCord) )
			{
				s.TranslationX = initXCord;
				s.TranslationY = initYCord;
				return;
			}

			//Now that we have checked for "every" potential error we can start focusing in creating our new grid
			Dictionary<string, List<string>> newGrid = new Dictionary<string, List<string>>();

			int initialPoint = Int32.Parse(initYCord.ToString() + initXCord.ToString());
			int finalPoint = Int32.Parse(finalYCord.ToString() + finalXCord.ToString());


			//If the initial points is greater than the final points we shifting to the right
			if (initialPoint > finalPoint)
			{
				newGrid = shiftRight(initialPoint, finalPoint);
			}
			else if (finalPoint > initialPoint)
			{
				newGrid = shiftLeft(initialPoint, finalPoint);
			}

			updateXaml(newGrid, false);

			foreach (KeyValuePair<string, List<string>> gridItem in newGrid)
			{
				grid.Add(gridItem.Key, gridItem.Value);
			}

			newGrid.Clear();

			//This sets the image to the final grid spot it was dropped since visually it wouldn't go to its dropped location
			s.TranslationX = finalXCord;
			s.TranslationY = finalYCord;

			TicTacToeViewModel.inityCord = -1;
			TicTacToeViewModel.finalYCord = -1;
			TicTacToeViewModel.initXCord = -1;
			TicTacToeViewModel.finalXCord = -1;

		}

		//Basically the logic between shifting the icons right
		Dictionary<string, List<string>> shiftRight(int initialPoint, int finalPoint)
		{
			//Similar to how grid dictionary is set up, we are going to set up newGrid
			Dictionary<string, List<string>> newGrid = new Dictionary<string, List<string>>();

			string strFinPoint = finalPoint.ToString();
			string strInitPoint = initialPoint.ToString();

			//the only way the string is length 1 is if the point is in the first row causing the first number of int to be 0
			if(strFinPoint.Length == 1)
			{
				strFinPoint = "0" + strFinPoint;
			}
			if(strInitPoint.Length == 1)
			{
				strInitPoint = "0" + strInitPoint;
			}

			List<string> finalPointColor = grid[strInitPoint];
			//The final point now contains the block that got dropped there
			newGrid.Add(strFinPoint, finalPointColor);

			//Now this checks the rest of the items in grid to move to the right if its between the final point and less than the initial point
			foreach (KeyValuePair<string, List<string>> gridItem in grid)
			{
				string nextGridKeyString = gridItem.Key;

				if(Int32.Parse(gridItem.Key) >= finalPoint && Int32.Parse(gridItem.Key) < initialPoint)
				{
					//If its in the last column we have to move to next row and set column to 0, else we just go to the next column
					if(nextGridKeyString[1] == '2')
					{
						int nextRow =Int32.Parse(nextGridKeyString[0].ToString());
						nextRow++;
						nextGridKeyString = string.Empty;
						nextGridKeyString = nextRow.ToString() + "0";
					}
					else
					{
						int nextColumn = Int32.Parse(nextGridKeyString[1].ToString());
						nextColumn++;
						nextGridKeyString = nextGridKeyString.Remove(1);
						nextGridKeyString = nextGridKeyString + nextColumn.ToString();
					}

					newGrid.Add(nextGridKeyString, gridItem.Value);
				}
			}

			removeRepeatingKeys(newGrid);

			//Anything that is still in grid that wasn't deleted should be added to newGrid to have the completed grid
			foreach(KeyValuePair<string, List<string>> item in grid)
			{
				newGrid.Add(item.Key, item.Value);
			}

			//We want to overrite grid so we should clear it of data now that we have all its data in newGrid
			grid.Clear();

			return newGrid;
		}

		//Basically the logic between shifting the icons left
		Dictionary<string, List<string>> shiftLeft(int initialPoint, int finalPoint)
		{
			Dictionary<string, List<string>> newGrid = new Dictionary<string, List<string>>();

			string strFinPoint = finalPoint.ToString();
			string strInitPoint = initialPoint.ToString();

			if (strFinPoint.Length == 1)
			{
				strFinPoint = "0" + strFinPoint;
			}
			if (strInitPoint.Length == 1)
			{
				strInitPoint = "0" + strInitPoint;
			}

			newGrid.Add(strFinPoint, grid[strInitPoint]);

			foreach (KeyValuePair<string, List<string>> gridItem in grid)
			{
				string nextGridKeyString = gridItem.Key;

				if (Int32.Parse(gridItem.Key) <= finalPoint && Int32.Parse(gridItem.Key) > initialPoint)
				{
					if (nextGridKeyString[1] == '0')
					{
						int nextRow = Int32.Parse(nextGridKeyString[0].ToString());
						nextRow--;
						nextGridKeyString = string.Empty;
						nextGridKeyString = nextRow.ToString() + "2";
					}
					else
					{
						int nextColumn = Int32.Parse(nextGridKeyString[1].ToString());
						nextColumn--;
						nextGridKeyString = nextGridKeyString.Remove(1);
						nextGridKeyString = nextGridKeyString + nextColumn.ToString();
					}

					newGrid.Add(nextGridKeyString, gridItem.Value);

				}

			}

			removeRepeatingKeys(newGrid);

			foreach (KeyValuePair<string, List<string>> item in grid)
			{
				newGrid.Add(item.Key, item.Value);
			}

			grid.Clear();

			return newGrid;
		}

	
		void removeRepeatingKeys(Dictionary<string, List<string>> goodGrid)
		{
			foreach (KeyValuePair<string, List<string>> newItem in goodGrid)
			{
				if (grid.ContainsKey(newItem.Key))
				{
					grid.Remove(newItem.Key);
				}
			}
		}

		void updateXaml(Dictionary<string, List<string>> updatedGrid, bool tapped)
		{
			List<string> gridItem = new List<string>();

			gridItem = updatedGrid["00"];
			string color = gridItem[0];
			Cell00.BackgroundColor = Color.FromHex(color);

			gridItem = updatedGrid["01"];
			color = gridItem[0];
			Cell01.BackgroundColor = Color.FromHex(color);

			gridItem = updatedGrid["02"];
			color = gridItem[0];
			Cell02.BackgroundColor = Color.FromHex(color);

			gridItem = updatedGrid["10"];
			color = gridItem[0];
			Cell10.BackgroundColor = Color.FromHex(color);

			gridItem = updatedGrid["11"];
			color = gridItem[0];
			Cell11.BackgroundColor = Color.FromHex(color);

			gridItem = updatedGrid["12"];
			color = gridItem[0];
			Cell12.BackgroundColor = Color.FromHex(color);

			gridItem = updatedGrid["20"];
			color = gridItem[0];
			Cell20.BackgroundColor = Color.FromHex(color);

			gridItem = updatedGrid["21"];
			color = gridItem[0];
			Cell21.BackgroundColor = Color.FromHex(color);

			gridItem = updatedGrid["22"];
			color = gridItem[0];
			Cell22.BackgroundColor = Color.FromHex(color);

			if(tapped)
			{
				string colorText;
				List<string> text;

				if(tappedCord == "00")
				{
					text = updatedGrid["00"];
					colorText = text[1];

					Cell00.Text = colorText;
				}
				else if (tappedCord == "01")
				{
					text = updatedGrid["01"];
					colorText = text[1];

					Cell01.Text = colorText;
				}
				else if (tappedCord == "02")
				{
					text = updatedGrid["02"];
					colorText = text[1];

					Cell02.Text = colorText;
				}
				else if (tappedCord == "10")
				{
					text = updatedGrid["10"];
					colorText = text[1];

					Cell10.Text = colorText;
				}
				else if (tappedCord == "11")
				{
					text = updatedGrid["11"];
					colorText = text[1];

					Cell11.Text = colorText;
				}
				else if (tappedCord == "12")
				{
					text = updatedGrid["12"];
					colorText = text[1];

					Cell12.Text = colorText;
				}
				else if (tappedCord == "20")
				{
					text = updatedGrid["20"];
					colorText = text[1];

					Cell20.Text = colorText;
				}
				else if (tappedCord == "21")
				{
					text = updatedGrid["21"];
					colorText = text[1];

					Cell21.Text = colorText;
				}
				else
				{
					text = updatedGrid["22"];
					colorText = text[1];

					Cell22.Text = colorText;
				}
			}

			var row = Cell11.GetValue(Grid.RowProperty);
			var column = Cell11.GetValue(Grid.ColumnProperty);

			MainGrid.Children.Add(Cell00, 0, 0);
			MainGrid.Children.Add(Cell01, 1, 0);
			MainGrid.Children.Add(Cell02, 2, 0);
			MainGrid.Children.Add(Cell10, 0, 1);
			MainGrid.Children.Add(Cell11, 1, 1);
			MainGrid.Children.Add(Cell12, 2, 1);
			MainGrid.Children.Add(Cell20, 0, 2);
			MainGrid.Children.Add(Cell21, 1, 2);
			MainGrid.Children.Add(Cell22, 2, 2);
		}

		void initializeGrid()
		{
			/*TODO: Find a way to do this without hard coding eventually*/

			List<string> gridItem00 = new List<string>();
			List<string> gridItem01 = new List<string>();
			List<string> gridItem02 = new List<string>();
			List<string> gridItem10 = new List<string>();
			List<string> gridItem11 = new List<string>();
			List<string> gridItem12 = new List<string>();
			List<string> gridItem20 = new List<string>();
			List<string> gridItem21 = new List<string>();
			List<string> gridItem22 = new List<string>();


			gridItem00.Add("#000000");
			gridItem00.Add("Black");
			grid.Add("00", gridItem00);

			gridItem01.Add("#32a852");
			gridItem01.Add("Green");
			grid.Add("01", gridItem01);

			gridItem02.Add("#a83232");
			gridItem02.Add("Red");
			grid.Add("02", gridItem02);

			gridItem10.Add("#3254a8");
			gridItem10.Add("Blue");
			grid.Add("10", gridItem10);

			gridItem11.Add("#ffffff");
			gridItem11.Add("White");
			grid.Add("11", gridItem11);

			gridItem12.Add("#a8a832");
			gridItem12.Add("Yellow");
			grid.Add("12", gridItem12);

			gridItem20.Add("#a87132");
			gridItem20.Add("Brown");
			grid.Add("20", gridItem20);

			gridItem21.Add("#a83298");
			gridItem21.Add("Purple");
			grid.Add("21", gridItem21);

			gridItem22.Add("#995f5f");
			gridItem22.Add("IDK");
			grid.Add("22", gridItem22);

		}

	}
}
