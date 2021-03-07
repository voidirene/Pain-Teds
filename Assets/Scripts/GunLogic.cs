﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunLogic : MonoBehaviour //this script is used for GENERAL gun logic that applies to ALL guns, for gun properties specific to the players, use PlayerGunLogic
{
    //to get some variables on the gun's state
    private PlayerMovement pM;
    [HideInInspector]
    public PlayerGunLogic pGL;

    //the projectile prefabs, need to be set in the inspector
    public GameObject bullet;
    public GameObject puddle;
    public GameObject bomb;
    public GameObject pierce;

    //time related variables for shooting
    public float fireRate = 1; //the time the gun should be in 'cooldown' after a shot, can be set in inspector
    private float timeBetweenShots; //the time the gun has passed since the last shot

    [HideInInspector]
    public AudioSource source;

    void Start()
    {
        //get these components from the parent character
        pGL = GetComponentInParent<PlayerGunLogic>();
        pM = GetComponentInParent<PlayerMovement>();

        source = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        //for shooting
        if (pGL.fireShot == true && timeBetweenShots <= 0) //if player wants to shoot and gun is off cooldown
        {
            FireShot(); //shoot
            pGL.currentAmmo -= 1;
            source.Play();
        }
        else if (timeBetweenShots > 0) //else if it's on cooldown
        {
            timeBetweenShots -= Time.deltaTime; //decrease cooldown and reject the player's input to fire
            pGL.fireShot = false;
        }

        //for ability use
        if (pGL.fireAbility == true)
        {
            FireAbility();
            pGL.fireAbility = false;
        }
    }

    //method for shooting bullets
    void FireShot()
    {
        timeBetweenShots = fireRate; //put the gun on cooldown
        pGL.fireShot = false;

        //this is where the shooting magic happens

        if (pGL.selectedGun == "blue" || pGL.selectedGun == "red") //single bullet shots
        {
            GameObject spawnedBullet = Instantiate(bullet, gameObject.transform); //instantiate a bullet at the gun's position, and get it
            if (!pM.facingRight)
            {
                spawnedBullet.GetComponent<Rigidbody>().AddForce(-500, 0, 0); //shoot left
            }
            else if (pM.facingRight)
            {
                spawnedBullet.GetComponent<Rigidbody>().AddForce(500, 0, 0); //shoot right
            }
        }
        else if (pGL.selectedGun == "yellow") //spread pattern shots
        {
            for (int i = 0; i <= 2; i++) //loop 3 times
            {
                GameObject spawnedBullet = Instantiate(bullet, gameObject.transform); //instantiate a bullet at the gun's position, and get it

                switch (i)
                {
                    case 0:
                        if (!pM.facingRight)
                        {
                            spawnedBullet.GetComponent<Rigidbody>().AddForce(-500, 100, 0);
                        }
                        else if (pM.facingRight)
                        {
                            spawnedBullet.GetComponent<Rigidbody>().AddForce(500, 100, 0);
                        }
                        break;
                    case 1:
                        if (!pM.facingRight)
                        {
                            spawnedBullet.GetComponent<Rigidbody>().AddForce(-500, 0, 0);
                        }
                        else if (pM.facingRight)
                        {
                            spawnedBullet.GetComponent<Rigidbody>().AddForce(500, 0, 0);
                        }
                        break;
                    case 2:
                        if (!pM.facingRight)
                        {
                            spawnedBullet.GetComponent<Rigidbody>().AddForce(-500, -100, 0);
                        }
                        else if (pM.facingRight)
                        {
                            spawnedBullet.GetComponent<Rigidbody>().AddForce(500, -100, 0);
                        }
                        break;
                }
            }
        }
    }

    //method for firing any of the abilities
    void FireAbility()
    {
        if (pGL.selectedGun == "blue") //blue gun ability is the bomb
        {
            GameObject spawnedBomb = Instantiate(bomb, gameObject.transform);
            if (!pM.facingRight)
            {
                spawnedBomb.GetComponent<Rigidbody>().AddForce(-200, 300, 0);
            }
            else
            {
                spawnedBomb.GetComponent<Rigidbody>().AddForce(200, 300, 0);
            }

        }

        if (pGL.selectedGun == "red") //red gun ability is the piercing bullet
        {
            GameObject spawnedPierce = Instantiate(pierce, gameObject.transform);
            if (!pM.facingRight)
            {
                spawnedPierce.GetComponent<Rigidbody>().AddForce(-500, 0, 0); //shoot left
            }
            else if (pM.facingRight)
            {
                spawnedPierce.GetComponent<Rigidbody>().AddForce(500, 0, 0); //shoot right
            }
        }

        if (pGL.selectedGun == "yellow") //yellow gun ability is the aoe puddle
        {
            GameObject spawnedPuddle = Instantiate(puddle, gameObject.transform);
            if (!pM.facingRight)
            {
                spawnedPuddle.transform.position = new Vector3(spawnedPuddle.transform.position.x - 3, gameObject.transform.position.y - 0.5f, spawnedPuddle.transform.position.z);
            }
            else
            {
                spawnedPuddle.transform.position = new Vector3(spawnedPuddle.transform.position.x + 3, gameObject.transform.position.y - 0.5f, spawnedPuddle.transform.position.z);
            }
        }
    }
}