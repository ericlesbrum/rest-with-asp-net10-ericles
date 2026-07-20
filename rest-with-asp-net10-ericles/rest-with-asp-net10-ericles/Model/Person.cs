using rest_with_asp_net10_ericles.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rest_with_asp_net10_ericles.Model;

[Table("person")]
public class Person : BaseEntity
{
    [Required]
    [Column("first_name", TypeName = "varchar(80)")]
    [MaxLength(80)]
    public required string FirstName { get; set; }
    [Required]
    [Column("last_name", TypeName = "varchar(80)")]
    [MaxLength(80)]
    public required string LastName { get; set; }
    [Required]
    [Column("address", TypeName = "varchar(100)")]
    [MaxLength(100)]
    public required string Address { get; set; }
    [Required]
    [Column("gender", TypeName = "varchar(6)")]
    [MaxLength(6)]
    public required string Gender { get; set; }
    [Required]
    [Column("birthday", TypeName = "date")]
    [DataType(DataType.Date)]
    public DateTime? Birthday { get; set; }
}
