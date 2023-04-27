using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BallastLane.Domain.Entities;

public class Record
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
