using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, 0);

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }

    public void SetMaxHealth(int health)
    {
        if (slider != null)
        {
            slider.maxValue = health;
            slider.value = health;
        }
    }

    public void SetHealth(int health)
    {
        if (slider != null)
        {
            slider.value = Mathf.Clamp(health, 0, slider.maxValue);
        }
    }
}
