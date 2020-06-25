using System;
using System.IO;
using System.Net;
using System.Text;

namespace DBBrokerClient
{
    public class WebRequester
    {
        public static void Main()
        {
            string simulated_post = "";
            Encoding encode_1252 = Encoding.GetEncoding(1252);

            StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "http_form", encode_1252);
            simulated_post = reader.ReadToEnd();
            reader.Dispose();

            DateTime execution_time = DateTime.Now;
            for (int cont = 6272; cont <= 6272; cont++)
            {
                try
                {
                    simulated_post = simulated_post.Replace("#putted_id#", "7593");
                    var data = encode_1252.GetBytes(simulated_post);

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.vizeuonline.com.br/cadastro/fisica");
                    request.Method = "POST";
                                                            
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }

                    var response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    
                    /*


                    byte[] file = new byte[400000];

                    int b = 0, i = 0;
                    while (true)
                    {
                        if ((b = stream.ReadByte()) == -1)
                            break;
                        file[i] = (byte)b;
                        i++;
                    }

                    FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\VIZEU\\" + cont /* Path.GetFileName(item) , FileMode.Create, FileAccess.ReadWrite);
                    fs.Write(file, 0, i);
                    fs.Dispose();
                    */
                    Console.WriteLine(responseString);
                }
                catch (Exception ex)
                {
                    try
                    {
                        StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\VIZEU\\00_Erros.txt", true);
                        writer.AutoFlush = true;
                        writer.WriteLine(/*Path.GetFileName(item)*/ "cliente_" + cont + " - Erro ao baixar: " + ex.Message);
                        writer.Dispose();
                    }
                    catch { }
                }
            }
            Console.Out.WriteLine("Download executado em " + (DateTime.Now - execution_time).ToString());
            Console.In.ReadLine();
        }

    }
}
