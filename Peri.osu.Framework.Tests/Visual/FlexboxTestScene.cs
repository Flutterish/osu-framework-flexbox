using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;
using osuTK.Graphics;

namespace osu.Framework.Tests.Visual {
	public abstract class FlexboxTestScene : TestScene {
		protected override ITestSceneTestRunner CreateRunner () => new FlexboxTestSceneTestRunner();

		private class FlexboxTestSceneTestRunner : FlexboxTestGame, ITestSceneTestRunner {
			private TestSceneTestRunner.TestRunner runner;

			protected override void LoadAsyncComplete () {
				base.LoadAsyncComplete();
				Add( runner = new TestSceneTestRunner.TestRunner() );
			}

			public void RunTestBlocking ( TestScene test ) => runner.RunTestBlocking( test );
		}

		public class SampleBox : FlexboxItem {
			TextFlowContainer text;

			public SampleBox () {
				Basis = 100.Pixels();

				Drawable = new Container {
					Margin = new MarginPadding( 5 ),
					Children = new Drawable[] {
						new Box {
							RelativeSizeAxes = Axes.Both,
							Colour = Color4.Orange
						},
						text = new TextFlowContainer {
							Origin = Anchor.Centre,
							Anchor = Anchor.Centre,
							TextAnchor = Anchor.Centre,
							AutoSizeAxes = Axes.Both
						}
					}
				};
			}

			public string Text {
				set => text.Text = value;
			}
		}

		public class SampleFlexbox : Flexbox {
			public SampleFlexbox () {

			}
		}

		public class SampleFlexboxContainer : Container {
			public SampleFlexboxContainer ( Flexbox flexbox ) {
				AutoSizeAxes = Axes.Both;
				Children = new Drawable[] {
					new Box {
						RelativeSizeAxes = Axes.Both,
						Colour = Color4.Purple
					},
					flexbox
				};
			}
		}
	}
}
