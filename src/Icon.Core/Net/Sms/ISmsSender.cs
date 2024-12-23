using System.Threading.Tasks;

namespace Icon.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}