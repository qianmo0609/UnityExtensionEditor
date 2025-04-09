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
        //��ʾ����ʵ��������ʹ�������Ĵ�С
        EditorWindow.GetWindow(typeof(TestWindowUseEditorGUILayout));
        //��ʾ���ڰ����Զ�λ�úʹ�С������(0,0)�㣬��600����800,������ʹ�������Ĵ�С
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
        // �л��۵��͸���Ŀ�국��
        isExpanded = EditorGUILayout.Foldout(isExpanded, "Additional Settings");
        float targetFade = isExpanded ? 1f : 0f;

        // �����ض���currentFade
        currentFade = Mathf.Lerp(currentFade, targetFade, 0.01f);

        // ��ʼ�����飬���������ݣ�����ɼ�
        if (EditorGUILayout.BeginFadeGroup(currentFade))
        {
            EditorGUILayout.LabelField("Extra Field 1");
            EditorGUILayout.LabelField("Extra Field 2");
        }
        EditorGUILayout.EndFadeGroup();

        // �����ȻFade�����»���
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
        // �����ť�������չ����������
        if (EditorGUILayout.DropdownButton(
            new GUIContent("Options"), // ��ť��ʾ���ı�
            FocusType.Passive,          // �������ͣ�ͨ���� Passive��
            EditorStyles.toolbarButton // ��ť��ʽ����ѡ��
        ))
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Reset Values"), false, () => {
                // ִ�в���
                Debug.Log("Reset values");
            });
            menu.AddItem(new GUIContent("Export Data"), false, () => {
                Debug.Log("Export data");
            });
            menu.ShowAsContext(); // ��ʾ�˵�
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
        EditorGUILayout.Vector4Field("λ�ã�",new Vector4(r.x,r.y,r.width,r.height));
    }

    void TestHelpBox()
    {
        EditorGUILayout.HelpBox("xxxxx ����һ������Box", MessageType.Warning);
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


// �Զ��� EditorTool ��
public class CustomEditorTool : EditorTool
{
    // ������ͼ��
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

    // �����ʱ����
    public override void OnActivated()
    {
        Debug.Log("Custom tool activated.");
    }

    // ���ߴ��ڻ״̬ʱ���ڳ�����ͼ�н��л��ƺʹ�����
    public override void OnToolGUI(EditorWindow window)
    {
        // ��ȡ��ǰѡ�е���Ϸ����
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject != null)
        {
            // �ڴ˴�����Զ���Ľ����߼�����������ֱ�����������¼���
            Handles.color = Color.red;
            Handles.DrawWireCube(selectedObject.transform.position, Vector3.one);
        }

        base.OnToolGUI(window);
    }
}
