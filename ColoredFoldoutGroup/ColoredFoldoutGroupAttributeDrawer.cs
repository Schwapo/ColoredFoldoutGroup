#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

public class ColoredFoldoutGroupAttributeDrawer : OdinGroupDrawer<ColoredFoldoutGroupAttribute>
{
    private const string Base64BoxImage = "iVBORw0KGgoAAAANSUhEUgAAAAsAAAAICAYAAAAvOAWIAAAACXBIWXMAAAsTAAALEwEAmpwYAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAACbSURBVHgBlZAxDgIhFET5W9nBBTiLVsSa0OsN1BOAJ5Aj7AmI5RYUngUuAJ2l85OtXN1kJyFMJi8D/5OAvPeBiC6wSizVcMYQwo0ARq313jk3SSnf32StVeWcD6WU54DGkzHm9QtkoahZayfY68BPcyBWNBepQWwQw633vluD+N/MMRxTSsc5WIiLeEDYkTjAWh64zuL/6iKY+weeqTYV/ohdfQAAAABJRU5ErkJggg==";

    private static GUIStyle boxStyle;
    private static GUIStyle BoxStyle
    {
        get
        {
            if (boxStyle == null)
            {
                boxStyle = new GUIStyle(EditorStyles.helpBox);
                boxStyle.margin = new RectOffset(0, 0, 0, 2);
                boxStyle.normal.background = ColoredFoldoutGroupAttribute.GenerateTexture(11, 8, Base64BoxImage);
            }

            return boxStyle;
        }
    }
    
    private ValueResolver<string> titleGetter;
    private ValueResolver<Color> headerColorGetter;
    private ValueResolver<Color> labelColorGetter;
    private ValueResolver<Color> boxColorGetter;

    private Color headerColor;
    private Color labelColor;
    private Color boxColor;

    protected override void Initialize()
    {
        titleGetter = ValueResolver.GetForString(Property, Attribute.GroupName);
        headerColorGetter = ValueResolver.Get<Color>(Property, Attribute.HeaderColor);
        labelColorGetter = ValueResolver.Get<Color>(Property, Attribute.LabelColor);
        boxColorGetter = ValueResolver.Get<Color>(Property, Attribute.BoxColor);

        if (Attribute.HasDefinedExpanded)
            Property.State.Expanded = Attribute.Expanded;
    }

    protected override void DrawPropertyLayout(GUIContent label)
    {
        GUILayout.Space(Attribute.MarginTop);

        HandleValueResolverErrors();

        var labelStyle = new GUIStyle("foldout")
        {
            fontStyle = Attribute.BoldLabel ? FontStyle.Bold : FontStyle.Normal,
            normal = { textColor = labelColor },
        };

        label = GUIHelper.TempContent(titleGetter.HasError ? Property.Label.text : titleGetter.GetValue());

        GUIHelper.PushColor(boxColor);
        BeginBox();
        GUIHelper.PopColor();

        BeginColoredBoxHeader(Attribute, Property);
        GUIHelper.PushColor(labelColor);
        Attribute.Expanded = SirenixEditorGUI.Foldout(Attribute.Expanded, label, labelStyle);
        GUIHelper.PopColor();
        SirenixEditorGUI.EndBoxHeader();

        if (SirenixEditorGUI.BeginFadeGroup(this, Property.State.Expanded))
        {
            GUILayout.Space(1f);
            foreach (var property in Property.Children)
                property.Draw();
        }
        SirenixEditorGUI.EndFadeGroup();
        EndBox();

        GUILayout.Space(Attribute.MarginBottom);
    }

    private void HandleValueResolverErrors()
    {
        if (titleGetter.HasError)
            SirenixEditorGUI.ErrorMessageBox(titleGetter.ErrorMessage);

        if (headerColorGetter.HasError)
        {
            if (ColorUtility.TryParseHtmlString(Attribute.HeaderColor, out Color color))
                headerColor = color;
            else
                headerColor = EditorGUIUtility.isProSkin ? HexColor("#4b4b4b") : HexColor("#dbdbdb");
        }
        else
        {
            headerColor = headerColorGetter.GetValue();
        }

        if (labelColorGetter.HasError)
        {
            if (ColorUtility.TryParseHtmlString(Attribute.LabelColor, out Color color))
                labelColor = color;
            else
                labelColor = EditorGUIUtility.isProSkin ? HexColor("#D2D2D2") : HexColor("020202");
        }
        else
        {
            labelColor = labelColorGetter.GetValue();
        }

        if (boxColorGetter.HasError)
        {
            if (ColorUtility.TryParseHtmlString(Attribute.BoxColor, out Color color))
                boxColor = color;
            else
                boxColor = EditorGUIUtility.isProSkin ? HexColor("#404040") : HexColor("#CFCFCF");
        }
        else
        {
            boxColor = boxColorGetter.GetValue();
        }
    }

    private void BeginColoredBoxHeader(ColoredFoldoutGroupAttribute attribute, InspectorProperty property)
    {
        GUILayout.Space(-3f);

        var rect = EditorGUILayout.BeginHorizontal(SirenixGUIStyles.BoxHeaderStyle);

        var e = Event.current;
        if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
            property.State.Expanded = !property.State.Expanded;

        if (Event.current.type == EventType.Repaint)
        {
            rect = new Rect(rect.x - 3f, rect.y + 1f, rect.width + 6f, rect.height - 1f);

            var texture = attribute.HasDefinedGradient
                ? ColoredFoldoutGroupAttribute.Gradients[attribute.Gradient]
                : Texture2D.whiteTexture;

            GUI.DrawTexture(rect, texture, ScaleMode.StretchToFill, true, 0f, headerColor, 0f, 2f);
            rect.y += 1f;
            SirenixEditorGUI.DrawBorders(rect, 0, 0, 0, 1, new Color(0f, 0f, 0f, 0.4f));
        }

        GUIHelper.PushLabelWidth(GUIHelper.BetterLabelWidth - 4f);
    }

    public static void BeginBox()
    {
        BeginIndentedVertical(BoxStyle);
        float labelWidth = GUIHelper.BetterLabelWidth - 4f;
        GUIHelper.PushHierarchyMode(hierarchyMode: false);
        GUIHelper.PushLabelWidth(labelWidth);
    }

    public static void EndBox()
    {
        GUIHelper.PopLabelWidth();
        GUIHelper.PopHierarchyMode();
        EndIndentedVertical();
    }

    public static void BeginIndentedVertical(GUIStyle style)
    {
        GUILayout.BeginHorizontal(GUIStyle.none);
        if (EditorGUI.indentLevel != 0)
        {
            float num = GUIHelper.BetterLabelWidth - GUIHelper.CurrentIndentAmount;
            float pixels = 0f;
            if (num < 1f)
            {
                num = 1f;
                pixels = 1f - num;
            }
            GUILayout.Space(pixels);
            GUIHelper.PushLabelWidth(num);
            GUILayout.Space(GUIHelper.CurrentIndentAmount);
        }
        GUIHelper.PushIndentLevel(0);
        EditorGUILayout.BeginVertical(style);
    }

    public static void EndIndentedVertical()
    {
        EditorGUILayout.EndVertical();
        GUIHelper.PopIndentLevel();
        GUILayout.EndHorizontal();
        if (EditorGUI.indentLevel != 0)
        {
            GUIHelper.PopLabelWidth();
        }
    }

    private static Color HexColor(string hexColor)
    {
        hexColor = hexColor.StartsWith("#") ? hexColor : $"#{hexColor}";
        ColorUtility.TryParseHtmlString(hexColor, out Color color);
        return color;
    }
}
#endif