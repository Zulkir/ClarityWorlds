using System.Collections.Generic;
using System.Linq;
using Clarity.Common.Infra.TreeReadWrite;

namespace Clarity.App.Worlds.SaveLoad.Converters
{
    public class SaveLoadVersionConverterBase : SaveLoadConverterReaderBase, ISaveLoadVersionConverter
    {
        private readonly List<ISaveLoadConverterTask> converterTasks;
        private ITrwReader previous;

        protected SaveLoadVersionConverterBase()
        {
            converterTasks = new List<ISaveLoadConverterTask>();
        }

        protected override ITrwReader GetImmediatePrevious() => converterTasks.LastOrDefault() ?? Previous;

        public ITrwReader Previous
        {
            get => previous;
            set
            {
                previous = value;
                UpdateFirstTaskPrevious(value);
            }
        }

        private void UpdateFirstTaskPrevious(ITrwReader value)
        {
            var firstTask = converterTasks.FirstOrDefault();
            if (firstTask != null)
                firstTask.Previous = value;
        }

        protected void AddTask(ISaveLoadConverterTask task)
        {
            var newTaskIndex = converterTasks.Count;
            converterTasks.Add(task);
            task.Previous = newTaskIndex == 0 ? Previous : converterTasks[newTaskIndex - 1];
        }
    }
}