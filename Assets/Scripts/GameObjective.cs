using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjective : MonoBehaviour, IDamageable
{
    [Range(0, 100)] [SerializeField] int HP;
    [SerializeField] Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        StartCoroutine(flashColor());
        if (HP <= 0)
        {
            Destroy(gameObject);
            gameManager.instance.totalObjectives--;
            gameManager.instance.updateObjectiveUI();
        }
    }

    IEnumerator flashColor()
    {
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = Color.white;
    }

}
