using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Camera FPCamera;
    [SerializeField] float range = 100f;
    [SerializeField] float damage = 30f;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitEffect;
    [SerializeField] Ammo ammoSlot;
    [SerializeField] AmmoType ammoType;
    [SerializeField] float timeBetweenShots = 0.5f;
    [SerializeField] TextMeshProUGUI ammoText;

    string spath;
    long lastFilePosition = 0;
    bool canShoot = true;

    private void OnEnable()
    {
        canShoot = true;
    }

    private void Start()
    {
        spath ="C:/Users/user/Desktop/some games embedded/zomboid-survival-master_1/zomboid-survival-master/Assets/shoot_command.txt";
    }
    void Update()
    {
        DisplayAmmo();
        ReadFromFileAndShoot(); 
       
    }
  private void ReadFromFileAndShoot()
{
    if (!File.Exists(spath)) return;

    FileStream fs = new FileStream(spath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
    
    try
    {
        fs.Seek(lastFilePosition, SeekOrigin.Begin);

        using (StreamReader reader = new StreamReader(fs, System.Text.Encoding.UTF8, false, 1024, true))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Trim() == "Shoot" && canShoot)
                {
                    StartCoroutine(Shoot());
                }
            }

            lastFilePosition = fs.Position;
        }

        // Clear file after reading
        fs.SetLength(0);
        lastFilePosition = 0;
    }
    finally
    {
        fs.Close();
    }
}

    private void DisplayAmmo()
    {
        int currentAmmo = ammoSlot.GetCurrentAmmo(ammoType);
        ammoText.text = currentAmmo.ToString();
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        if (ammoSlot.GetCurrentAmmo(ammoType) > 0)
        {
            PlayMuzzleFlash();
            ProcessRaycast();
            ammoSlot.ReduceCurrentAmmo(ammoType);
        }
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }

    private void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    private void ProcessRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, range))
        {
            CreateHitImpact(hit);
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            if (target == null) return;
            target.TakeDamage(damage);
        }
        else
        {
            return;
        }
    }

    private void CreateHitImpact(RaycastHit hit)
    {
        GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, 0.1f);
    }
   
}