using Xamarin.Forms;
using System;
using System.Collections.Generic;
using GestureSample.ViewModels;
using System.Threading.Tasks;

namespace GestureSample.Views
{
	public partial class GridXaml
	{
		public int finalXCord;
		public int finalYCord;
		public int initXCord;
		public int initYCord;

		List<Label> gridBlocks = new List<Label>();

		int prevFinalPoint = Int32.Parse(TicTacToeViewModel.finalCoordinatesString);

		public GridXaml()
		{
			InitializeComponent();

			gridBlocks.Add(Block1);
			gridBlocks.Add(Block2);
			gridBlocks.Add(Block3);
			gridBlocks.Add(Block4);
			gridBlocks.Add(Block5);
			gridBlocks.Add(Block6);
			gridBlocks.Add(Block7);
			gridBlocks.Add(Block8);
			gridBlocks.Add(Block9);

		}

		//This is an animation used to inform the user when the block is ready to get moved
		async void shakeLabel(object sender, MR.Gestures.Label label)
		{
			uint timeout = 50;

			if (label == null) return;

			await label.TranslateTo(-10, 0, timeout);
			await label.TranslateTo(10, 0, timeout);

			await label.TranslateTo(-5, 0, timeout);
			await label.TranslateTo(5, 0, timeout);

			label.TranslationX = 0;
		}

		////This is used to let the user know when he can move the block
		void longPressing(object sender, MR.Gestures.LongPressEventArgs e)
		{
			var label = e.Sender as MR.Gestures.Label;

			if (label == null) return;

			shakeLabel(sender, label);
		}

		//This function is basically used to do the animation as we are panning
		void Image_Panning(object sender, MR.Gestures.PanEventArgs e)
		{
			initXCord = TicTacToeViewModel.initXCord;
			initYCord = TicTacToeViewModel.inityCord;
			finalXCord = TicTacToeViewModel.finalXCord;
			finalYCord = TicTacToeViewModel.finalYCord;

			if (initXCord == -1 || initYCord == -1) return;

			int initialPoint = Int32.Parse(initYCord.ToString() + initXCord.ToString());

			//Here we are initializing the variable s to the image that we are controlling/panning
			var label = e.Sender as MR.Gestures.Label;

			if (label == null) return;
			
			MainGrid.RaiseChild(label);

			//These two lines is what does the animations. TotalDistance is what made the animation smooth. += Is what assigns a handler to an event
			label.TranslationX += e.TotalDistance.X;
			label.TranslationY += e.TotalDistance.Y;

			//This sets up once we try to trade images to bring it back to its original spot, due to it going past the grid length which kept causing bugs.
			if (e.ViewPosition.Y > MainGrid.Height) {
				TicTacToeViewModel.finalXCord = TicTacToeViewModel.initXCord;
				TicTacToeViewModel.finalYCord = TicTacToeViewModel.inityCord;
				return;
			}

			if (finalXCord < 0 || finalYCord < 0) return;

			int finalPoint = Int32.Parse(finalYCord.ToString() + finalXCord.ToString());

			//We are rotating the rest of the blocks as we are panning a block.
			if (initialPoint > finalPoint && prevFinalPoint != finalPoint)
				shiftRightAnimation(initialPoint, finalPoint, label);
			else if (initialPoint < finalPoint && prevFinalPoint != finalPoint) 
				shiftLeftAnimation(initialPoint, finalPoint, label);

			prevFinalPoint = finalPoint;

			//This is used to update the initial point so it stops blocks from shifting that don't need to be shifted, therefore causeing overlap, page 246 of BuJo
			string initialUpdatedStringPoint = prevFinalPoint.ToString(); ;

			if (initialUpdatedStringPoint.Length == 1)
				initialUpdatedStringPoint = "0" + initialUpdatedStringPoint;
			
			TicTacToeViewModel.inityCord = Int32.Parse(initialUpdatedStringPoint[0].ToString());
			TicTacToeViewModel.initXCord = Int32.Parse(initialUpdatedStringPoint[1].ToString());

		}

		//Used to shift and update icons while dragging the icon thats intended to get dropped
		void shiftLeftAnimation(int initialPoint, int finalPoint, MR.Gestures.Label label)
		{
			Console.WriteLine("Initial Point: " + initialPoint + " Final Point: " + finalPoint);

			int row, column;

			//Now this checks the rest of the items in grid to move to the right if its between the final point and less than the initial point
			foreach (MR.Gestures.Label gridItem in gridBlocks)
			 {
				 if ((Int32.Parse(gridItem.GetValue(Grid.RowProperty).ToString() + gridItem.GetValue(Grid.ColumnProperty).ToString()) <= finalPoint &&
					Int32.Parse(gridItem.GetValue(Grid.RowProperty).ToString() + gridItem.GetValue(Grid.ColumnProperty).ToString()) > initialPoint) && gridItem != label) {
					//	If its in the first column we have to move to next row and set column to 0, else we just go to the next column
					if (Int32.Parse(gridItem.GetValue(Grid.ColumnProperty).ToString()) == 0) {
						row = Int32.Parse(gridItem.GetValue(Grid.RowProperty).ToString());
						row--;
						gridItem.SetValue(Grid.RowProperty, row);
						gridItem.SetValue(Grid.ColumnProperty, 2);
					} else {
						column = Int32.Parse(gridItem.GetValue(Grid.ColumnProperty).ToString());
						row = Int32.Parse(gridItem.GetValue(Grid.RowProperty).ToString());
						column--;
						gridItem.SetValue(Grid.ColumnProperty, column);
						gridItem.SetValue(Grid.RowProperty, row);
					}
				}
			 }
		}

		//Used to shift icons and update while dragging the icon thats intended to get dropped
		void shiftRightAnimation(int initialPoint, int finalPoint, MR.Gestures.Label label)
		{
			Console.WriteLine("Initial Point: " + initialPoint + " Final Point: " + finalPoint);

			int row, column;

			//Now this checks the rest of the items in grid to move to the right if its between the final point and less than the initial point
			foreach (MR.Gestures.Label gridItem in gridBlocks)
			{
				if ((Int32.Parse(gridItem.GetValue(Grid.RowProperty).ToString() + gridItem.GetValue(Grid.ColumnProperty).ToString()) >= finalPoint &&
					Int32.Parse(gridItem.GetValue(Grid.RowProperty).ToString() + gridItem.GetValue(Grid.ColumnProperty).ToString()) < initialPoint) && gridItem != label) {
					//	If its in the first column we have to move to the previous row and set column to 2, else we just go to the next column
					if (Int32.Parse(gridItem.GetValue(Grid.ColumnProperty).ToString()) == 2) {
						row = Int32.Parse(gridItem.GetValue(Grid.RowProperty).ToString());
						row++;
						gridItem.SetValue(Grid.RowProperty, row);
						gridItem.SetValue(Grid.ColumnProperty, 0);
					} else {
						column = Int32.Parse(gridItem.GetValue(Grid.ColumnProperty).ToString());
						row = Int32.Parse(gridItem.GetValue(Grid.RowProperty).ToString());
						column++;
						gridItem.SetValue(Grid.ColumnProperty, column);
						gridItem.SetValue(Grid.RowProperty, row);
					}
				}
			}
		}
	
		//This is called after panning is done, to update the block that got dropped
		void updateBlockDragged(object sender, MR.Gestures.PanEventArgs e)
		{
			if(finalXCord == -1 || finalYCord == -1) return;

			int finalCoordinatePoint = Int32.Parse(finalYCord.ToString() + finalXCord.ToString());

			string stringFinPoint = finalCoordinatePoint.ToString();

			//the only way the string is length 1 is if the point is in the first row causing the first number of int to be 0
			if (stringFinPoint.Length == 1) 
				stringFinPoint = "0" + stringFinPoint;

			var label = e.Sender as MR.Gestures.Label;

			//Here we are getting the coordinates from the view model because for whatever reason we can't accurately get the coordinates through the file behind xaml
			initXCord = TicTacToeViewModel.initXCord;
			initYCord = TicTacToeViewModel.inityCord;

			finalXCord = TicTacToeViewModel.finalXCord;
			finalYCord = TicTacToeViewModel.finalYCord;

			//If the user doesn't have an image selected we can't put back the block to its final position
			if (label == null) return;

			//This sets the block thats getting dragged to its final spot it was dropped in
			int row = Int32.Parse(stringFinPoint[0].ToString());
			int column = Int32.Parse(stringFinPoint[1].ToString());
			label.SetValue(Grid.RowProperty, row);
			label.SetValue(Grid.ColumnProperty, column);
			label.TranslationX = finalXCord;
			label.TranslationY = finalYCord;

			//resets everything after getting dropped
			TicTacToeViewModel.inityCord = -1;
			TicTacToeViewModel.finalYCord = -1;
			TicTacToeViewModel.initXCord = -1;
			TicTacToeViewModel.finalXCord = -1;
		}
	}
}
