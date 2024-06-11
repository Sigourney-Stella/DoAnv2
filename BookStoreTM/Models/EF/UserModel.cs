using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BookStoreTM.Models.EF
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = " Hãy nhập tên đăng nhập ")]

        public string Username { get; set; }
        [Required(ErrorMessage = "Hãy nhập Email ")]

        public string Email { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = " hãy nhập mật khẩu ")]
        public string Password { get; set; }


    }
}
