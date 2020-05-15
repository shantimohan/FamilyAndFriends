using SQLite;
using System;
using System.ComponentModel;

namespace FnF_Tabs_MVVM_XFA.Models
{
    [Table("people")]
    public class Item : INotifyPropertyChanged
    {
        [PrimaryKey]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime Anniversary { get; set; }
        [MaxLength(10)]
        public string Category { get; set; }
        [MaxLength(500)]
        public string Notes { get; set; }
        public string ImageURI { get; set; }

        private bool _ShowActionMenu;
        public bool ShowActionMenu
        {
            get { return _ShowActionMenu; }
            set
            {
                _ShowActionMenu = value;
                this.RaisedOnPropertyChanged("IsVisible");
            }
        }

        #region Interface Member

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisedOnPropertyChanged(string _PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(_PropertyName));
            }
        }

        #endregion
    }
}