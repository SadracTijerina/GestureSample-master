namespace GestureSample.Views
{
	public partial class ContentPageSampleXaml
	{
		public ContentPageSampleXaml()
		{
			InitializeComponent();
		}

		public void Image_Panning(object sender, MR.Gestures.PanEventArgs e)
		{
			var s = e.Sender as MR.Gestures.Image;

			if (s == null)
			{
				return;
			}

			MainGrid.RaiseChild(s);

			//These two lines is what does the animations not sure how the operator works though. The + when removed makes the squares just dissapear once in another grid block
			s.TranslationX += e.DeltaDistance.X;
			s.TranslationY += e.DeltaDistance.Y;
		}
        
		public void tradeImages(object sender, MR.Gestures.PanEventArgs e)
		{

		}
	}
}
