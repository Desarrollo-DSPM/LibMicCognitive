using System;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace LibMicCognitive
{
    public class Reconocimiento
    {
        static string YourSubscriptionKey = "<Llave de Azure>";
        static string YourServiceRegion = "<Region>";

        async static Task<SpeechRecognitionResult> SttTask()
        {
            var speechConfig = SpeechConfig.FromSubscription(YourSubscriptionKey, YourServiceRegion);
            speechConfig.SpeechRecognitionLanguage = "<Idioma>";

            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

            Console.WriteLine("Habla al microfono.");
            var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
            return speechRecognitionResult;
        }

        [UnmanagedCallersOnly(EntryPoint = "voice")]
        public static IntPtr Voice()
        {
            SpeechRecognitionResult res = SttTask().GetAwaiter().GetResult();
            switch (res.Reason)
            {
                case ResultReason.RecognizedSpeech:
                    Console.WriteLine(res.Text);
                    return Marshal.StringToHGlobalAnsi(res.Text);
                case ResultReason.NoMatch:
                    return Marshal.StringToHGlobalAnsi("Sin Resultados");
                case ResultReason.Canceled:
                    var cancellation = CancellationDetails.FromResult(res);
                    return Marshal.StringToHGlobalAnsi($"Error: {cancellation.Reason}");
                default:
                    return Marshal.StringToHGlobalAnsi($"Texto: {res.Text}");
            }
        }
    }
}
