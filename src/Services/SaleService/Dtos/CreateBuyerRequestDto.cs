namespace SaleService.Dtos
{
    public class CreateBuyerRequestDto
    {

        public CreateBuyerRequestDto(string firstName, string lastName = null)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
