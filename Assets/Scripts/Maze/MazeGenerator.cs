using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public class MazeGenerator : MonoBehaviour
    {
        public int height = 21;
        public int width = 21;

        public Vector2Int startPosition = new Vector2Int(1, 1);
        public GameObject wall;

        private int[,] _map;

        private readonly Vector2Int[] _directions =
        {
            new Vector2Int(0, 2),
            new Vector2Int(0, -2),
            new Vector2Int(-2, 0),
            new Vector2Int(2, 0),
        };

        private void Start()
        {
            if (GenerateMaze())
            {
                GenerateMazeObject();
            }
        }

        private void GenerateMazeObject()
        {
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (_map[x, y] != 1)
                        continue;

                    var wallPrefab = Instantiate(
                        wall,
                        new Vector2(x, y),
                        Quaternion.identity,
                        transform
                    );

                    wallPrefab.name = $"Wall_{x}_{y}";
                }
            }
        }

        public bool GenerateMaze()
        {
            if (width % 2 == 0 || height % 2 == 0)
            {
                Debug.LogError("미로의 너비와 높이는 홀수여야 합니다.");
                return false;
            }

            if (startPosition.x <= 0 ||
                startPosition.x >= width - 1 ||
                startPosition.y <= 0 ||
                startPosition.y >= height - 1)
            {
                Debug.LogError("시작 위치는 미로의 테두리 안쪽이어야 합니다.");
                return false;
            }

            if (startPosition.x % 2 == 0 || startPosition.y % 2 == 0)
            {
                Debug.LogError("시작 위치의 x와 y는 홀수여야 합니다.");
                return false;
            }

            _map = new int[width, height];

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    _map[x, y] = 1;
                }
            }

            var stack = new Stack<Vector2Int>();
            stack.Push(startPosition);

            _map[startPosition.x, startPosition.y] = 0;

            var random = new System.Random();

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                var neighbors = new List<Vector2Int>();

                foreach (var direction in _directions)
                {
                    var neighbor = current + direction;

                    if (neighbor.x > 0 &&
                        neighbor.x < width - 1 &&
                        neighbor.y > 0 &&
                        neighbor.y < height - 1 &&
                        _map[neighbor.x, neighbor.y] == 1)
                    {
                        neighbors.Add(neighbor);
                    }
                }

                if (neighbors.Count == 0)
                    continue;

                stack.Push(current);

                var chosen = neighbors[random.Next(neighbors.Count)];
                var between = (current + chosen) / 2;

                _map[chosen.x, chosen.y] = 0;
                _map[between.x, between.y] = 0;

                stack.Push(chosen);
            }

            return true;
        }
    }
}