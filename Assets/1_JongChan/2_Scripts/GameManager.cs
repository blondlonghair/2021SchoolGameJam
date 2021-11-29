using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Cinemachine;
using Cinemachine.Editor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : SingletonMonoDestroy<GameManager>
{
    [SerializeField] private SkillIconUI skill1IconUI;
    [SerializeField] private SkillIconUI skill2IconUI;
    [SerializeField] private Player player;
    [SerializeField] private LerpSlider hpBar;
    [SerializeField] private GameObject hitEffect;

    [SerializeField] private GameObject pausePanel;

    private CinemachineImpulseSource impulse;
    private Image hitEffectSpriteImage;
    private Color hitEffectColor;

    private Coroutine coroutine;

    private void Start()
    {
        impulse = GetComponent<CinemachineImpulseSource>();
        hitEffect.TryGetComponent(out hitEffectSpriteImage);
        hitEffectColor = hitEffectSpriteImage.color;
    }

    void Update()
    {
        skill1IconUI.SetValue(1 - (player.skill1Timer / player.skill1Interval));
        skill2IconUI.SetValue(1 - (player.skill2Timer / player.skill2Interval));
        
        hpBar.SliderValue(player.curHp / player.maxHp);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }
    }

    public void PauseButton()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void HitEffect()
    {
        impulse.GenerateImpulse(new Vector3(3,3,0));
        StartCoroutine(Co_HitEffect());
    }

    IEnumerator Co_HitEffect()
    {
        hitEffectColor.a = 1;
        hitEffectSpriteImage.color = hitEffectColor;
        hitEffect.SetActive(true);

        yield return YieldCache.WaitForSeconds(0.3f);

        while (hitEffectSpriteImage.color.a > 0.1)
        {
            yield return YieldCache.WaitForSeconds(0.1f);
            hitEffectColor.a -= 0.1f;
            hitEffectSpriteImage.color = hitEffectColor;
        }
        
        hitEffect.SetActive(false);
    }

    // IEnumerator Co_CameraShake()
    // {
    //     Vector3 cameraPos = camera.transform.position;
    //
    //     for (int i = 0; i < 10; i++)
    //     {
    //         yield return YieldCache.WaitForSeconds(0.1f);
    //         camera.transform.position = new Vector3(Random.Range(cameraPos.x - 5, cameraPos.x + 5),
    //             Random.Range(cameraPos.y - 5, cameraPos.y + 5), cameraPos.z);   
    //     }
    //
    //     yield return null;
    // }
}
