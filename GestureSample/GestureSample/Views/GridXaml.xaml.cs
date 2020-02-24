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

		public GridXaml()
		{
			InitializeComponent();
		}

		//This is used to let the user know when he can move the block
		void longPressing(object sender, MR.Gestures.PanEventArgs e)
		{
			var label = e.Sender as MR.Gestures.Label;

			if (label == null)
			{
				return;
			}

			label.FontAttributes = FontAttributes.Bold;
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
			var label = e.Sender as MR.Gestures.Label;

			if (label == null)
			{
				return;
			}

			MainGrid.RaiseChild(label);

			//These two lines is what does the animations. TotalDistance is what made the animation smooth
			label.TranslationX += e.TotalDistance.X;
			label.TranslationY += e.TotalDistance.Y;

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

			var label = e.Sender as MR.Gestures.Label;

			//If the user doesn't have an image selected we don't want to try and shuffle the blocks
			if (label == null)
			{
				return;
			}

			label.FontAttributes = FontAttributes.None;

			//if the points are equal just move the block back to its intial block since in some occasions it would be hovering over another grid item since it wasn't fully dragged over there. 
			//As well as check if we went past the grid length we should just go back to its position since it would struggle when dealing with the length, not width
			if ( finalYCord < 0 || finalYCord > 2 || (initXCord == finalXCord && initYCord == finalYCord) )
			{
				label.TranslationX = initXCord;
				label.TranslationY = initYCord;
				return;
			}

			int initialPoint = Int32.Parse(initYCord.ToString() + initXCord.ToString());
			int finalPoint = Int32.Parse(finalYCord.ToString() + finalXCord.ToString());


			//If the initial points is greater than the final points we shifting to the right
			if (initialPoint > finalPoint)
			{
				shiftRight(initialPoint, finalPoint);
			}
			else if (initialPoint < finalPoint)
			{
				shiftLeft(initialPoint, finalPoint);
			}

			//This sets the image to the final grid spot it was dropped since visually it wouldn't go to its dropped location
			label.TranslationX = finalXCord;
			label.TranslationY = finalYCord;

			TicTacToeViewModel.inityCord = -1;
			TicTacToeViewModel.finalYCord = -1;
			TicTacToeViewModel.initXCord = -1;
			TicTacToeViewModel.finalXCord = -1;

		}

		//Basically the logic between shifting the icons right
		void shiftRight(int initialPoint, int finalPoint)
		{
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

			List<MR.Gestures.Label> Cells = new List<MR.Gestures.Label>();

			Cells.Add(Block1);
			Cells.Add(Block2);
			Cells.Add(Block3);

			Cells.Add(Block4);
			Cells.Add(Block5);
			Cells.Add(Block6);

			Cells.Add(Block7);
			Cells.Add(Block8);
			Cells.Add(Block9);

			//The final point should now contain the block that got dropped there. The if statements is trying to find which is the inital point
			foreach (MR.Gestures.Label selectedItem in Cells)
			{
				if(selectedItem.GetValue(Grid.RowProperty).ToString() + selectedItem.GetValue(Grid.ColumnProperty).ToString() == strInitPoint)
				{ 
					
					int row = Int32.Parse(strFinPoint[0].ToString());
					int column = Int32.Parse(strFinPoint[1].ToString());
					selectedItem.SetValue(Grid.RowProperty, row);
					selectedItem.SetValue(Grid.ColumnProperty, column);

					//Now this checks the rest of the items in grid to move to the right if its between the final point and less than the initial point
					foreach (MR.Gestures.Label gridItem in Cells)
					{
						if ((Int32.Parse(gridItem.GetValue(Grid.RowProperty).ToString() + gridItem.GetValue(Grid.ColumnProperty).ToString()) >= finalPoint &&
							Int32.Parse(gridItem.GetValue(Grid.RowProperty).ToString() + gridItem.GetValue(Grid.ColumnProperty).ToString()) < initialPoint) && gridItem != selectedItem)
						{
							//If its in the last column we have to move to next row and set column to 0, else we just go to the next column
							if (Int32.Parse(gridItem.GetValue(Grid.ColumnProperty).ToString()) == 2)
							{
								row = Int32.Parse(gridItem.GetValue(Grid.RowProperty).ToString());
								row++;
								gridItem.SetValue(Grid.RowProperty, row);
								gridItem.SetValue(Grid.ColumnProperty, 0);
							}
							else
							{
								column = Int32.Parse(gridItem.GetValue(Grid.ColumnProperty).ToString());
								row = Int32.Parse(gridItem.GetValue(Grid.RowProperty).ToString());
								column++;
								gridItem.SetValue(Grid.ColumnProperty, column);
								gridItem.SetValue(Grid.RowProperty, row);
							}
						}

					}
					break;
				}
			}

		}

		//Basically the logic between shifting the icons left
		void shiftLeft(int initialPoint, int finalPoint)
		{
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

			List<MR.Gestures.Label> Cells = new List<MR.Gestures.Label>();

			Cells.Add(Block1);
			Cells.Add(Block2);
			Cells.Add(Block3);

			Cells.Add(Block4);
			Cells.Add(Block5);
			Cells.Add(Block6);

			Cells.Add(Block7);
			Cells.Add(Block8);
			Cells.Add(Block9);

			//The final point should now contain the block that got dropped there. The if statements is trying to find which is the inital point
			foreach (MR.Gestures.Label selectedItem in Cells)
			{
				if (selectedItem.GetValue(Grid.RowProperty).ToString() + selectedItem.GetValue(Grid.ColumnProperty).ToString() == strInitPoint)
				{
					int row = Int32.Parse(strFinPoint[0].ToString());
					int column = Int32.Parse(strFinPoint[1].ToString());
					selectedItem.SetValue(Grid.RowProperty, row);
					selectedItem.SetValue(Grid.ColumnProperty, column);

					//Now this checks the rest of the items in grid to move to the right if its between the final point and less than the initial point
					foreach (MR.Gestures.Label gridItem in Cells)
					{
						if ((Int32.Parse(gridItem.GetValue(Grid.RowProperty).ToString() + gridItem.GetValue(Grid.ColumnProperty).ToString()) <= finalPoint &&
							Int32.Parse(gridItem.GetValue(Grid.RowProperty).ToString() + gridItem.GetValue(Grid.ColumnProperty).ToString()) > initialPoint) &&  gridItem != selectedItem)
						{
							//If its in the first column we have to move to next row and set column to 2, else we just go to the next column
							if (Int32.Parse(gridItem.GetValue(Grid.ColumnProperty).ToString()) == 0)
							{
								row = Int32.Parse(gridItem.GetValue(Grid.RowProperty).ToString());
								row--;
								gridItem.SetValue(Grid.RowProperty, row);
								gridItem.SetValue(Grid.ColumnProperty, 2);

							}
							else
							{
								column = Int32.Parse(gridItem.GetValue(Grid.ColumnProperty).ToString());
								row = Int32.Parse(gridItem.GetValue(Grid.RowProperty).ToString());
								column--;
								gridItem.SetValue(Grid.ColumnProperty, column);
								gridItem.SetValue(Grid.RowProperty, row);
							}
						}
					}

					break;
				}
			}
		}
	}
}
