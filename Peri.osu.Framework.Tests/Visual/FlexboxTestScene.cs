using osu.Framework.Testing;
using osu.Framework.Tests;

namespace osu.Framework.Tests.Visual {
	public class FlexboxTestScene : TestScene {
		protected override ITestSceneTestRunner CreateRunner () => new FlexboxTestSceneTestRunner();

		private class FlexboxTestSceneTestRunner : FlexboxTestGame, ITestSceneTestRunner {
			private TestSceneTestRunner.TestRunner runner;

			protected override void LoadAsyncComplete () {
				base.LoadAsyncComplete();
				Add( runner = new TestSceneTestRunner.TestRunner() );
			}

			public void RunTestBlocking ( TestScene test ) => runner.RunTestBlocking( test );
		}
	}
}
