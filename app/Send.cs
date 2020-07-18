using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace app {

    public static class Sender {
        public static string BitsToString(bool[] bits)
        {
            System.Text.StringBuilder strBuilder =
                new System.Text.StringBuilder(new string('0', bits.Length));
            
            for (var i = 0; i < bits.Length; ++i)
            {
                strBuilder[i] = bits[i] ? '1' : '0';
            }

            return strBuilder.ToString();
        }

        public static bool[] StringToBits(string str)
        {
            var bits = new bool[str.Length];
            for (var i = 0; i < str.Length; ++i)
            {
                bits[i] = (str[i] == '1') ? true : false;
            }
            
            return bits;
        }

        public static async Task<Value> Send(Value value) {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://icfpc2020-api.testkontur.ru/aliens/send?apiKey=911f0ee2b60c4fe8a7048b89dc69b7de");
            
            var requestContent = new StringContent(BitsToString(Modem.Modulate(value)), Encoding.UTF8, MediaTypeNames.Text.Plain);
            using var response = await httpClient.PostAsync("", requestContent);
            
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            
            var responseString = await response.Content.ReadAsStringAsync();
            Console.Write(responseString);
            return Modem.Demodulate(StringToBits(responseString));
        }
    }

}
