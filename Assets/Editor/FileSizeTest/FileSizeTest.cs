using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class FileSizeTest : EditorWindow
{
    static bool ShowPathNames = true;
    static bool ShowErrorFiles = true;
    List<string> filePaths = new List<string>();
    static List<string> ErrorFiles = new List<string>();

    const int MAX_FILE_SIZE = 100000000;

    [MenuItem("File Size Test/Test Program")]
    public static void ShowWindow()
    {
        GetWindow(typeof(FileSizeTest));        
    }
    void OnEnable()
    {
        filePaths.Clear();
        filePaths.Add(Application.dataPath);
        filePaths.Add(Application.dataPath.Substring(0, Application.dataPath.Length - 6) + "ProjectSettings");
    }

    void OnGUI()
    {
        GUILayout.Space(10);
        DrawFilePath();
        GUILayout.Space(10);
        GetButtonInput();
        GUILayout.Space(10);
        DrawErrorFiles();
    }

    void TestFilepathFiles(string path)
    {
        try
        {
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; ++i)
            {
                try
                {
                    FileStream file = File.Open(files[i], FileMode.Open);
                    if(file.Length > MAX_FILE_SIZE)
                    {
                        Debug.LogError("File Too Large! " + files[i]);
                        if (!ErrorFiles.Contains(files[i])) ErrorFiles.Add(files[i]);
                    }
                    file.Flush();
                    file.Close();
                    file.Dispose();
                }
                catch (FileNotFoundException)
                {
                    Debug.LogError("File Not Found! " + files[i]);
                }
            }
        }
        catch (DirectoryNotFoundException)
        {
            Debug.LogError("Directory Not Found! " + path);
        }
        
    }

    void GetButtonInput()
    {
        if (GUILayout.Button("Run Test."))
        {
            // run func.
            for(int i = 0; i < filePaths.Count; ++i)
            {
                TestFilepathFiles(filePaths[i]);
            }
            Debug.Log("Finished");
        }
    }

    void DrawErrorFiles()
    {
        if(ErrorFiles.Count > 0)
        {
            ShowErrorFiles = EditorGUILayout.Foldout(ShowErrorFiles, "Error Files");
            if (ShowErrorFiles)
            {
                for(int i = 0; i < ErrorFiles.Count; ++i)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Filepath: ", GUILayout.Width(51));
                    EditorGUILayout.TextField(ErrorFiles[i]);
                    GUILayout.EndHorizontal();
                }
            }
        }
    }

    void DrawFilePath()
    {
        ShowPathNames = EditorGUILayout.Foldout(ShowPathNames, "Filepaths");
        if (ShowPathNames)
        {
            for (int i = 0; i < filePaths.Count; ++i)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Filepath: ", GUILayout.Width(51));
                filePaths[i] = EditorGUILayout.TextField(filePaths[i]);
                GUILayout.EndHorizontal();
            }
        }
    }

}
