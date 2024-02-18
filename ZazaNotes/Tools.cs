using System.Security.Cryptography;
using System.Text;

namespace Zaza.Notes;

public static class Tools {
    public static void ChangeById<T>(this List<T> list, Guid guid, IIdentifiable? newElement = null)
        where T : IIdentifiable {
        for (int i = 0; i < list.Count; i++) {
            if (list[i].Id == guid) {
                if (newElement != null) {
                    list[i] = (T)newElement;
                }
                else {
                    list.RemoveAt(i);
                }
                return;
            }
        }
    }

    public static string CreateSHA256(this string input) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(input)));
}
