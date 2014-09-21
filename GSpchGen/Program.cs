using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

//NAudio
using NAudio;
using NAudio.Wave;


//Debug
using System.Diagnostics;

namespace GSpchGen
{
    class Program
    {
        static void Main(string[] args)
        {
            string sLang = "zh-CN";
            string sText = "";
            
            Uri uriMp3;
            string sMp3Path;
            string sWavPath;

            
            //Read text from text.txt
            if (args.Count() > 0)
            {
                sText = File.ReadAllText(args[0], Encoding.UTF8);
            }
            else
            {
                sText = File.ReadAllText(Environment.CurrentDirectory + @"\text.txt", Encoding.UTF8);
            }
            
            //Detect Language
            using (WebClient wcLang = new WebClient())
            {
                wcLang.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0 (compatible; MSIE 9.0; Windows;)");
                Uri uriLang = new Uri("http://translate.google.cn/translate_a/t?client=t&sl=auto&text=" + sText);
                
                wcLang.DownloadFile(uriLang, "lang.txt");
                string parseResult = Utils.GetLang(File.ReadAllText("lang.txt"));
                if (parseResult == "") { parseResult = "zh-CN"; }
                sLang = parseResult;
            }

            uriMp3 = new Uri("http://translate.google.cn/translate_tts?tl=" + sLang + "&q=" + sText);  
            sMp3Path = Environment.CurrentDirectory + @"\tmp.mp3";
            sWavPath=Environment.CurrentDirectory+@"\..\res\sound\"+sText+".wav";

            //Get MP3 from Google
            using (WebClient wcMp3Speech = new WebClient())
            {
                wcMp3Speech.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0 (compatible; MSIE 9.0; Windows;)");
                wcMp3Speech.DownloadFile(uriMp3, sMp3Path);
            }

            //Now convert the MP3 to WAV
            using (Mp3FileReader reader = new Mp3FileReader(new FileStream(sMp3Path,FileMode.OpenOrCreate)))
            {
                WaveFileWriter.CreateWaveFile(sWavPath, reader);
            }

            //Now copy the WAV to speech.wav
            if (File.Exists(Environment.CurrentDirectory + @"\..\res\sound\speech.wav"))
            {
                File.Delete(Environment.CurrentDirectory + @"\..\res\sound\speech.wav");
            }

            File.Copy(sWavPath, Environment.CurrentDirectory + @"\..\res\sound\speech.wav");
        }
    }
}
