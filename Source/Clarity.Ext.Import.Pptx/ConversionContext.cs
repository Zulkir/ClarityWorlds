using System.Collections.Generic;
using Clarity.App.Worlds.SaveLoad;
using Clarity.Engine.Objects.WorldTree;
using Microsoft.Office.Interop.PowerPoint;

namespace Clarity.Ext.Import.Pptx
{
    public class ConversionContext
    {
        public int NextId { get; set; }
        public IFileLoadInfo LoadInfo { get; set; }
        public Presentation PpPresentation { get; set; }
        public IWorld World { get; set; }
        public IList<ISceneNode> SlideNodes { get; set; }

        public Slide CurrentSlide { get; set; }
        public ISceneNode CurrentSlideNode { get; set; }
        
        public int GetNewId() => NextId++;
    }
}