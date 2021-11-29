using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    [SerializeField] private SkillIconUI skill1IconUI;
    [SerializeField] private SkillIconUI skill2IconUI;
    [SerializeField] private Player player;
    [SerializeField] private LerpSlider hpBar;
    
    void Update()
    {
        skill1IconUI.SetValue(1 - (player.skill1Timer / player.skill1Interval));
        skill2IconUI.SetValue(1 - (player.skill2Timer / player.skill2Interval));
        
        hpBar.SliderValue(player.curHp / player.maxHp);
    }
}
