using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestEditor : MonoBehaviour
{
    //创建顶部菜单栏选项
    [MenuItem("Tool/test")]
    public static void test()
    {
        Debug.Log("test.....");
    }

    [MenuItem("Tool/testDialog")]
    public static void DisplayDialog()
    {
        EditorUtility.DisplayDialog("Tips", "Hello World", "Completely");
    }

    //创建右键Create菜单栏选项
    [MenuItem("Assets/Create/testFolder")]
    public static void test2()
    {
        Debug.Log("test2....");
    }

    [MenuItem("Tool/testHotKey0 _g")]
    public static void testHotKey0()
    {
        Debug.Log("testHotKeytestHotKey0..........");
    }

    [MenuItem("Tool/testHotKey #g")]
    public static void testHotKey()
    {
        Debug.Log("testHotKeytestHotKey..........");
    }

    [MenuItem("Tool/testHotKey1 #&g")]
    public static void testHotKey1()
    {
        Debug.Log("testHotKeytestHotKey1..........");
    }

    [MenuItem("Tool/testHotKey2 #LEFT")]
    public static void testHotKey2()
    {
        Debug.Log("testHotKeytestHotKey2..........");
    }

    [MenuItem("CONTEXT/TestTool/Test")]
    public static void testKey()
    {
        Debug.Log("testKey..........");
    }
}
