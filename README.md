# ColoredFoldoutGroup

Lets you group class members inside a foldout group with customizable colors, gradients, margin and more.

### Requires [Odin Inspector]

### Examples
Examples can be found in the downloaded files.

![](example.gif)

### Usage
Simply put the downloaded ColoredFoldoutGroup folder in your project
and start using the attribute as in the example file.
You can move the files, but make sure that `ColoredFoldoutGroupAttribute.cs`
is not in an editor folder or it will be removed during build, causing errors.

Colors can be provided via [ValueResolvers] or by using hex colors like `#FF0000`.
If it can't resolve your value it will first try to parse it into a hex color and
if this also fails, default colors will be used.

[Odin Inspector]: https://odininspector.com/
[ValueResolvers]: https://odininspector.com/documentation/sirenix.odininspector.editor.valueresolvers.valueresolver-1
