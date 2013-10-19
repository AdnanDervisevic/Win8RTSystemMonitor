using System;
namespace Veritas.Classes
{
    public class NameValueItem
    {
        private string name = string.Empty;
        private DateTime dateTime = DateTime.Now;
        private bool dateTimeEnable = false;

        /// <summary>
        /// Sets the name to a dateTime.
        /// </summary>
        public DateTime DateTime
        {
            set 
            { 
                this.dateTime = value;
                this.dateTimeEnable = true;
            }
            get { return this.dateTime; }
        }

        /// <summary>
        /// The Name.
        /// </summary>
        public string Name
        {
            get
            {
                if (this.dateTimeEnable)
                    return this.dateTime.ToString("mm");
                else
                    return this.name;
            }
            set
            {
                this.name = value;
                this.dateTimeEnable = false;
            }
        }

        /// <summary>
        /// The value.
        /// </summary>
        public long Value { get; set; }

        /// <summary>
        /// Constructs a new NameValueItem with a name and a value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public NameValueItem(string name, long value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Constructs a new NameValueItem with a datetime and a value.
        /// </summary>
        /// <param name="dateTime">The datetime.</param>
        /// <param name="value">The value.</param>
        public NameValueItem(DateTime dateTime, long value)
        {
            this.DateTime = dateTime;
            this.Value = value;
        }
    }
}