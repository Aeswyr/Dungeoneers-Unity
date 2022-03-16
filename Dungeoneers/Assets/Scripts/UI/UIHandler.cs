using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{

    public static UIHandler instance => FindObjectOfType<UIHandler>();
    private Stack<MenuType> menuStack = new Stack<MenuType>();
    [SerializeField] private GameObject canvas;
    [SerializeField] private MenuType currentMenu;
    [SerializeField] private GameObject currentMenuObj;
    [SerializeField] private GameObject[] menuPrefabs;
    [SerializeField] private GameObject eventSystem;

    public void NextMenu(MenuType type) {
        menuStack.Push(currentMenu);
        Destroy(currentMenuObj);
        currentMenu = type;
        currentMenuObj = Instantiate(FetchMenuFromType(currentMenu), canvas.transform);
    }

    public void Back() {
        if (!menuStack.TryPop(out var result))
            return;
        Destroy(currentMenuObj);
        currentMenu = result;
        currentMenuObj = Instantiate(FetchMenuFromType(currentMenu), canvas.transform);
    }

    private GameObject FetchMenuFromType(MenuType type) {
        if (type == MenuType.DEFAULT)
            return null;

        return (menuPrefabs[(int)type - 1]);
    }

    public void ToggleMasterEventSystem(bool val) {
        eventSystem.SetActive(val);
    }
}

public enum MenuType {
    DEFAULT, MAIN, LOCAL, ONLINE, OPTIONS,
}
