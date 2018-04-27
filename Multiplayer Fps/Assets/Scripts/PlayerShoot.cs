﻿using UnityEngine;
using UnityEngine.Networking;
[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    private PlayerWeapon currentWeapon;    
    
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private LayerMask mask;

    private WeaponManager weaponManager;

	void Start ()
    {
		if(cam == null)
        {
            this.enabled = false;
        }
        weaponManager = GetComponent<WeaponManager>();
	}


    private void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();

        if (currentWeapon.fireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }

        }
    }

    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    [ClientRpc]
    void RpcDoShootEffect()
    {
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
    }

    [Client]
    private void Shoot()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        CmdOnShoot();

        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.Range, mask))
        {
            if (_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(_hit.collider.name, currentWeapon.Damage);
            }
        }

    }

    [Command]
    private void CmdPlayerShot(string _PlayerID, int _damage)
    {
        Player _player = GameManager.GetPlayer(_PlayerID);
        _player.RpcTakeDamage(_damage);
    }

}
