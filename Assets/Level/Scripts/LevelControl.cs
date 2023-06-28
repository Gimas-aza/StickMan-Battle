using Assets.Player;
using UnityEngine;
using Zenject;

namespace Assets.Level
{
    public class LevelControl : MonoBehaviour
    {
        [SerializeField] private Cell _prefabCell;
        [SerializeField] private PlayerData[] _playersData;

        private Cell[,] _createdСells = new Cell[4, 9];
        private Cell _cell;
        private int _levelNumber = 1;
        private GlobalEventsSystem _gameEvents;
       
        [Inject]
        private void Construct(GlobalEventsSystem gameEvents)
        {
            _gameEvents = gameEvents;
        }

        void Start()
        {
            SwitchLevel();
        }
        
        public void SwitchLevel()
        {
            SpawnLevel();
            ReSpawnPlayers();
        }

        private void SpawnLevel()
        {
            var levelGeneration = new LevelGeneration();
            LevelGeneratorCell[,] cells = levelGeneration.Generate();

            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int z = 0; z < cells.GetLength(1); z++)
                {
                    if (_levelNumber == 1) 
                    {
                        _cell = Instantiate(_prefabCell, new Vector3(x * 7.28f, 0, z * 6.57f), Quaternion.identity);
                        _cell.transform.parent = transform;
                        _createdСells[x, z] = _cell;
                    }
                    else
                    {
                        _cell = _createdСells[x, z];
                    }

                    _cell.WallLeft.SetActive(cells[x, z].WallLeft);
                    _cell.WallBottom.SetActive(cells[x, z].WallBottom);
                }
            }
            _levelNumber++;
        }

        private void ReSpawnPlayers()
        {
            _gameEvents.onDisablePlayerMovement?.Invoke(true);
            ResetPosition();
            _gameEvents.onDisablePlayerMovement?.Invoke(false);
            _gameEvents.onHealthToMaximum?.Invoke();
        }

        private void ResetPosition()
        {
            foreach (PlayerData player in _playersData)
            {                
                Transform playerTransform = player.gameObject.transform;

                playerTransform.position = player.Spawn;
                playerTransform.eulerAngles = new Vector3(0, player.RotationY, 0);
            }
        }
    }
}