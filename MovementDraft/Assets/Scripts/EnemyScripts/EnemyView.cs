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
        checkSelection();
        if(enemyModel.IsSelected == true)
        {
            print("Selected");
            enemyHealthUI.enabled = true;
            healthBarWorldPosition = transform.position + new Vector3(0.0f, 3.0f, 0.0f);
            healthBarScreenPosition = Camera.main.WorldToScreenPoint(healthBarWorldPosition);

            if (enemyHealthUI != null)
            {
                enemyHealthUI.transform.position = healthBarScreenPosition;
                enemyHealthUI.value = enemyModel.currentHealth / 100;
            }
        }
        else
        {
            print("No selection");
            enemyHealthUI.enabled = false;
        }
    }

    public void Death()
    {
        StartCoroutine("DeathEffect",0.5f);
        //GetComponent<NavMeshAgent>().enabled = false;
        //GetComponent<EnemyController>().enabled = false;
        //GetComponent<EnemyModel>().enabled = false;
        //GetComponent<EnemyView>().enabled = false;
        //GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        Destroy(gameObject);
        Destroy(enemyHealthUI);
    }

    private void checkSelection()
    {
        if (enemyModel.IsSelected == true)
        {
            gameObject.GetComponent<Light>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<Light>().enabled = false;
        }
    }

    IEnumerator DeathEffect()
    {
        GameObject explosion = (GameObject)Instantiate(zombieDeathExplosion, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3);
        Destroy(explosion);
    }
}
