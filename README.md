# osu.Framework.Flexbox
A flexbox container for osu-framework.

Includes features such as:
* flex-basis, flex-grow and flex-shrink
* flex-direction
* flex-spacing
* flex-wrap
* min-width, max-width and width
* min-height, max-height and height
* avaiable in `px` and `%` units if applicable

The `Flexbox` container is included in `osu.Framework.Graphics.Containers`.

For a guide how flexboxes work [click here](https://css-tricks.com/snippets/css/a-guide-to-flexbox/).

## Not implemented yet
* align-content
* align-items
* align-self
* order
* transforming between relative and absolute units

## Sample code
```cs
// Side menu with a body that fills the rest of the space:
var menu = new Flexbox {
    Width = 500,
    RelativeSizeAxes = Axes.Y,
    Children = new FlexboxItem[] {
        new() {
            Basis = 50.Pixels(),
            Drawable = new Box() {
                Colour = Colour4.Gray
            }
        },
        new() {
            Grow = 1,
            Drawable = new Box() {
                Colour = Color4.DarkGray
            }
        }
    }
};

// A flexbox child animating its flex properties:
this.TransformBindableTo( this.GetFlexboxProperties().BasisBindable, 100.Pixels(), 400, Easing.Out );
```
