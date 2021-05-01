using osu.Framework.Platform;

namespace osu.Framework.Tests {
	public static class Program {
		public static void Main () {
			using ( GameHost host = Host.GetSuitableHost( "visual-tests" ) )
			using ( var game = new FlexboxTestBrowser() )
				host.Run( game );
		}
	}
}
