using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.SaveLoad;
using Clarity.App.Worlds.SaveLoad.Import;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.SaveLoad
{
    public class SaveLoadGuiCommands : ISaveLoadGuiCommands
    {
        private readonly IReadOnlyList<ISaveLoadFormat> formats;
        private readonly Lazy<IMainForm> mainFormLazy;
        private readonly ISaveLoadService saveLoadService;
        private readonly SaveFileDialog saveFileDialog;
        private readonly OpenFileDialog openFileDialog;

        public Command New { get; }
        public Command Save { get; }
        public Command SaveAs { get; }
        public Command Open { get; }
        public IReadOnlyList<Command> ImportCommands { get; }

        private IMainForm MainForm { get { return mainFormLazy.Value; } }

        public SaveLoadGuiCommands(Lazy<IMainForm> mainFormLazy, ISaveLoadService saveLoadService, 
            IReadOnlyList<ISaveLoadFormat> formats, IReadOnlyList<IPresentationImporter> presentationImporters)
        {
            this.mainFormLazy = mainFormLazy;
            this.saveLoadService = saveLoadService;
            this.formats = formats;
            saveFileDialog = new SaveFileDialog();
            openFileDialog = new OpenFileDialog();
            foreach (var format in formats)
            {
                var filter = new FileDialogFilter(format.Name, format.FileExtension);
                saveFileDialog.Filters.Add(filter);
                openFileDialog.Filters.Add(filter);
            }
            New = Create("New", ExecNew, Keys.Control | Keys.N);
            Save = Create("Save", ExecSave, Keys.Control | Keys.S);
            SaveAs = Create("Save As", ExecSaveAs);
            Open = Create("Open", ExecOpen, Keys.Control | Keys.O);
            ImportCommands = presentationImporters.Select(i => Create(i.Name, (s, a) => ExecImport(i))).ToArray();
        }

        private void ExecNew(object sender, EventArgs args)
        {
            if (saveLoadService.HasUnsavedChanges)
            {
                // TODO
                //var result = MessageBox.Show(MainForm.Form, "Save the current world?", "Unsaved Changes", MessageBoxType.Question);
                //switch (result)
                //{
                //    case DialogResult.Yes:
                //        ExecSave(sender, args);
                //        break;
                //    case DialogResult.No:
                //        break;
                //    case DialogResult.Cancel:
                //        return;
                //    default:
                //        throw new ArgumentOutOfRangeException();
                //}
            }
            saveLoadService.New();
        }

        private void ExecSave(object sender, EventArgs args)
        {
            if (saveLoadService.FileName == null || saveLoadService.Format == null)
                ExecSaveAs(sender, args);
            else
                // todo: check save settings
                saveLoadService.Save(SaveWorldFlags.EditableWorld | SaveWorldFlags.ReadOnlyWorld);
        }

        private void ExecSaveAs(object sender, EventArgs args)
        {
            saveFileDialog.FileName = saveLoadService.FileName ?? "NewWorld";
            var result = saveFileDialog.ShowDialog(MainForm.Form);
            switch (result)
            {
                case DialogResult.Ok:
                case DialogResult.Yes:
                    var format = formats.Single(x => x.Name == saveFileDialog.CurrentFilter.Name);
                    saveLoadService.Format = format;
                    saveLoadService.FileName = saveFileDialog.FileName;
                    // todo: check save settings
                    saveLoadService.Save(SaveWorldFlags.EditableWorld | SaveWorldFlags.ReadOnlyWorld);
                    break;
            }
        }

        private void ExecOpen(object sender, EventArgs args)
        {
            var result = openFileDialog.ShowDialog(MainForm.Form);
            switch (result)
            {
                case DialogResult.Ok:
                case DialogResult.Yes:
                    var format = formats.Single(x => x.Name == openFileDialog.CurrentFilter.Name);
                    saveLoadService.Format = format;
                    saveLoadService.FileName = openFileDialog.FileName;
                    // todo: check save settings
                    saveLoadService.Load(LoadWorldPreference.EditableOnly);
                    break;
            }
        }

        private void ExecImport(IPresentationImporter importer)
        {
            var dialog = new OpenFileDialog();
            dialog.Filters.Add(new FileDialogFilter(importer.Name, importer.FileExtensions.ToArray()));
            var result = dialog.ShowDialog(MainForm.Form);
            switch (result)
            {
                case DialogResult.Ok:
                case DialogResult.Yes:
                    saveLoadService.Import(importer, dialog.FileName, LoadWorldPreference.EditableOnly);
                    break;
            }
        }

        private static Command Create(string text, EventHandler<EventArgs> handler, Keys? shortcut = null)
        {
            var command = new Command
            {
                MenuText = text,
                ToolBarText = text,
                ToolTip = text
            };
            command.Executed += handler;
            if (shortcut.HasValue)
                command.Shortcut = shortcut.Value;
            return command;
        }
    }
}