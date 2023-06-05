using System;
using UnityEngine;

namespace Assets.Level
{
    public class LevelGeneratorCell
    {
        public int X;
        public int Z;

        public bool WallLeft = true;
        public bool WallBottom = true;

        public bool Visited = false;
    }
}
