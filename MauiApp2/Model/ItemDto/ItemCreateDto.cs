
namespace MauiApp2.Model.ItemDto
{
    public class ItemCreateDto
    {

        public string? Title { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }

        public List<string>? ImageUrl { get; set; }

        public List<string>? VideoUrl { get; set; }

        public DateTime? ReleaseDate { get; set; }



    }
}
