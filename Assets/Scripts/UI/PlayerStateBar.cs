using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerStateBar : MonoBehaviour
    {
        public Image healthImage;
        public Image healthDelayImage;
        public Image powerImage;
        public Vector3 tmp;
        private Animator _powerFillYellowAnimator;

        public float delaySpeed;

        private void Awake()
        {
            _powerFillYellowAnimator = powerImage.GetComponent<Animator>();
            healthDelayImage.fillAmount = 1f;
        }

        /// <summary>
        /// 接收Health的变更百分比
        /// </summary>
        /// <param name="percentage">百分比：Current/Max</param>
        public void OnHealthChange(float percentage)
        {
            //Debug.Log("healthChange");
            healthImage.fillAmount = percentage;
            StartCoroutine(HealthDelayChange(percentage));
        }

        private IEnumerator HealthDelayChange(float percentage)
        {
            while (healthDelayImage.fillAmount > percentage)
            {
                if (healthDelayImage.fillAmount - Time.deltaTime * delaySpeed <= percentage)
                {
                    healthDelayImage.fillAmount = percentage;
                }
                else healthDelayImage.fillAmount -= Time.deltaTime * delaySpeed;
                yield return null;
            }
        }

        public void OnPowerChange(float percentage)
        {
            powerImage.fillAmount = percentage;
        }

        public void PowerLack()
        {
            _powerFillYellowAnimator.SetBool("isPowerLack", true);
        }

        public void PowerLackEnd()
        {
            _powerFillYellowAnimator.SetBool("isPowerLack", false);
        }
    }
}
