using Clarity.Engine.Gui.Menus;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms
{
    public class EtoMenuBuilder : IGuiMenuBuilder
    {
        private readonly ISubmenu menu;
        private bool justStartedSection;

        public EtoMenuBuilder(ISubmenu menu)
        {
            this.menu = menu;
            menu.Items.Clear();
            justStartedSection = true;
        }

        public void StartSection()
        {
            if (!justStartedSection)
                menu.Items.AddSeparator();
            justStartedSection = true;
        }

        public void AddCommand(IGuiCommand command, bool enabled = true)
        {
            menu.Items.Add(new Command((s, a) => command.Execute())
            {
                MenuText = command.Text,
                Shortcut = Converters.ToEto(command.ShortcutKey, command.ShortcutModifyers),
                Enabled = enabled
            });
            justStartedSection = false;
        }

        public IGuiMenuBuilder AddSubmenu(string text)
        {
            var submenu = menu.Items.GetSubmenu(text);
            justStartedSection = false;
            return new EtoMenuBuilder(submenu);
        }
    }
}