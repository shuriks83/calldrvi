using System;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace calldrvi
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Использование: calldrvi.exe \"<command>\""); // тут return _Result(false, "01");  !!!
                return;
            }

            string pipeName = "kktpipe";
            
            // объединяем все аргументы в одну строку с пробелами
            string command = string.Join(" ", args);

//            Console.WriteLine(string.Format("Команда: {0}", command));

            try
            {
                using (NamedPipeClientStream pipe = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut))
                {
                    pipe.Connect(3000); // ждать до 3 сек

                    // явное открытие потоков
                    StreamWriter writer = new StreamWriter(pipe, Encoding.UTF8);
                    StreamReader reader = new StreamReader(pipe, Encoding.UTF8);

                    writer.WriteLine(command);
                    writer.Flush();

                    string response = reader.ReadLine();
                    Console.WriteLine(response ?? "1 91,нет ответа от демона"); // 

                    writer.Close();
                    reader.Close();
                }
            }
            catch (TimeoutException)
            {
                Console.WriteLine("1 92,не удалось подключиться к демону (тайм-аут)");
            }
            catch (IOException ex)
            {
                Console.WriteLine("1 93,ошибка обмена " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("1 94,ошибка " + ex.Message);
            }
        }
    }

}
