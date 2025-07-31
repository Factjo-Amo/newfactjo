namespace Newfactjo.Models
{
    public class HiddenCategory
    {
        public int Id { get; set; } // معرف السطر (Primary Key)
        public int CategoryId { get; set; } // رقم التصنيف الذي نريد إخفاءه
    }
}
