
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Infra
{
    public class SceneChanger : MonoBehaviour
    {
        public void ChangeToTutorial()
        {
            SceneParameters.IsTutorial = true;
            SceneParameters.PresentationFilePath = "C:/Clarity/TutorialWorld.cw";
            SceneManager.LoadScene("UnityClarity", LoadSceneMode.Single);
        }

        public void ChangeToClarity()
        {
            SceneParameters.IsTutorial = false;
            SceneParameters.PresentationFilePath = "C:/Clarity/UnityTestWorld.cw";
            SceneManager.LoadScene("UnityClarity", LoadSceneMode.Single);
        }
    }
}