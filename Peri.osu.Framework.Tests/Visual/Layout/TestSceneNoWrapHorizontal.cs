using NUnit.Framework;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Utils;
using osuTK.Graphics;
using System.Collections.Generic;

namespace osu.Framework.Tests.Visual.Layout {
	[TestFixture]
	public class TestSceneNoWrapHorizontal : FlexboxTestScene {
		const int itemCount = 5;
		static List<Color4> colors = new() {
			Color4.Red,
			Color4.Green,
			Color4.Blue
		};
		public TestSceneNoWrapHorizontal () {
			Flexbox flexbox = new() {
				Width = 800,
				Height = 100
			};
			AddLabel( $"Flexbox" );
			AddSliderStep( "Spacing", 0, 5, 0, v => flexbox.Spacing = (FlexSpacing)v );

			Add( new Container {
				AutoSizeAxes = Axes.Both,
				Children = new Drawable[] {
					new Box {
						Colour = Color4.Gray,
						RelativeSizeAxes = Axes.Both
					},
					flexbox
				}
			} );

			for ( int i = 0; i < itemCount; i++ ) {
				FlexboxItem item = new FlexboxItem {
					Basis = 100.Pixels(),
					Drawable = new Box {
						Colour = colors[ i % colors.Count ].Darken( RNG.NextSingle( 0, 0.2f ) )
					}
				};

				flexbox.Add( item );

				AddLabel( $"Item {i+1}" );
				AddSliderStep( "Flex basis", 0, 1000, 100, v => item.Basis = v.Pixels() );
				AddSliderStep( "Flex grow", 0, 1, 0d, v => item.Grow = v );
				AddSliderStep( "Flex shrink", 0, 1, 1d, v => item.Shrink = v );

				AddSliderStep( "Min width", 0, 1000, 0, v => item.MinWidth = v.Pixels() );
				AddSliderStep( "Max width", 0, 1000, 1000, v => item.MaxWidth = v.Pixels() );
				AddSliderStep( "Min height", 0, 1000, 0, v => item.MinHeight = v.Pixels() );
				AddSliderStep( "Max height", 0, 1000, 100, v => item.MaxHeight = v.Pixels() );
			}
		}
	}
}
