using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.VisualStudio.Shell;
using System.Drawing;
using Microsoft.VisualStudio.ComponentModelHost;
using Recoding.WhereAmI2015;
using Recoding.WhereAmI2015.SettingsConverters;

namespace WhereAmI2015
{
    public class WhereAmIOptionPageGrid : DialogPage
    {
        IWhereAmISettings settings
        {
            get
            {
                var componentModel = (IComponentModel)(Site.GetService(typeof(SComponentModel)));
                IWhereAmISettings s = componentModel.DefaultExportProvider.GetExportedValue<IWhereAmISettings>();

                return s;
            }
        }

        #region -- Interface fields --

        #endregion

        private Color _FilenameColor = Color.FromArgb(234, 234, 234);
        private Color _FoldersColor = Color.FromArgb(243, 243, 243);
        private Color _ProjectColor = Color.FromArgb(243, 243, 243);

        private bool _ViewFilename = true;
        private bool _ViewFolders = true;
        private bool _ViewProject = true;

        private double _FilenameSize = 60;
        private double _FoldersSize = 52;
        private double _ProjectSize = 52;

        private AdornmentPositions _Position = AdornmentPositions.TopRight;
        private double _Opacity = 1;

        [Category("Filename")]
        [DisplayName("Show")]
        [Description("The enable or disable the filename")]
        public bool ViewFilename
        {
            get { return _ViewFilename; }
            set { _ViewFilename =  value; }
        }

        [Browsable(true)]
        [Category("Filename")]
        [DisplayName("Filename color")]
        [Description("The foreground color of the filename")]
        public Color FilenameColor
        {
            get { return _FilenameColor; }
            set { _FilenameColor = value; }
        }

        [Category("Filename")]
        [DisplayName("Filename size")]
        [Description("The size in points of the filename")]
        public double FilenameSize
        {
            get { return _FilenameSize; }
            set { _FilenameSize = value; }
        }



        [Category("Folders")]
        [DisplayName("Show")]
        [Description("The enable or disable the folders structure")]
        public bool ViewFolders
        {
            get { return _ViewFolders; }
            set { _ViewFolders = value; }
        }

        [Browsable(true)]
        [Category("Folders")]
        [DisplayName("Folders color")]
        [Description("The foreground color of the folders structure")]
        public Color FoldersColor
        {
            get { return _FoldersColor; }
            set { _FoldersColor = value; }
        }

        [Category("Folders")]
        [DisplayName("Folders size")]
        [Description("The size in points of the folders path")]
        public double FoldersSize
        {
            get { return _FoldersSize; }
            set { _FoldersSize = value; }
        }

        [Category("Project")]
        [DisplayName("Show")]
        [Description("The enable or disable the project name")]
        public bool ViewProject
        {
            get { return _ViewProject; }
            set { _ViewProject = value; }
        }

        [Browsable(true)]
        [Category("Project")]
        [DisplayName("Project color")]
        [Description("The foreground color of the project name")]
        public Color ProjectColor
        {
            get { return _ProjectColor; }
            set { _ProjectColor = value; }
        }

        [Category("Project")]
        [DisplayName("Project size")]
        [Description("The size in points of the project name")]
        public double ProjectSize
        {
            get { return _ProjectSize; }
            set { _ProjectSize = value; }
        }

        [Category("Appearance")]
        [DisplayName("Position")]
        [Description("The position in the view of the text block")]
        [Browsable(true)]
        public AdornmentPositions Position
        {
            get { return _Position; }
            set { _Position = value; }
        }

        [Category("Appearance")]
        [DisplayName("Opacity")]
        [Description("Opacity of the text. Insert a value between 0 and 1.")]
        [TypeConverter(typeof(PercentageConverter))]
        public double Opacity
        {
            get { return _Opacity; }
            set { _Opacity = value; }
        }

        private void BindSettings()
        {
            _FilenameColor = settings.FilenameColor;
            _FoldersColor = settings.FoldersColor;
            _ProjectColor = settings.ProjectColor;

            _FilenameSize = settings.FilenameSize;
            _FoldersSize = settings.FoldersSize;
            _ProjectSize = settings.ProjectSize;

            _ViewFilename = settings.ViewFilename;
            _ViewFolders = settings.ViewFolders;
            _ViewProject = settings.ViewProject;

            _Position = settings.Position;
            _Opacity = settings.Opacity;
        }

        protected override void OnActivate(CancelEventArgs e)
        {
            base.OnActivate(e);

            BindSettings();
        }

        protected override void OnApply(PageApplyEventArgs e)
        {
            if (e.ApplyBehavior == ApplyKind.Apply)
            {
                settings.FilenameColor = FilenameColor;
                settings.FoldersColor = FoldersColor;
                settings.ProjectColor = ProjectColor;

                settings.FilenameSize = FilenameSize;
                settings.FoldersSize = FoldersSize;
                settings.ProjectSize = ProjectSize;

                settings.ViewFilename = ViewFilename;
                settings.ViewFolders = ViewFolders;
                settings.ViewProject = ViewProject;

                settings.Position = Position;
                settings.Opacity = Opacity;

                settings.Store();
            }

            base.OnApply(e);
        }
    }
}
