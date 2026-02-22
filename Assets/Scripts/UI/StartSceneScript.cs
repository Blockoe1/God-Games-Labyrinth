using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGL
{
    public class StartSceneScript : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene("MazeScene");
        }
    }
}
