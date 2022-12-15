using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;
using UnityEngine.UIElements;


public class StartBtn : MonoBehaviour
{
    private void Awake()
    {
        //GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene(1); });

        Button startBtn = GetComponent<UIDocument>().rootVisualElement.Q<Button>("startBtn");

        startBtn.clicked += () => { SceneManager.LoadScene(1); };

    }
    
}
