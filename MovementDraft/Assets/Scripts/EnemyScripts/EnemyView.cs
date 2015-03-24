using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyView : MonoBehaviour 
{
    public GameObject zombieDeathExplosion;
    private Slider enemyHealthUI;

    private EnemyController enemyController;
    private EnemyModel enemyModel;

    private Vector3 healthBarWorldPosition;
    private Vector3 healthBarScreenPosition;

	// Use this for initialization
    void Start()
    {
        enemyHealthUI = GameObject.FindGameObjectWithTag("EnemyUI").GetComponent<Slider>();
        enemyController = GetComponent<EnemyController>();
        enemyModel = GetComponent<EnemyModel>();
    }
	
	// Update is called once per frame
	void Update () 
    {
        healthBarWorldPosition = transform.position + new Vector3(0.0f, 3.0f, 0.0f);
        healthBarScreenPosition = Camera.main.WorldToScreenPoint(healthBarWorldPosition);
        enemyHealthUI.transform.position = healthBarScreenPosition;

        enemyHealthUI.value = enemyModel.currentHealth/100;
    }

    public void Death()
    {
        StartCoroutine("DeathEffect",0.5f);
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<EnemyController>().enabled = false;
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;

        Destroy(enemyHealthUI);
    }

    IEnumerator DeathEffect()
    {
        GameObject explosion = (GameObject)Instantiate(zombieDeathExplosion, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3);
        Destroy(explosion);

    }
}
