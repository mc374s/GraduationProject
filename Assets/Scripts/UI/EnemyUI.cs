using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField]
    public Image HealthGauge;

    [SerializeField]
    public Damageable representedDamageable;

    public Image knockDownButton = null;
    // Start is called before the first frame update
    void Start()
    {
        HealthGauge.fillAmount = 0;
        if (HealthGauge)
        {
            StartCoroutine(FillGauge());
        }
        if (knockDownButton)
        {
            knockDownButton.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = Vector3.zero;
    }
    //private void LateUpdate()
    //{
    //    if (knockDownButton && knockDownButton.enabled)
    //    {
    //        knockDownButton.enabled = false;
    //    }
    //}

    private IEnumerator FillGauge()
    {
        float usedTime = 1;

        for (float start = 0; start < usedTime; start += Time.deltaTime)
        {
            HealthGauge.fillAmount = start / usedTime;
            yield return null;
        }

        HealthGauge.fillAmount = 1;
    }

    public void ChangeHealth(Damageable damageable)
    {
        HealthGauge.fillAmount = (float)damageable.CurrentHealth / (float)damageable.startingHealth;
    }

    public void ShowKnockDownButton()
    {
        knockDownButton.enabled = true;
    }
    public void HideKnockDownButton()
    {
        knockDownButton.enabled = false;
    }

}
