using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reputation
{
    [SerializeField]
    private bool targetable = true;

    public enum Team //move this later to singleton gamerules object
    {
        none,
        Side1,
        SideA,
        Neutral,
        Pirate
    }
    [SerializeField]
    private Team team;

    public Team Side
    {
        get { return team; }
        set { team = value;
              //update indicators
        }
    }
}
