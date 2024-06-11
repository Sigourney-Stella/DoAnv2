using System.ComponentModel.DataAnnotations;

namespace BookStoreTM.Models.EF
{
    public class LoginViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = " Hãy nhập username ")]

        public string Username { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = " hãy nhập password ")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }


    }
}
