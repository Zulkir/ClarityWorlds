using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.AppModes;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.WorldTree;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Resources;

namespace Clarity.App.Worlds.DirtyHacks 
{
    public class DirtyHackService : IDirtyHackService
    {
        private readonly IWorldTreeService worldTreeService;
        private IAppModeService appModeService;
        private readonly IReadOnlyList<IStoryLayout> storyLayouts;
        private readonly IEmbeddedResources embeddedResources;
        private int layerToShow = 0;

        public DirtyHackService(IWorldTreeService worldTreeService, IAppModeService appModeService, 
            IReadOnlyList<IStoryLayout> storyLayouts, IEmbeddedResources embeddedResources)
        {
            this.worldTreeService = worldTreeService;
            this.appModeService = appModeService;
            this.storyLayouts = storyLayouts;
            this.embeddedResources = embeddedResources;
        }

        public bool TryHandleInput(IInteractionEvent args)
        {
            if (args is IKeyEvent kargs)
                return TryHandleKeyboard(kargs);
            return false;
        }

        private bool TryHandleKeyboard(IKeyEvent args)
        {
            if (appModeService.Mode != AppMode.Presentation)
                return false;

            if (args.ComplexEventType == KeyEventType.Up)
            {
                var scene = worldTreeService.World.Scenes.First();
                var cStory = worldTreeService.MainRoot.GetComponent<IStoryComponent>();
                if (args.EventKey == Key.J)
                {
                    cStory.StartLayoutType = storyLayouts.Select(x => x.Type).Single(x => x.ToString().Contains("NestedSpheres"));
                    cStory.ShowAux1 = true;
                    cStory.ShowAux2 = false;
                    scene.BackgroundColor = Color4.Black;
                    scene.Skybox = null;
                    return true;
                }

                if (args.EventKey == Key.K)
                {
                    cStory.StartLayoutType = storyLayouts.Select(x => x.Type).Single(x => x.ToString().Contains("Building"));
                    cStory.ShowAux1 = false;
                    cStory.ShowAux2 = false;
                    scene.BackgroundColor = Color4.Black;
                    scene.Skybox = null;
                    return true;
                }

                if (args.EventKey == Key.L)
                {
                    cStory.StartLayoutType = storyLayouts.Select(x => x.Type).Last(x => x.ToString().Contains("Sphere"));
                    cStory.ShowAux1 = false;
                    cStory.ShowAux2 = false;
                    scene.BackgroundColor = Color4.Black;
                    scene.Skybox = embeddedResources.Skybox("Skyboxes/stars.skybox");
                    return true;
                }

                if (args.EventKey == Key.O || args.EventKey == Key.P)
                {
                    if (args.EventKey == Key.P)
                    {
                        layerToShow++;
                        layerToShow %= 3;
                    }
                    else if (args.EventKey == Key.O)
                    {
                        layerToShow--;
                        layerToShow += 3;
                        layerToShow %= 3;
                    }

                    switch (layerToShow)
                    {
                        case 0:
                            cStory.HideMain = false;
                            cStory.ShowAux3 = false;
                            cStory.ShowAux4 = false;
                            break;
                        case 1:
                            cStory.HideMain = true;
                            cStory.ShowAux3 = true;
                            cStory.ShowAux4 = false;
                            break;
                        case 2:
                            cStory.HideMain = true;
                            cStory.ShowAux3 = false;
                            cStory.ShowAux4 = true;
                            break;
                    }

                    return true;
                }
            }
            return false;
        }
    }
}