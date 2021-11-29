using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadManager : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Color color;
    
    void Start()
    {
        color = spriteRenderer.color;
        TryGetComponent(out anim);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            anim.SetTrigger("isPut");
        }
    }

    public void OnDeadEnd()
    {
        StartCoroutine(Co_DeadEnd());
    }

    IEnumerator Co_DeadEnd()
    {
        while (spriteRenderer.color.a < 0.99f)
        {
            color.a += 0.01f;
            spriteRenderer.color = color;
            yield return YieldCache.WaitForSeconds(0.01f);
        }

        anim.SetTrigger("isIdle");

        while (spriteRenderer.color.a > 0.01f)
        {
            color.a -= 0.01f;
            spriteRenderer.color = color;
            yield return YieldCache.WaitForSeconds(0.01f);
        }
    }
    
    public void PutOn()
    {
        SceneManager.LoadScene("Scenes/Stage1");
    }
}
