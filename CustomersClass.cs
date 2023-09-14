using System.ComponentModel.DataAnnotations;

namespace FirstWebApi
{
    public class CustomersClass
    {
        [Key]
        public int CustomerId { get; set; }

        public string FirstName { get; set; }
    }
}
