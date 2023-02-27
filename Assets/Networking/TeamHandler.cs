using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TeamHandler : MonoBehaviour
{
    public static TeamHandler instance;

    [SerializeField] private List<PlayerNetwork> teamA = new List<PlayerNetwork>(); //Serialized for testing
    [SerializeField] private List<PlayerNetwork> teamB = new List<PlayerNetwork>(); //Serialized for testing


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    //[Server]
    public void AddPlayer(PlayerNetwork player)
    {
        //Set team
        if (teamA.Count <= teamB.Count)
        {
            player.SetTeam(0);
            player.SetColor(Color.green);
            teamA.Add(player);
        }
        else
        {
            player.SetTeam(1);
            player.SetColor(Color.red);
            teamB.Add(player);
        }
    }

    //[Server]
    public void RemovePlayer(PlayerNetwork player)
    {
        if (player.GetTeam() == 0)
        {
            teamA.Remove(player);
        }
        else if (player.GetTeam() == 1)
        {
            teamB.Remove(player);
        }
        else
        {
            Debug.LogError("Player was removed with a team other than 0 or 1");
        }
    }

    public void ChangeTeam(PlayerNetwork player)
    {
        if (player.GetTeam() == 0)
        {
            player.SetTeam(1);
            player.SetColor(Color.red);
            teamA.Remove(player);
            teamB.Add(player);
        }
        else if (player.GetTeam() == 1)
        {
            player.SetTeam(0);
            player.SetColor(Color.green);
            teamB.Remove(player);
            teamA.Add(player);
        }
        else
        {
            Debug.Log("Player with team other than 0 or 1 tried to change team");
        }
    }
}