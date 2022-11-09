using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class ObjectSpawnController : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    public void SpawnPlayer(NetworkRunner runner, PlayerRef player)
    {
        Vector3 spawnPosition = new Vector3(0, 0, 0);
        Quaternion spawnRotation = Quaternion.identity;
        NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, spawnRotation, player);
        _spawnedCharacters.Add(player, networkPlayerObject);
        Debug.Log("Spawn player");
    }
}
