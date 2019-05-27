using System;
using Assets.Scripts.Gui;
using Assets.Scripts.Infra;
using Assets.Scripts.Rendering;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.StoryGraph.Editing.Flowchart;
using Clarity.App.Worlds.Views;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.Numericals.Geometry;
using Clarity.Common.Numericals.OtherTuples;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Utilities;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Viewports;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using cna = Clarity.Common.Numericals.Algebra;

namespace Assets.Scripts.Interaction.Minimap
{
    public class MinimapVrNavigationMode : IMinimapVrNavigationMode
    {
        private UcRenderTextureImage rttImge;

        private static IntSize2 MinimapImageSize { get; } = new IntSize2(1024, 512);
        private const float MovementSpeed = 1.0f;
        private const float ZoomSpeed = 1.0f;

        private readonly IGlobalObjectService globalObjectService;
        private readonly IRenderService renderService;
        private readonly IInputService inputService;
        private readonly IRayHitIndex rayHitIndex;
        private readonly IStoryService storyService;
        private readonly IVrHeadPositionService headPositionService;

        public bool ZoomEnabled { get; set; }

        private GameObject minimap;
        private Text textAbove;

        private IViewport[] viewports;
        private GameObject laser;

        public MinimapVrNavigationMode(IGlobalObjectService globalObjectService, IRenderService renderService, IInputService inputService, 
            IRayHitIndex rayHitIndex, IStoryService storyService, IVrHeadPositionService headPositionService)
        {
            this.globalObjectService = globalObjectService;
            this.renderService = renderService;
            this.inputService = inputService;
            this.rayHitIndex = rayHitIndex;
            this.storyService = storyService;
            this.headPositionService = headPositionService;
        }

        public string UserFriendlyName => "Minimap";
        public bool IsEnabled { get; private set; }

        public void Initialize()
        {
            minimap = GameObject.CreatePrimitive(PrimitiveType.Quad);
            minimap.name = "Minimap";
            minimap.transform.SetParent(globalObjectService.VrLeftHand.transform, false);
            minimap.transform.localRotation = Quaternion.AngleAxis(75.0f, Vector3.right);
            minimap.transform.localPosition = new Vector3(0.0F, -0.1F, 0.25F);
            minimap.transform.localScale = new Vector3(0.8f, -0.4f, 1f);
            var material = new Material(Shader.Find("Unlit/Texture"));
            rttImge = (UcRenderTextureImage)renderService.CreateRenderTargetImage(MinimapImageSize);
            material.mainTexture = rttImge.Texture;
            minimap.transform.GetComponent<MeshRenderer>().material = material;

            var textCanvasObj = new GameObject("Minimap - Text Above");
            textCanvasObj.transform.SetParent(minimap.transform, false);
            //textAbove.transform.localRotation = Quaternion.AngleAxis(75.0f, Vector3.right);
            //textAbove.transform.localPosition = new Vector3(0.0F, 1F, 0.25F);
            //textAbove.transform.localScale
            var canvas = textCanvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            var textAboveTransform = textCanvasObj.GetComponent<RectTransform>();
            textAboveTransform.localPosition = new Vector3(0, -0.6f, 0);
            textAboveTransform.localScale = new Vector3(1.25f, -2.5f, 1);
            textAboveTransform.sizeDelta = new Vector2(100, 100);
            var canvasScaler = textCanvasObj.AddComponent<CanvasScaler>();
            canvasScaler.dynamicPixelsPerUnit = 100;
            var textObject = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/MinimapTextPrefab"));
            textObject.transform.SetParent(textCanvasObj.transform, false);
            textAbove = textObject.GetComponent<Text>();
            var textTransform = textAbove.GetComponent<RectTransform>();
            textAbove.text = "";

            var storyGraphView = AmFactory.Create<StoryGraphView>();
            var viewport = AmFactory.Create<Viewport>();
            viewport.Width = MinimapImageSize.Width;
            viewport.Height = MinimapImageSize.Height;
            viewport.View = storyGraphView;
            viewports = new IViewport[] { viewport };
            laser = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Laser"));
            SetEnabled(false);
        }

        public void SetEnabled(bool enable)
        {
            IsEnabled = enable;
            minimap.SetActive(enable);
            //minimap.transform.GetComponent<MeshCollider>().enabled = enabled;
            //minimap.transform.GetComponent<MeshRenderer>().enabled = enabled;
            laser.SetActive(enable);
        }

        public void Update(FrameTime frameTime)
        {
            if (!IsEnabled)
                return;
            HandleLaser();
            if (ZoomEnabled)
                HandleZoom();

            viewports[0].View.Update(frameTime);
            renderService.Render(rttImge, viewports, frameTime.TotalSeconds);
        }
        

        private void HandleZoom()
        {
            var dpad = SteamVR_Actions.default_DpadOrientation.GetAxis(SteamVR_Input_Sources.LeftHand);
            var zoomMode = SteamVR_Actions.default_MinimapZoomModifier.GetState(SteamVR_Input_Sources.LeftHand);
            if (zoomMode)
            {
                var internalDelta = dpad.y * 10000; //Mathf.RoundToInt(dpad.y * ZoomSpeed * Time.deltaTime);
                var delta = internalDelta > 0.2f
                    ? 1
                    : internalDelta < -0.2f
                        ? -1
                        : 0;
                if (delta != 0)
                {
                    var margs = new MouseEventArgs
                    {
                        ComplexEventType = MouseEventType.Wheel,
                        WheelDelta = delta,
                        Viewport = viewports[0],
                        State = new MouseState
                        {
                            Buttons = MouseButtons.None,
                            Position = new IntVector2(MinimapImageSize.Width / 2, MinimapImageSize.Height / 2),
                            HmgnPosition = new cna.Vector2(0.5f, 0.5f),
                            NormalizedPosition = new cna.Vector2(0.5f, 0.5f),
                        },
                    };
                    inputService.OnInputEvent(margs);
                }
            }
            else
            {
                var delta = new cna.Vector2(dpad.x, dpad.y) * MovementSpeed * Time.deltaTime;
                var pixelDelta = new IntVector2((int)Math.Round(delta.X * -MinimapImageSize.Width),
                    (int)Math.Round(delta.Y * MinimapImageSize.Height));
                if (pixelDelta != IntVector2.Zero)
                {
                    var margs = new MouseEventArgs
                    {
                        ComplexEventType = MouseEventType.Move,
                        Delta = pixelDelta,
                        Viewport = viewports[0],
                        State = new MouseState
                        {
                            Buttons = MouseButtons.Right,
                            Position = new IntVector2(MinimapImageSize.Width / 2, MinimapImageSize.Height / 2),
                            HmgnPosition = new cna.Vector2(0.5f, 0.5f),
                            NormalizedPosition = new cna.Vector2(0.5f, 0.5f),
                        },
                    };
                    inputService.OnInputEvent(margs);
                }
            }
        }

        private void HandleLaser()
        {
            var laserLocalPos = SteamVR_Actions.default_LaserPose.GetLocalPosition(SteamVR_Input_Sources.RightHand); // Relative to player
            var laserLocalRot = SteamVR_Actions.default_LaserPose.GetLocalRotation(SteamVR_Input_Sources.RightHand); // Relative to player
            var laserPos = globalObjectService.VrPlayer.transform.TransformPoint(laserLocalPos);
            var laserRot = globalObjectService.VrPlayer.transform.rotation * laserLocalRot;

            var laserDirection = laserRot * Vector3.forward;

            textAbove.text = "";

            var ray = new Ray(laserPos, laserDirection);
            if (!Physics.Raycast(ray, out var hitInfo))
            {
                var farMidPoint = laserPos + laserDirection * 50;
                laser.transform.position = farMidPoint;
                laser.transform.rotation = laserRot * Quaternion.Euler(90, 0, 0);
                laser.transform.localScale = new Vector3(0.005f, 50, 0.0005f);
                return;
            }

            var midPoint = (laserPos + hitInfo.point) / 2;
            laser.transform.position = midPoint;
            laser.transform.rotation = laserRot * Quaternion.Euler(90, 0, 0);
            laser.transform.localScale = new Vector3(0.005f, hitInfo.distance / 2, 0.005f);
            if (hitInfo.collider.gameObject != minimap)
                return;

            var hitTexCoord = hitInfo.textureCoord;
            var pixelPos = new IntVector2(Mathf.RoundToInt(hitTexCoord.x * MinimapImageSize.Width),
                Mathf.RoundToInt(hitTexCoord.y * MinimapImageSize.Height));
            var hits = rayHitIndex.CastRay(new RayHitInfo(viewports[0], viewports[0].View.Layers[0], pixelPos));
            var firstHit = hits.FirstOrNull();
            var cGizmo = firstHit?.Node.SearchComponent<StoryFlowchartNodeGizmoComponent>();
            if (cGizmo == null)
                return;

            var storyNode = cGizmo.ReferencedNode;
            if (!storyService.GlobalGraph.Children[storyNode.Id].IsEmpty())
                return;

            textAbove.text = storyNode.Name;
            if (!SteamVR_Actions.default_InteractUI.GetStateDown(SteamVR_Input_Sources.RightHand))
                return;

            inputService.OnInputEvent(new MouseEventArgs
            {
                ComplexEventType = MouseEventType.DoubleClick,
                EventButtons = MouseButtons.Left,
                Viewport = viewports[0],
                State = new MouseState
                {
                    Buttons = MouseButtons.Left,
                    Position = pixelPos,
                    HmgnPosition = Hmgnize(pixelPos.ToVector()),
                    NormalizedPosition = Normalize(pixelPos.ToVector()),
                }
            });

            headPositionService.ResetHeadPosition();
        }

        public void FixedUpdate()
        {
            
        }

        public void NavigateTo(ISceneNode node, IStoryPath storyPath, CameraProps initialCameraProps, float? initialFloorHeight,
            float? targetFloorHeight)
        {
            FixedTeleportVrNavigationMode.TeleportTo(globalObjectService.VrPlayerCarrier, node, targetFloorHeight);
        }

        private cna.Vector2 Normalize(cna.Vector2 v)
        {
            return new cna.Vector2(
                2f * (v.X - MinimapImageSize.Width / 2f) / MinimapImageSize.Height,
                1f - v.Y * 2f / MinimapImageSize.Height);
        }

        private cna.Vector2 Hmgnize(cna.Vector2 v)
        {
            return new cna.Vector2(
                v.X / MinimapImageSize.Width * 2f - 1f,
                1f - v.Y * 2f / MinimapImageSize.Height);
        }

        public void ShowHints(float seconds)
        {
            // TODO: at least think about implementing 
        }

        public void HideHints()
        {
            // TODO
        }
    }
}