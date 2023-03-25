using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Nikolay_YW.HockeyDB
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        [NotNull]
        public string Usrename { get; set; }
        [NotNull]
        public string Password { get; set; }
    }
}