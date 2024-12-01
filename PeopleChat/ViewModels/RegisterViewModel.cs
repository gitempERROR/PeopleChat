using CommunityToolkit.Mvvm.ComponentModel;
using PeopleChat.Models;
using PeopleChat.Resources;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;

namespace PeopleChat.ViewModels
{
	public partial class RegisterViewModel : ViewModelBase
	{
        [ObservableProperty]
        private string login = "";

        [ObservableProperty]
        private string password = "";

        [ObservableProperty]
        private string repeatPassword = "";

        public void NavigateToAuth()
        {
            NavigateToRoute(new NavigationEventArgs(RouteNames.Auth));
        }

        public void Register()
        {
            if (Login == "") return;
            if (Password != RepeatPassword || Password == "") return;
            byte[] encryptedData = Encrypt(Password);
            DBContexManager.CreateNewUser(Login, encryptedData);
        }

        public byte[] Encrypt(string password) 
        {
            using (Aes aes = Aes.Create())
            {
                byte[] encryptedData;
                string keyString = "01234567890123456789012345678901";
                byte[] key = Encoding.UTF8.GetBytes(keyString);

                aes.Key = key;
                aes.IV = new byte[16]; //������ ������������� (IV)
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV); // ������������� ������
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(password);
                    }
                    // ��������� ������������� ������ � ����������� ������ ����
                    encryptedData = ms.ToArray();
                }
                return encryptedData;
            }
        }
    }
}