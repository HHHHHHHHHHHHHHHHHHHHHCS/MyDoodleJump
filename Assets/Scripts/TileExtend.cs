using System;

public enum TileType
{
    NormalTile = 0,
    BrokenTile,
    OnceTile,
    SpringTile,
    MoveHorTile,
    MoveVerTile,
}

[Serializable]
public struct NormalTile
{
    /// <summary>
    /// 最小的高度
    /// </summary>
    public float minHeight;
    /// <summary>
    /// 最大的高度
    /// </summary>
    public float maxHeight;
    /// <summary>
    /// 生成的权值
    /// </summary>
    public float weight;
}

[Serializable]
public struct BrokenTile
{
    /// <summary>
    /// 最小的高度
    /// </summary>
    public float minHeight;
    /// <summary>
    /// 最大的高度
    /// </summary>
    public float maxHeight;
    /// <summary>
    /// 生成的权值
    /// </summary>
    public float weight;
}

[Serializable]
public struct OnceTile 
{
    /// <summary>
    /// 最小的高度
    /// </summary>
    public float minHeight;
    /// <summary>
    /// 最大的高度
    /// </summary>
    public float maxHeight;
    /// <summary>
    /// 生成的权值
    /// </summary>
    public float weight;
}

[Serializable]
public struct SpringTile 
{
    /// <summary>
    /// 最小的高度
    /// </summary>
    public float minHeight;
    /// <summary>
    /// 最大的高度
    /// </summary>
    public float maxHeight;
    /// <summary>
    /// 生成的权值
    /// </summary>
    public float weight;
}

[Serializable]
public struct MoveHorTile 
{
    /// <summary>
    /// 最小的高度
    /// </summary>
    public float minHeight;
    /// <summary>
    /// 最大的高度
    /// </summary>
    public float maxHeight;
    /// <summary>
    /// 生成的权值
    /// </summary>
    public float weight;
    /// <summary>
    /// 移动的距离
    /// </summary>
    public float distance;
    /// <summary>
    /// 移动的速度
    /// </summary>
    public float speed;
}

[Serializable]
public struct MoveVerTile 
{
    /// <summary>
    /// 最小的高度
    /// </summary>
    public float minHeight;
    /// <summary>
    /// 最大的高度
    /// </summary>
    public float maxHeight;
    /// <summary>
    /// 生成的权值
    /// </summary>
    public float weight;
    /// <summary>
    /// 移动的距离
    /// </summary>
    public float distance;
    /// <summary>
    /// 移动的速度
    /// </summary>
    public float speed;
}