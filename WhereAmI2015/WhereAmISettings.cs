using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhereAmI2015
{
    [Export(typeof(IWhereAmISettings))]
    public class WhereAmISettings : IWhereAmISettings
    {
        /// <summary>
        /// The real store in which the settings will be saved
        /// </summary>
        readonly WritableSettingsStore writableSettingsStore;

        const string CollectionPath = "WhereAmI2015";

        public WhereAmISettings()
        {

        }

        [ImportingConstructor]
        public WhereAmISettings(SVsServiceProvider vsServiceProvider) : this()
        {
            var shellSettingsManager = new ShellSettingsManager(vsServiceProvider);
            writableSettingsStore = shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);

            LoadSettings();
        }

        public Color FilenameColor { get { return _FilenameColor; } set { _FilenameColor = value; } }
        private Color _FilenameColor;

        public Color FoldersColor { get { return _FoldersColor; } set { _FoldersColor = value; } }
        private Color _FoldersColor;

        public Color ProjectColor { get { return _ProjectColor; } set { _ProjectColor = value; } }
        private Color _ProjectColor;

        public bool ViewFilename { get { return _ViewFilename; } set { _ViewFilename = value; } }
        private bool _ViewFilename = true;

        public bool ViewFolders { get { return _ViewFolders; } set { _ViewFolders = value; } }
        private bool _ViewFolders = true;

        public bool ViewProject { get { return _ViewProject; } set { _ViewProject = value; } }
        private bool _ViewProject = true;

        public double FilenameSize { get { return _FilenameSize; } set { _FilenameSize = value; } }
        private double _FilenameSize;

        public double FoldersSize { get { return _FoldersSize; } set { _FoldersSize = value; } }
        private double _FoldersSize;

        public double ProjectSize { get { return _ProjectSize; } set { _ProjectSize = value; } }
        private double _ProjectSize;

        public void Store()
        {
            try
            {
                if (!writableSettingsStore.CollectionExists(CollectionPath))
                {
                    writableSettingsStore.CreateCollection(CollectionPath);
                }

                writableSettingsStore.SetInt32(CollectionPath, "FilenameColor", this.FilenameColor.ToArgb());
                writableSettingsStore.SetInt32(CollectionPath, "FoldersColor", this.FoldersColor.ToArgb());
                writableSettingsStore.SetInt32(CollectionPath, "ProjectColor", this.ProjectColor.ToArgb());

                writableSettingsStore.SetString(CollectionPath, "ViewFilename", this.ViewFilename.ToString());
                writableSettingsStore.SetString(CollectionPath, "ViewFolders", this.ViewFolders.ToString());
                writableSettingsStore.SetString(CollectionPath, "ViewProject", this.ViewProject.ToString());

                writableSettingsStore.SetString(CollectionPath, "FilenameSize", this.FilenameSize.ToString());
                writableSettingsStore.SetString(CollectionPath, "FoldersSize", this.FoldersSize.ToString());
                writableSettingsStore.SetString(CollectionPath, "ProjectSize", this.ProjectSize.ToString());
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
        }

        public void Defaults()
        {
            writableSettingsStore.DeleteCollection(CollectionPath);
            LoadSettings();
        }

        private void LoadSettings()
        {
            // Default values
            _FilenameSize = 60;
            _FoldersSize = _ProjectSize = 52;

            _FilenameColor = Color.FromArgb(234, 234, 234);
            _FoldersColor = _ProjectColor = Color.FromArgb(243, 243, 243);

            try
            {
                // Retrieve the Id of the current theme used in VS from user's settings, this is changed a lot in VS2015
                string visualStudioThemeId = VSRegistry.RegistryRoot(Microsoft.VisualStudio.Shell.Interop.__VsLocalRegistryType.RegType_UserSettings).OpenSubKey("ApplicationPrivateSettings").OpenSubKey("Microsoft").OpenSubKey("VisualStudio").GetValue("ColorTheme", "de3dbbcd-f642-433c-8353-8f1df4370aba", Microsoft.Win32.RegistryValueOptions.DoNotExpandEnvironmentNames).ToString();

                string parsedThemeId = Guid.Parse(visualStudioThemeId.Split('*')[2]).ToString();

                switch (parsedThemeId)
                {
                    case "de3dbbcd-f642-433c-8353-8f1df4370aba": // Light
                    case "a4d6a176-b948-4b29-8c66-53c97a1ed7d0": // Blue
                    default:
                        // Just use the defaults
                        break;

                    case "1ded0138-47ce-435e-84ef-9ec1f439b749": // Dark
                        _FilenameColor = Color.FromArgb(48, 48, 48);
                        _FoldersColor = _ProjectColor = Color.FromArgb(40, 40, 40);
                        break;
                }

                // Tries to retrieve the configurations if previously saved
                if (writableSettingsStore.PropertyExists(CollectionPath, "FilenameColor"))
                {
                    this.FilenameColor = Color.FromArgb(writableSettingsStore.GetInt32(CollectionPath, "FilenameColor", this.FilenameColor.ToArgb()));
                }

                if (writableSettingsStore.PropertyExists(CollectionPath, "FoldersColor"))
                {
                    this.FoldersColor = Color.FromArgb(writableSettingsStore.GetInt32(CollectionPath, "FoldersColor", this.FoldersColor.ToArgb()));
                }

                if (writableSettingsStore.PropertyExists(CollectionPath, "ProjectColor"))
                {
                    this.ProjectColor = Color.FromArgb(writableSettingsStore.GetInt32(CollectionPath, "ProjectColor", this.ProjectColor.ToArgb()));
                }

                if (writableSettingsStore.PropertyExists(CollectionPath, "ViewFilename"))
                {
                    bool b = this.ViewFilename;
                    if (Boolean.TryParse(writableSettingsStore.GetString(CollectionPath, "ViewFilename"), out b))
                        this.ViewFilename = b;
                }

                if (writableSettingsStore.PropertyExists(CollectionPath, "ViewFolders"))
                {
                    bool b = this.ViewFolders;
                    if (Boolean.TryParse(writableSettingsStore.GetString(CollectionPath, "ViewFolders"), out b))
                        this.ViewFolders = b;
                }

                if (writableSettingsStore.PropertyExists(CollectionPath, "ViewProject"))
                {
                    bool b = this.ViewProject;
                    if (Boolean.TryParse(writableSettingsStore.GetString(CollectionPath, "ViewProject"), out b))
                        this.ViewProject = b;
                }

                if (writableSettingsStore.PropertyExists(CollectionPath, "FilenameSize"))
                {
                    double d = this.FilenameSize;
                    if (Double.TryParse(writableSettingsStore.GetString(CollectionPath, "FilenameSize"), out d))
                        this.FilenameSize = d;
                }

                if (writableSettingsStore.PropertyExists(CollectionPath, "FoldersSize"))
                {
                    double d = this.FoldersSize;
                    if (Double.TryParse(writableSettingsStore.GetString(CollectionPath, "FoldersSize"), out d))
                        this.FoldersSize = d;
                }

                if (writableSettingsStore.PropertyExists(CollectionPath, "ProjectSize"))
                {
                    double d = this.ProjectSize;
                    if (Double.TryParse(writableSettingsStore.GetString(CollectionPath, "ProjectSize"), out d))
                        this.ProjectSize = d;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
        }
    }
}
