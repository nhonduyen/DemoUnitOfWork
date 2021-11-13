namespace Recruiter.API.Services
{
    public interface ICryptoService
    {
        string ComputeSha256Hash(string input);
        string ConvertByteArrayToString(byte[] array);
    }
}
