using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlayerLink : MonoBehaviour
{
    [SerializeField] private InputHandler input;
    [SerializeField] private GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SpawnPlayer() {
        GameObject player = Instantiate(playerPrefab);
        player.GetComponent<PlayerHandler>().LinkInputs(input);
    }
}
