using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class CharacterMenuController : MonoBehaviour
{
    [SerializeField] private GameObject panelHolder;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject panelPrefab;
    private int players;

    public void OnPlayerJoined(PlayerInput input) {
        UIHandler.instance.ToggleMasterEventSystem(false);
        
        CharacterCreationPanelController newPanel = Instantiate(panelPrefab, panelHolder.transform).GetComponent<CharacterCreationPanelController>();
        GameObject infoPrev = infoPanel;
        infoPanel = Instantiate(infoPrev, panelHolder.transform);

        MultiplayerEventSystem mSystem = input.gameObject.GetComponent<MultiplayerEventSystem>();
        mSystem.playerRoot = newPanel.gameObject;
        mSystem.SetSelectedGameObject(newPanel.SelectedRoot);
        input.uiInputModule = input.gameObject.GetComponent<InputSystemUIInputModule>();

        Destroy(infoPrev);
        players++;
        if (players >= 4)
            infoPanel.SetActive(false);
    }
}
