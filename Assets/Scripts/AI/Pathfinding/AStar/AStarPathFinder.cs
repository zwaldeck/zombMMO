using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AI.PathFinding.AStar
{
    public class AStartPathFinder : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;

        public void Start()
        {
            var tiles = tilemap.GetTilesBlock(tilemap.cellBounds);
            foreach (var tile in tiles)
            {
            }
        }
    }
}