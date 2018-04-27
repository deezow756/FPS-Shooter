using UnityEngine;

[System.Serializable]
public class PlayerWeapon {

    public string Name = "Glock";
    public int Damage = 10;
    public float Range = 100f;

    public float fireRate = 0f;

    public GameObject graphics;
}
