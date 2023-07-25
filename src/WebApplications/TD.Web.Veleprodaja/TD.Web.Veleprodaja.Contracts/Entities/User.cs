using System.ComponentModel.DataAnnotations;
using TD.Core.Contracts;

namespace TD.Web.Veleprodaja.Contracts.Entities
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
    }
}
