using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperties : MonoBehaviour
{
    public int health;
    [SerializeField] GameObject experienceItemPrefab;
    private MeshRenderer meshRenderer;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void TakeDamage(int damage) {
        ChangeColorToRed();
        Invoke(nameof(ChangeColorToWhite), 0.025f);

        health -= damage;
        Debug.Log("Dusmana vurdun");
        if(health <= 0 ) {
            KillYourself();
        }
    }
    public void KillYourself() {
        SFXManager.PlaySoundFX(SoundType.KillEnemy);
        GameObject droppedItem = Instantiate(experienceItemPrefab, this.transform.position + new Vector3(0f,1f,0f), Quaternion.identity);

        Destroy(this.transform.parent.gameObject);
    }

    private IEnumerator ChangeColor(MeshRenderer mesh , Color color, float delay)
    {
        mesh.material.color = color;
        yield return new WaitForSeconds(delay);
    }
    private void ChangeColorToRed() {
        meshRenderer.material.color = Color.red;
    }
    private void ChangeColorToWhite()
    {
        meshRenderer.material.color = Color.white;
    }
}
