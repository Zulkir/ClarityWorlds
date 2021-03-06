﻿using System;
using Clarity.App.Worlds.Gui;
using Clarity.App.Worlds.UndoRedo;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms
{
    public class UndoRedoGui : IGuiObserver<IUndoRedoService, object>
    {
        private IUndoRedoService UndoRedo { get; }
        public Command UndoCommand { get; }
        public Command RedoCommand { get; }

        public UndoRedoGui(IUndoRedoService undoRedo) 
        {
            UndoRedo = undoRedo;
            undoRedo.SetObserver(this);
            UndoCommand = new Command(Undo)
            {
                ID = "Undo",
                MenuText = "Undo",
                ToolBarText = "Undo",
                Shortcut = Keys.Control | Keys.Z,
                Enabled = false,
            };
            RedoCommand = new Command(Redo)
            {
                ID = "Redo",
                MenuText = "Redo",
                ToolBarText = "Redo",
                Shortcut = Keys.Control | Keys.Y,
                Enabled = false,
            };
            Update();
        }

        public void OnEvent(IUndoRedoService sender, object eventArgs)
        {
            Update();
        }

        private void Undo(object sender, EventArgs args)
        {
            UndoRedo.Undo();
        }

        private void Redo(object sender, EventArgs args)
        {
            UndoRedo.Redo();
        }

        private void Update()
        {
            UndoCommand.Enabled = UndoRedo.CanUndo;
            RedoCommand.Enabled = UndoRedo.CanRedo;
        }
    }
}