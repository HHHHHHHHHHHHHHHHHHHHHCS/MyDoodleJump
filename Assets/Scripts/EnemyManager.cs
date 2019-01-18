using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人类型
/// </summary>
public enum EnemyType
{
    None = 0,
    Enemy1,//不动
    Enemy2,//pingpong
    Enemy3//循环滚动
}

/// <summary>
/// 敌人管理器
/// </summary>
public class EnemyManager
{
    public List<EnemyBase> ShowEnemyList { get; private set; }
    public ObjectPool<EnemyBase> EnemyPool { get; private set; }

    private GameData gameData;

    public EnemyManager OnInit()
    {
        gameData = MainGameManager.GameData;
        ShowEnemyList = new List<EnemyBase>();
        EnemyPool = new ObjectPool<EnemyBase>(gameData.enemyPrefab, 0, gameData.enemyParent);
        return this;
    }

    public void OnUpdate()
    {
        for(int i = ShowEnemyList.Count-1;i>=0;i--)
        {
            ShowEnemyList[i].OnUpdate();
        }
    }

    /// <summary>
    /// 是否能创建敌人
    /// </summary>
    public EnemyType CheckNeedCreate()
    {
        var rd = Random.Range(0, gameData.AllEnemyWeight);
        if (rd <= 1)
        {
            return EnemyType.None;
        }

        rd -= 1;
        if (rd <= gameData.enemy1Weight)
        {
            return EnemyType.Enemy1;
        }

        rd -= gameData.enemy1Weight;
        if (rd <= gameData.enemy2Weight)
        {
            return EnemyType.Enemy2;
        }

        rd -= gameData.enemy2Weight;
        if (rd <= gameData.enemy3Weight)
        {
            return EnemyType.Enemy3;
        }

        return EnemyType.None;
    }

    /// <summary>
    /// 创建敌人
    /// </summary>
    public void SpawnEnemy(TileBase tile)
    {
        var type = CheckNeedCreate();
        if (type != EnemyType.None)
        {
            var temp = EnemyPool.Get();
            temp.Init(type, tile.transform.position.y);
            ShowEnemyList.Add(temp);
        }
    }

    /// <summary>
    /// 回收敌人
    /// </summary>
    public void RecoveryEnemy(EnemyBase enemy)
    {
        enemy.HideAll();
        ShowEnemyList.Remove(enemy);
        EnemyPool.Put(enemy);
    }
}