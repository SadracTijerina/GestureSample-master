﻿using System;
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

		public static int finalXCord { get; set; }
		public static int finalYCord { get; set; }

		public static int initXCord { get; set; }
		public static int inityCord { get; set; }


		public TicTacToeViewModel()
		{
			InitBoard();
		}

		protected override void OnTapping(MR.Gestures.TapEventArgs e)
		{
			base.OnTapping(e);

			if (e.Touches == null) return;

			if (signsOnBoard == 9)
			{
				InitBoard();
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

			finalXCord = (int)(e.Touches[0].X * 3 / e.ViewPosition.Width);
			finalYCord = (int)(e.Touches[0].Y * 3 / e.ViewPosition.Height);


			//var s = e.Sender as MR.Gestures.Image;

			//if (s == null)
			//{
			//	return;
			//}

			////GridXaml.RaiseChild(s);

			//s.TranslationX += e.DeltaDistance.X;
			//s.TranslationY += e.DeltaDistance.Y;

			//var xCord = e.DeltaDistance.X;
			//var yCord = e.DeltaDistance.Y;
		}


		protected override void OnLongPressing(MR.Gestures.LongPressEventArgs e)
		{
			base.OnLongPressing(e);
			//y:188.57
			//x:338.66
			//AddText("Position width: " + e.ViewPosition.Width);
			AddText("X: " + e.Touches[0].X);
			//AddText("Position height: " + )
			AddText("Y: " + e.Touches[0].Y);

			initXCord = (int)(e.Touches[0].X * 3 / e.ViewPosition.Width);
			//On APA app it would probably be * 5 cause there is about 5 rows I believe
			inityCord = (int)(e.Touches[0].Y * 3 / e.ViewPosition.Height);
		}

		protected override void OnPanned(MR.Gestures.PanEventArgs e)
		{
			base.OnPanned(e);

			updateDashboard();
		}


		private void updateDashboard()
		{

		}

		private void InitBoard()
		{
			AddText("New game");

			board = new[] {
				new [] {' ', ' ', ' '},
				new [] {' ', ' ', ' '},
				new [] {' ', ' ', ' '},
			};
			next = 'X';
			signsOnBoard = 0;

			NotifyPropertyChanged(() => ImageCell00);
			NotifyPropertyChanged(() => ImageCell01);
			NotifyPropertyChanged(() => ImageCell02);
			NotifyPropertyChanged(() => ImageCell10);
			NotifyPropertyChanged(() => ImageCell11);
			NotifyPropertyChanged(() => ImageCell12);
			NotifyPropertyChanged(() => ImageCell20);
			NotifyPropertyChanged(() => ImageCell21);
			NotifyPropertyChanged(() => ImageCell22);
		}

		private string ValueToImage(char value)
		{
			if (value == 'X') return ImagePath + "X.png";
			if (value == 'O') return ImagePath + "O.png";
			return "";
		}
	}
}
