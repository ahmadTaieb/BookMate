using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookMate.Entities
{
    public class Club
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } 
        public bool Hidden { get; set; } = false;
        public string? ImageUrl { get; set; }
        public string? ApplicationUserId {  get; set; }
        [ForeignKey("ApplicationUserId")]
        [JsonIgnore]
        public ApplicationUser? ApplicationUser {  get; set; }

        public ICollection<ApplicationUserClub> ApplicationUsersMember { get; set; }
        
    }
}
