namespace Biometric_API.Models
{
    public class BiometricDataModels
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Data { get; set; } //path to file
    }
}