using Assets.Items;
using Assets.Player;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets
{
    public class GlobalEventsSystem : MonoBehaviour
    {
        [HideInInspector] public UnityEvent onGameOverMenu = new();
        [HideInInspector] public UnityEvent onHealthToMaximum = new();
        [HideInInspector] public UnityEvent<bool> onDisablePlayerMovement = new();
    }
}
