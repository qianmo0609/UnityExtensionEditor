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
        //��ʾ����ʵ��������ʹ�������Ĵ�С
        EditorWindow.GetWindow(typeof(TestWindowUseEditorGUI));
        //��ʾ���ڰ����Զ�λ�úʹ�С������(0,0)�㣬��600����800,������ʹ�������Ĵ�С
        //EditorWindow.GetWindowWithRect<TestWindowUseEditorGUI>(new Rect(new Vector2(0,0),new Vector2(600,800)));
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

        MonsterData m = ScriptableObject.CreateInstance<MonsterData>();
        mMonsterData = new UnityEditor.SerializedObject(m);
        mAtt = mMonsterData.FindProperty("att1");
        mObj = mMonsterData.FindProperty("obj");

        texture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Art/bg_window.png");
        Debug.Log(texture);
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
        //����Field
        TestField();
        //����DrawPreviewTexture
        TestDrawPreviewTexture();
        //����DrawRect
        TestDrawRect();
        //���� DrawTextureAlpha
        TestDrawTextureAlpha();
        //���� DrawTextureTransparent
        TestDrawTextureTransparent();
        //���� DropdownButton
        TestDropdownButton();
        //���� DropShadowLabel
        TestDropShadowLabel();
    }

    #region ����DropShadowLabel
    public void TestDropShadowLabel()
    {
        //���ƴ���ͶӰ�ı�ǩ
        EditorGUI.DropShadowLabel(new Rect(900, 650, 200, 200),"����һ����Ӱ����",new GUIStyle() { fontSize = 30});
    }
    #endregion

    #region ���� DropdownButton
    public void TestDropdownButton()
    {
        //������ʾ�Լ��������˵�����
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

    #region ���� DrawTextureTransparent
    public void TestDrawTextureTransparent()
    {
        EditorGUI.DrawTextureTransparent(new Rect(600, 590, 200, 200), texture,ScaleMode.ScaleToFit);
    }
    #endregion

    #region ���� DrawTextureAlpha
    public void TestDrawTextureAlpha()
    {
        //�ھ����ڻ��������Alphaͨ��
        EditorGUI.DrawTextureAlpha(new Rect(300, 590, 200, 200),texture);
    }
    #endregion

    #region ����DrawRect
    public void TestDrawRect()
    {
        //�ڴ���ָ��λ�û���һ��ָ����ɫ�ľ���
        EditorGUI.DrawRect(new Rect(900, 990, 200, 200),Color.red);
    }
    #endregion

    #region ����DrawPreviewTexture
    Texture texture;
    public void TestDrawPreviewTexture()
    {
        //�ھ����ڻ�������
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
    SerializedProperty mAtt;

    SerializedProperty mObj;
    public void TestField()
    {
        //BoundsField ��������Bounds��Center��Extents�ֶ�
        EditorGUI.BoundsField(new Rect(0,300,250,50),new Bounds(Vector3.zero,Vector3.one));
        //BoundsIntField ��������BoundsInt��Position��Size�ֶ�
        EditorGUI.BoundsIntField(new Rect(0,350,250,50),new BoundsInt(Vector3Int.zero,Vector3Int.one));
        //ColorField ����ѡ��Color�ֶ�
        EditorGUI.ColorField(new Rect(0,400,100,20),Color.white);
        //CurveField ���ڱ༭AnimationCurve���ֶ�
        EditorGUI.CurveField(new Rect(0,430,100,20),new AnimationCurve());
        //EnumFlagsField 
        EditorGUI.EnumFlagsField(new Rect(0,460,100,20),SpeedOption.Fast);
        //DoubleField ��������˫���ȸ��������ֶ�
        EditorGUI.DoubleField(new Rect(0,490,100,20),0);
        //FloatField �������븡�������ı��ֶ�
        EditorGUI.FloatField(new Rect(0,520,100,20),0);
        //IntField ���������������ֶ�
        EditorGUI.IntField(new Rect(0,550,100,20),0);
        //LongField �������볤�������ֶ�
        EditorGUI.LongField(new Rect(0, 580, 100, 20),0);
        //LabelField ����һ����ǩ�ֶ�
        EditorGUI.LabelField(new Rect(0, 610, 100, 20), "����һ����ǩ");
        //LayerField ����һ����ѡ���ֶ�
        EditorGUI.LayerField(new Rect(0, 640, 100, 20), 0);
        //GradientField ����һ�����ڱ༭Gradient���ֶ�
        EditorGUI.GradientField(new Rect(0, 670, 100, 20), new Gradient());
        //MaskField ����һ�������ֶ�
        EditorGUI.MaskField(new Rect(0,700,100,20),0,new string[] {"player","enemy","npc"});
        //MultiFloatField ͬһ������������ֵ
        EditorGUI.MultiFloatField(new Rect(0, 730, 200, 20), new GUIContent[] {new GUIContent("��һ��"),new GUIContent("�ڶ���")}, new float[] {2.5f,3.0f});
        //MultiIntField ͬһ������������
        EditorGUI.MultiIntField(new Rect(0, 760, 200, 20), new GUIContent[] { new GUIContent("��һ��"), new GUIContent("�ڶ���") }, new int[] { 6, 100 });
        //MultiPropertyField ͬһ�а����������
        //EditorGUI.MultiPropertyField(new Rect(0,790,200,20), new GUIContent[] { new GUIContent("��һ��"), new GUIContent("�ڶ���") }, mAtt);
        //ObjectField ����һ�������ֶΣ�����ͨ���ϷŶ������ʹ�ö���ѡ����ѡ��������������
        EditorGUI.ObjectField(new Rect(0,820,250,20),mObj);
        //PasswordField ����һ������������ֶ�
        EditorGUI.PasswordField(new Rect(0,850,250,20),"����һ������","ssssss");
        //PropertyField ���SerializedProperty ����һ���ֶ�
        EditorGUI.PropertyField(new Rect(0,880,200,20),mAtt,new GUIContent("����һ�������ֶ�"),true);
        //RectField ������������Rect��xywh���ֶ�
        EditorGUI.RectField(new Rect(300,880,200,20),"����һ��Rect�ֶ�",new Rect(0,0,200.0f,200.0f));
        //RectIntField ������������RectInt��xywh���ֶ�
        EditorGUI.RectIntField(new Rect(300,810,200,20),"����һ��RectInt�ֶ�",new RectInt(0,0,200,200));
        //TagField ������ǩѡ���ֶ�
        EditorGUI.TagField(new Rect(600,880,250,20),"����һ����ǩѡ���ֶ�","player");
        //TextField ����һ���ı��ֶ�
        EditorGUI.TextField(new Rect(600,810,250,20),"����һ���ı��ֶ�","�����롣����");
        //Vector2Field ����Vector2��xy�ֶ�
        EditorGUI.Vector2Field(new Rect(0, 990, 200, 20), "����һ��Vector2�ֶ�", Vector2.zero);
        //Vector2IntField ����Vector2Int��xy�ֶ�
        EditorGUI.Vector2IntField(new Rect(0, 1030, 200, 20), "����һ��Vector2Int�ֶ�", Vector2Int.zero);
        //Vector3Field ����Vector3��xyz�ֶ�
        EditorGUI.Vector3Field(new Rect(0, 1070, 200, 20), "����һ��Vector3�ֶ�", Vector3.zero);
        //Vector3IntField ����Vector3Intd��xyz�ֶ�
        EditorGUI.Vector3IntField(new Rect(0, 1110, 200, 20), "����һ��Vector3Int�ֶ�", Vector3Int.zero);
        //Vector4Field ����Vector4��xyzw�ֶ�
        EditorGUI.Vector4Field(new Rect(0, 1150, 200, 20), "����һ��Vector4�ֶ�", Vector4.zero);
        //DelayedDoubleField ����һ����������˫���ȸ��������ӳ��ı��ֶ�
        EditorGUI.DelayedDoubleField(new Rect(300,990,200,20),"����һ��Double�ӳ��ı��ֶ�",20);
        //DelayedFloatField ����һ���������븡�������ӳ��ı��ֶ� 
        EditorGUI.DelayedFloatField(new Rect(300, 1030, 200, 20), "����һ��Float�ӳ��ı��ֶ�", 20);
        //DelayedIntField ����һ�����������������ӳ��ı��ֶ�
        EditorGUI.DelayedIntField(new Rect(300, 1070, 200, 20), "����һ��Int�ӳ��ı��ֶ�", 20);
        //DelayedTextField ����һ�����������ӳ��ı��ֶ�
        EditorGUI.DelayedTextField(new Rect(300, 1110, 200, 20), "����һ���ı��ӳ��ı��ֶ�", "");
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
        EditorGUI.indentLevel--;
        EditorGUI.indentLevel--;
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
