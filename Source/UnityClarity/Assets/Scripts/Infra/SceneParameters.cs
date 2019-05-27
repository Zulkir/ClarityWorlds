namespace Assets.Scripts.Infra
{
    public static class SceneParameters
    {
        public static bool IsTutorial { get; set; }
        public static string PresentationFilePath { get; set; }

        static SceneParameters()
        {
            PresentationFilePath = "C:/Clarity/UnityTestWorld.cw";
        }
    }
}