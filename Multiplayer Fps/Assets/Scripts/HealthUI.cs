using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour{

    [SerializeField]
    private Text txtHealth;
    private int maxHealth;

    public void SetDefaultHealthUI(int _amount)
    {
        maxHealth = _amount;
        txtHealth.text = _amount.ToString() + "/" + _amount.ToString();
    }

    public void UpdateHealthUI(int _amount)
    {
        txtHealth.text = _amount.ToString() + "/" + maxHealth.ToString();
    }
}
