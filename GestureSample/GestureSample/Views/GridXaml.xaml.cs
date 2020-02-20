﻿using Xamarin.Forms;
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

			//TODO: Figure out what to do if the user goes out of grid range with panning!
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

			var s = e.Sender as MR.Gestures.Image;

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

			updateXaml(newGrid);

			foreach (KeyValuePair<string, string> gridItem in newGrid)
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
		Dictionary<string, string> shiftRight(int initialPoint, int finalPoint)
		{
			//Similar to how grid dictionary is set up, we are going to set up newGrid
			Dictionary<string, string> newGrid = new Dictionary<string, string>();

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

			//The final point now contains the block that got dropped there
			newGrid.Add(strFinPoint, grid[strInitPoint]);

			//Now this checks the rest of the items in grid to move to the right if its between the final point and less than the initial point
			foreach (KeyValuePair<string, string> gridItem in grid)
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
			foreach(KeyValuePair<string, string> item in grid)
			{
				newGrid.Add(item.Key, item.Value);
			}

			//We want to overrite grid so we should clear it of data now that we have all its data in newGrid
			grid.Clear();

			return newGrid;
		}

		//Basically the logic between shifting the icons left
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

	}
}
