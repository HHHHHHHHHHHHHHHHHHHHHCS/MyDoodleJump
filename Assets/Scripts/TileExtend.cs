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
public class NormalTile
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
public class BrokenTile : NormalTile
{

}

[Serializable]
public class OnceTile : NormalTile
{

}

[Serializable]
public class SpringTile : NormalTile
{

}

[Serializable]
public class MoveHorTile : NormalTile
{
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
public class MoveVerTile : MoveHorTile
{

}