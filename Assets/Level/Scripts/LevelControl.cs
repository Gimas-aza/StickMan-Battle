using UnityEngine;
using Zenject;

namespace Assets.Level
{
    public class LevelControl : MonoBehaviour
    {
        [SerializeField] private Cell _prefabCell;

        private Cell[,] _createdСells = new Cell[4, 9];
        private Cell _cell;
        private int _levelNumber = 1;
        private GameEventsServise _gameEvents;
       
        [Inject]
        private void Construct(GameEventsServise gameEvents)
        {
            _gameEvents = gameEvents;
        }

        void Start()
        {
            SpawnLevel();
        }
        
        public void SwitchLevel()
        {
            SpawnLevel();
            _gameEvents.DisablePlayerMovement.Invoke(true);
            _gameEvents.RestartLevel.Invoke();
            _gameEvents.DisablePlayerMovement.Invoke(false);
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
    }
}