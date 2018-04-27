using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
[RequireComponent(typeof(HealthUI))]
public class Player : NetworkBehaviour {

    [SyncVar]
    private bool isDead;
    public bool IsDead
    {
        get { return isDead; }
        protected set { isDead = value; }
    }

    [SerializeField]
    private int maxHealth = 100;
    [SyncVar]
    private int currentHealth;
    
    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;
    [SerializeField]
    private Behaviour playerUI;
    private HealthUI healthUI;

    public void Setup()
    {
        healthUI = playerUI.transform.GetComponentInChildren<HealthUI>();
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }
        healthUI.SetDefaultHealthUI(maxHealth);
        SetDefaults();
    }
    [ClientRpc]
    public void RpcTakeDamage(int _amount)
    {
        if (IsDead)
            return;

        currentHealth -= _amount;
        healthUI.UpdateHealthUI(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        IsDead = true;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = false;

        StartCoroutine(Respawn());
    }  
    
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3f);
        SetDefaults();
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;
    }

    public void SetDefaults()
    {
        IsDead = false;

        currentHealth = maxHealth;
        //healthUI.SetDefaultHealthUI(maxHealth);
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        Collider _col = GetComponent<Collider>();
        if(_col != null)
        {
            _col.enabled = true;
        }
    }
}
