using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu.Framework.Tests.Visual.Layout {
	[TestFixture]
	public class TestSceneFlexboxLayout : FlexboxTestScene {
		Flexbox flexbox;

		FlexboxElement a;
		FlexboxElement b;
		FlexboxElement c;

		public TestSceneFlexboxLayout () {
			Add( new Container {
				AutoSizeAxes = Axes.Both,
				Anchor = Anchor.Centre,
				Origin = Anchor.Centre,
				Children = new Drawable[] {
					new Box {
						RelativeSizeAxes = Axes.Both,
						Colour = Colour4.Gray
					},
					flexbox = new Flexbox {
						Direction = FlexDirection.Row,
						Width = 500,
						Height = 200,

						Children = new FlexboxElement[] {
							a = new() {
								Basis = 100.Pixels(),
								Drawable = new Box {
									Colour = Colour4.Red,
									Margin = new MarginPadding( 10 )
								}
							},
							b = new() {
								Basis = 100.Pixels(),
								Drawable = new Box {
									Colour = Colour4.Green,
									Margin = new MarginPadding( 5 )
								}
							},
							c = new() {
								Basis = 100.Pixels(),
								Drawable = new Box {
									Colour = Colour4.Blue
								}
							}
						}
					}
				}
			} );

			AddSliderStep( "Direction", 0, 1, 0, dir => flexbox.Direction = (FlexDirection)dir );
			AddSliderStep( "Spacing", 0, 5, 0, sp => flexbox.Spacing = (FlexSpacing)sp );
			AddSliderStep( "Red basis", 0, 500, 100, v => a.Basis = v.Pixels() );
			AddSliderStep( "Green basis", 0, 500, 100, v => b.Basis = v.Pixels() );
			AddSliderStep( "Blue basis", 0, 500, 100, v => c.Basis = v.Pixels() );
			AddSliderStep( "Red grow", 0, 1, 0d, v => a.Grow = v );
			AddSliderStep( "Green grow", 0, 1, 0d, v => b.Grow = v );
			AddSliderStep( "Blue grow", 0, 1, 0d, v => c.Grow = v );
			AddSliderStep( "Red shrink", 0, 1, 1d, v => a.Shrink = v );
			AddSliderStep( "Green shrink", 0, 1, 1d, v => b.Shrink = v );
			AddSliderStep( "Blue shrink", 0, 1, 1d, v => c.Shrink = v );
		}
	}
}
