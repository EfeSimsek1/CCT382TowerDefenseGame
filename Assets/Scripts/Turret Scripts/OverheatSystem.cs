using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OverheatSystem : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float heatPerShot;
    [SerializeField] private float coolingPerSec;
    [SerializeField] private float heatLimit;
    [SerializeField] private Slider heatBar;
    [SerializeField] private float overHeatTime;
    private bool isCooling;

    public UnityEvent<float> onOverHeat = new UnityEvent<float>();

    private float currentHeat;
    private TurretFire tf;
    private TurretAim ta;

    private void Awake()
    {
        isCooling = false;
        currentHeat = 0;
        tf = GetComponent<TurretFire>();
        ta = GetComponent<TurretAim>();
        tf.onFire.AddListener(HeatUp);
    }

    void Update()
    {
        currentHeat -= Time.fixedDeltaTime * coolingPerSec;
        currentHeat = Mathf.Clamp(currentHeat, 0, 100);

        heatBar.value = currentHeat / heatLimit;
    }

    private void HeatUp()
    {
        if (isCooling) return;

        currentHeat += heatPerShot;

        if (currentHeat > heatLimit)
        {
            currentHeat = heatLimit;

            // Overheat turret
            tf.canFire = false;
            ta.enabled = false;
            StartCoroutine(Cooling());
        }
    }

    public void HeatUp(float heat)
    {
        if (isCooling) return;

        currentHeat += heat;

        if (currentHeat > heatLimit)
        {
            currentHeat = heatLimit;

            // Overheat turret
            tf.canFire = false;
            ta.enabled = false;
            StartCoroutine(Cooling());
        }
    }

    private IEnumerator Cooling()
    {
        isCooling = true;

        while (currentHeat > 0)
        {
            yield return null;
        }

        ta.enabled = true;
        tf.canFire = true;
        isCooling = false;
    }


}
