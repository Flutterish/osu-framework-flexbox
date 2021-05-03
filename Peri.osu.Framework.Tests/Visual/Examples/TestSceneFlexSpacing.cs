using NUnit.Framework;
using osu.Framework.Graphics.Containers;
using System;
using System.Linq;

namespace osu.Framework.Tests.Visual.Examples {
	[TestFixture]
	public class TestSceneFlexSpacing : FlexboxTestScene {
		Flexbox flexbox;

		public TestSceneFlexSpacing () {
			Add( new SampleFlexboxContainer(
				flexbox = new SampleFlexbox {
					Width = 800,
					Height = 110,

					Children = new FlexboxItem[] {
						new SampleBox() { Text = "1" },
						new SampleBox() { Text = "2" },
						new SampleBox() { Text = "3" },
						new SampleBox() { Text = "4" },
					}
				}	
			) );

			AddSliderStep( "Spacing", 0, 5, 0, v => {
				flexbox.Spacing = (FlexSpacing)v;
			} );
			foreach ( var (e,i) in Enum.GetValues<FlexSpacing>().Zip( Enumerable.Range( 0, Enum.GetValues<FlexSpacing>().Count() ) ) ) {
				AddLabel( $"{i}: {e}" );
			}
		}
	}
}
