using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.Entities
{
    public class ApplicationUserClub
    {
        public int Id { get; set; }
        public string? ApplicationUserId { get; set; }
        //[ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser? ApplicationUser { get; set; }
        
        public Guid? ClubId { get; set; }
        //[ForeignKey("ClubId")]
        public virtual Club? Club { get; set; }
    }
}