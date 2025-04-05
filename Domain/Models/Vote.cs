using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Vote
    {
        public int PollId { get; set; }

        public string VoterId { get; set; }
        public DateTime VoteTimestamp { get; set; }
    }
}