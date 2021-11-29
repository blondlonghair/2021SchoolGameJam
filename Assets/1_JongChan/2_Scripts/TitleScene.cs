using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private SpriteRenderer blackBG;
    [SerializeField] private SpriteRenderer titleText;
    
    private enum Title
    {
        GameStart,
        BlackOff,
        TitleOnOff
    }

    private Title titleState;
    private Color black;
    private Color title;
    private bool isOnOff;
    
    void Start()
    {
        black = blackBG.color;
        title = titleText.color;
    }

    void Update()
    {
        switch (titleState)
        {
            case Title.GameStart:
                OnGameStart();
                break;
            case Title.BlackOff:
                BlackOff();
                break;
            case Title.TitleOnOff:
                TitleOnOff();
                break;
        }
    }

    private void OnGameStart()
    {
        black.a -= Time.deltaTime;
        blackBG.color = black;

        if (blackBG.color.a < 0.01)
        {
            blackBG.gameObject.SetActive(false);
            titleState = Title.BlackOff;
        }
    }

    private void BlackOff()
    {
        if (titleText.color.a < 0.01)
        {
            isOnOff = true;
        }

        if (titleText.color.a > 0.99)
        {
            isOnOff = false;
        }

        if (isOnOff)
        {
            title.a += 0.01f;
            titleText.color = title;
        }

        else
        {
            title.a -= 0.01f;
            titleText.color = title;
        }

        if (Input.anyKey)
        {
            SceneManager.LoadScene("Scenes/Intro");
        }
    }

    private void TitleOnOff()
    {
        
    }
}
