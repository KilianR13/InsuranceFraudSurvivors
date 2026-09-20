using UnityEngine;
using System.IO;

public class PermanentUpgradeManager : MonoBehaviour
{
    public static PermanentUpgradeManager Instance { get; private set; }

    public PermanentUpgradeData data = new PermanentUpgradeData();

    private string savePath;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "playerSaveData.json");

        Load();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    private void Load()
    {
        if (!File.Exists(savePath))
            return;

        string json = File.ReadAllText(savePath);
        data = JsonUtility.FromJson<PermanentUpgradeData>(json);
    }
}