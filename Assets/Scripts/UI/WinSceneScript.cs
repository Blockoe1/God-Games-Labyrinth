using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGL
{
    public class WinSceneScript : MonoBehaviour
    {
        public void BacktoMenu()
        {
            SceneManager.LoadScene("StartScene");
        }
    }
}