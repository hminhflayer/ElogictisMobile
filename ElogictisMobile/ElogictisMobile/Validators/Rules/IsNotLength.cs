using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ElogictisMobile.Validators.Rules
{
    public class IsNotLength<T> : IValidationRule<T>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the validation Message.
        /// </summary>
        public string ValidationMessage { get; set; }
        private Regex passwordRegExp = new Regex("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{8,20})");


        #endregion

        #region Methods

        /// <summary>
        /// Check the Email has null or empty
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>returns bool value</returns>
        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = $"{value }";
            return this.passwordRegExp.IsMatch(str);
        }

        #endregion
    }
}
