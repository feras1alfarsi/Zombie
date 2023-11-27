using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootingcontroller : MonoBehaviour
{
    public Animator animator;
    public Transform firepoint;
    public float fireRate = 0.1f;
    public float fireRange = 10f;
    private float nextFireTime = 0f;
    public bool isAuto = false;
    public int maxAmmo = 30;
    private int currentAmmo;
    public float reloadTime = 1.5f;
    private bool isReloading = false;
    public ParticleSystem muzzleFlash;
    public ParticleSystem bloodEffect;
    public int damagePershot = 10;

    [Header("Sound Effects")]
    public AudioSource soundAudioSource;
    public AudioClip shootingSoundClip;
    public AudioClip reloadSoundClip;
    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        if (isReloading)
            return;

        if(isAuto == true)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + 1f / fireRange;
                Shoot();
            }
            else
            {
                animator.SetBool("Shoot", false);
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + 1f / fireRange;
                Shoot();
            }
            else
            {
                animator.SetBool("Shoot", false);
            }
        }

        if(Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            Reload();
        }
        
    }

    private void Shoot()
    {
        if (currentAmmo > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(firepoint.position, firepoint.forward, out hit, fireRange))
            {
                Debug.Log(hit.transform.name);

                // apply damage to zombie
                ZoombieAI zombieAI = hit.collider.GetComponent<ZoombieAI>();
                if(zombieAI != null)
                {
                    zombieAI.TakeDamage(damagePershot);

                    // Play eefect particle system at teh hit point.
                    ParticleSystem blood = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(blood.gameObject, blood.main.duration);
                }
            }

            muzzleFlash.Play();
            animator.SetBool("Shoot", true);
            currentAmmo--;

            soundAudioSource.PlayOneShot(shootingSoundClip);
        }
        else
        {
            //Reload
            Reload();
        }
        
            
    }

    private void Reload()
    {
        if(!isReloading && currentAmmo < maxAmmo)
        {
            //reload anim
            animator.SetTrigger("Reload");
            isReloading = true;
            // play reload sound
            soundAudioSource.PlayOneShot(reloadSoundClip);
            Invoke("FinishReloading", reloadTime);

        }
    }

    private void FinishReloading()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
        // reset reload anim
        animator.ResetTrigger("Reload");
    }
}
