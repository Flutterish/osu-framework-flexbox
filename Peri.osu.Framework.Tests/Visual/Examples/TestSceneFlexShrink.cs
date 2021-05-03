using NUnit.Framework;
using osu.Framework.Graphics.Containers;

namespace osu.Framework.Tests.Visual.Examples {
	[TestFixture]
	public class TestSceneFlexShrink : FlexboxTestScene {
		Flexbox flexbox;

		public TestSceneFlexShrink () {
			Add( new SampleFlexboxContainer(
				flexbox = new SampleFlexbox {
					Width = 500,
					Height = 110,
				}
			) );

			AddLabel( "Flexbox" );
			AddSliderStep( "Width", 0, 1000, 500, v => flexbox.Width = v );

			for ( int i = 0; i < 3; i++ ) {
				SampleBox item = new SampleBox();
				item.Shrink = i == 1 ? 2 : 1;
				item.Basis = 200.Pixels();
				item.ShrinkBindable.BindValueChanged( v => {
					item.Text = $"{v.NewValue:N2}";
				}, true );

				flexbox.Add( item );
				AddLabel( $"Item {i+1}" );
				AddSliderStep( "Shrink", 0, 5, item.Shrink, v => item.Shrink = v );
				AddSliderStep( "Min size", 0, 800, 0, v => item.MinWidth = v.Pixels() );
				AddSliderStep( "Max size", 0, 800, 800, v => item.MaxWidth = v.Pixels() );
			}
		}
	}
}
