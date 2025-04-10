using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class TestWindowUseEditorGUI : EditorWindow
{
    [MenuItem("CustomWindow/ShowTestWin")]
    public static void ShowWindow()
    {
        //显示窗口实例，可以使用鼠标更改大小
        EditorWindow.GetWindow(typeof(TestWindowUseEditorGUI));
        //显示窗口按照自定位置和大小，比如(0,0)点，宽600，长800,不可以使用鼠标更改大小
        //EditorWindow.GetWindowWithRect<TestWindowUseEditorGUI>(new Rect(new Vector2(0,0),new Vector2(600,800)));
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

        MonsterData m = ScriptableObject.CreateInstance<MonsterData>();
        mMonsterData = new UnityEditor.SerializedObject(m);
        mAtt = mMonsterData.FindProperty("att1");
        mObj = mMonsterData.FindProperty("obj");

        AttData m1 = ScriptableObject.CreateInstance<AttData>();
        mAttData = new UnityEditor.SerializedObject(m1);
        mAtt1 = mAttData.FindProperty("a1");
        Debug.Log(mAtt1);

        texture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Art/bg_window.png");
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
        //测试Field
        TestField();
        //测试DrawPreviewTexture
        TestDrawPreviewTexture();
        //测试DrawRect
        TestDrawRect();
        //测试 DrawTextureAlpha
        TestDrawTextureAlpha();
        //测试 DrawTextureTransparent
        TestDrawTextureTransparent();
        //测试 DropdownButton
        TestDropdownButton();
        //测试 DropShadowLabel
        TestDropShadowLabel();
        //测试 EnumPopup,IntPopup,Popup
        TestPopup();
        //测试FocusTextInControl
        //TestFocusTextInControl();
        //测试Foldout
        TestFoldout();
        //测试GetPropertyHeight
        TestGetPropertyHeight();
        //测试HandlePrefixLabel
        TestHandlePrefixLabel();
        //测试HelpBox
        TestHelpBox();
        //测试InspectorTitlebar
        //TestInspectorTitlebar();
        //测试Slider
        TestSlider();
        //测试PrefixLabel
        TestPrefixLabel();
        //测试ProgressBar
        TestProgressBar();
        //测试SelectableLabel
        TestSelectableLabel();
        //测试TextArea
        TestTextArea();
        //测试Toggle
        TestToggle();
        //测试LinkButton
        TestLinkButton();
    }

    #region 测试Toggle
    bool toglevalue = false;
    bool toggleLeftValue = false;
    public void TestToggle()
    {
        toglevalue = EditorGUI.Toggle(new Rect(600, 290, 200, 20), "testToggle", toglevalue);
        toggleLeftValue = EditorGUI.ToggleLeft(new Rect(600, 320, 200, 20), "testToggleLeft", toggleLeftValue);
    }
    #endregion

    #region 测试TextArea
    string inputTxt = "请输入。。。";
    public void TestTextArea()
    {
        inputTxt = EditorGUI.TextArea(new Rect(600, 260, 200, 20), inputTxt);
    }
    #endregion

    #region 测试SelectableLabel
    public void TestSelectableLabel()
    {
        EditorGUI.SelectableLabel(new Rect(600, 230, 200, 20), "testSelectableLabel");
    }
    #endregion

    #region 测试ProgressBar

    float processValue = 0;

    public void TestProgressBar()
    {
        EditorGUI.ProgressBar(new Rect(600, 200, 200, 20),processValue, "testProgressBar");
    }
    #endregion

    #region 测试PrefixLabel
    public void TestPrefixLabel()
    {
       EditorGUI.PrefixLabel(new Rect(600, 180, 200, 20), new GUIContent("testPrefixLabel"));
    }
    #endregion

    #region 测试LinkButton
    public void TestLinkButton()
    {
        if(EditorGUI.LinkButton(new Rect(900, 200, 200, 20), "TestLinkButton"))
        {
            Application.OpenURL("Www.baidu.com");
        }
    }
    #endregion

    #region 测试Slider

    int valueInt = 0;
    float valueFloat = 50.8f;
    public void TestSlider()
    {
        valueInt = EditorGUI.IntSlider(new Rect(300, 410, 200, 20),valueInt,0,100);
        valueFloat = EditorGUI.Slider(new Rect(300, 440, 200, 20), valueFloat, 0, 100);
        //弃用的
        //EditorGUI.MinMaxSlider();
    }
    #endregion

    #region 测试InspectorTitlebar
    public void TestInspectorTitlebar()
    {
        EditorGUI.InspectorTitlebar(new Rect(300, 390, 200, 20),new UnityEngine.Object[] {});
    }
    #endregion

    #region 测试 HelpBox
    public void TestHelpBox()
    {
        EditorGUI.HelpBox(new Rect(300, 350, 200, 20),"测试HelpBox",MessageType.Warning);
    }
    #endregion

    #region HandlePrefixLabel
    public void TestHandlePrefixLabel()
    {
        EditorGUI.HandlePrefixLabel(new Rect(300, 520, 200, 20), new Rect(300, 322, 200, 20), new GUIContent("testHandlePrefixLabel"));
    }
    #endregion

    #region 测试GetPropertyHeight
    public void TestGetPropertyHeight()
    {
        //获取 PropertyField 控件所需的高度。
        float h = EditorGUI.GetPropertyHeight(mAtt, true);
        Debug.Log(h);
    }
    #endregion

    #region 测试Foldout
    bool _isShow = false;
    public void TestFoldout()
    {
        _isShow = EditorGUI.Foldout(new Rect(300, 290, 200, 20), _isShow, "testFoldout");
        if (_isShow)
        {
            if (Selection.activeTransform)
            {
                Selection.activeTransform.position = EditorGUI.Vector3Field(new Rect(300, 310, 200, 100), "Position", Selection.activeTransform.position);
            }
            else
            {
                EditorGUI.LabelField(new Rect(300, 310, 200, 20), "请先选中一个物体！");
            }
        }
    }
    #endregion

    #region ????测试FocusTextInControl
    public void TestFocusTextInControl()
    {
        EditorGUI.FocusTextInControl("test");
    }
    #endregion

    #region 测试 EnumPopup,IntPopup,Popup
    public enum PopUpEnum
    {
        first,
        second, 
        third, 
        fourth,
    }

    public void TestPopup()
    {
        EditorGUI.EnumPopup(new Rect(300,200,200,20),PopUpEnum.fourth);
        EditorGUI.IntPopup(new Rect(300,230,200,20),0,new GUIContent[] {new GUIContent("第一个"),new GUIContent("第二个")},new int[] {0,1}); ;
        EditorGUI.Popup(new Rect(300, 260, 200, 20),0, new GUIContent[] { new GUIContent("One"), new GUIContent("Two") });
    }
    #endregion

    #region 测试DropShadowLabel
    public void TestDropShadowLabel()
    {
        //绘制带有投影的标签
        EditorGUI.DropShadowLabel(new Rect(900, 650, 200, 200),"这是一个阴影文字",new GUIStyle() { fontSize = 30});
    }
    #endregion

    #region 测试 DropdownButton
    public void TestDropdownButton()
    {
        //用于显示自己的下拉菜单内容
        if(EditorGUI.DropdownButton(new Rect(900, 590, 200, 200),new GUIContent("TEST"), FocusType.Keyboard))
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("test1"), true, () => { });
            genericMenu.AddItem(new GUIContent("test2"), false, () => { });
            genericMenu.AddItem(new GUIContent("test3"), false, () => { });
            genericMenu.AddItem(new GUIContent("test4"), false, () => { });
            genericMenu.AddItem(new GUIContent("test5"), false, () => { });
            genericMenu.DropDown(new Rect(950,410, 400, 200));
        }
    }
    #endregion

    #region 测试 DrawTextureTransparent
    public void TestDrawTextureTransparent()
    {
        EditorGUI.DrawTextureTransparent(new Rect(600, 590, 200, 200), texture,ScaleMode.ScaleToFit);
    }
    #endregion

    #region 测试 DrawTextureAlpha
    public void TestDrawTextureAlpha()
    {
        //在矩形内绘制纹理的Alpha通道
        EditorGUI.LabelField(new Rect(300, 480, 200, 200), "在矩形内绘制纹理的Alpha通道");
        EditorGUI.DrawTextureAlpha(new Rect(300, 590, 200, 200),texture);
    }
    #endregion

    #region 测试DrawRect
    public void TestDrawRect()
    {
        EditorGUI.LabelField(new Rect(900, 880, 250, 200), "在窗口指定位置绘制一个指定颜色的矩形");
        //在窗口指定位置绘制一个指定颜色的矩形
        EditorGUI.DrawRect(new Rect(900, 990, 200, 200),Color.red);
    }
    #endregion

    #region 测试DrawPreviewTexture
    Texture texture;
    public void TestDrawPreviewTexture()
    {
        EditorGUI.LabelField(new Rect(600, 880, 200, 200), "在矩形内绘制纹理");
        //在矩形内绘制纹理
        EditorGUI.DrawPreviewTexture(new Rect(600, 990, 200, 200), texture);
    }
    #endregion

    #region Field
    [Serializable]
    public class Attribute
    {
        public float hp;
        public float maxhp;
        public float mp;
        public float maxmp;
    }

    [Serializable]
    public class MonsterData : ScriptableObject
    {
        public Attribute att1;
        public GameObject obj;

        public MonsterData()
        {
            this.att1 = new Attribute();
        }
    }
    SerializedObject mMonsterData;
    SerializedObject mAttData;

    [Serializable]
    public class AttData : ScriptableObject
    {
        public int a1 = 1;
        public int a2 = 2;
        public int a3 = 3;
        public int a4 = 4;
    }

    SerializedProperty mAtt;
    SerializedProperty mAtt1;

    SerializedProperty mObj;
    public void TestField()
    {
        //BoundsField 用于输入Bounds的Center和Extents字段
        EditorGUI.BoundsField(new Rect(0,300,250,50), "BoundsField", new Bounds(Vector3.zero,Vector3.one));
        //BoundsIntField 用于输入BoundsInt的Position和Size字段
        EditorGUI.BoundsIntField(new Rect(0,360,250,50), "BoundsIntField", new BoundsInt(Vector3Int.zero,Vector3Int.one));
        //ColorField 用于选择Color字段
        EditorGUI.ColorField(new Rect(0,430,100,20), "ColorField ", Color.white);
        //CurveField 用于编辑AnimationCurve的字段
        EditorGUI.CurveField(new Rect(0,460,250,20), "CurveField", new AnimationCurve());
        //EnumFlagsField 
        EditorGUI.EnumFlagsField(new Rect(0,490,200,20), "EnumFlagsField", SpeedOption.Fast);
        //DoubleField 用于输入双精度浮点数的字段
        EditorGUI.DoubleField(new Rect(0,520,200,20), "DoubleField", 0);
        //FloatField 用于输入浮点数的文本字段
        EditorGUI.FloatField(new Rect(0,550,200,20), "FloatField", 0);
        //IntField 用于输入整数的字段
        EditorGUI.IntField(new Rect(0,580,200,20), "IntField", 0);
        //LongField 用于输入长整数的字段
        EditorGUI.LongField(new Rect(0, 610, 200, 20), "LongField", 0);
        //LabelField 创建一个标签字段
        EditorGUI.LabelField(new Rect(0, 640, 100, 20), "这是一个标签");
        //LayerField 创建一个层选择字段
        EditorGUI.LayerField(new Rect(0, 670, 250, 20), "LayerField", 0);
        //GradientField 创建一个用于编辑Gradient的字段
        EditorGUI.GradientField(new Rect(0, 700, 250, 20), "GradientField", new Gradient());
        //MaskField 创建一个掩码字段
        EditorGUI.MaskField(new Rect(0,730,100,20), "MaskField", 0,new string[] {"player","enemy","npc"});
        //MultiFloatField 同一行输入多个浮点值
        EditorGUI.MultiFloatField(new Rect(0, 760, 200, 20),new GUIContent("MultiFloatField"), new GUIContent[] {new GUIContent("第一个"),new GUIContent("第二个")}, new float[] {2.5f,3.0f});
        //MultiIntField 同一行输入多个整数
        EditorGUI.MultiIntField(new Rect(0, 810, 200, 20),new GUIContent[] { new GUIContent("第一个"), new GUIContent("第二个") }, new int[] { 6, 100 });
        //MultiPropertyField 同一行包含多个属性，标签数组决定显示的属性数量。使用的属性不应超过4个
        mAtt1 = mAttData.FindProperty("a1");
        //mAtt = mMonsterData.FindProperty("att1");
        EditorGUI.MultiPropertyField(new Rect(0,830,300,20), new GUIContent[] { new GUIContent("第一个"), new GUIContent("第二个"), new GUIContent("第三个"), new GUIContent("第四个") },mAtt1, new GUIContent("MultiPropertyField"));
        //ObjectField 创建一个对象字段，可以通过拖放对象或者使用对象选择器选择对象来分配对象
        EditorGUI.ObjectField(new Rect(0,870,250,20),mObj);
        //PasswordField 创建一个输入密码的字段
        EditorGUI.PasswordField(new Rect(0,900,250,20),"这是一个密码","ssssss");
        //PropertyField 针对SerializedProperty 创建一个字段
        EditorGUI.PropertyField(new Rect(0,930,200,20),mAtt,new GUIContent("这是一个属性字段"),true);
        //RectField 创建用于输入Rect的xywh的字段
        EditorGUI.RectField(new Rect(300,880,200,20),"这是一个Rect字段",new Rect(0,0,200.0f,200.0f));
        //RectIntField 创建用于输入RectInt的xywh的字段
        EditorGUI.RectIntField(new Rect(300,810,200,20),"这是一个RectInt字段",new RectInt(0,0,200,200));
        //TagField 创建标签选择字段
        EditorGUI.TagField(new Rect(600,880,250,20),"这是一个标签选择字段","player");
        //TextField 创建一个文本字段
        EditorGUI.TextField(new Rect(600,810,250,20),"这是一个文本字段","请输入。。。");
        //Vector2Field 输入Vector2的xy字段
        EditorGUI.Vector2Field(new Rect(0, 1030, 200, 20), "这是一个Vector2字段", Vector2.zero);
        //Vector2IntField 输入Vector2Int的xy字段
        EditorGUI.Vector2IntField(new Rect(0, 1070, 200, 20), "这是一个Vector2Int字段", Vector2Int.zero);
        //Vector3Field 输入Vector3的xyz字段
        EditorGUI.Vector3Field(new Rect(0, 1110, 200, 20), "这是一个Vector3字段", Vector3.zero);
        //Vector3IntField 输入Vector3Intd的xyz字段
        EditorGUI.Vector3IntField(new Rect(0, 1150, 200, 20), "这是一个Vector3Int字段", Vector3Int.zero);
        //Vector4Field 输入Vector4的xyzw字段
        EditorGUI.Vector4Field(new Rect(0, 1190, 200, 20), "这是一个Vector4字段", Vector4.zero);
        //DelayedDoubleField 创建一个用于输入双精度浮点数的延迟文本字段
        EditorGUI.DelayedDoubleField(new Rect(300,990,200,20),"这是一个Double延迟文本字段",20);
        //DelayedFloatField 创建一个用于输入浮点数的延迟文本字段 
        EditorGUI.DelayedFloatField(new Rect(300, 1030, 200, 20), "这是一个Float延迟文本字段", 20);
        //DelayedIntField 创建一个用于输入整数的延迟文本字段
        EditorGUI.DelayedIntField(new Rect(300, 1070, 200, 20), "这是一个Int延迟文本字段", 20);
        //DelayedTextField 创建一个用于输入延迟文本字段
        EditorGUI.DelayedTextField(new Rect(300, 1110, 200, 20), "这是一个文本延迟文本字段", "");
    }
    #endregion

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
        EditorGUI.indentLevel--;
        EditorGUI.indentLevel--;
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
