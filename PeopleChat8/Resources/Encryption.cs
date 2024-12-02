using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PeopleChat8.Resources
{
    public static class Encryption
    {
        private static string keyString = "01234567890123456789012345678901";
        public static byte[] Encrypt(string password)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] encryptedData;
                byte[] key = Encoding.UTF8.GetBytes(keyString);

                aes.Key = key;
                aes.IV = new byte[16]; //Вектор инициализации (IV)
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV); // Зашифровываем данные
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(password);
                    }
                    // Сохраняем зашифрованные данные в статический массив байт
                    encryptedData = ms.ToArray();
                }
                return encryptedData;
            }
        }
    }
}