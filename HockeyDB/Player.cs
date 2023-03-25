using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Nikolay_YW.HockeyDB
{
    public class Player
    {
        [Key]
        public int Id { get; set; }
        [NotNull]
        public string Name { get; set; }
        [NotNull]
        public string Team { get; set; }
        public int Goals { get; set; }
        public int MayGoals { get; set; }
        public double PenaltyTime { get; set; }
    }
}