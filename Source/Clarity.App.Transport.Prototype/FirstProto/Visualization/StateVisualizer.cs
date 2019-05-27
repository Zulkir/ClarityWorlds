using System.Collections.Generic;
using Clarity.App.Transport.Prototype.FirstProto.Simulation;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Utilities;

namespace Clarity.App.Transport.Prototype.FirstProto.Visualization
{
    public class StateVisualizer : IStateVisualizer
    {
        private readonly ISceneNode rootNode;
        private readonly List<string> sites;
        private readonly Dictionary<string, Vector3> sitePositions;
        private readonly Stack<ISceneNode> siteNodePool;
        private readonly Stack<ISceneNode> packageNodePool;

        public ISceneNode RootNode => rootNode;

        public StateVisualizer()
        {
            sites = new List<string>();
            sitePositions = new Dictionary<string, Vector3>();
            siteNodePool = new Stack<ISceneNode>();
            packageNodePool = new Stack<ISceneNode>();
            rootNode = AmFactory.Create<SceneNode>();
        }

        public void UpdateToState(ISimState simState)
        {
            foreach (var child in rootNode.ChildNodes)
                if (child.HasComponent<SiteComponent>())
                    siteNodePool.Push(child);
                else if (child.HasComponent<PackageComponent>())
                    packageNodePool.Push(child);
            rootNode.ChildNodes.Clear();

            foreach (var simStateSite in simState.GetSites())
                if (!sites.Contains(simStateSite))
                {
                    sites.Add(simStateSite);
                    RecalculateSitePositions();
                }

            foreach (var site in sites)
            {
                var siteNode = GetSiteNode(site);
                siteNode.Transform = Transform.Translation(sitePositions[site]);
                rootNode.ChildNodes.Add(siteNode);
            }

            foreach (var package in simState.GetPackages())
            {
                var packageNode = GetPackageNode(package);
                packageNode.Transform = Transform.Translation(GetPackagePos(simState, package));
                rootNode.ChildNodes.Add(packageNode);
            }
        }

        private void RecalculateSitePositions()
        {
            sitePositions.Clear();
            for (var i = 0; i < sites.Count; i++)
            {
                var site = sites[i];
                var a = MathHelper.TwoPi * i / sites.Count;
                var position = 30f * new Vector3(MathHelper.Cos(a), 0, MathHelper.Sin(a));
                sitePositions.Add(site, position);
            }
        }

        private ISceneNode GetSiteNode(string site)
        {
            var node = siteNodePool.Count > 0 ? siteNodePool.Pop() : SiteComponent.CreateNode();
            node.GetComponent<SiteComponent>().Site = site;
            return node;
        }

        private ISceneNode GetPackageNode(SimPackage package)
        {
            var node = packageNodePool.Count > 0 ? packageNodePool.Pop() : PackageComponent.CreateNode();
            node.GetComponent<PackageComponent>().Package = package;
            return node;
        }

        private Vector3 GetPackagePos(ISimState simState, SimPackage package)
        {
            var fromPos = sitePositions[package.FromSite];
            var toPos = sitePositions[package.ToSite];
            var amount = (simState.Timestamp - package.DepartureTime) / (package.ArrivalTime - package.DepartureTime);
            return Vector3.Lerp(fromPos, toPos, (float)amount);
        }
    }
}