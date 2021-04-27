using Sirenix.OdinInspector;
using UnityEngine;

public class ColoredFoldoutGroupExample : MonoBehaviour
{
    // ----- Repaint constantly for color rainbow effect (not necessary for normal use) -----
    [OnInspectorGUI(nameof(Repaint))]
    private static void Repaint() => Sirenix.Utilities.Editor.GUIHelper.RequestRepaint();
    private static Color RainbowColor => Color.HSVToRGB((Mathf.Sin(Time.realtimeSinceStartup / 5.0f) + 1.0f) / 2.0f, 0.7f, 0.7f);
    // --------------------------------------------------------------------------------------


    // Colors can be provided via ValueResolvers or by using hex colors like "#FF0000".
    // If it can't resolve your value it will first try to parse it into a hex color and
    // if this also fails, default colors will be used.


    [ColoredFoldoutGroup("Solid Color", HeaderColor = "RainbowColor")]
    public string value01;

    [ColoredFoldoutGroup("Gradient - LeftToRight", HeaderColor = "RainbowColor", Gradient = "LeftToRight")]
    public string value02;

    [ColoredFoldoutGroup("Gradient - RightToLeft", HeaderColor = "RainbowColor", Gradient = "RightToLeft")]
    public string value03;

    [ColoredFoldoutGroup("Gradient - Centered", HeaderColor = "RainbowColor", Gradient = "Centered")]
    public string value04;

    [ColoredFoldoutGroup("MarginTop & MarginBottom", MarginTop = 25, MarginBottom = 25)]
    public string value05;

    [ColoredFoldoutGroup("BoldLabel", BoldLabel = true)]
    public string value06;

    [ColoredFoldoutGroup("LabelColor", LabelColor = "RainbowColor", BoldLabel = true)]
    public string value07;

    [ColoredFoldoutGroup("Expanded", Expanded = true, MarginTop = 25)]
    public string value08;

    [ColoredFoldoutGroup("BoxColor", HeaderColor = "RainbowColor", BoxColor = "RainbowColor", MarginTop = 25, Expanded = true)]
    public string value09;
}