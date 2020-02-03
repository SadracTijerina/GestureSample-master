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

		public int finalXCord { get; set; }
		public int finalYCord { get; set; }

		public int initXCord { get; set; }
		public int inityCord { get; set; }


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


			checkGameOver();
		}

		protected override void OnPanning(MR.Gestures.PanEventArgs e)
		{
			base.OnPanning(e);

			finalXCord = (int)(e.Touches[0].X * 3 / e.ViewPosition.Width);
			finalYCord = (int)(e.Touches[0].Y * 3 / e.ViewPosition.Height);

			//var xCord = e.DeltaDistance.X;
			//var yCord = e.DeltaDistance.Y;
		}

		protected override void OnPanned(MR.Gestures.PanEventArgs e)
		{
			base.OnPanned(e);

			tradeImages();
		}

		protected override void OnLongPressing(MR.Gestures.LongPressEventArgs e)
		{
			base.OnLongPressing(e);

			initXCord = (int)(e.Touches[0].X * 3 / e.ViewPosition.Width);
			inityCord = (int)(e.Touches[0].Y * 3 / e.ViewPosition.Height);
		}


		private void tradeImages () {
			AddText("Initial (x,y): ({0},{1}) Final (x,y): ({2},{3}", initXCord, inityCord, finalXCord, finalYCord);

			string xString = initXCord.ToString();
			string yString = inityCord.ToString();

			string initialImage = "ImageCell" + xString + yString;

			xString = finalXCord.ToString();
			yString = finalYCord.ToString();

			string finalImage = "ImageCell" + xString + yString;

			/*Trying to figure out how to trade the grids */


	

		}

		private void checkGameOver()
		{
			char winner = GetWinner();

			if(winner != ' ')
			{
				AddText("{0} won! Congratulations!", winner);
				signsOnBoard = 9;
			}
			else if(signsOnBoard == 9)
			{
				AddText("A draw. Try again.");
			}
		}

		private char GetWinner()
		{
			char sign = board[1][1];
			if (sign != ' ')
			{
				if (sign == board[0][0] && sign == board[2][2]
					|| sign == board[0][1] && sign == board[2][1]
					|| sign == board[0][2] && sign == board[2][0]
					|| sign == board[1][0] && sign == board[1][2])
				{
					return sign;
				}
			}

			sign = board[0][0];
			if (sign != ' ')
			{
				if (sign == board[0][1] && sign == board[0][2]
					|| sign == board[1][0] && sign == board[2][0])
				{
					return sign;
				}
			}

			sign = board[0][2];
			if (sign != ' ')
			{
				if (sign == board[1][2] && sign == board[2][2])
				{
					return sign;
				}
			}

			sign = board[2][0];
			if (sign != ' ')
			{
				if (sign == board[2][1] && sign == board[2][2])
				{
					return sign;
				}
			}

			return ' ';
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
