using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private ControllerPlayerLink[] players;
    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectsOfType<ControllerPlayerLink>();
        foreach (var player in players)
            player.SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
