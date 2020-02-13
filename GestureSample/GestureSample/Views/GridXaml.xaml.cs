using Xamarin.Forms;
using System;
using System.Collections.Generic;
using GestureSample.ViewModels;

namespace GestureSample.Views
{
	public partial class GridXaml
	{

		public int finalXCord;
		public int finalYCord;
		public int initXCord;
		public int initYCord;

		Dictionary<string, string> grid = new Dictionary<string, string>();

		public GridXaml()
		{
			InitializeComponent();
			initializeGrid();
		}

		//This function is basically used to do the animation as we are panning
		void Image_Panning(object sender, MR.Gestures.PanEventArgs e)
		{
			//Here we are initializing the variable s to the image that we are controlling/panning
			var s = e.Sender as MR.Gestures.Image;

			if (s == null)
			{
				return;
			}

			MainGrid.RaiseChild(s);

			//These two lines is what does the animations not sure how the operator works though. The + when removed makes the squares just dissapear once in another grid block
			//0.5 was smooth but slow
			s.TranslationX += e.DeltaDistance.X * 0.5;
			s.TranslationY += e.DeltaDistance.Y * 0.5;

			if(e.DeltaDistance.Y > MainGrid.Height / 3 || e.DeltaDistance.X > MainGrid.Width / 3)
			{
				//This would be used to update in real time if possible...
			}

		}

		//This is called after panning is done, to update the grid
		void tradeImages(object sender, MR.Gestures.PanEventArgs e)
		{
			initXCord = TicTacToeViewModel.initXCord;
			initYCord = TicTacToeViewModel.inityCord;

			finalXCord = TicTacToeViewModel.finalXCord;
			finalYCord = TicTacToeViewModel.finalYCord;

			var s = e.Sender as MR.Gestures.Image;

			if (s == null)
			{
				return;
			}

			Dictionary<string, string> newGrid = new Dictionary<string, string>();

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

			if(finalPoint > initialPoint || initialPoint > finalPoint)
			{
				updateXaml(newGrid);

				foreach (KeyValuePair<string, string> gridItem in newGrid)
				{
					grid.Add(gridItem.Key, gridItem.Value);
				}

				newGrid.Clear();
			}

			s.TranslationX = finalXCord;
			s.TranslationY = finalYCord;
		}


		void initializeGrid()
		{
			/*TODO: Find a way to do this without hard coding eventually*/
			grid.Add("00", "#000000");
			grid.Add("01", "#32a852");
			grid.Add("02", "#a83232");
			grid.Add("10", "#3254a8");
			grid.Add("11", "#ffffff");
			grid.Add("12", "#a8a832");
			grid.Add("20", "#a87132");
			grid.Add("21", "#a83298");
			grid.Add("22", "#995f5f");
		}

		Dictionary<string, string> shiftRight(int initialPoint, int finalPoint)
		{
			Dictionary<string, string> newGrid = new Dictionary<string, string>();

			string strFinPoint = finalPoint.ToString();
			string strInitPoint = initialPoint.ToString();

			if(strFinPoint.Length == 1)
			{
				strFinPoint = "0" + strFinPoint;
			}
			else if(strInitPoint.Length == 1)
			{
				strInitPoint = "0" + strInitPoint;
			}

			newGrid.Add(strFinPoint, grid[strInitPoint]);

			foreach (KeyValuePair<string, string> gridItem in grid)
			{
				string nextGridKeyString = gridItem.Key;

				if(Int32.Parse(gridItem.Key) >= finalPoint && Int32.Parse(gridItem.Key) < initialPoint)
				{
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

			foreach(KeyValuePair<string, string> item in grid)
			{
				newGrid.Add(item.Key, item.Value);
			}

			grid.Clear();

			return newGrid;
		}

		Dictionary<string, string> shiftLeft(int initialPoint, int finalPoint)
		{
			Dictionary<string, string> newGrid = new Dictionary<string, string>();

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

			foreach (KeyValuePair<string, string> gridItem in grid)
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

			foreach (KeyValuePair<string, string> item in grid)
			{
				newGrid.Add(item.Key, item.Value);
			}

			grid.Clear();

			return newGrid;
		}

		void removeRepeatingKeys(Dictionary<string, string> goodGrid)
		{
			foreach (KeyValuePair<string, string> newItem in goodGrid)
			{
				if (grid.ContainsKey(newItem.Key))
				{
					grid.Remove(newItem.Key);
				}
			}
		}

		void updateXaml(Dictionary<string, string> updatedDashboard)
		{
			

			Cell00.BackgroundColor = Color.FromHex(updatedDashboard["00"]);
			Cell01.BackgroundColor = Color.FromHex(updatedDashboard["01"]);
			Cell02.BackgroundColor = Color.FromHex(updatedDashboard["02"]);
			Cell10.BackgroundColor = Color.FromHex(updatedDashboard["10"]);
			Cell11.BackgroundColor = Color.FromHex(updatedDashboard["11"]);
			Cell12.BackgroundColor = Color.FromHex(updatedDashboard["12"]);
			Cell20.BackgroundColor = Color.FromHex(updatedDashboard["20"]);
			Cell21.BackgroundColor = Color.FromHex(updatedDashboard["21"]);
			Cell22.BackgroundColor = Color.FromHex(updatedDashboard["22"]);


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

	}
}
