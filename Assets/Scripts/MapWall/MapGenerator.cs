using UnityEngine;

public enum StageType
{
    Square,
    Rectangle,
    Circle
}

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

    //ステージタイプ
    public StageType currentStageType;

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

    void Update()
    {
        // 1キー → 正方形
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentStageType = StageType.Square;
            GenerateMap();
        }

        // 2キー → 長方形
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentStageType = StageType.Rectangle;
            GenerateMap();
        }

        // 3キー → 円形
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentStageType = StageType.Circle;
            GenerateMap();
        }
    }

    [ContextMenu("Generate Map")]
    void GenerateMap()
    {
        ClearMap();

        Vector3 playerSpawnPos;

        switch (currentStageType)
        {
            // 正方形
            case StageType.Square:
                width = 40;
                height = 40;
                break;

            // 長方形
            case StageType.Rectangle:
                width = 60;
                height = 30;
                break;

            // 円形
            case StageType.Circle:
                width = 60;
                height = 60;
                break;
        }

        // =========================
        // プレイヤースポーン位置決定
        // =========================

        if (currentStageType == StageType.Circle)
        {
            playerSpawnPos = new Vector3(9, -0.5f, 12);
        }
        else
        {
            playerSpawnPos = new Vector3(2, -0.5f, 2);
        }

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {

                // =========================
                // 円形ステージ判定
                // =========================
                if (currentStageType == StageType.Circle)
                {
                    float centerX = (width - 1) / 2f;
                    float centerZ = (height - 1) / 2f;

                    float radius = width / 2f - 1;

                    float dx = x - centerX;
                    float dz = z - centerZ;

                    float distance = Mathf.Sqrt(dx * dx + dz * dz);

                    // 円の外側は生成しない
                    if (distance > radius)
                    {
                        continue;
                    }
                }

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
                // 外周壁
                // =========================

                if (currentStageType == StageType.Circle)
                {
                    float centerX = (width - 1) / 2f;
                    float centerZ = (height - 1) / 2f;

                    float radius = width / 2f - 1;

                    float dx = x - centerX;
                    float dz = z - centerZ;

                    float distance = Mathf.Sqrt(dx * dx + dz * dz);

                    // 円周を壁にする
                    if (distance >= radius - 1)
                    {
                        Instantiate(
                            HardWall,
                            objPos,
                            Quaternion.identity,
                            transform
                        );

                        continue;
                    }
                }
                else
                {
                    // 四角・長方形
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
                }

                // =========================
                // プレイヤー周辺安全地帯
                // =========================
                int playerX = (int)playerSpawnPos.x;
                int playerZ = (int)playerSpawnPos.z;

                int safeRange;

                if (currentStageType == StageType.Circle)
                {
                    safeRange = 12;
                }
                else
                {
                    safeRange = 5;
                }

                float dxSafe = x - playerX;
                float dzSafe = z - playerZ;

                float safeDistance = Mathf.Sqrt(dxSafe * dxSafe + dzSafe * dzSafe);

                if (safeDistance <= safeRange + 1.5f)
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
            playerSpawnPos,
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
            Vector3 enemyPos;

            while (true)
            {
                enemyPos = new Vector3(
                    Random.Range(6, width - 3),
                    0,
                    Random.Range(6, height - 3)
                );

                // 円形じゃないならそのままOK
                if (currentStageType != StageType.Circle)
                {
                    break;
                }

                // 円形なら円内チェック
                float centerX = (width - 1) / 2f;
                float centerZ = (height - 1) / 2f;
                float radius = width / 2f - 1;

                float dx = enemyPos.x - centerX;
                float dz = enemyPos.z - centerZ;

                float distance = Mathf.Sqrt(dx * dx + dz * dz);

                // 円内ならOK
                if (distance < radius)
                {
                    break;
                }
            }

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