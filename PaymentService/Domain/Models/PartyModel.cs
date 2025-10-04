namespace Domain.Models
{
    public class PartyModel
    {
        public string Name { get; private set; } = null!; 
        public string Document { get; private set; } = null!;

		private PartyModel(string name, string document)
		{
			Name = name;
			Document = document;
        }

        public static PartyModel Create(string name, string document)
        {
            return new PartyModel(name, document);
        }
    }
}
