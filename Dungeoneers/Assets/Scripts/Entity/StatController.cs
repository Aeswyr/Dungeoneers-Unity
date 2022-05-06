using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatController : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public int armor;
    private ResourceController resource;
    public void OnHit() {
        if (armor > 0) {
            armor -= 1;
            health -= 2;
            resource.SetArmor(armor);
        } else {
            health -= 8;
        }
        resource.SetHP(health);
    }
    public void SetResource(ResourceController resource) {
        this.resource = resource;
        resource.SetMaxHP(maxHealth);
        resource.SetArmor(armor);
    }
}
