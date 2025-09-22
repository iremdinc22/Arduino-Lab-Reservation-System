namespace LabReservation.Models
{
    public class User
{
    public int Id { get; set; }
    public string StudentNumber { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } = "User"; // varsayılan olarak "User"
}

}
