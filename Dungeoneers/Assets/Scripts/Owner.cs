using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owner : MonoBehaviour
{
    private GameObject owner;
    public bool IsOwner(GameObject obj) {
        return obj == owner;
    }

    public void SetOwner(GameObject obj) {
        owner = obj;
    }
}
