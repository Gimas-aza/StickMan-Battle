using Assets.Items;
using Assets.Player;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets
{
    public class GameEventsServise : MonoBehaviour
    {
        [HideInInspector] public UnityEvent TriggerWall = new();
        [HideInInspector] public UnityEvent RestartLevel = new();
        [HideInInspector] public UnityEvent GameOverMenu = new();
        [HideInInspector] public UnityEvent HealthToMaximum = new();
        [HideInInspector] public UnityEvent<bool> DisablePlayerMovement = new();
    }
}
