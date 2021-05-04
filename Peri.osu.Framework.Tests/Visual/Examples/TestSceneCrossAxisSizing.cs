using NUnit.Framework;
using osu.Framework.Graphics.Containers;
using System;
using System.Linq;

namespace osu.Framework.Tests.Visual.Examples {
	[TestFixture]
	public class TestSceneCrossAxisSizing : FlexboxTestScene {
		Flexbox flexbox;

		public TestSceneCrossAxisSizing () {
			Add( new SampleFlexboxContainer(
				flexbox = new SampleFlexbox {
					Width = 800,
					Height = 310,
				}
			) );

			AddLabel( "Flexbox" );
			AddSliderStep( "Height", 0, 500, 300, v => flexbox.Height = v );
			AddSliderStep( "Item alignment", 0, 3, 0, v => flexbox.ItemAlignment = (ItemAlignment)v );
			foreach ( var (e, i) in Enum.GetValues<ItemAlignment>().Zip( Enumerable.Range( 0, Enum.GetValues<ItemAlignment>().Count() ) ) ) {
				AddLabel( $"{i}: {e}" );
			}

			for ( int i = 0; i < 3; i++ ) {
				SampleBox item = new SampleBox();
				item.Grow = 1;
				item.HeightBindable.BindValueChanged( v => {
					item.Text = $"{v.NewValue}";
				}, true );

				flexbox.Add( item );
				AddLabel( $"Item {i+1}" );
				AddSliderStep( "Cross size [%]", 0, 100, 100, v => item.Height = v.Percent() );
				AddSliderStep( "Min cross size [px]", 0, 500, 0, v => item.MinHeight = v.Pixels() );
				AddSliderStep( "Max cross size [px]", 0, 500, 300, v => item.MaxHeight = v.Pixels() );
			}
		}
	}
}
