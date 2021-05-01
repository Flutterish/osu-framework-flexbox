using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Platform;
using osu.Framework.Testing;

namespace osu.Framework.Tests {
	public class FlexboxTestBrowser : FlexboxTestGame {
		protected override void LoadComplete () {
			base.LoadComplete();

			AddRange( new Drawable[]
			{
				new TestBrowser("Peri.osu.Framework"),
				new CursorContainer()
			} );
		}

		public override void SetHost ( GameHost host ) {
			base.SetHost( host );
			host.Window.CursorState |= CursorState.Hidden;
		}
	}
}
