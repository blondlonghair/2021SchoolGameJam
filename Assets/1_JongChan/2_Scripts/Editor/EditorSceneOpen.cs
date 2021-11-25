using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// 에디터에서 씬을 빠르고 쉽게 열어서 작업할수 있도록 도와줍니다.

public class EditorSceneOpen
{
    [MenuItem( "Scenes/1.JongChan" )]
    public static void OpenScene_Title()
    {
        OpenScene( "Assets/1_JongChan/1_Scenes/JongChan.unity" );
    }

    [MenuItem( "Scenes/1.SungBum" )]
    public static void OpenScene_Game()
    {
        //자기 씬 이름 입력
        OpenScene( "Assets/1.Suoki/Scenes/Game.unity" );
    }
    public static void OpenScene( string scenepath )
    {
        if( EditorSceneManager.GetActiveScene().isDirty == true )
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
        EditorSceneManager.OpenScene( scenepath );
    }
}