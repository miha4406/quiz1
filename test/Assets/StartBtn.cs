using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartBtn : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene(1); });

    }
    
}
