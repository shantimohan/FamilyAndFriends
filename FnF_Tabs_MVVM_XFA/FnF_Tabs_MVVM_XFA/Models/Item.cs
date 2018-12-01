using SQLite;
using System;

namespace FnF_Tabs_MVVM_XFA.Models
{
    [Table("people")]
    public class Item
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
    }
}