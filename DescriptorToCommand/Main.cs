using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using Kbg.NppPluginNET.PluginInfrastructure;
using SimpleJSON;

namespace DescriptorToCommand
{
    /// <summary>
    /// Powered by:
    /// https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// https://github.com/mhallin/SimpleJSON.NET
    /// A copy of the licenses can be found in this project.
    /// </summary>
    class Main
    {
        internal const string PluginName = "Descriptor to Command";
        static string iniFilePath = null;
        
        static IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());
        static INotepadPPGateway notepad = new NotepadPPGateway();

        public static void OnNotification(ScNotification notification)
        {
        }

        internal static void CommandMenuInit()
        {
            InitializeIniFile();

            PluginBase.SetCommand(0, "Copy Command", CopyCommand, new ShortcutKey(true, true, true, Keys.C));
            PluginBase.SetCommand(1, "Show Command", ShowCommand, new ShortcutKey(true, true, true, Keys.S));
            PluginBase.SetCommand(2, "Configuration", OpenConfiguration, new ShortcutKey(false, false, false, Keys.None));
        }

        private static void InitializeIniFile()
        {
            StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            iniFilePath = sbIniFilePath.ToString();
            if (!Directory.Exists(iniFilePath)) Directory.CreateDirectory(iniFilePath);
            iniFilePath = Path.Combine(iniFilePath, PluginName + ".ini");
        }

        internal static void SetToolBarIcon()
        {
        }

        internal static void PluginCleanUp()
        {
        }

        #region Menu Functions
        private static void CopyCommand()
        {
            string command = "";

            try
            {
                int length = editor.GetLength();
                if (length == 0) throw new DescriptorException("No descriptor could be found in the current document.");

                // For some reason length has to be 1 higher to get all the text.
                length++;

                command = CreateCommand(editor.GetText(length));

                Clipboard.SetText(command);
            }
            catch (ParseError exc)
            {
                string error = string.Format("JSON Exception:\n{0} at {1}.", exc.Message, exc.Position);
                ShowError(error);
                return;
            }
            catch (DescriptorException exc)
            {
                ShowError(exc.Message);
                return;
            }
        }

        private static void ShowCommand()
        {
            string command = "";

            try
            {
                int length = editor.GetLength();
                if (length == 0) throw new DescriptorException("No descriptor could be found in the current document.");

                // For some reason length has to be 1 higher to get all the text.
                length++;

                command = CreateCommand(editor.GetText(length));

                notepad.FileNew();
                editor.SetText(command);
                editor.SelectAll();
            }
            catch (ParseError exc)
            {
                string error = string.Format("JSON Exception:\n{0} at {1}.", exc.Message, exc.Position);
                ShowError(error);
                return;
            }
            catch (DescriptorException exc)
            {
                ShowError(exc.Message);
                return;
            }
            catch (Exception exc)
            {
                ShowError(string.Format("Unknown Error:\n{0}", exc.Message));
                return;
            }
        }

        private static void OpenConfiguration()
        {
            notepad.OpenFile(iniFilePath);
        }

        #endregion

        private static string CreateCommand(string descriptorJson)
        {
            JObject desc = JSONDecoder.Decode(descriptorJson);
            
            // Item name
            if (!desc.ObjectValue.ContainsKey("name"))
                throw new DescriptorException("Item 'name' was not found. Make sure your current document has a valid item descriptor in it.");
            string name = desc["name"].StringValue;

            // Item count
            int count = 1;
            if (desc.ObjectValue.ContainsKey("count"))
                count = (int)desc["count"];

            // Base command
            StringBuilder command = new StringBuilder();
            command.AppendFormat("/spawnitem {0} {1}", name, count);

            // Item Parameters
            if (desc.ObjectValue.ContainsKey("parameters"))
            {
                JObject parameters = desc["parameters"];
                string sParameters = JSONEncoder.Encode(parameters);
                
                // Add parameters to command
                command.AppendFormat(" '{0}'", sParameters.Replace("'", "\\'"));
            }

            return command.ToString();
        }

        private static string GetConfigString(string section, string key)
        {
            StringBuilder sb = new StringBuilder(1024);
            Win32.GetPrivateProfileString(section, key, "", sb, 1024, iniFilePath);
            return sb.ToString();
        }

        private static void SetConfigString(string section, string key, string value)
        {
            Win32.WritePrivateProfileString(section, key, value, iniFilePath);
        }

        internal static void ShowError(string error)
        {
            MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public class DescriptorException : Exception
        {
            public DescriptorException(string message) : base(message) { }
        }
    }
}