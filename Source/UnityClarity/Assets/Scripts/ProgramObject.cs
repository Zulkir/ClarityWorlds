using Clarity.Core.AppCore.Infra;
using Clarity.Engine.Platforms;
using Clarity.Ext.StoryLayout.Building;
using UnityEngine;

namespace Assets.Scripts
{
    public class ProgramObject : MonoBehaviour
    {
        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            var extensions = new IExtension[]
            {
                new UnityExtension(this),
                new BuildingStoryLayoutExtension(),
            };
            var environment = new UcEnvironment(extensions);
            var appLifecycle = new AppLifecycle();
            appLifecycle.StartAndRun(environment);
        }
    }
}