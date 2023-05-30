using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStateBar : MonoBehaviour
{
    public Image healthImage;
    public Image healthDelayImage;
    public Image powerImage;

    public float delaySpeed;

    private void Awake()
    {
        healthDelayImage.fillAmount = 1f;
    }

    /// <summary>
    /// 接收Health的变更百分比
    /// </summary>
    /// <param name="persentage">百分比：Current/Max</param>
    public void OnHealthChange(float persentage)
    {
        //Debug.Log("healthchange");
        healthImage.fillAmount = persentage;
        StartCoroutine(healthDelayChange(persentage));
    }

    IEnumerator healthDelayChange(float persentage)
    {
        while (healthDelayImage.fillAmount > persentage)
        {
            if (healthDelayImage.fillAmount - Time.deltaTime * delaySpeed <= persentage)
            {
                healthDelayImage.fillAmount = persentage;
            }
            else healthDelayImage.fillAmount -= Time.deltaTime * delaySpeed;
            yield return null;
        }
    }
}
