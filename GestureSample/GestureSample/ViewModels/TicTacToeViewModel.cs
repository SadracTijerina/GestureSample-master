using System;
using Xamarin.Forms;

/* This viewmodel is just basically used to set/get coordinates of the image we are panning since in the code behind it wouldn't get it accurately.*/
namespace GestureSample.ViewModels
{
	public class TicTacToeViewModel : CustomEventArgsViewModel
	{
		public static int finalXCord = -1;
		public static int finalYCord = -1;

		public static int initXCord = -1;
		public static int inityCord = -1;

		public TicTacToeViewModel()
		{
		}

		//Used to get the final point
		protected override void OnPanning(MR.Gestures.PanEventArgs e)
		{
			base.OnPanning(e);

			if (initXCord == -1 || inityCord == -1)
			{
				return;
			}

			finalXCord = (int)(e.Touches[0].X * 3 / e.ViewPosition.Width);
			finalYCord = (int)(e.Touches[0].Y * 3 / e.ViewPosition.Height);

			//Checks if point isn't valid, if it isn't valid set them -1 since thats how we check them in GridXaml.
			if (initXCord < 0 || initXCord > 2 || inityCord < 0 || inityCord > 2 || finalXCord < 0 || finalXCord > 2 || finalYCord < 0 || finalYCord > 2)
			{
				initXCord = -1;
				inityCord = -1;
				finalXCord = -1;
				finalYCord = -1;
			}

		}

		//Just used to get the initial point
		protected override void OnLongPressing(MR.Gestures.LongPressEventArgs e)
		{
			base.OnLongPressing(e);

			initXCord = (int)(e.Touches[0].X * 3 / e.ViewPosition.Width);
			//On APA app it would probably be * 5 cause there is about 5 rows I believe
			inityCord = (int)(e.Touches[0].Y * 3 / e.ViewPosition.Height);
		}

	}
}
