using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSeatTester
{
    /// <summary>
    /// Represents a comboBoxItem with a value and its corresponding displayed value 
    /// </summary>
    public class ComboBoxItem
    {
        /// <summary>
        /// Displayed value
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Value object
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Constructs and initializes a comboBoxItem
        /// </summary>
        /// <param name="text">displayed value</param>
        /// <param name="value">the value object</param>
        public ComboBoxItem(String text, Object value)
        {
            this.Text = text;
            this.Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
