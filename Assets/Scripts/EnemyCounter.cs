using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    // script to manage the number of enemies and call coresponding code that relies on the number
    private int enemies = 0; // count of enmies
    private TextMeshProUGUI text;
    [SerializeField] GameObject scene;
    SceneManager sm;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        sm = scene.GetComponent<SceneManager>();
    }

    public void AddEnemy() {
        ++enemies;
        text.text = $"enemies={enemies};";
    }

    public void RemoveEnemy() {
        --enemies;
        text.text = $"enemies={enemies};";
    }
}
