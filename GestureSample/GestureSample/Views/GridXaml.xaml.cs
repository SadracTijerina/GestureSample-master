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

		//SortedDictionary<int, string> grid;

		public GridXaml()
		{
			InitializeComponent();

			//grid = new SortedDictionary<int, string>();

			//grid.Add(00, "#000000");
			//grid.Add(01, "#32a852");
			//grid.Add(02, "#a83232");
			//grid.Add(10, "#3254a8");
			//grid.Add(11, "#ffffff");
			//grid.Add(12, "#a8a832");
			//grid.Add(20, "#a87132");
			//grid.Add(21, "#a83298");
			//grid.Add(22, "#995f5f");
		}

		//So I need to do panning on the back file of xaml if I want to add the animation. Ill explain further in the code in this function
		void Image_Panning(object sender, MR.Gestures.PanEventArgs e)
		{

			//finalXCord = (int)(e.Touches[0].X * 3 / e.ViewPosition.Width);
			//finalYCord = (int)(e.Touches[0].Y * 3 / e.ViewPosition.Height) - 1;

		

			//Here we are initializing the variable s to the image that we are controlling/panning
			var s = e.Sender as MR.Gestures.Image;

			if (s == null)
			{
				return;
			}

			///This would raise the image to the top of the stack when moving around the icons. Only thing is its not able to recognize MainGrid.
			//MainGrid.RaiseChild(s);

			//These two lines is what does the animations not sure how the operator works though. The + when removed makes the squares just dissapear once in another grid block
			s.TranslationX += e.DeltaDistance.X;
			s.TranslationY += e.DeltaDistance.Y;

		}

		//void longPressed(object sender, MR.Gestures.LongPressEventArgs e)
		//{
		//	initXCord = (int)(e.Touches[0].X * 3 / e.ViewPosition.Width);
		//	////on APA app it would probably be * 5 cause there is about 5 rows I believe
		//	initYCord = (int)(e.Touches[0].Y * 3 / e.ViewPosition.Height);

		//}

		void tradeImages(object sender, MR.Gestures.PanEventArgs e)
		{

			initXCord = TicTacToeViewModel.initXCord;
			initYCord = TicTacToeViewModel.inityCord;

			finalXCord = TicTacToeViewModel.finalXCord;
			finalYCord = TicTacToeViewModel.finalYCord;

			SortedDictionary<int, string> newGrid = new SortedDictionary<int, string>();

			var s = e.Sender as MR.Gestures.Image;

			if (s == null)
			{
				return;
			}


			Console.WriteLine("Initial X Cordinate: " + initXCord + " Initial Y Cordinate: " + initYCord);
			//Console.WriteLine("Final X Cordinate: " + finalXCord + " Final Y Cordinate: " + finalYCord);

			//string initCordString = initXCord.ToString() + initYCord.ToString();
			//int initCord = Int32.Parse(initCordString);

			//string finalCordString = finalXCord.ToString() + finalYCord.ToString();
			//int finalCord = Int32.Parse(finalCordString);

			//grid[initCord] = grid[finalCord];

			//foreach (KeyValuePair<int, string> gridItem in grid)
			//{
			//	//Here I am trying to basically compare if the finalCord is smaller than the other value coordinates if so, 
			//	//we need to push them one more column to the right.
			//	//Change the colors when getting pushed pertaining to the coordinates.

			//	if (finalCord == gridItem.Key)
			//	{
			//		newGrid.Add(gridItem.Key, grid[initCord]);
			//		grid.Remove(gridItem.Key);
			//	}
			//	else if (finalCord < gridItem.Key)
			//	{
			//		newGrid.Add(gridItem.Key, grid[gridItem.Key - 1]);
			//		grid.Remove(gridItem.Key);
			//	}
			//	else
			//	{
			//		newGrid.Add(gridItem.Key, gridItem.Value);
			//		grid.Remove(gridItem.Key);
			//	}
			//}

			//foreach (KeyValuePair<int, string> printGrid in newGrid)
			//{
			//	Console.WriteLine("Key = {0}, Value = {1}", printGrid.Key, printGrid.Value);
			//}
		}

	}
}
