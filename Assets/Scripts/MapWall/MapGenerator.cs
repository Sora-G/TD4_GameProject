using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // 壊れない壁
    public GameObject HardWall;

    // 壊れる壁
    public GameObject BreakWall;

    // ダメージ床
    public GameObject DamageFloor;

    // 回転床
    public GameObject SpinFloor;

    // プレイヤー
    public GameObject Player_Tank;

    // 敵
    public GameObject Enemy;

    // 通常床
    public GameObject Floor;

    // マップサイズ
    public int width = 40;
    public int height = 40;

    // 各生成率
    [Range(0, 100)]
    public int hardWallPercent = 10;

    [Range(0, 100)]
    public int breakWallPercent = 15;

    [Range(0, 100)]
    public int damageFloorPercent = 5;

    [Range(0, 100)]
    public int spinFloorPercent = 5;

    void Start()
    {
        GenerateMap();
    }

    [ContextMenu("Generate Map")]
    void GenerateMap()
    {
        ClearMap();

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                // 通常床位置
                Vector3 floorPos = new Vector3(x, -0.5f, z);

                // ギミック床位置
                Vector3 gimmickFloorPos = new Vector3(x, -0.4f, z);

                // 壁位置
                Vector3 objPos = new Vector3(x, 0, z);

                // =========================
                // まず通常床を生成
                // =========================
                Instantiate(
                    Floor,
                    floorPos,
                    Quaternion.identity,
                    transform
                );

                // =========================
                // 外周は硬い壁
                // =========================
                if (x == 0 || z == 0 || x == width - 1 || z == height - 1)
                {
                    Instantiate(
                        HardWall,
                        objPos,
                        Quaternion.identity,
                        transform
                    );

                    continue;
                }

                // =========================
                // プレイヤー周辺安全地帯
                // =========================
                int playerX = 4;
                int playerZ = 4;
                int safeRange = 5;

                if (Mathf.Abs(x - playerX) <= safeRange &&
                    Mathf.Abs(z - playerZ) <= safeRange)
                {
                    continue;
                }

                int rand = Random.Range(0, 100);

                // 壊れない壁
                if (rand < hardWallPercent)
                {
                    Instantiate(
                        HardWall,
                        objPos,
                        Quaternion.identity,
                        transform
                    );
                }

                // 壊れる壁
                else if (rand < hardWallPercent + breakWallPercent)
                {
                    Instantiate(
                        BreakWall,
                        objPos,
                        Quaternion.identity,
                        transform
                    );
                }

                // ダメージ床
                else if (rand < hardWallPercent + breakWallPercent + damageFloorPercent)
                {
                    Instantiate(
                        DamageFloor,
                        gimmickFloorPos,
                        Quaternion.identity,
                        transform
                    );
                }

                // 回転床
                else if (rand < hardWallPercent + breakWallPercent + damageFloorPercent + spinFloorPercent)
                {
                    Instantiate(
                        SpinFloor,
                        gimmickFloorPos,
                        Quaternion.identity,
                        transform
                    );
                }
            }
        }

        // =========================
        // プレイヤー生成
        // =========================
        Instantiate(
            Player_Tank,
            new Vector3(2, -0.5f, 2),
            Quaternion.identity,
            transform
        );

        // =========================
        // 敵生成
        // =========================

        // 敵数をランダム決定（2～8体）
        int enemyCount = Random.Range(2, 9);

        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 enemyPos = new Vector3(
                Random.Range(6, width - 3),
                0,
                Random.Range(6, height - 3)
            );

            Instantiate(
                Enemy,
                enemyPos,
                Quaternion.identity,
                transform
            );
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