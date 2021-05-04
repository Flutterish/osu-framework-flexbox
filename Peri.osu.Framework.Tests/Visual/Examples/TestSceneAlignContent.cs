using NUnit.Framework;
using osu.Framework.Graphics.Containers;
using System;
using System.Linq;

namespace osu.Framework.Tests.Visual.Examples {
	[TestFixture]
	public class TestSceneAlignContent : FlexboxTestScene {
		Flexbox flexbox;

		public TestSceneAlignContent () {
			Add( new SampleFlexboxContainer(
				flexbox = new SampleFlexbox {
					Width = 500,
					Height = 500,
					Wrap = FlexWrap.Wrap
				}
			) );

			AddLabel( "Flexbox" );
			AddSliderStep( "Width", 0, 1000, 500, v => flexbox.Width = v );
			AddSliderStep( "Content alignment", 0, 4, 0, v => flexbox.ContentAlignment = (ContentAlignment)v );
			foreach ( var (e, i) in Enum.GetValues<ContentAlignment>().Zip( Enumerable.Range( 0, Enum.GetValues<ContentAlignment>().Count() ) ) ) {
				AddLabel( $"{i}: {e}" );
			}

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
