using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public static MazeGenerator Instance;

    public Vector2Int mazeSize;
    private Vector2Int BlockSize => mazeSize / 2;

    private Block[,] _blocks;
    private bool[,] _existWalls;
    private DisjointSet _disjointSet;
    private readonly Dictionary<int, List<int>> _lastRowBlocks = new Dictionary<int, List<int>>();

    [SerializeField] private float delayCreateTime = 0.25f;
    [SerializeField] private bool isDelayCreate;
    [SerializeField] private bool isDrawGizmo;

    [SerializeField] GameObject wallPrefab;
    [SerializeField] GameObject[] obstaclePrefabs; // 장애물 프리팹 배열
    [SerializeField] GameObject[] itemPrefabs; // 아이템 프리팹 배열
    [SerializeField] private float obstacleSpawnProbability = 0.1f; // 장애물 생성 확률
    [SerializeField] private float itemSpawnProbability = 0.05f; // 아이템 생성 확률

    private void Awake()
    {
        mazeSize = GameManager.Instance.GetMazeSize();
        Debug.Log(mazeSize);
        var size = BlockSize;
        var disjointSetSize = BlockSize.x * BlockSize.y;

        _blocks = new Block[size.x, size.y];
        _existWalls = new bool[mazeSize.x, mazeSize.y];
        _disjointSet = new DisjointSet(disjointSetSize);
    }

    public void SetMazeSize(Vector2Int size)
    {
        mazeSize = size;
    }

    private void Start()
    {
        InitBlocks();

        if (isDelayCreate && isDrawGizmo)
        {
            StartCoroutine(DelayCreateMaze());
        }
        else
        {
            for (int y = 0; y < BlockSize.y - 1; y++)
            {
                RandomMergeRowBlocks(y);
                DropDownGroups(y);
            }

            OrganizeLastLine();
            MakeHoleInPath();

            if (!isDrawGizmo)
            {
                BuildWalls();
                SpawnObstaclesAndItems(); // 장애물과 아이템 생성
            }
        }
    }

    private void InitBlocks()
    {
        for (int x = 0; x < BlockSize.x; x++)
        {
            for (int y = 0; y < BlockSize.y; y++)
            {
                _blocks[x, y] = new Block();
            }
        }
    }

    private void RandomMergeRowBlocks(int row)
    {
        for (int x = 0; x < BlockSize.x - 1; x++)
        {
            var canMerge = Random.Range(0, 2) == 1;
            var currentBlockNumber = _blocks[x, row].BlockNumber;
            var nextBlockNumber = _blocks[x + 1, row].BlockNumber;

            if (canMerge && !_disjointSet.IsUnion(currentBlockNumber, nextBlockNumber))
            {
                _disjointSet.Merge(currentBlockNumber, nextBlockNumber);
                _blocks[x, row].OpenWay[(int)Direction.Right] = true;
            }
        }
    }

    private void DropDownGroups(int row)
    {
        _lastRowBlocks.Clear();

        for (int x = 0; x < BlockSize.x; x++)
        {
            var blockNumber = _blocks[x, row].BlockNumber;
            var parentNumber = _disjointSet.Find(_blocks[x, row].BlockNumber);

            if (!_lastRowBlocks.ContainsKey(parentNumber))
            {
                _lastRowBlocks.Add(parentNumber, new List<int>());
            }

            _lastRowBlocks[parentNumber].Add(blockNumber);
        }

        foreach (var group in _lastRowBlocks)
        {
            if (group.Value.Count == 0) continue;

            var randomDownCount = Random.Range(1, group.Value.Count);

            for (int i = 0; i < randomDownCount; i++)
            {
                var randomBlockIndex = Random.Range(0, group.Value.Count);

                var currentBlockNumber = group.Value[randomBlockIndex];
                var currentBlockPosition = Block.GetPosition(currentBlockNumber, BlockSize);

                var currentBlock = _blocks[currentBlockPosition.x, currentBlockPosition.y];
                var underBlock = _blocks[currentBlockPosition.x, currentBlockPosition.y + 1];

                _disjointSet.Merge(currentBlock.BlockNumber, underBlock.BlockNumber);
                currentBlock.OpenWay[(int)Direction.Down] = true;

                group.Value.RemoveAt(randomBlockIndex);
            }
        }
    }

    private void OrganizeLastLine()
    {
        var lastRow = BlockSize.y - 1;

        for (int x = 0; x < BlockSize.x - 1; x++)
        {
            var currentBlock = _blocks[x, lastRow];
            var nextBlock = _blocks[x + 1, lastRow];

            if (!_disjointSet.IsUnion(currentBlock.BlockNumber, nextBlock.BlockNumber))
            {
                currentBlock.OpenWay[(int)Direction.Right] = true;
            }
        }
    }

    private IEnumerator DelayCreateMaze()
    {
        for (int y = 0; y < BlockSize.y - 1; y++)
        {
            yield return StartCoroutine(DelayRandomMergeBlocks(y));
            yield return StartCoroutine(DelayDropDownGroups(y));

            MakeHoleInPath();

            yield return new WaitForSeconds(delayCreateTime);
        }

        yield return new WaitForSeconds(delayCreateTime);

        yield return StartCoroutine(DelayCleanUpLastLine());
        MakeHoleInPath();
    }

    private IEnumerator DelayRandomMergeBlocks(int row)
    {
        for (int x = 0; x < BlockSize.x - 1; x++)
        {
            var canMerge = Random.Range(0, 2) == 1;
            var currentBlockNumber = _blocks[x, row].BlockNumber;
            var nextBlockNumber = _blocks[x + 1, row].BlockNumber;

            if (canMerge && !_disjointSet.IsUnion(currentBlockNumber, nextBlockNumber))
            {
                _disjointSet.Merge(currentBlockNumber, nextBlockNumber);
                _blocks[x, row].OpenWay[(int)Direction.Right] = true;
            }

            MakeHoleInPath();

            yield return new WaitForSeconds(delayCreateTime);
        }
    }

    private IEnumerator DelayDropDownGroups(int row)
    {
        _lastRowBlocks.Clear();

        for (int x = 0; x < BlockSize.x; x++)
        {
            var blockNumber = _blocks[x, row].BlockNumber;
            var parentNumber = _disjointSet.Find(_blocks[x, row].BlockNumber);

            if (!_lastRowBlocks.ContainsKey(parentNumber))
            {
                _lastRowBlocks.Add(parentNumber, new List<int>());
            }

            _lastRowBlocks[parentNumber].Add(blockNumber);
        }

        foreach (var group in _lastRowBlocks)
        {
            if (group.Value.Count == 0) continue;

            var randomDownCount = Random.Range(1, group.Value.Count);

            for (int i = 0; i < randomDownCount; i++)
            {
                var randomBlockIndex = Random.Range(0, group.Value.Count);

                var currentBlockNumber = group.Value[randomBlockIndex];
                var currentBlockPosition = Block.GetPosition(currentBlockNumber, BlockSize);

                var currentBlock = _blocks[currentBlockPosition.x, currentBlockPosition.y];
                var underBlock = _blocks[currentBlockPosition.x, currentBlockPosition.y + 1];

                _disjointSet.Merge(currentBlock.BlockNumber, underBlock.BlockNumber);
                currentBlock.OpenWay[(int)Direction.Down] = true;

                group.Value.RemoveAt(randomBlockIndex);

                MakeHoleInPath();

                yield return new WaitForSeconds(delayCreateTime);
            }
        }
    }

    private IEnumerator DelayCleanUpLastLine()
    {
        var lastRow = BlockSize.y - 1;

        for (int x = 0; x < BlockSize.x - 1; x++)
        {
            var currentBlock = _blocks[x, lastRow];
            var nextBlock = _blocks[x + 1, lastRow];

            if (!_disjointSet.IsUnion(currentBlock.BlockNumber, nextBlock.BlockNumber))
            {
                currentBlock.OpenWay[(int)Direction.Right] = true;
            }

            MakeHoleInPath();

            yield return new WaitForSeconds(delayCreateTime);
        }
    }

    private void MakeHoleInPath()
    {
        for (int x = 0; x < BlockSize.x; x++)
        {
            for (int y = 0; y < BlockSize.y; y++)
            {
                var adjustPosition = new Vector2Int(x * 2 + 1, y * 2 + 1);
                _existWalls[adjustPosition.x, adjustPosition.y] = true;

                if (_blocks[x, y].OpenWay[(int)Direction.Down])
                    _existWalls[adjustPosition.x, adjustPosition.y + 1] = true;
                if (_blocks[x, y].OpenWay[(int)Direction.Right])
                    _existWalls[adjustPosition.x + 1, adjustPosition.y] = true;
            }
        }
    }

    private void BuildWalls()
    {
        for (int x = 0; x < mazeSize.x; x++)
        {
            for (int y = 0; y < mazeSize.y; y++)
            {
                if (_existWalls[x, y]) continue;

                var mazeHalfSize = new Vector3(mazeSize.x, 0, mazeSize.y) / 2;
                var wallPosition = new Vector3(x, 0.5f, y) - mazeHalfSize + transform.position;

                // wallPrefab이 null이 아닌지 확인 후 인스턴스화
                if (wallPrefab != null)
                {
                    Instantiate(wallPrefab, wallPosition, Quaternion.identity, transform);
                }
                else
                {
                    Debug.LogError("wallPrefab이 할당되지 않았습니다!");
                }
            }
        }
    }

    private void SpawnObstaclesAndItems()
    {
        for (int x = 0; x < mazeSize.x; x++)
        {
            for (int y = 0; y < mazeSize.y; y++)
            {
                // 벽이 있는 경우에는 스킵
                if (!_existWalls[x, y]) continue;

                var mazeHalfSize = new Vector3(mazeSize.x, 0, mazeSize.y) / 2;
                var spawnPosition = new Vector3(x, 0.5f, y) - mazeHalfSize + transform.position;

                // 장애물 생성
                if (Random.value < obstacleSpawnProbability)
                {
                    var obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
                    Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity, transform);
                }
                // 아이템 생성
                else if (Random.value < itemSpawnProbability)
                {
                    var itemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
                    Instantiate(itemPrefab, spawnPosition, Quaternion.identity, transform);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying && isDrawGizmo)
        {
            Gizmos.color = Color.red;

            for (int x = 0; x < mazeSize.x; x++)
            {
                for (int y = 0; y < mazeSize.y; y++)
                {
                    if (!_existWalls[x, y])
                    {
                        var mazeHalfSize = new Vector3(mazeSize.x, 0, mazeSize.y) / 2;
                        var wallPosition = new Vector3(x, 0.5f, y) - mazeHalfSize + transform.position;
                        Gizmos.DrawCube(wallPosition, Vector3.one);
                    }
                }
            }
        }
    }

    public class Block
    {
        public int BlockNumber { get; private set; }
        public readonly bool[] OpenWay = new bool[4];

        private static int s_increaseGroupNumber;

        public Block()
        {
            BlockNumber = s_increaseGroupNumber++;
        }

        public static Vector2Int GetPosition(int blockNumber, Vector2Int size)
        {
            return new Vector2Int(blockNumber / size.x, blockNumber % size.y);
        }

        public Vector2Int GetPosition(Vector2Int size) => GetPosition(BlockNumber, size);
        public int GetParentIndex(Vector2Int size) => BlockNumber * size.x + BlockNumber % size.y;
    }

    public class DisjointSet
    {
        public int ParentsSize { get; private set; }
        public readonly int[] Parents;

        public DisjointSet(int parentSize)
        {
            ParentsSize = parentSize;
            Parents = new int[parentSize];

            for (int i = 0; i < ParentsSize; i++)
                Parents[i] = i;
        }

        public int Find(int x)
        {
            if (x == Parents[x]) return x;

            return Parents[x] = Find(Parents[x]);
        }

        public void Merge(int a, int b)
        {
            a = Find(a);
            b = Find(b);

            if (a == b) return;

            if (a > b) Parents[a] = b;
            else Parents[b] = a;
        }

        public bool IsUnion(int a, int b)
        {
            a = Find(a);
            b = Find(b);

            return a == b;
        }
    }

    public enum Direction : sbyte
    {
        Down = 0,
        Right,
    }
}