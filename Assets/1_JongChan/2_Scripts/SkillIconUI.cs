using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class SkillIconUI : MonoBehaviour
{
    private Image skillCoolImage;

    void Start()
    {
        transform.GetChild(2).TryGetComponent(out skillCoolImage);
    }

    public void SetValue(float value)
    {
        skillCoolImage.fillAmount = value;
    }
}
