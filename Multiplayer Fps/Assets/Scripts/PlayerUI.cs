using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    RectTransform thrusterFuelFill;
    [SerializeField]
    Text healthText;
    [SerializeField]
    RectTransform healthBarFill;

    private PlayerController controller;
    private Player player;

    public void SetPlayer(Player _player)
    {
        player = _player;
    }

    public void SetController(PlayerController _controller)
    {
        controller = _controller;
    }

    void Update()
    {
        SetFuelAmount(_amount: controller.GetThrusterFuelAmount());
        SetHealthAmount(_amount: player.GetCurrentHealth(), _max: player.GetMaxHealth());
    }

    void SetFuelAmount(float _amount)
    {
        thrusterFuelFill.localScale = new Vector3(1f, _amount, 1f);
    }

    void SetHealthAmount(int _amount, int _max)
    {
        float _amountf = _amount;
        healthBarFill.localScale = new Vector3(_amountf / 100, 1, 1);
        healthText.text = _amount.ToString();
    }
}
