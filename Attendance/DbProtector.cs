using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class DbProtector
{
    private static readonly string password = "MyStrongPassword123!";
    private static readonly byte[] salt = Encoding.UTF8.GetBytes("SomeSaltHere");

    public static void EncryptFile(string inputFile, string outputFile)
    {
       // lock (SyncRoot)
        {
            using (var aes = Aes.Create())
            {
                var key = new Rfc2898DeriveBytes(password, salt, 10000);
                aes.Key = key.GetBytes(32);
                aes.IV = key.GetBytes(16);

                using (var fsOutput = new FileStream(outputFile, FileMode.Create))
                using (var cs = new CryptoStream(fsOutput, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (var fsInput = new FileStream(inputFile, FileMode.Open))
                {
                    fsInput.CopyTo(cs);
                }
            }
        }
    }
    public static object SyncRoot = new object();
    public static void DecryptFile(string inputFile, string outputFile)
    {
        again:
       // lock(SyncRoot)
        {
            if (File.Exists(outputFile))
            {
                try
                {
                    File.Delete(outputFile); // cleanup stale temp copy
                }
                catch
                {
                    System.Threading.Thread.Sleep(1);
                    goto again;
                    throw new IOException($"Temp DB file {outputFile} is still locked by another process.");
                }
            }
            using (var aes = Aes.Create())
            {
                var key = new Rfc2898DeriveBytes(password, salt, 10000);
                aes.Key = key.GetBytes(32);
                aes.IV = key.GetBytes(16);

                using (var fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var cs = new CryptoStream(fsInput, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (var fsOutput = new FileStream(outputFile, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    cs.CopyTo(fsOutput);
                }
            }
        }
    }
}
