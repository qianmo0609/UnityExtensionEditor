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
        //��ʾ����ʵ��
        EditorWindow.GetWindow(typeof(TestWindow));
        //��ʾ���ڰ����Զ�λ�úʹ�С������(0,0)�㣬��600����800
        EditorWindow.GetWindowWithRect<TestWindow>(new Rect(new Vector2(0,0),new Vector2(600,800)));
    }

    SerializedProperty isFastProp;
    SerializedObject serializedObject;

    #region �����������ڷ���
    private void OnEnable()
    {
        //�ڼ��ؽű��������ö���ʱ����
        //Debug.Log("OnEnable");
        ISFastProp o = ScriptableObject.CreateInstance<ISFastProp>();
        serializedObject = new UnityEditor.SerializedObject(o);
        isFastProp = serializedObject.FindProperty("isFast");
    }

    private void CreateGUI()
    {
        //���Editorδ���£�������ͼ���û�����
        //Debug.Log("CreateGUI");
    }

    private void Update()
    {
        //ÿ֡����һ���Ը��½ű����߼�
        //Debug.Log("Updaete");
    }

    private void OnDisable()
    {
        //���ű������û��߶�����������ɺ�������Դʱ����
        //Debug.Log("OnDisable");
    }
    #endregion

    

    private void OnGUI()
    {
        //ÿ֡��ε��ã�������Ⱦ�ʹ���GUI�¼�
        //��Ⱦ���ڵ�ʵ������
        //Debug.Log("OnGUI");
        if (EditorGUI.actionKey)
        {
            //�Ƿ�ס��ƽ̨��صġ�action���޸ļ�?(ֻ��),�ü��� macOS ��Ϊ Command���� Windows ��Ϊ Control
            Debug.Log("������actionKey");
        }

        //����indentLevel
        TestIndentLevel();
        //����showMixedValue
        ShowValueTest2();
        //����BeginChangeCheck��EndChangeCheck
        ChangeCheck();
        //����BeginDisabledGroup��EndDisabledGroup
        TestDisabledGroup();
        //����BeginFoldoutHeaderGroup��EndFoldoutHeaderGroup
        TestFoldoutHeaderGroup();
        //����BeginProperty��EndProperty
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
        //ֻ�е��û��ı������ֵ���Ű�����ֵ��ȥ��
        //�����ڶ����༭ʱ����Ϊ���ж�����䵥��ֵ��
        //��ʹ�û�û�д����ؼ���
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
                EditorGUI.LabelField(new Rect(0, 220, 200, 20),"����ѡ��һ�����壡");
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
            this.ShowTips("�������ɹ���");
        }
    }
    #endregion
    public void ShowTips(string content)
    {
        EditorUtility.DisplayDialog("��ʾ",content,"Ok");
    }

    #region IndentLevel
    public void TestIndentLevel()
    {
        //ʹ��indentLevel�����ı�
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
        // ��isFast����ֵת��Ϊenumֵ
        SpeedOption speedOptionEnumValue = SpeedOption.Fast;
        // �������˵�����ʾö��ֵ��
        speedOptionEnumValue = (SpeedOption)EditorGUILayout.EnumPopup("Speed", speedOptionEnumValue);
        // ��showMixedValue����Ϊfalse���������Ͳ���Ӱ�����¿ؼ�������еĻ�����
        EditorGUI.showMixedValue = false;
    }

    public void ShowValueTest()
    {
        serializedObject.Update();
        // �Ա�׼��ʽ��ʾisFast����ֵ-��Ϊ�л���
        EditorGUILayout.PropertyField(isFastProp);
        // ʹ�����涨���SpeedOptionö�ٽ�isFast����ֵ��ʾΪ�����б�
        // ����ֵ�Ƿ��ڸÿ��ڱ�����
        EditorGUI.BeginChangeCheck();
        // ���isFast���Ա�ʾ�����ͬ��ֵ��������Ӧ����ʾ��
        EditorGUI.showMixedValue = isFastProp.hasMultipleDifferentValues;
        // ��isFast����ֵת��Ϊenumֵ
        SpeedOption speedOptionEnumValue;
        if (isFastProp.boolValue == true)
            speedOptionEnumValue = SpeedOption.Fast;
        else
            speedOptionEnumValue = SpeedOption.Slow;
        // �������˵�����ʾö��ֵ��
        speedOptionEnumValue = (SpeedOption)EditorGUILayout.EnumPopup("Speed", speedOptionEnumValue);
        // ��showMixedValue����Ϊfalse���������Ͳ���Ӱ�����¿ؼ�������еĻ�����
        EditorGUI.showMixedValue = false;
        // �����������иı���ֵ������ֵӦ�������ж���
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
