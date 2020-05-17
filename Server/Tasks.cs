using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Server
{
    public class Tasks
    {
        public static byte[] ReadBlock(string fileName, int offset, int length,string text)
        {
            try
            {
                using (FileStream fsSource = File.OpenRead(@"C:\Files\Server\" + fileName))
                //using (FileStream fsSource = File.OpenRead(@"D:\" + fileName))
                {
                    if (length > fsSource.Length | length < 0)
                        length = (int)fsSource.Length;
                    byte[] a1 = new byte[length];
                    try
                    {
                        fsSource.Seek(offset, SeekOrigin.Begin);
                        fsSource.Read(a1, 0, length);
                        byte[] a2 = Encoding.ASCII.GetBytes(text);
                        byte[] rv = new byte[a1.Length + a2.Length];
                        System.Buffer.BlockCopy(a1, 0, rv, 0, a1.Length);
                        System.Buffer.BlockCopy(a2, 0, rv, a1.Length, a2.Length);
                        return rv;
                    }
                    catch
                    {
                        Console.WriteLine("Ошибка чтения из файла");
                        return null;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл не найден");
                return null;
            }
        }
        public static byte[] GetTime()
        {
            byte[] result;
            string timeNow = (DateTime.Now).ToString();
            result = Encoding.UTF8.GetBytes(timeNow);
            return result;
        }

        public static string GetHash()
        {
            return "hello"; 
        }
    }
}
