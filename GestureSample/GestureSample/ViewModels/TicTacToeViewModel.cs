using GestureSample.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GestureSample.ViewModels
{
	public class TicTacToeViewModel : CustomEventArgsViewModel
	{

		protected char[][] board;
		protected char next;
		protected int signsOnBoard;

		public string ImageCell00 { get { return ValueToImage(board[0][0]); }  }
		public string ImageCell01 { get { return ValueToImage(board[0][1]); }  }
		public string ImageCell02 { get { return ValueToImage(board[0][2]); }  }
		public string ImageCell10 { get { return ValueToImage(board[1][0]); }  }
		public string ImageCell11 { get { return ValueToImage(board[1][1]); } }
		public string ImageCell12 { get { return ValueToImage(board[1][2]); }  }
		public string ImageCell20 { get { return ValueToImage(board[2][0]); } }
		public string ImageCell21 { get { return ValueToImage(board[2][1]); }  }
		public string ImageCell22 { get { return ValueToImage(board[2][2]); } }

		protected double translationX = 0;
		public double TranslationX
		{
			get { return translationX; }
			set { SetProperty(ref translationX, value); }
		}

		protected double translationY = 0;
		public double TranslationY
		{
			get { return translationY; }
			set { SetProperty(ref translationY, value); }
		}

		public static int finalXCord = -1;
		public static int finalYCord = -1;

		public static int initXCord = -1;
		public static int inityCord = -1;


		public TicTacToeViewModel()
		{
		}

		protected override void OnTapping(MR.Gestures.TapEventArgs e)
		{
			base.OnTapping(e);

			if (e.Touches == null) return;

			if (signsOnBoard == 9)
			{
				return;
			}

			if (e.Touches == null || e.Touches.Length == 0)
			{
				AddText("Touch coordinates are missing.");
				return;
			}

			int xCord = (int)(e.Touches[0].X * 3 / e.ViewPosition.Width);
			int yCord = (int)(e.Touches[0].Y * 3 / e.ViewPosition.Height);

			if (Device.RuntimePlatform == Device.macOS)
				yCord = 2 - yCord;

		}

		protected override void OnPanning(MR.Gestures.PanEventArgs e)
		{
			base.OnPanning(e);

			var s = e.Sender as MR.Gestures.Image;

			if (initXCord == -1 || inityCord == -1)
			{
				return;
			}

			finalXCord = (int)(e.Touches[0].X * 3 / e.ViewPosition.Width);
			finalYCord = (int)(e.Touches[0].Y * 3 / e.ViewPosition.Height);

			if (s == null)
			{
				return;
			}


			//s.TranslationX += e.DeltaDistance.X;
			//s.TranslationY += e.DeltaDistance.Y;

		}

		protected override void OnLongPressing(MR.Gestures.LongPressEventArgs e)
		{
			base.OnLongPressing(e);

			initXCord = (int)(e.Touches[0].X * 3 / e.ViewPosition.Width);
			//On APA app it would probably be * 5 cause there is about 5 rows I believe
			inityCord = (int)(e.Touches[0].Y * 3 / e.ViewPosition.Height);
		}

		protected override void OnPanned(MR.Gestures.PanEventArgs e)
		{
			base.OnPanned(e);
		}


		private string ValueToImage(char value)
		{
			if (value == 'X') return ImagePath + "X.png";
			if (value == 'O') return ImagePath + "O.png";
			return "";
		}
	}
}
