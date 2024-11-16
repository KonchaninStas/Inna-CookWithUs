namespace CookWithUs.Application.Entities
{
    internal class DescriptionStep
    {
        public DescriptionStep(string title, string details)
        {
            Title = title;
            Details = details;
        }

        public string Title { get; }

        public string Details { get; }
    }
}
