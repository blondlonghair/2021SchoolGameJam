using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMono<GameManager>
{
    [SerializeField] private SkillIconUI skill1IconUI;
    [SerializeField] private SkillIconUI skill2IconUI;
    [SerializeField] private Player player;
    [SerializeField] private LerpSlider hpBar;
    [SerializeField] private GameObject hitEffect;

    [SerializeField] private GameObject pausePanel;

    private Image hitEffectSpriteImage;
    private Color hitEffectColor;

    private Coroutine coroutine;

    private void Start()
    {
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
}
