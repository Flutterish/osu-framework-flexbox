using NUnit.Framework;
using osu.Framework.Graphics.Containers;

namespace osu.Framework.Tests.Visual.Examples {
	[TestFixture]
	public class TestSceneFlexGrow : FlexboxTestScene {
		Flexbox flexbox;

		public TestSceneFlexGrow () {
			Add( new SampleFlexboxContainer(
				flexbox = new SampleFlexbox {
					Width = 800,
					Height = 110,
				}
			) );

			for ( int i = 0; i < 3; i++ ) {
				SampleBox item = new SampleBox();
				item.Grow = i == 1 ? 2 : 1;
				item.GrowBindable.BindValueChanged( v => {
					item.Text = $"{v.NewValue:N2}";
				}, true );

				flexbox.Add( item );
				AddLabel( $"Item {i+1}" );
				AddSliderStep( "Grow", 0, 5, item.Grow, v => item.Grow = v );
				AddSliderStep( "Min size", 0, 800, 0, v => item.MinWidth = v.Pixels() );
				AddSliderStep( "Max size", 0, 800, 800, v => item.MaxWidth = v.Pixels() );
			}
		}
	}
}
