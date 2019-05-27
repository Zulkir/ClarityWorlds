using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Transport.Prototype.Queries.Visual;
using Clarity.App.Transport.Prototype.Runtime;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Media.Models;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Resources;
using Clarity.Engine.Utilities;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Effects;
using Clarity.Engine.Visualization.Elements.Materials;
using Clarity.Engine.Visualization.Elements.RenderStates;

namespace Clarity.App.Transport.Prototype.Temp
{
    public class SiteTrafficVisualQuery : IVisualQuery
    {
        private struct Segment
        {
            public Vector3 P1;
            public Vector3 P2;
            public Color4 Color;
        }

        public abstract class Component : SceneNodeComponentBase<Component>, IVisualComponent
        {
            public IEnumerable<IVisualElement> VisualElements { get; set; }
            public IEnumerable<IVisualElement> GetVisualElements() => VisualElements ?? EmptyArrays<IVisualElement>.Array;
            public IEnumerable<IVisualEffect> GetVisualEffects() => EmptyArrays<IVisualEffect>.Array;
        }

        private const int NumSegments = 64;

        private readonly IDataRetrievalService dataRetrievalService;
        private readonly IPlaybackService playbackService;
        private readonly IModel3D lineModel;
        private readonly Component component;
        private Segment[] segments;
        private ModelVisualElement<SiteTrafficVisualQuery>[] visualElems;
        private int numElements;
        private double timestamp;

        public ISceneNode SceneNode { get; }

        public SiteTrafficVisualQuery(IDataRetrievalService dataRetrievalService, IPlaybackService playbackService, IEmbeddedResources embeddedResources)
        {
            this.dataRetrievalService = dataRetrievalService;
            this.playbackService = playbackService;
            lineModel = embeddedResources.LineModel();
            visualElems = EmptyArrays<ModelVisualElement<SiteTrafficVisualQuery>>.Array;
            SceneNode = AmFactory.Create<SceneNode>();
            component = AmFactory.Create<Component>();
            component.VisualElements = visualElems;
            SceneNode.Components.Add(component);
        }

        public bool CheckIsValid(out string errorMessage)
        {
            errorMessage = null;
            return true;
        }

        public void OnTimestampChanged(double timestamp)
        {
            this.timestamp = timestamp;
        }

        public void Update(FrameTime frameTime)
        {
            if (!dataRetrievalService.TryGetTable("SitePositions", out var sitePositionsTable) ||
                !sitePositionsTable.TryGetField("X", out var xField) ||
                !sitePositionsTable.TryGetField("Y", out var yField) ||
                !dataRetrievalService.TryGetTable("SiteTraffic", out var siteTrafficTable) ||
                !siteTrafficTable.TryGetField("Traffic", out var trafficField))
            {
                numElements = 0;
                return;
            }

            var delay = playbackService.Speed;
            var startTimestamp = timestamp - delay;

            var sitePositionsState = dataRetrievalService
                .GetLogForTable(sitePositionsTable)
                .GetStateAt(timestamp)
                .GetTableState(sitePositionsTable);
            var siteTrafficState = dataRetrievalService
                .GetLogForTable(siteTrafficTable)
                .GetStateAt(startTimestamp)
                .GetTableState(siteTrafficTable);
            
            var trafficByKey = new Dictionary<int, List<(double Timestamp, DPolyCubic Val)>>();
            foreach (var key in siteTrafficState.Keys)
            {
                var (from, to) = SiteTrafficDataQuery.ToFromToPair(key);
                if (!sitePositionsState.HasRow(from) || !sitePositionsState.HasRow(to))
                    continue;
                var list = trafficByKey.GetOrAdd(key, x => new List<(double Timestamp, DPolyCubic Val)>());
                list.Add((startTimestamp, siteTrafficState.GetValue<DPolyCubic>(key, trafficField.Index)));
            }
            foreach (var readState in dataRetrievalService
                .GetLogForTable(siteTrafficTable)
                .Read(startTimestamp, timestamp))
            {
                if (readState.Entry.Table != siteTrafficTable)
                    continue;
                var key = readState.Entry.RowKey;
                var (from, to) = SiteTrafficDataQuery.ToFromToPair(key);
                if (!sitePositionsState.HasRow(from) || !sitePositionsState.HasRow(to))
                    continue;
                var list = trafficByKey.GetOrAdd(key, x => new List<(double Timestamp, DPolyCubic Val)>());
                var entryTimestamp = readState.Entry.Timestamp;
                var tableState = readState.State.GetTableState(siteTrafficTable);
                list.Add((entryTimestamp, tableState.GetValue<DPolyCubic>(key, trafficField.Index)));
            }

            numElements = trafficByKey.Keys.Count * NumSegments;
            if (visualElems.Length < numElements)
            {
                segments = new Segment[numElements];
                visualElems = Enumerable.Range(0, numElements)
                    .Select(i => ModelVisualElement.New(this)
                        .SetModel(lineModel)
                        .SetMaterial(StandardMaterial.New(this)
                            .SetIgnoreLighting(true)
                            .SetDiffuseColor(s => s.segments[i].Color))
                        .SetRenderState(StandardRenderState.New()
                            .SetLineWidth(3))
                        .SetTransform(s =>
                        {
                            var segment = s.segments[i];
                            var point = segment.P1;
                            var toward = segment.P2 - segment.P1;
                            return new Transform(toward.Length(), Quaternion.RotationToFrame(toward, Vector3.UnitY), point);
                        }))
                    .ToArray();
            }
            component.VisualElements = visualElems.Take(numElements);

            var index = 0;
            foreach (var key in trafficByKey.Keys)
            {
                var (from, to) = SiteTrafficDataQuery.ToFromToPair(key);
                var fromPos = new Vector3(
                    sitePositionsState.GetValue<float>(from, xField.Index),
                    0,
                    sitePositionsState.GetValue<float>(from, yField.Index));
                var toPos = new Vector3(
                    sitePositionsState.GetValue<float>(to, xField.Index),
                    0,
                    sitePositionsState.GetValue<float>(to, yField.Index));
                var adjustment = Vector3.Cross(toPos - fromPos, Vector3.UnitY).Normalize() * 0.5f;
                var fromPosAdjusted = fromPos + adjustment;
                var toPosAdjusted = toPos + adjustment;
                var points = Enumerable.Range(0, NumSegments + 1)
                    .Select(x => (float)x / NumSegments)
                    .Select(x =>
                    {
                        var a = x;
                        var p = Vector3.Lerp(fromPosAdjusted, toPosAdjusted, x);
                        p.Y = MathHelper.Sin(x * MathHelper.Pi);
                        return (p, a);
                    });

                foreach (var pair in points.SequentialPairs())
                {
                    var a = pair.First.a;
                    var t = timestamp - delay * a;
                    var polys = trafficByKey[key];
                    // todo: .WhereNext()

                    var polyIndex = 0;// Enumerable.Range(0, polys.Length - 1).First(x => polys[x + 1].Timestamp >= t);
                    while (polyIndex + 1 < polys.Count && polys[polyIndex + 1].Timestamp < t)
                        polyIndex++;
                    var poly = polys[polyIndex].Val;
                    var traffic = poly.ValueAt(t);
                    var coeff = 0.8f + 0.2f * MathHelper.Sin((float)(t / playbackService.Speed * 5 * MathHelper.TwoPi));
                    segments[index++] = new Segment
                    {
                        P1 = pair.First.p,
                        P2 = pair.Second.p,
                        Color = IntensityToColor((float)traffic, coeff)
                    };
                }
            }
        }

        private static Color4 IntensityToColor(float intensity, float coeff)
        {
            var c0 = new Color4(0, 0, 0, 0f);
            var b1 = 0.5f;
            var c1 = new Color4(0, 0, 1, 1f);
            var b2 = 1.0f;
            var c2 = new Color4(0, 1, 0, 1f);
            var b3 = 2.0f;
            var c3 = new Color4(1, 1, 0, 1f);
            var b4 = 3.0f;
            var c4 = new Color4(1, 0, 0, 1f);

            intensity = (float)Math.Log(intensity * 4 + 1, 2);

            if (intensity < b1)
                return Color4.Lerp(c0, c1, intensity / b1) * coeff;
            if (intensity < b2)
                return Color4.Lerp(c1, c2, (intensity - b1) / (b2 - b1)) * coeff;
            if (intensity < b3)
                return Color4.Lerp(c2, c3, (intensity - b2) / (b3 - b2)) * coeff;
            if (intensity < b4)
                return Color4.Lerp(c3, c4, (intensity - b3) / (b4 - b3)) * coeff;
            return c4 * coeff;
        }

        public void OnAttached() {  }
        public void OnTableLayoutChanged() {  }
        public void OnDataLogUpdated(IDataLogUpdatedEvent evnt) {  }
    }
}