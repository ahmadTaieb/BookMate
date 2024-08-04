using BookMate.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class PostResponse
    {
        public string Id { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public int? TotalReacts {  get; set; }
        public int? Like {  get; set; }
        public int? Love {  get; set; }
        public int? Laugh{  get; set; }
        public int? Sad { get; set; }
        public string? ApplicationUserId { get; set; }
        public string? ApplicationUserName {  get; set; } 
        public string? ApplicationUserImageUrl { get; set; }
        public Guid? ClubId { get; set; }
        
    }
}
