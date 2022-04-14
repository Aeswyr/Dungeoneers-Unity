using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatController : MonoBehaviour
{
    public int health;
    public int maxHealth;
    private ResourceController resource;
    public void OnHit() {
        health -= 1;
        resource.SetHP(health);
    }
    public void SetResource(ResourceController resource) {
        this.resource = resource;
        resource.SetMaxHP(maxHealth);
    }
}
