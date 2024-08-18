using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceItem : MonoBehaviour
{
    [SerializeField] int experience;
    private PlayerMovement playerMovement;
    private EnemyMovement[] enemyMovements;

    private void Start() {
        playerMovement = FindObjectOfType<PlayerMovement>();
        enemyMovements = FindObjectsOfType<EnemyMovement>();
    }

    private void OnTriggerEnter(Collider other)
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
        PlayerProperties.Instance.GainExperience(experience);
        PlayerProperties.Instance.StartGrow();
        Invoke(nameof(PlayerProperties.Instance.EndGrow), 1f);
    }
}
