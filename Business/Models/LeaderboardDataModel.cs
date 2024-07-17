using System;
using System.Collections.Generic;
using System.Linq;
namespace Business.Models
{
    public class LeaderboardDataModel
    {
        public int Id { get; set; }

        public int Points { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;
    }
}
