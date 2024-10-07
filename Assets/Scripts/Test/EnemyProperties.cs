using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperties : MonoBehaviour
{
    public int health;
    [SerializeField] GameObject experienceItemPrefab;
    private float initialExperinceItemScale;
    private float experinceItemScale = 1;
    private MeshRenderer meshRenderer;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void Start() {
        initialExperinceItemScale = experinceItemScale;
        GrowDroppedItem();
    }

    public void TakeDamage(int damage) {
        ChangeColorToRed();
        Invoke(nameof(ChangeColorToWhite), 0.025f);

        health -= damage;
        if(health <= 0 ) {
            KillYourself();
        }
    }
    public void KillYourself() {
        SFXManager.PlaySound3D(SoundType.KillEnemy, this.transform.position);
        GameObject droppedItem = Instantiate(experienceItemPrefab, this.transform.position + new Vector3(0f,experinceItemScale,0f), Quaternion.identity);
        droppedItem.transform.localScale = Vector3.one * experinceItemScale;
        Debug.Log(droppedItem.transform.localScale);

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

    public void GrowDroppedItem() {
        if (FindObjectOfType<PlayerProperties>().newScale.y == 0f)
        {
            experinceItemScale = initialExperinceItemScale;
        }
        else
        {
            experinceItemScale = initialExperinceItemScale * FindObjectOfType<PlayerProperties>().newScale.y;
        }
    }
}
