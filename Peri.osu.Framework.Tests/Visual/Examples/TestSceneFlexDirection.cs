using NUnit.Framework;
using osu.Framework.Graphics.Containers;
using System;
using System.Linq;

namespace osu.Framework.Tests.Visual.Examples {
	[TestFixture]
	public class TestSceneFlexDirection : FlexboxTestScene {
		Flexbox flexbox;

		public TestSceneFlexDirection () {
			Add( new SampleFlexboxContainer(
				flexbox = new SampleFlexbox {
					Width = 800,
					Height = 110,
					Spacing = FlexSpacing.SpaceEvenly,

					Children = new FlexboxItem[] {
						new SampleBox() { Text = "1" },
						new SampleBox() { Text = "2" },
						new SampleBox() { Text = "3" },
						new SampleBox() { Text = "4" },
					}
				}	
			) );

			AddSliderStep( "Direction", 0, 1, 0, v => {
				flexbox.Direction = (FlexDirection)v;
				if ( v == 0 ) {
					flexbox.Width = 800;
					flexbox.Height = 110;
				}
				else {
					flexbox.Width = 110;
					flexbox.Height = 800;
				}
			} );
			foreach ( var (e, i) in Enum.GetValues<FlexDirection>().Zip( Enumerable.Range( 0, Enum.GetValues<FlexDirection>().Count() ) ) ) {
				AddLabel( $"{i}: {e}" );
			}
		}
	}
}
