using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceItem : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerProperties playerProperties;
    private EnemyMovement[] enemyMovements;
    private EnemyProperties[] enemyProperties;
    [SerializeField] int amountToHeal = 10;
    [Range(1,2)] [SerializeField] private float yAxisGrowAmount;
    [Range(1,2)] [SerializeField] private float xzAxisGrowAmount;

    private void Start() {
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerProperties = FindObjectOfType<PlayerProperties>();
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            GiveHealth(amountToHeal);
            GrowPlayer();
            GrowPlayerStats();
            GrowBullets();
            GrowEnemyRanges();
            GrowShotSpeeds(); 
            GrowHand();
            GrowExperienceItem();
            SFXManager.PlaySound(SoundType.YerdenItemAlma);
            Destroy(this.gameObject);
        }
    }

    private void GrowExperienceItem()
    {
        enemyProperties = FindObjectsOfType<EnemyProperties>();
        foreach(EnemyProperties enemy in enemyProperties) {
            enemy.GrowDroppedItem();
        }

    }

    private void GrowHand()
    {
        GameObject.Find("HandHolder").transform.localScale = new Vector3(playerProperties.newScale.y,playerProperties.newScale.y,playerProperties.newScale.y);
    }

    private void GiveHealth(int amountToHeal)
    {
        playerProperties.GiveHealth(amountToHeal);
    }

    private void GrowEnemyRanges()
    {
        enemyMovements = FindObjectsOfType<EnemyMovement>();

        foreach(EnemyMovement enemy in enemyMovements) {
            enemy.GrowAttackRange();
        }
    }

    private void GrowPlayerStats()
    {
        playerMovement.GrowJumpSpeed();
        playerMovement.GrowDashSpeed();
        playerMovement.GrowSpeed();
        playerMovement.GrowHeight();
    }

    private void GrowBullets()
    {
        // PlayerMovement.Instance.GrowBulletscale(new Vector3(0.2f,0.2f,0.2f));
        playerMovement.GrowBulletscale();
    }

    private void GrowPlayer()
    {
        playerProperties.GainExperience(yAxisGrowAmount, xzAxisGrowAmount);
    }

    private void GrowShotSpeeds(){
        playerMovement.GrowShotSpeed();
        foreach (EnemyMovement enemy in enemyMovements)
        {
            enemy.GrowShotSpeed();
        }
    }
}
