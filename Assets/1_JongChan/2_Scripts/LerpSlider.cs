using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LerpSlider : MonoBehaviour
{
    private Image backSlider;
    private Image frontSlider;
    
    void Start()
    {
        transform.GetChild(1).TryGetComponent(out backSlider);
        transform.GetChild(2).TryGetComponent(out frontSlider);
    }

    public void SliderValue(float value)
    {
        StartCoroutine(_SliderValue(value));
    }

    IEnumerator _SliderValue(float value)
    {
        frontSlider.fillAmount = value;
        while (value < backSlider.fillAmount)
        {
            yield return null;
            backSlider.fillAmount = Mathf.Lerp(backSlider.fillAmount, value, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
