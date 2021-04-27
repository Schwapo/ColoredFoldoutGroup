using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[Conditional("UNITY_EDITOR")]
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
public class ColoredFoldoutGroupAttribute : PropertyGroupAttribute
{
    private const string Base64LTR = "iVBORw0KGgoAAAANSUhEUgAAACAAAAABCAYAAAC/iqxnAAAACXBIWXMAAAsTAAALEwEAmpwYAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAA8SURBVHgBXYzbCgAwCELr//+4m1NYMPYgZtnx7p7MdDqqyiljNmbTLO3t7qH+zCAi9Pf2fwaWLX9Ytr0DV4Vnh59YSmgAAAAASUVORK5CYII=";
    private const string Base64RTL = "iVBORw0KGgoAAAANSUhEUgAAACAAAAABCAYAAAC/iqxnAAAACXBIWXMAAAsTAAALEwEAmpwYAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAA9SURBVHgBbYxJDgAgCAMhLP/m42pr0HjwMKE0A1pVIyIUTDNTdxdMwS7MzQTa/fYy8+ffjhz3/c3cN+xlAXIYC/sLm0njAAAAAElFTkSuQmCC";
    private const string Base64CTR = "iVBORw0KGgoAAAANSUhEUgAAACAAAAABCAYAAAC/iqxnAAAACXBIWXMAAAsTAAALEwEAmpwYAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAABISURBVHgBbYqxEcAwCANNYwNzszPQREG+lCm4Fy9JRDyqKmaGc45MXj8Hbpi523uLu18CWFW1uptEZgrzkA7jhP7jdez4c/cCo4w6jbJC8QwAAAAASUVORK5CYII=";

    private static Texture2D gradientLTR;
    private static Texture2D gradientRTL;
    private static Texture2D gradientCTR;

    public static Dictionary<string, Texture2D> Gradients = new Dictionary<string, Texture2D>
    {
        { "LeftToRight", gradientLTR ??= GenerateTexture(32, 1, Base64LTR) },
        { "RightToLeft", gradientRTL ??= GenerateTexture(32, 1, Base64RTL) },
        { "Centered" ,   gradientCTR ??= GenerateTexture(32, 1, Base64CTR) },
    };

    public bool HasDefinedGradient { get; private set; }
    public bool HasDefinedExpanded { get; private set; }
    public bool BoldLabel { get; set; }
    public int MarginTop { get; set; }
    public int MarginBottom { get; set; }

    private string gradient;
    public string Gradient
    {
        get => gradient;
        set
        {
            gradient = value;
            HasDefinedGradient = true;
        }
    }

    private bool expanded;
    public bool Expanded
    {
        get => expanded;
        set
        {
            expanded = value;
            HasDefinedExpanded = true;
        }
    }

    public string HeaderColor;
    public string LabelColor;
    public string BoxColor;

    public ColoredFoldoutGroupAttribute(string groupName, float order = 0f) : base(groupName, order)
    {
    }

    public static Texture2D GenerateTexture(int width, int height, string base64)
    {
        var texture = new Texture2D(32, 1, TextureFormat.RGBA32, false) { wrapMode = TextureWrapMode.Clamp };
        texture.LoadImage(Convert.FromBase64String(base64));
        return texture;
    }
}