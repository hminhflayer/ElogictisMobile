using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms.Internals;

namespace ElogictisMobile.Models
{
    /// <summary>
    /// Model for Health care page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class HealthCare
    {

        #region Property

        /// <summary>
        /// Gets or sets the property that has been displays the Category.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the property that has been displays the Category value.
        /// </summary>
        public string CategoryValue { get; set; }

        /// <summary>
        /// Gets or sets the property that has been displays the background gradient start.
        /// </summary>
        public string BackgroundGradientStart { get; set; }

        /// <summary>
        /// Gets or sets the property that has been displays the background gradient end.
        /// </summary>
        public string BackgroundGradientEnd { get; set; }

        #endregion

    }
}