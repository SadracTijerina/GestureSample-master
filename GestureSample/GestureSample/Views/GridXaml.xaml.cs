
namespace GestureSample.Views
{
	public partial class GridXaml
	{
		public GridXaml()
		{
			InitializeComponent();
		}

		void Image_Panning(object sender, MR.Gestures.PanEventArgs e)
		{
			var s = e.Sender as MR.Gestures.Image;

			if (s == null)
			{
				return;
			}

			//GridXaml.RaiseChild(s);

			s.TranslationX += e.DeltaDistance.X;
			s.TranslationY += e.DeltaDistance.Y;


		}


	}
}
