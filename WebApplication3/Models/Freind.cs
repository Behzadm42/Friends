using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Friend
    {
        public Friend()
        {
        }
        public Friend(int id, string number, string name, string image)
        {
            Name = name;
            Id = id;
            Number = number;
            Image = image;
        }
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Required]
        [Display(Name  ="Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Number")]
        public string Number { get; set; }
        [Required]
        [Display(Name = "Image")]
        public string Image { get; set; }
    }
}
