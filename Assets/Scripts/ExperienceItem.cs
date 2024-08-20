using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceItem : MonoBehaviour
{
    [SerializeField] int experience;
    private PlayerMovement playerMovement;
    private PlayerProperties playerProperties;
    private EnemyMovement[] enemyMovements;

    private void Start() {
        playerMovement = FindObjectOfType<PlayerMovement>();
        enemyMovements = FindObjectsOfType<EnemyMovement>();
        playerProperties = FindObjectOfType<PlayerProperties>();
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            GrowPlayer();
            GrowPlayerStats();
            // GrowBullets(); 
            GrowEnemyRanges();
            Destroy(this.gameObject);
        }
    }

    private void GrowEnemyRanges()
    {
        foreach(EnemyMovement enemy in enemyMovements) {
            enemy.GrowAttackRange();
        }
    }

    private void GrowPlayerStats()
    {
        playerMovement.GrowJumpSpeed();
        playerMovement.GrowDashSpeed();
        playerMovement.GrowSpeed();
    }

    private void GrowBullets()
    {
        // PlayerMovement.Instance.GrowBulletscale(new Vector3(0.2f,0.2f,0.2f));
    }

    private void GrowPlayer()
    {
        playerProperties.GainExperience(experience);
    }
}
