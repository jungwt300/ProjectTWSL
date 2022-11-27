using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss;
public class BossWeapon : MonoBehaviour
{
    private BossAttack bossAttack;

    private void Start()
    {
        bossAttack = GameObject.Find("boss").GetComponent<BossAttack>();
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.name == "player")
        {
            Debug.Log("PlayerHit");
            if(bossAttack.isHitboxOn == true)
            {
                bossAttack.OnHit();
            }
        }
    }
}
