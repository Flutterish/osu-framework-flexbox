using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace osu.Framework.Tests.Visual.Examples {
	[TestFixture]
	public class TestSceneCrossAxisSizingWithOneAbsoluteItem : FlexboxTestScene {
		Flexbox flexbox;

		public TestSceneCrossAxisSizingWithOneAbsoluteItem () {
			Add( new SampleFlexboxContainer(
				flexbox = new SampleFlexbox {
					Width = 800,
					AutoSizeAxes = Axes.Y
				}
			) );

			SampleBox aitem = new SampleBox();
			aitem.Grow = 1;
			aitem.Height = 100.Pixels();
			aitem.HeightBindable.BindValueChanged( v => {
				aitem.Text = $"{v.NewValue}";
			}, true );

			flexbox.Add( aitem );
			AddLabel( $"Item 0" );
			AddSliderStep( "Cross size [px]", 0, 500, 100, v => aitem.Height = v.Pixels() );
			AddSliderStep( "Min cross size [px]", 0, 500, 0, v => aitem.MinHeight = v.Pixels() );
			AddSliderStep( "Max cross size [px]", 0, 500, 300, v => aitem.MaxHeight = v.Pixels() );

			for ( int i = 0; i < 3; i++ ) {
				var item = new SampleBox();
				item.Grow = 1;
				item.HeightBindable.BindValueChanged( v => {
					item.Text = $"{v.NewValue}";
				}, true );

				flexbox.Add( item );
				AddLabel( $"Item {i+1}" );
				AddSliderStep( "Cross size [%]", 0, 100, 100, v => item.Height = v.Percent() );
				AddSliderStep( "Min cross size [px]", 0, 500, 0, v => item.MinHeight = v.Pixels() );
				AddSliderStep( "Max cross size [px]", 0, 500, 300, v => item.MaxHeight = v.Pixels() );
			}
		}
	}
}
