using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Enemy boss;
    public Slider slider;
    public bool hideWhenDie = true;
    public float hideDelay = 1;

    // Start is called before the first frame update
    void Start()
    {
        // set slider value based on boss max health
        slider.maxValue = boss.maxHealth;
        slider.value = boss.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float health = boss.GetHealth();
        slider.value = health;

        if (health <= 0 && hideWhenDie)
        {
            StartCoroutine(HideUI());
        }
    }

    private IEnumerator HideUI()
    {
        yield return new WaitForSeconds(hideDelay);
        gameObject.SetActive(false);
    }

    //private Vector3 GetPlayerLocation()
    //{
    //    return PlayerController.instance.gameObject.transform.position;
    //}

    //private Vector3 GetBossLocation()
    //{
    //    return boss.gameObject.transform.position;
    //}
}
