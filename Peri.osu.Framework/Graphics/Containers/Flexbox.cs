using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace osu.Framework.Graphics.Containers {
	// TODO invalidation
	public class Flexbox : CompositeDrawable {
		internal Dictionary<Drawable, ItemData> childData = new();

		public void Add ( FlexboxItem element ) {
			AddInternal( element.Drawable );
			childData.Add( element.Drawable, new ItemData( element ) );
		}
		public void AddRange ( IEnumerable<FlexboxItem> elements ) {
			foreach ( var i in elements ) {
				Add( i );
			}
		}
		public IReadOnlyList<FlexboxItem> Children {
			get => childData.Select( x => x.Value.Source ).ToImmutableList();
			set {
				Clear();
				AddRange( value );
			}
		}

		public void Clear () {
			childData.Clear();
			ClearInternal();
		}
		public void Remove ( Drawable drawable ) {
			RemoveInternal( drawable );
			childData.Remove( drawable );
		}

		public FlexSpacing Spacing = FlexSpacing.Start;
		public FlexDirection Direction = FlexDirection.Row;
		public FlexWrap Wrap = FlexWrap.NoWrap;

		new public Axes AutoSizeAxes {
			get => base.AutoSizeAxes;
			set => base.AutoSizeAxes = value;
		}

		internal class ItemData {
			public ItemData ( FlexboxItem element ) {
				Source = element;
			}

			public double Size;
			public double MaxSize;
			public double MinSize;
			public double Position;

			public FlexboxItem Source;
			public Drawable Drawable => Source.Drawable;
		}

		protected override void Update () {
			base.Update();

			if ( Wrap is FlexWrap.NoWrap ) {
				spaceLine( childData.Values );
			}
			else {
				List<List<ItemData>> lines = new();
				List<ItemData> line = new();
				lines.Add( line );
				double lineSize = 0;
				bool isHorizontal = Direction is FlexDirection.Row;
				double totalSpace = isHorizontal ? DrawWidth : DrawHeight;

				foreach ( var i in childData.Values ) {
					if ( isHorizontal && !i.Source.Height.IsAbsolute ) {
						throw new InvalidOperationException( $"Cannot have a horizontally wrapping flexbox with an item that scales vertically (yet). All the items need to have an absolute {nameof(FlexboxItem)}.{nameof(FlexboxItem.Height)}" );
					}
					else if ( !isHorizontal && !i.Source.Width.IsAbsolute ) {
						throw new InvalidOperationException( $"Cannot have a vertically wrapping flexbox with an item that scales horozontally (yet). All the items need to have an absolute {nameof(FlexboxItem)}.{nameof(FlexboxItem.Width)}" );
					}

					var size = ( i.Source.Basis.IsAbsolute ? i.Source.Basis.Amout : ( totalSpace * i.Source.Basis.Amout ) ) + i.Drawable.Margin.Left + i.Drawable.Margin.Right;
					if ( line.Count != 0 && lineSize + size > totalSpace ) {
						line = new();
						lines.Add( line );
						line.Add( i );
						lineSize = size;
					}
					else {
						line.Add( i );
						lineSize += size;
					}
				}

				double lineOffset = 0;
				foreach ( var _line in lines ) {
					spaceLine( _line );
					foreach ( var item in _line ) {
						if ( isHorizontal ) {
							item.Drawable.Y = (float)lineOffset;
						}
						else {
							item.Drawable.X = (float)lineOffset;
						}
					}
					lineOffset += _line.Max( x => isHorizontal ? x.Drawable.Height : x.Drawable.Width );
				}
			}
		}

		void spaceLine ( IEnumerable<ItemData> items ) {
			bool isHorizontal = Direction is FlexDirection.Row;

			double totalSpace = isHorizontal ? DrawWidth : DrawHeight;
			double takenSpace = 0;
			foreach ( var i in items ) {
				i.Drawable.Anchor = Anchor.TopLeft;
				i.Drawable.Origin = Anchor.TopLeft;
				if ( isHorizontal ) {
					i.Size = ( i.Source.Basis.IsAbsolute ? i.Source.Basis.Amout : ( totalSpace * i.Source.Basis.Amout ) ) + i.Drawable.Margin.Left + i.Drawable.Margin.Right;
					i.MinSize = i.Source.MinWidth.IsAbsolute ? i.Source.MinWidth.Amout : ( totalSpace * i.Source.MinWidth.Amout ) + i.Drawable.Margin.Left + i.Drawable.Margin.Right;
					i.MaxSize = i.Source.MaxWidth.IsAbsolute ? i.Source.MaxWidth.Amout : ( totalSpace * i.Source.MaxWidth.Amout ) + i.Drawable.Margin.Left + i.Drawable.Margin.Right;
				}
				else {
					i.Size = ( i.Source.Basis.IsAbsolute ? i.Source.Basis.Amout : ( totalSpace * i.Source.Basis.Amout ) ) + i.Drawable.Margin.Bottom + i.Drawable.Margin.Top;
					i.MinSize = i.Source.MinHeight.IsAbsolute ? i.Source.MinHeight.Amout : ( totalSpace * i.Source.MinHeight.Amout ) + i.Drawable.Margin.Bottom + i.Drawable.Margin.Top;
					i.MaxSize = i.Source.MaxHeight.IsAbsolute ? i.Source.MaxHeight.Amout : ( totalSpace * i.Source.MaxHeight.Amout ) + i.Drawable.Margin.Bottom + i.Drawable.Margin.Top;
				}

				i.Size = Math.Min( Math.Max( i.Size, i.MinSize ), i.MaxSize );
				takenSpace += i.Size;
			}

			if ( totalSpace > takenSpace ) {
				while ( totalSpace > takenSpace ) {
					var growable = items.Where( x => x.Source.Grow > 0 && x.Size < x.MaxSize ).ToArray();
					if ( !growable.Any() ) break;

					var freeSpace = totalSpace - takenSpace;
					var totalGrow = growable.Sum( x => x.Source.Grow );
					double limit = 1;

					foreach ( var i in growable ) {
						var growBy = freeSpace * ( i.Source.Grow / totalGrow );
						if ( i.Size + growBy > i.MaxSize ) {
							limit = Math.Min( limit, ( i.MaxSize - i.Size ) / growBy );
						}
					}

					foreach ( var i in growable ) {
						i.Size += freeSpace * ( i.Source.Grow / totalGrow ) * limit;
					}

					takenSpace = items.Sum( x => x.Size );
				}
			}
			else if ( totalSpace < takenSpace ) {
				while ( totalSpace < takenSpace ) {
					var shrinkable = items.Where( x => x.Source.Shrink > 0 && x.Size > x.MinSize ).ToArray();
					if ( !shrinkable.Any() ) break;

					var excessSpace = takenSpace - totalSpace;
					var totalScaledShrink = shrinkable.Sum( x => x.Source.Shrink * x.Size );
					double limit = 1;

					foreach ( var i in shrinkable ) {
						var shrinkBy = excessSpace * ( i.Source.Shrink * i.Size / totalScaledShrink );
						if ( i.Size - shrinkBy < i.MinSize ) {
							limit = Math.Min( limit, ( i.Size - i.MinSize ) / shrinkBy );
						}
					}

					foreach ( var i in shrinkable ) {
						i.Size -= excessSpace * ( i.Source.Shrink * i.Size / totalScaledShrink ) * limit;
					}

					takenSpace = items.Sum( x => x.Size );
				}
			}

			spaceItems( totalSpace - takenSpace, items );
			if ( isHorizontal ) {
				foreach ( var i in items ) {
					i.Drawable.Width = (float)i.Size - i.Drawable.Margin.Left - i.Drawable.Margin.Right;
					i.Drawable.X = (float)i.Position;
				}
			}
			else {
				foreach ( var i in items ) {
					i.Drawable.Height = (float)i.Size - i.Drawable.Margin.Top - i.Drawable.Margin.Bottom;
					i.Drawable.Y = (float)i.Position;
				}
			}

			if ( isHorizontal ) {
				foreach ( var i in items ) {
					i.Drawable.Height = (float)Math.Min( 
						Math.Max( 
							i.Source.Height.IsAbsolute ? i.Source.Height.Amout : ( i.Source.Height.Amout * DrawHeight ),
							i.Source.MinHeight.IsAbsolute ? i.Source.MinHeight.Amout : ( i.Source.MinHeight.Amout * DrawHeight )
						),
						i.Source.MaxHeight.IsAbsolute ? i.Source.MaxHeight.Amout : ( i.Source.MaxHeight.Amout * DrawHeight )
					) - i.Drawable.Margin.Bottom - i.Drawable.Margin.Top;
					i.Drawable.Y = 0;
				}
			}
			else {
				foreach ( var i in items ) {
					i.Drawable.Width = (float)Math.Min(
						Math.Max(
							i.Source.Width.IsAbsolute ? i.Source.Width.Amout : ( i.Source.Width.Amout * DrawWidth ),
							i.Source.MinWidth.IsAbsolute ? i.Source.MinWidth.Amout : ( i.Source.MinWidth.Amout * DrawWidth )
						),
						i.Source.MaxWidth.IsAbsolute ? i.Source.MaxWidth.Amout : ( i.Source.MaxWidth.Amout * DrawWidth )
					) - i.Drawable.Margin.Left - i.Drawable.Margin.Right;
					i.Drawable.X = 0;
				}
			}
		}

		void spaceItems ( double remainingSpace, IEnumerable<ItemData> items ) {
			var count = items.Count();

			double pos = 0;
			double gap = 0;

			if ( Spacing is FlexSpacing.Start ) {
				pos = 0;
			}
			else if ( Spacing is FlexSpacing.End ) {
				pos = remainingSpace;
			}
			else if( Spacing is FlexSpacing.Centre || count == 1 ) {
				pos = remainingSpace / 2;
			}
			else if ( Spacing is FlexSpacing.SpaceBetween ) {
				gap = remainingSpace / ( count - 1 );
			}
			else if ( Spacing is FlexSpacing.SpaceEvenly ) {
				gap = remainingSpace / ( count + 1 );
				pos = gap;
			}
			else if ( Spacing is FlexSpacing.SpaceAround ) {
				gap = remainingSpace / count;
				pos = gap / 2;
			}

			foreach ( var i in items ) {
				i.Position = pos;
				pos += i.Size + gap;
			}
		}
	}

	public class FlexboxItem {
		/// <summary>
		/// The base size of this element
		/// </summary>
		public Unit Basis = 0.Pixels();

		/// <summary>
		/// Minimum width
		/// </summary>
		public Unit MinWidth = 0.Pixels();
		/// <summary>
		/// Target width when this is the perpendicular direction
		/// </summary>
		public Unit Width = 100.Percent();
		/// <summary>
		/// Maximum width
		/// </summary>
		public Unit MaxWidth = 100.Percent();
		/// <summary>
		/// Minimum height
		/// </summary>
		public Unit MinHeight = 0.Pixels();
		/// <summary>
		/// Target height when this is the perpendicular direction
		/// </summary>
		public Unit Height = 100.Percent();
		/// <summary>
		/// Maximum height
		/// </summary>
		public Unit MaxHeight = 100.Percent();
		/// <summary>
		/// Weight of remaining space this element wants to occupy.
		/// </summary>
		public double Grow;
		/// <summary>
		/// If non-zero, this is the inverse goal ratio of this element when shrinking.
		/// </summary>
		public double Shrink = 1;

		public Drawable Drawable { get; init; }
		public static implicit operator Drawable ( FlexboxItem element )
			=> element.Drawable;
	}

	public struct Unit {
		public double Amout;
		public bool IsAbsolute;
	}

	public static class FlexboxExtensions {
		public static FlexboxItem GetFlexboxProperties ( this Drawable self )
			=> ( self.Parent as Flexbox ).childData[ self ].Source;

		public static Unit Pixels ( this double value )
			=> new Unit { Amout = value, IsAbsolute = true };

		public static Unit Percent ( this double value )
			=> new Unit { Amout = value / 100, IsAbsolute = false };

		public static Unit Pixels ( this int value )
			=> new Unit { Amout = value, IsAbsolute = true };

		public static Unit Percent ( this int value )
			=> new Unit { Amout = value / 100d, IsAbsolute = false };
	}

	public enum FlexSpacing {
		/// <summary>
		/// Items will be packed towards the start
		/// </summary>
		Start,
		/// <summary>
		/// Items will be packed thowards the end
		/// </summary>
		End,
		/// <summary>
		/// Items will be packed in the centre
		/// </summary>
		Centre,
		/// <summary>
		/// Items will have even gaps between, but no gap to the edges
		/// </summary>
		SpaceBetween,
		/// <summary>
		/// Items will have even spacing on both sides
		/// </summary>
		SpaceAround,
		/// <summary>
		/// Items will have even gaps between themselves and the edges
		/// </summary>
		SpaceEvenly
	}

	// TODO item order
	public enum FlexDirection {
		/// <summary>
		/// Items will be ordered horizontally
		/// </summary>
		Row,
		/// <summary>
		/// Items will be ordered vertically
		/// </summary>
		Column
	}

	public enum FlexWrap {
		/// <summary>
		/// All flex items will fin into a single line
		/// </summary>
		NoWrap,
		/// <summary>
		/// Flex items will create new lines when they would need to shrink
		/// </summary>
		Wrap
	}
}
