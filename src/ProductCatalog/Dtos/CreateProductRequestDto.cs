namespace ProductCatalog.Dtos
{
    public class CreateProductRequestDto
    {
        public CreateProductRequestDto(string name, string photo, int initialHand)
        {
            Name = name;
            Photo = photo;
            InitialHand = initialHand;
        }

        public string Name { get;private set; }
        public string Photo { get; private set; }
        public int InitialHand { get; private set; }
    }
}
