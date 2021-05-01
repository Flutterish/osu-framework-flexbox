using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

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
						Wrap = FlexWrap.Wrap,
						Width = 500,
						Height = 200,
						AutoSizeAxes = Axes.Y,

						Children = new FlexboxElement[] {
							a = new() {
								Basis = 100.Pixels(),
								Height = 100.Pixels(),
								Grow = 1,
								Drawable = new Box {
									Colour = Colour4.Red,
									Margin = new MarginPadding { Horizontal = 5 }
								}
							},
							b = new() {
								Basis = 100.Pixels(),
								Height = 100.Pixels(),
								Grow = 1,
								Drawable = new Box {
									Colour = Colour4.Green,
									Margin = new MarginPadding { Horizontal = 5 }
								}
							},
							c = new() {
								Basis = 100.Pixels(),
								Height = 100.Pixels(),
								Grow = 1,
								Drawable = new Box {
									Colour = Colour4.Blue,
									Margin = new MarginPadding { Horizontal = 5 }
								}
							},
							new() {
								Basis = 100.Pixels(),
								Height = 100.Pixels(),
								Grow = 1,
								Drawable = new Box {
									Colour = Colour4.Red,
									Margin = new MarginPadding { Horizontal = 5 }
								}
							},
							new() {
								Basis = 100.Pixels(),
								Height = 100.Pixels(),
								Grow = 1,
								Drawable = new Box {
									Colour = Colour4.Green,
									Margin = new MarginPadding { Horizontal = 5 }
								}
							},
							new() {
								Basis = 100.Pixels(),
								Height = 100.Pixels(),
								Grow = 1,
								Drawable = new Box {
									Colour = Colour4.Blue,
									Margin = new MarginPadding { Horizontal = 5 }
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
		}
	}
}
