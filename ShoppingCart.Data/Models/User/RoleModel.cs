using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Data.Models.User;

public class RoleModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    public Guid RoleID { get; set; }
    public RoleEnum RoleType { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    [ForeignKey("UserID")] public Guid UserID { get; set; }
    public virtual ICollection<UserModel> UserModels { get; set; }
}