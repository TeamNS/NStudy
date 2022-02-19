using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreService.Models
{
    public class Note
    {
        [Key]
        public Int64 UniqueID { get; set; }

        [Required]
        public string NoteTitle { get; set; }

        [Required]
        public string NoteContents { get; set; }

        [Required]
        public Int64 UserUID { get; set; }

        [ForeignKey("UserUID")]
        public virtual User User { get; set; }
    }
}
