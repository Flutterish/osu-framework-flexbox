using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace osu.Framework.Tests.Visual.Examples {
	[TestFixture]
	public class TestSceneFlexWrap : FlexboxTestScene {
		Flexbox flexbox;

		public TestSceneFlexWrap () {
			Add( new SampleFlexboxContainer(
				flexbox = new SampleFlexbox {
					Width = 500,
					AutoSizeAxes = Axes.Y,
					Wrap = FlexWrap.Wrap
				}
			) );

			AddLabel( "Flexbox" );
			AddSliderStep( "Width", 0, 1000, 500, v => flexbox.Width = v );

			for ( int i = 0; i < 16; i++ ) {
				SampleBox item = new SampleBox {
					Height = 100.Pixels(),
					Grow = 1,
					Basis = 100.Pixels(),
					Text = $"{i+1}"
				};

				flexbox.Add( item );
			}
		}
	}
}
