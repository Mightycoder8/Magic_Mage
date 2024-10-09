using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamage
{
    public int Enemyhealth;

    public void hit()
    {
        Damage(Enemyhealth);
    }
    public void Damage(int damage)
    {
        Enemyhealth--;
        if(Enemyhealth<1)
        {
            Destroy(this.gameObject);
        }
    }
}
