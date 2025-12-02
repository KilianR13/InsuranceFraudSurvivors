using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Levels/Level Data")]
public class LevelData : ScriptableObject
{
    public string sceneName;      // nombre exacto de la escena en Build Settings
    public Sprite previewImage;   // imagen de previsualización
    public string levelDisplayName;
}
