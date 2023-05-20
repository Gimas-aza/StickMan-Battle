using UnityEngine;
using UnityEngine.Events;
using Assets.Player;
using Zenject;

namespace Assets.Level
{
    public class Wall : MonoBehaviour
    {
        private GameEventsServise _gameEvents;

        [Inject]
        private void Construct(GameEventsServise gameEvents)
        {
            _gameEvents = gameEvents;
        }

        // Переделать
        private void OnCollisionEnter(Collision collision)
        {
            GameObject gameObject = collision.gameObject;

            //if (gameObject.TryGetComponent(out MotionControl player))
            //{
            //    _gameEvents.TriggerWall.Invoke();
            //}
        }
    }
}