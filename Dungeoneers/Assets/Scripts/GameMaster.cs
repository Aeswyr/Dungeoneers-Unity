using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : Singleton<GameMaster>
{

    List<PlayerHandler> players = new List<PlayerHandler>();
    public void RegisterPlayer(PlayerHandler player) {
        players.Add(player);
    }

    public List<PlayerHandler> GetPlayers() {
        return players;
    }
}
