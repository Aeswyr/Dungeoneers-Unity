using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMenuController : MonoBehaviour
{
    [SerializeField] private GameObject panelHolder;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject panelPrefab;
    private int players;

    public void OnPlayerJoined() {
        Instantiate(panelPrefab, panelHolder.transform);
        GameObject infoPrev = infoPanel;
        infoPanel = Instantiate(infoPrev, panelHolder.transform);
        Destroy(infoPrev);
        players++;
        if (players >= 4)
            infoPanel.SetActive(false);
    }
}
