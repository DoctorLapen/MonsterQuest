using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonsterQuest
{
    public class BackButtonHandler : MonoBehaviour
    {
        public void ReturnToPreviousScene()
        {
            SceneManager.LoadScene(PreviousSceneHolder.SceneName);
        }


    }
}