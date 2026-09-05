using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Levels/Level Data")]
public class LevelData : ScriptableObject
{
    public string sceneName;            // Exact name of the scene in "Build Settings". Otherwise it will explode.
    public Sprite previewImage;         // Preview image of the level. Decoration purposes.
    public string levelDisplayName;     // Name of the level. Doesn't have to match the scene name.
}
