using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // 壁Prefab
    public GameObject Wall;

    //プレイヤーPrefab
    public GameObject Player_Tank;

    //敵のPrefab
    public GameObject Enemy;

    // マップデータ
    string[] map =
{
    "11111111111111111111111111",
    "10000000000000000000000001",
    "10000300000000000000000001",
    "10000000000000000000000001",
    "10000000000000000000000001",
    "10000000000000000000010001",
    "10000000000000000000000001",
    "10000000000000000000000001",
    "10000000000000000000000001",
    "10000000000000020000000001",
    "10000000000000000000000001",
    "10000000000000000000000001",
    "10000000000000000000000001",
    "10000000000000000000000001",
    "10000000000000000000000001",
    "10000000000000000000000001",
    "10000000000000000000000001",
    "10000000000000000000000001",
    "10000000000000000000000001",
    "10000000000000000000030001",
    "10000000000000000000000001",
    "10000300000000000000000001",
    "10000000000000000000000001",
    "10000000000000000000000001",
    "11111111111111111111111111",
};



    void Start()
    {
        GenerateMap();
    }

    [ContextMenu("Generate Map")]

    void GenerateMap()
    {
        ClearMap();

        for (int z = 0; z < map.Length; z++)
        {
            for (int x = 0; x < map[z].Length; x++)
            {
                char chip = map[z][x];

                if (chip == '1')
                {
                    Instantiate(
                        Wall,
                        new Vector3(x, 1, z),
                        Quaternion.identity,
                        transform
                    );
                }

                if (chip == '2')
                {
                    Instantiate(
                        Player_Tank,
                        new Vector3(x, 1, z),
                        Quaternion.identity,
                        transform
                    );
                }

                if (chip == '3')
                {
                    Instantiate(
                        Enemy,
                        new Vector3(x, 1, z),
                        Quaternion.identity,
                        transform
                    );
                }
            }
        }
    }

    [ContextMenu("Clear Map")]

    void ClearMap()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}