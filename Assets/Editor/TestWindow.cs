using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TestWindow : EditorWindow
{
    [MenuItem("CustomWindow/ShowTestWin")]
    public static void ShowWindow()
    {
        //显示窗口实例
        EditorWindow.GetWindow(typeof(TestWindow));
        //显示窗口按照自定位置和大小，比如(0,0)点，宽600，长800
        EditorWindow.GetWindowWithRect<TestWindow>(new Rect(new Vector2(0,0),new Vector2(600,800)));
    }

    SerializedProperty isFastProp;
    SerializedObject serializedObject;

    #region 部分生命周期方法
    private void OnEnable()
    {
        //在加载脚本或者启用对象时调用
        //Debug.Log("OnEnable");
        ISFastProp o = ScriptableObject.CreateInstance<ISFastProp>();
        serializedObject = new UnityEditor.SerializedObject(o);
        isFastProp = serializedObject.FindProperty("isFast");
    }

    private void CreateGUI()
    {
        //如果Editor未更新，则生成图形用户界面
        //Debug.Log("CreateGUI");
    }

    private void Update()
    {
        //每帧调用一次以更新脚本的逻辑
        //Debug.Log("Updaete");
    }

    private void OnDisable()
    {
        //当脚本被禁用或者对象被销毁以完成和清理资源时调用
        //Debug.Log("OnDisable");
    }
    #endregion

    

    private void OnGUI()
    {
        //每帧多次调用，用于渲染和处理GUI事件
        //渲染窗口的实际内容
        //Debug.Log("OnGUI");
        if (EditorGUI.actionKey)
        {
            //是否按住了平台相关的“action”修改键?(只读),该键在 macOS 上为 Command，在 Windows 上为 Control
            Debug.Log("按下了actionKey");
        }

        //测试indentLevel
        TestIndentLevel();
        //测试showMixedValue
        ShowValueTest2();
        //测试BeginChangeCheck，EndChangeCheck
        ChangeCheck();
        //测试BeginDisabledGroup，EndDisabledGroup
        TestDisabledGroup();
        //测试BeginFoldoutHeaderGroup，EndFoldoutHeaderGroup
        TestFoldoutHeaderGroup();
        //测试BeginProperty，EndProperty
        TestProperty();
    }

    #region Property
    public void TestProperty()
    {
        serializedObject.Update();

        EditorGUI.BeginProperty(new Rect(0, 230, 20, 20), null, isFastProp);
        GUI.backgroundColor = Color.white;
        EditorGUI.PropertyField(new Rect(0, 230, 100, 20), isFastProp, null);
        EditorGUI.PropertyField(new Rect(0, 250, 100, 20), isFastProp, null);
        EditorGUI.PropertyField(new Rect(0, 270, 100, 20), isFastProp, null);
        /*EditorGUI.BeginChangeCheck();
        bool newV = isFastProp.boolValue;
        if (GUI.Button(new Rect(0, 250, 100, 50), "TestProperty"))
        {
            newV = !newV;
        }
        //只有当用户改变了这个值，才把它赋值回去。
        //否则，在多对象编辑时，将为所有对象分配单个值；
        //即使用户没有触摸控件。
        if (EditorGUI.EndChangeCheck())
        {
            isFastProp.boolValue = newV;
        }*/
        EditorGUI.EndProperty();
        serializedObject.ApplyModifiedProperties();
    }
    #endregion

    #region FoldoutHeaderGroup
    bool isShow = false;

    public void TestFoldoutHeaderGroup()
    {
        isShow = EditorGUI.BeginFoldoutHeaderGroup(new Rect(new Vector2(0, 200), new Vector2(120, 50)), isShow, "TEST Folder", null, ShowHeaderContextMenu);
        if (isShow)
        {
            if (Selection.activeTransform)
            {
                Selection.activeTransform.position = EditorGUI.Vector3Field(new Rect(0, 220, 200, 100), "Position", Selection.activeTransform.position);
            }
            else
            {
                EditorGUI.LabelField(new Rect(0, 220, 200, 20),"请先选中一个物体！");
            }
        }

        EditorGUI.EndFoldoutHeaderGroup();
    }

    Color mColor;

    void ShowHeaderContextMenu(Rect position)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("RGB/Red"), mColor.Equals(Color.red), (color) => { mColor = (Color)color; }, Color.red);
        menu.AddItem(new GUIContent("RGB/Black"), mColor.Equals(Color.black), (color) => { mColor = (Color)color; }, Color.black);
        menu.AddItem(new GUIContent("RGB/White"), mColor.Equals(Color.white), (color) => { mColor = (Color)color; }, Color.white);
        menu.ShowAsContext();
    }
    #endregion

    #region DisabledGroup
    public void TestDisabledGroup()
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.TextField(new Rect(new Vector2(0,150),new Vector2(300,50)), "TestDisabledGroup");
        EditorGUI.DropdownButton(new Rect(new Vector2(300, 150), new Vector2(100, 50)),this.titleContent,FocusType.Keyboard);
        EditorGUI.EndDisabledGroup();
    }
    #endregion

    #region ChangeCheck
    float value = 0;

    public void ChangeCheck()
    {
        EditorGUI.BeginChangeCheck();
        value = EditorGUILayout.Slider(value, 0, 1);
        if (EditorGUI.EndChangeCheck())
        {
            this.ShowTips("变量检查成功！");
        }
    }
    #endregion
    public void ShowTips(string content)
    {
        EditorUtility.DisplayDialog("提示",content,"Ok");
    }

    #region IndentLevel
    public void TestIndentLevel()
    {
        //使用indentLevel缩进文本
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField("P1:");
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField("P2:");
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField("P3:");
        EditorGUI.indentLevel--;
        EditorGUI.indentLevel--;
        EditorGUILayout.LabelField("P1:");
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField("P2:");
    }
    #endregion

    #region Test showMixedValue
    public void ShowValueTest2() {
        EditorGUI.showMixedValue = true;
        // 将isFast布尔值转换为enum值
        SpeedOption speedOptionEnumValue = SpeedOption.Fast;
        // 在下拉菜单中显示枚举值：
        speedOptionEnumValue = (SpeedOption)EditorGUILayout.EnumPopup("Speed", speedOptionEnumValue);
        // 将showMixedValue设置为false，这样它就不会影响以下控件（如果有的话）：
        EditorGUI.showMixedValue = false;
    }

    public void ShowValueTest()
    {
        serializedObject.Update();
        // 以标准方式显示isFast布尔值-作为切换：
        EditorGUILayout.PropertyField(isFastProp);
        // 使用上面定义的SpeedOption枚举将isFast布尔值显示为下拉列表：
        // 检查该值是否在该块内被更改
        EditorGUI.BeginChangeCheck();
        // 如果isFast属性表示多个不同的值，下拉框应该显示：
        EditorGUI.showMixedValue = isFastProp.hasMultipleDifferentValues;
        // 将isFast布尔值转换为enum值
        SpeedOption speedOptionEnumValue;
        if (isFastProp.boolValue == true)
            speedOptionEnumValue = SpeedOption.Fast;
        else
            speedOptionEnumValue = SpeedOption.Slow;
        // 在下拉菜单中显示枚举值：
        speedOptionEnumValue = (SpeedOption)EditorGUILayout.EnumPopup("Speed", speedOptionEnumValue);
        // 将showMixedValue设置为false，这样它就不会影响以下控件（如果有的话）：
        EditorGUI.showMixedValue = false;
        // 如果在这个块中改变了值，将新值应用于所有对象：
        if (EditorGUI.EndChangeCheck())
            isFastProp.boolValue = (speedOptionEnumValue == SpeedOption.Fast ? true : false);
        serializedObject.ApplyModifiedProperties();
    }
}

public class ISFastProp : ScriptableObject
{
    public bool isFast = false;
}

public enum SpeedOption
{
    None,
    Fast,
    Slow
}

    #endregion
