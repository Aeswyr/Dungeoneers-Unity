using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAnchor : MonoBehaviour
{
    [SerializeField] private GameObject hpBarPrefab;
    private GameObject hpBar;
    private ResourceController resource;
    [SerializeField] private StatController stats;

    // Start is called before the first frame update
    void Start()
    {
        hpBar = Instantiate(hpBarPrefab, GameMaster.Instance.GetCanvas().transform);
        hpBar.transform.position = transform.position;
        resource = hpBar.GetComponent<ResourceController>();
        stats.SetResource(resource); 
    }

    void FixedUpdate()
    {
        hpBar.transform.position = transform.position;  
    }
}
