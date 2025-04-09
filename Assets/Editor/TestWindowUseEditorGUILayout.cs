using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

public class TestWindowUseEditorGUILayout : EditorWindow
{
    [MenuItem("CustomWindow/ShowTestWinLayout")]
    public static void ShowWindowLayout()
    {
        //显示窗口实例，可以使用鼠标更改大小
        EditorWindow.GetWindow(typeof(TestWindowUseEditorGUILayout));
        //显示窗口按照自定位置和大小，比如(0,0)点，宽600，长800,不可以使用鼠标更改大小
        //EditorWindow.GetWindowWithRect<TestWindowUseEditorGUILayout>(new Rect(new Vector2(0,0),new Vector2(600,800)));
    }

    private void OnEnable()
    { 
    }

    private void Update()
    {
        
    }

    private void OnGUI()
    {
        TestBuildTargetSelectionGrouping();
        TestFadeGroup();
        TestFoldoutHeaderGroup();
        TestHorizontal();
        TestVertical();
        TestScrollView();
        TestToggleGroup();
        TestDropdownButton();
        TestEditorToolbar();
        TestPopup();
        TestFoldout();
        TestGetControlRect();
        TestHelpBox();
        TestInspectorTitlebar();
        TestSlider();
        TestSpace();
    }

    public void TestBuildTargetSelectionGrouping()
    {
        BuildTargetGroup selectedBuildTargetGroup = EditorGUILayout.BeginBuildTargetSelectionGrouping();
        if (selectedBuildTargetGroup == BuildTargetGroup.Android)
        {
            EditorGUILayout.LabelField("Android specific things");
        }

        if (selectedBuildTargetGroup == BuildTargetGroup.Standalone)
        {
            EditorGUILayout.LabelField("Standalone specific things");
        }

        EditorGUILayout.EndBuildTargetSelectionGrouping();
    }
    
    private bool isExpanded = false;
    private float currentFade = 0f;
    void TestFadeGroup()
    {
        // 切换折叠和更新目标淡出
        isExpanded = EditorGUILayout.Foldout(isExpanded, "Additional Settings");
        float targetFade = isExpanded ? 1f : 0f;

        // 流畅地动画currentFade
        currentFade = Mathf.Lerp(currentFade, targetFade, 0.01f);

        // 开始淡出组，并绘制内容，如果可见
        if (EditorGUILayout.BeginFadeGroup(currentFade))
        {
            EditorGUILayout.LabelField("Extra Field 1");
            EditorGUILayout.LabelField("Extra Field 2");
        }
        EditorGUILayout.EndFadeGroup();

        // 如果仍然Fade，重新绘制
        if (Mathf.Abs(currentFade - targetFade) > 0.001f)
            Repaint();
    }

    bool showPosition = true;
    string status = "Select a GameObject";
    void TestFoldoutHeaderGroup() 
    {
        showPosition = EditorGUILayout.BeginFoldoutHeaderGroup(showPosition, status, null, ShowHeaderContextMenu);

        if (showPosition)
            if (Selection.activeTransform)
            {
                Selection.activeTransform.position =
                    EditorGUILayout.Vector3Field("Position", Selection.activeTransform.position);
                status = Selection.activeTransform.name;
            }

        if (!Selection.activeTransform)
        {
            status = "Select a GameObject";
            showPosition = false;
        }
        // End the Foldout Header that we began above.
        EditorGUILayout.EndFoldoutHeaderGroup();
    }
    void ShowHeaderContextMenu(Rect position)
    {
        var menu = new GenericMenu();
        menu.AddItem(new GUIContent("Move to (0,0,0)"), false, OnItemClicked);
        menu.DropDown(position);
    }

    void OnItemClicked()
    {
        Undo.RecordObject(Selection.activeTransform, "Moving to center of world");
        Selection.activeTransform.position = Vector3.zero;
    }

    void TestHorizontal()
    {
        Rect r = EditorGUILayout.BeginHorizontal("Button",GUILayout.Width(100),GUILayout.Height(100));
        if (GUI.Button(r, GUIContent.none))
            Debug.Log("Go here");
        GUILayout.Label("I'm inside the button");
        GUILayout.Label("So am I");
        EditorGUILayout.EndHorizontal();
    }

    void TestVertical()
    {
        Rect r = (Rect)EditorGUILayout.BeginVertical("Button");
        if (GUI.Button(r, GUIContent.none))
            Debug.Log("Go here");
        GUILayout.Label("I'm inside the button");
        GUILayout.Label("So am I");
        EditorGUILayout.EndVertical();
    }

    Vector2 scrollPos;
    string t = "This is a string inside a Scroll view!";
    void TestScrollView()
    {
        scrollPos =
    EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(100), GUILayout.Height(50));
        GUILayout.Label(t);
        EditorGUILayout.EndScrollView();
    }

    bool[] pos = new bool[3] { true, true, true };
    bool posGroupEnabled = true;
    void TestToggleGroup()
    {
        posGroupEnabled = EditorGUILayout.BeginToggleGroup("Align position", posGroupEnabled);
        pos[0] = EditorGUILayout.Toggle("x", pos[0]);
        pos[1] = EditorGUILayout.Toggle("y", pos[1]);
        pos[2] = EditorGUILayout.Toggle("z", pos[2]);
        EditorGUILayout.EndToggleGroup();
    }

    void TestDropdownButton()
    {
        // 如果按钮被点击，展开下拉内容
        if (EditorGUILayout.DropdownButton(
            new GUIContent("Options"), // 按钮显示的文本
            FocusType.Passive,          // 焦点类型（通常用 Passive）
            EditorStyles.toolbarButton // 按钮样式（可选）
        ))
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Reset Values"), false, () => {
                // 执行操作
                Debug.Log("Reset values");
            });
            menu.AddItem(new GUIContent("Export Data"), false, () => {
                Debug.Log("Export data");
            });
            menu.ShowAsContext(); // 显示菜单
        }
    }

    CustomEditorTool a = null;
    CustomEditorTool b = null;
    CustomEditorTool c = null;

    void TestEditorToolbar()
    {
        if(a == null)
        {
            a = new CustomEditorTool();
            b = new CustomEditorTool();
            c = new CustomEditorTool();
        }
        EditorGUILayout.EditorToolbar(a,b,c);
    }

    public enum TestPopupEnum { 
         None,
         First,
         Second,
    }

    TestPopupEnum selectEnum = TestPopupEnum.None;
    int selectInt = 0;
    int popupSelectInt = 0;
    void TestPopup()
    {
        selectEnum = (TestPopupEnum)EditorGUILayout.EnumPopup("EnumPopup Select:", selectEnum);
        selectInt = (int)EditorGUILayout.IntPopup("IntPopup Select",selectInt, new string[3] { "1", "2", "3" },new int[3] {1,2,3});
        popupSelectInt = EditorGUILayout.Popup("Popup :",popupSelectInt, new string[3] { "One", "Two", "Three" });
    }


    bool showFoldout = true;
    void TestFoldout()
    {
        showFoldout = EditorGUILayout.Foldout(showFoldout, status);
        if (showFoldout)
            if (Selection.activeTransform)
            {
                Selection.activeTransform.position =
                    EditorGUILayout.Vector3Field("Position", Selection.activeTransform.position);
                status = Selection.activeTransform.name;
            }

        if (!Selection.activeTransform)
        {
            status = "Select a GameObject";
            showFoldout = false;
        }
    }

    void TestGetControlRect()
    {
        Rect r = EditorGUILayout.GetControlRect(true,20);
        EditorGUILayout.Vector4Field("位置：",new Vector4(r.x,r.y,r.width,r.height));
    }

    void TestHelpBox()
    {
        EditorGUILayout.HelpBox("xxxxx 这是一个帮助Box", MessageType.Warning);
    }

    bool fold = true;
    Transform selectedTransform;
    void TestInspectorTitlebar()
    {
        if (Selection.activeGameObject)
        {
            selectedTransform = Selection.activeGameObject.transform;

            fold = EditorGUILayout.InspectorTitlebar(fold, selectedTransform);
            if (fold)
            {
                selectedTransform.localScale =
                    EditorGUILayout.Vector3Field("Scale", selectedTransform.localScale);
            }
        }
    }


    int sliderInt = 0;
    float sliderFloat = .0f;
    float slidermin = .0f;
    float slidermax = 100.0f;
    void TestSlider()
    {
        sliderInt = EditorGUILayout.IntSlider(sliderInt,-10,10);
        sliderFloat = EditorGUILayout.Slider(sliderFloat,-10.0f,10f);
        EditorGUILayout.MinMaxSlider(ref slidermin,ref slidermax,-10f,10f);
    }

    void TestSpace()
    {
        EditorGUILayout.LabelField("xxxxx");
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("xxxxx");
    }
}


// 自定义 EditorTool 类
public class CustomEditorTool : EditorTool
{
    // 工具栏图标
    [SerializeField] private Texture2D toolIcon;

    public override GUIContent toolbarIcon
    {
        get
        {
            return new GUIContent
            {
                image = toolIcon,
                text = "Custom Tool",
                tooltip = "This is a custom editor tool."
            };
        }
    }

    // 激活工具时调用
    public override void OnActivated()
    {
        Debug.Log("Custom tool activated.");
    }

    // 工具处于活动状态时，在场景视图中进行绘制和处理交互
    public override void OnToolGUI(EditorWindow window)
    {
        // 获取当前选中的游戏对象
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject != null)
        {
            // 在此处添加自定义的交互逻辑，例如绘制手柄、处理鼠标事件等
            Handles.color = Color.red;
            Handles.DrawWireCube(selectedObject.transform.position, Vector3.one);
        }

        base.OnToolGUI(window);
    }
}
