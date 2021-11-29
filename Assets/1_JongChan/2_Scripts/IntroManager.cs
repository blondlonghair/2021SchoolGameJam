using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        TryGetComponent(out anim);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            anim.SetTrigger("isPut");
        }
    }

    public void PutOn()
    {
        SceneManager.LoadScene("Scenes/Stage1");
    }
}
