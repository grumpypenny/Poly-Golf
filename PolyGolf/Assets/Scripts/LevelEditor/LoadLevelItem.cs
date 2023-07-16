using TMPro;
using UnityEngine;

public class LoadLevelItem : MonoBehaviour
{
    private LevelLoader levelLoader;
    private string filePath;

    public void SetLevelLoader(LevelLoader levelLoader)
    {
        this.levelLoader = levelLoader;
    }

    public void SetFilePath(string filePath)
    {
        this.filePath = filePath;
    }

    public void SetFileName(string fileName)
    {
        transform.GetComponentInChildren<TextMeshProUGUI>().text = fileName;
    }

    public void OnSelect()
    {
        if (levelLoader != null)
        {
            levelLoader.SetFilePath(filePath);
        }
    }
    
}
