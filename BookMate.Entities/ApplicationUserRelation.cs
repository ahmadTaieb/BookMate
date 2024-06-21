using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMate.Entities
{
    public class ApplicationUserRelation
    {
        
        public string? ApplicationUserParentId { get; set; }
        public virtual ApplicationUser? ApplicationUserParent { get; set; }
        public string? ApplicationUserChildId { get; set; }
        public virtual ApplicationUser? ApplicationUserChild { get; set; }
        
        public bool confirm { get; set; } = false;
        //Confirm = table.Column<bool>(type: "bit", nullable: false),



    }
}
