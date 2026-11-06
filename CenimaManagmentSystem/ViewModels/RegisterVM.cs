using System.ComponentModel.DataAnnotations;

namespace CenimaManagmentSystem.ViewModels
{
    public class RegisterVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }


        [EmailAddress, DataType(DataType.EmailAddress)] // داتا تايب دي بتساعدني في الفيو بتديني ايروور لو مدخلتش ايميل
        public string Email { get; set; }


        [DataType(DataType.Password)]
        public string Password { get; set; }


        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
