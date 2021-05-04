using NUnit.Framework;
using osu.Framework.Graphics.Containers;

namespace osu.Framework.Tests.Visual.Examples {
	[TestFixture]
	public class TestSceneCrossAxisSizing : FlexboxTestScene {
		Flexbox flexbox;

		public TestSceneCrossAxisSizing () {
			Add( new SampleFlexboxContainer(
				flexbox = new SampleFlexbox {
					Width = 800,
					Height = 310,
				}
			) );

			AddLabel( "Flexbox" );
			AddSliderStep( "Height", 0, 500, 300, v => flexbox.Height = v );

			for ( int i = 0; i < 3; i++ ) {
				SampleBox item = new SampleBox();
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
