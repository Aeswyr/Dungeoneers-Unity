using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : Singleton<GameMaster>
{

    [SerializeField] private GameObject hitboxPrefab;

    List<PlayerHandler> players = new List<PlayerHandler>();
    public void RegisterPlayer(PlayerHandler player) {
        players.Add(player);
    }

    public List<PlayerHandler> GetPlayers() {
        return players;
    }

    public GameObject CreateHitbox(int width, int height, GameObject owner = null, float duration = 1f/12) {
        hitboxPrefab.GetComponent<BoxCollider2D>().size = new Vector2(width, height);
        hitboxPrefab.GetComponent<Owner>().SetOwner(owner);
        hitboxPrefab.GetComponent<DestroyAfterDelay>().SetLifetime(duration);
        return Instantiate(hitboxPrefab);
    }
}
