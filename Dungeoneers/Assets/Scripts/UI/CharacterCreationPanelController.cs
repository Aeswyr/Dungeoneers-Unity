using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterCreationPanelController : MonoBehaviour
{
   [SerializeField] private StatDisplayController[] statDisplays;
   [SerializeField] private string[] statNames;
   [SerializeField] private GameObject statPointHolder;
   [SerializeField] private GameObject statPointPrefab;
   [SerializeField] private int statPoints;
   [SerializeField] private GameObject readyButton;
   private Stack<GameObject> points = new Stack<GameObject>();
    void Start() {
            for(int i = 0; i < 4; i++) {
                statDisplays[i].SetStat(1, statNames[i]);
            }
            for (int i = 0; i < statPoints; i++) {
                points.Push(Instantiate(statPointPrefab, statPointHolder.transform));
            }
    }
    public int GetStatPoints() {
        return statPoints;
    }

    public void AdjStatPoints(int adj) {
        statPoints += adj;
        if (adj > 0) {
            for(int i = 0; i < adj; i++) {
                points.Push(Instantiate(statPointPrefab, statPointHolder.transform));
            }
        } else if (adj < 0) {
            for(int i = 0; i < -1 * adj; i++) {
                Destroy(points.Pop());
            }
        }

        if (statPoints == 0) {
            readyButton.SetActive(true);
            statPointHolder.SetActive(false);
        }

        if (statPoints > 0 && !statPointHolder.activeInHierarchy) {
            readyButton.SetActive(false);
            statPointHolder.SetActive(true);
        }
    }

    public void OnReadyPressed() {
        SceneManager.LoadScene("PlayScene");
    }
}
