using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Level
{    
    public class Cell : MonoBehaviour
    {
        [SerializeField] private GameObject _wallLeft;
        [SerializeField] private GameObject _wallBottom;

        public GameObject WallLeft => _wallLeft;
        public GameObject WallBottom => _wallBottom;
    }
}
