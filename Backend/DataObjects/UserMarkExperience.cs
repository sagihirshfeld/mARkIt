using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mARkIt.Backend.DataObjects
{
    public class UserMarkExperience
    {
        [StringLength(450)]
        [ForeignKey("User")]
        [Key, Column(Order = 0)]
        public string UserId { get; set; }

        public virtual User User { get; set; }


        [StringLength(450)]
        [ForeignKey("Mark")]
        [Key, Column(Order = 1)]
        public string MarkId { get; set; }

        public virtual Mark Mark {get; set; }

        public bool HasUserRated { get; set; }

        public float UserRating { get; set; }

        public DateTime? LastSeen { get; set; }
    }
}