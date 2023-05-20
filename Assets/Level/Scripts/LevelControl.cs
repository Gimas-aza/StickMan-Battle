using System.Collections.Generic;
using UnityEngine;
using Assets.Player;
using Zenject;

namespace Assets.Level
{
    public class LevelControl : MonoBehaviour
    {
        [SerializeField] private GameObject _prefabWall;
        [SerializeField] private int _minCountWalls;
        [SerializeField] private int _maxCountWalls;

        private readonly float[,] _gridForSpawnWalls = { { -10.135f, -3.385f, 3.365f, 10.115f }, { -21.415f, -7.165f, 7.085f, 21.335f } };
        private readonly float[] _scaleWalls = { 6, 8, 10 };
        private readonly List<float[]> _mapWalls = new();
        private float _positionX, _positionZ;
        private float _scaleX, _scaleZ;
        private GameEventsServise _gameEvents;

        [Inject]
        private void Construct(GameEventsServise gameEvents)
        {
            _gameEvents = gameEvents;

            _gameEvents.RestartLevel.AddListener(SetWallsOnRandomPositions);
        }

        public void Start()
        {
            SetWallsOnRandomPositions();
        }

        public void SwitchLevel()
        {
            _gameEvents.RestartLevel.Invoke();
            _gameEvents.DisablePlayerMovement.Invoke(false);
        }

        private void SetWallsOnRandomPositions()
        {
            ClearLevel();
            GeneratWallsOnRandomPositions();
        }

        private void ClearLevel()
        {
            Wall[] allWallsOnLevel = GetComponentsInChildren<Wall>();
            foreach (Wall wall in allWallsOnLevel)
                Destroy(wall);
        }

        private void GeneratWallsOnRandomPositions()
        {
            float countWalls = Random.Range(_minCountWalls, _maxCountWalls);

            for (int levelSide = 0; levelSide <= 2; levelSide += 2)
            {
                for (int i = 0; i < countWalls; i++)
                {
                    _positionX = _gridForSpawnWalls[0, Random.Range(0, 4)];
                    _positionZ = _gridForSpawnWalls[1, Random.Range(levelSide, levelSide + 2)];

                    _scaleX = _scaleWalls[Random.Range(0, _scaleWalls.Length)];
                    _scaleZ = _scaleWalls[Random.Range(0, _scaleWalls.Length)];

                    if (CheckingPositionsWallsOnSameness()) continue;

                    LimitScalingWall();

                    SpawnWall();
                }
            }
        }

        private bool CheckingPositionsWallsOnSameness()
        {
            float rotation = 0;
            if (_scaleX > _scaleZ) rotation = 1;

            foreach (float[] positionWall in _mapWalls)
                if (positionWall[0] == _positionX && positionWall[1] == _positionZ && positionWall[2] == rotation)
                    return true;

            _mapWalls.Add(new float[] { _positionX, _positionZ, rotation });

            return false;
        }

        private void LimitScalingWall()
        {
            if (_scaleX > _scaleZ)
            {
                if (_positionX < 0) _positionX += _scaleX / 2.5f;
                else _positionX -= _scaleX / 2.5f;

                if (_positionZ < 0) _positionZ += 7.125f;
                else _positionZ -= 7.125f;

                _scaleZ = 1;
            }
            else
            {
                if (_positionZ < 0) _positionZ += _scaleZ / 7;
                else _positionZ -= _scaleZ / 7;

                if (_positionX < 0) _positionX += 3.375f;
                else _positionX -= 3.375f;

                _scaleX = 1;
            }
        }

        private void SpawnWall()
        {
            GameObject spawnWall;

            spawnWall = Instantiate(_prefabWall, new Vector3(_positionX, 2.5f, _positionZ), Quaternion.identity);
            spawnWall.transform.localScale = new Vector3(_scaleX, 5, _scaleZ);
            spawnWall.transform.parent = transform;
        }
    }
}