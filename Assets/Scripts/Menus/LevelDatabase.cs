using UnityEngine;

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "Levels/Level Database")]
public class LevelDatabase : ScriptableObject
{
    public LevelData[] levels;
}
