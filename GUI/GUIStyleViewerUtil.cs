using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Unity 自带的 GUIStyle 效果查看器
/// </summary>
public class GUIStyleViewerUtil : EditorWindow
{
    /// <summary>
    /// 滑动条
    /// </summary>
    private Vector2 mScrollView = Vector2.zero;
    /// <summary>
    /// 寻找的样式名称
    /// </summary>
    private string mSearchStyle = "";
    /// <summary>
    /// 样式缓存
    /// </summary>
    private static GUIStyle mBtnStyle;

    [MenuItem("Tools/GUIStyle 效果查看器")]
    private static void MenuClicked()
    {
        GetWindow<GUIStyleViewerUtil>("GUIStyle查看器", true);
        // 初始化一个按钮的样式
        mBtnStyle = GetBtnStyle();
    }

    private void OnGUI()
    {
        GUILayout.Space(20);

        // 设置一个查找的功能
        GUILayout.BeginHorizontal("HelpBox");
        GUILayout.Space(20);
        mSearchStyle = EditorGUILayout.TextField("", mSearchStyle, "SearchTextField");
        GUILayout.Label("", "SearchCancelButtonEmpty");
        GUILayout.EndHorizontal();

        // 设置滚动栏
        mScrollView = GUILayout.BeginScrollView(mScrollView);
        // 遍历并显示所有符合条件的 Unity 自带的 GUIStyle
        foreach (GUIStyle style in GUI.skin.customStyles)
        {
            if (style.name.ToLower().Contains(mSearchStyle.ToLower()))
            {
                GUILayout.Space(10);
                // 为了让显示的布局看着更舒服一些，需要以当前 style 为原型新建一个 GUIStyle
                // 因为接下来可能会对文字居中等方面进行调整，
                // 而如果直接更改原来的 style，则会导致系统中的对应 UI 样式和布局也发生改变
                GUIStyle tmp = new GUIStyle(style);
                DrawStyleItem(tmp);
            }
        }
        GUILayout.EndScrollView();
    }

    /// <summary>
    /// 绘制对应的 GUIStyle
    /// </summary>
    /// <param name="style"></param>
    private void DrawStyleItem(GUIStyle style)
    {

        GUILayout.BeginHorizontal("box", GUILayout.Height(style.fixedHeight));
        GUILayout.Space(20);
        // 获取按钮样式
        GUIStyle tmp = mBtnStyle;

        // 设置第一个按钮，点击后可以复制当前 GUIStyle 的名字信息
        if (GUILayout.Button("<color=#000000FF>复制到剪贴板</color>", tmp))
        {
            GUIUtility.systemCopyBuffer=style.name;
        }

        GUILayout.Space(20);

        // 设置第二个按钮，点击后在控制台输出该 GUIStyle 的设置参数
        if (GUILayout.Button("<color=#FFFFFFFF>查看Style参数</color>", tmp))
        {
            ShowGUIStyleMsg(style.name);
        }
        GUILayout.Space(30);

        // 显示当前 GUIStyle 的名字
        EditorGUILayout.SelectableLabel(style.name);

        // 设置左右对称布局，以此为分界线，把前后的布局对称
        GUILayout.FlexibleSpace();

        // 设置文本居中显示
        style.alignment = TextAnchor.MiddleCenter;
        // 显示当前的样式及其名称
        EditorGUILayout.SelectableLabel(style.name, style);

        GUILayout.Space(100);

        // 单独显示样式
        EditorGUILayout.SelectableLabel("", style,
                                        GUILayout.Height(style.fixedHeight),
                                         GUILayout.Width(style.fixedWidth));
        GUILayout.Space(100);

        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// 创建一个按钮样式，用来作为本界面中的按钮基础
    /// </summary>
    /// <returns></returns>
    public static GUIStyle GetBtnStyle()
    {
        // 使用系统自带的 "AC Button" 样式作为原型，因为 "AC Button" 本身就具有点击效果了
        GUIStyle style = new GUIStyle("AC Button");

        // 将样式的点击时的效果图，替换为系统自带的 "flow node 4 on" 样式
        GUIStyle active = new GUIStyle("flow node 4 on");
        style.active.background = active.normal.background;

        // 将平时按钮的效果图，替换为系统自带的 "flow node 4" 样式
        GUIStyle normal = new GUIStyle("flow node 4");
        style.normal.background = normal.normal.background;

        // 参考 "flow node 4" 的内部参数，设置一些图片的参数。
        // 如果不设置的话，则图片会按照原 "AC Button" 的样式进行显示
        // 因为图片不一样，所以会有显示上的问题。
        style.stretchHeight = true;
        style.stretchWidth = true;
        style.border = new RectOffset(11, 11, 11, 15);
        style.margin = new RectOffset(0, 0, 0, 0);
        style.padding = new RectOffset(0, 0, 0, 0);
        style.overflow = new RectOffset(7, 7, 6, 9);

        // 进行文本方面的设置
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 30;
        style.richText = true;

        return style;
    }

    /// <summary>
    /// 展示指定的 GUI 样式的一些数值
    /// </summary>
    /// <param name="style"></param>
    public void ShowGUIStyleMsg(string style)
    {
        // 清空控制台信息，防止混淆
        ClearConsole();

        GUIStyle tmp = new GUIStyle(style);
        Debug.Log("Style Name: " + tmp.name);
        Debug.Log("Style Normal: " + tmp.normal.background);
        Debug.Log("Style Normal Color: " + tmp.normal.textColor);
        Debug.Log("Style Active: " + tmp.active.background);
        Debug.Log("Style Active Color: " + tmp.active.textColor);
        Debug.Log("Style Hover: " + tmp.hover.background);
        Debug.Log("Style Hover Color: " + tmp.hover.textColor);
        Debug.Log("Style Focused: " + tmp.focused.background);
        Debug.Log("Style Focused Color: " + tmp.focused.textColor);
        Debug.Log("Style border: " + tmp.border);
        Debug.Log("Style margin: " + tmp.margin);
        Debug.Log("Style padding: " + tmp.padding);
        Debug.Log("Style overflow: " + tmp.overflow);
        Debug.Log("Style fixedWidth: " + tmp.fixedWidth);
        Debug.Log("Style fixedHeight: " + tmp.fixedHeight);
        Debug.Log("Style stretchWidth: " + tmp.stretchWidth);
        Debug.Log("Style stretchHeight: " + tmp.stretchHeight);
        Debug.Log("Style lineHeight: " + tmp.lineHeight);
        Debug.Log("Style Font: " + tmp.font);
        Debug.Log("Style Font Size: " + tmp.fontSize);
    }

    static MethodInfo clearMethod = null;
    private void ClearConsole()
    {
        if (clearMethod==null)
        {
            Type log = typeof(EditorWindow).Assembly.GetType("UnityEditor.LogEntries");
            clearMethod = log.GetMethod("Clear");
        }
        clearMethod.Invoke(null, null);
    }
}
