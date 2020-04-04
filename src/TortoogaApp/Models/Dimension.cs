using System.ComponentModel.DataAnnotations.Schema;

namespace TortoogaApp.Models
{
    [ComplexType]
    public class Dimension : IEntity
    {
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        [NotMapped]
        public double SquareFeet { get { return Length * Width; } }
    }
}