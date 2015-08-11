using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recoding.WhereAmI2015
{
    /// <summary>
    /// Available positions for the adornment layer
    /// </summary>
    public enum AdornmentPositions
    {
        [Description("Top-right corner")]
        /// <summary>
        /// Top-right corner of the view
        /// </summary>
        TopRight,

        [Description("Bottom-right corner")]
        /// <summary>
        /// Bottom-right corner of the view
        /// </summary>
        BottomRight
    }
}
