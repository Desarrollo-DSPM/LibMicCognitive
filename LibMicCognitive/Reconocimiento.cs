using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace LibMicCognitive
{
    public class Reconocimiento
    {
        private static string _reconocimiento = "";
        private static readonly SpeechConfig SpeechConfig = SpeechConfig.FromSubscription(Credentials.AzureKey, Credentials.Region);
        private static readonly AudioConfig AudioConfig = AudioConfig.FromDefaultMicrophoneInput();
        private static readonly SpeechRecognizer Recognizer = new(SpeechConfig, AudioConfig);

        private static async Task SttTask()
        {
            var stopRecognition = new TaskCompletionSource<int>();
            SpeechConfig.SpeechRecognitionLanguage = Credentials.SpeechRecognitionRegion;
            //Console.WriteLine("Habla al microfono.");

            using (Recognizer)
            {
                Recognizer.Recognizing += (s, e) =>
                {
                    Console.WriteLine(e.Result.Text);
                };

                Recognizer.Recognized += (s, e) =>
                {
                    _reconocimiento = e.Result.Text;
                };

                Recognizer.Canceled += (s, e) =>
                {
                    _reconocimiento = e.ErrorDetails;
                    stopRecognition.TrySetResult(0);
                };

                await Recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
                Task.WaitAny(stopRecognition.Task);
                await Recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
            }
        }

        [UnmanagedCallersOnly(EntryPoint = "voice")]
        public static IntPtr StartVoiceRecognition()
        {
            Console.WriteLine("Reconociendo");
            SttTask().GetAwaiter().GetResult();
            return Marshal.StringToHGlobalAnsi("Iniciado.");
            //switch (res.Reason)
            //{
            //    case ResultReason.RecognizedSpeech:
            //        Console.WriteLine(res.Text);
            //        byte[] bytesReconocimiento = Encoding.UTF8.GetBytes(res.Text);
            //        string base64 = Convert.ToBase64String(bytesReconocimiento);
            //        return Marshal.StringToHGlobalAnsi(base64);
            //    case ResultReason.NoMatch:
            //        return Marshal.StringToHGlobalAnsi("Sin Resultados");
            //    case ResultReason.Canceled:
            //        var cancellation = CancellationDetails.FromResult(res);
            //        return Marshal.StringToHGlobalAnsi($"Error: {cancellation.Reason}");
            //    default:
            //        return Marshal.StringToHGlobalAnsi("No hay una excepcion para esto aun jijiji");
            //}
        }

        [UnmanagedCallersOnly(EntryPoint = "stop")]
        public static IntPtr StopVoiceRecognition()
        {
            Console.WriteLine(_reconocimiento);
            byte[] bytesReconocimiento = Encoding.UTF8.GetBytes(_reconocimiento);
            string base64 = Convert.ToBase64String(bytesReconocimiento);
            return Marshal.StringToHGlobalAnsi(base64);
        }
    }
}