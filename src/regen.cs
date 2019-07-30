/*
 * * * * * * * * * * * * * * * * * * * * * * * *
 * regen_3-0_3-1.cs
 * * * * * * * * * * * * * * * * * * * * * * * *
 * Regenerate - WTB 3.0 to WTB 3.1 
 * Written by Phoenix Mandala for PriceSpider
 * DEL-24360
 * * * * * * * * * * * * * * * * * * * * * * * *
 * This program will convert a 3.0 WTB to the
 * newest 3.1 version.
 * It does so by first checking out the 3.1
 * WTB template.
 * Then, it will get the appropriate config
 * data from cdn.pricespider.com.
 * The program will attempt to automatically
 * parse the current WTB's Client and Widget
 * Config IDs through the provided widget.json.
 * The program will then replace the current
 * widget.json's config property with
 * the extracted data from cdn.
 * Then it will save the JSON and execute
 * yo widget.
 * Program will then terminate.
 * * * * * * * * * * * * * * * * * * * * * * * *
 * Before executing this program, be sure you
 * have checked out WTB 3.1!
 * * * * * * * * * * * * * * * * * * * * * * * *
 * Requirements:
 * Json.NET
 * > newtonsoft.com/json
 * > Newtonsoft.Json.dll
 * * * * * * * * * * * * * * * * * * * * * * * *
 * To build:
 * Run `make`.
 * * * * * * * * * * * * * * * * * * * * * * * *
 */


using System;
using System.Net;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace regen_3_0_to_3_1
{

    class Program
    {
        static void exit() {
            if (System.Windows.Forms.Application.MessageLoop) 
            {
                // WinForms app
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                // Console app
                System.Environment.Exit(1);
            }
        }
        static void outputError(Exception e, string message) {
            Console.ForegroundColor = ConsoleColor.Yellow; 
            Console.WriteLine("!!!");
            Console.WriteLine(message);
            Console.WriteLine("!!!");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e);
            Console.ForegroundColor = ConsoleColor.Gray;
            exit();
        }

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *");
            Console.WriteLine("* Before running this program, be sure you update to WTB 3.1! *");
            Console.WriteLine("* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *");
            Console.ForegroundColor = ConsoleColor.Gray;

              /**********************/
             /** Read widget.json **/
            /**********************/
            string widgetText = "";
            try {
                widgetText = System.IO.File.ReadAllText("./widget.json");
            }
            catch (Exception e) {
                outputError(e, "ERROR READING WIDGET.JSON");
            }

              /***********************/
             /** Parse widget.json **/
            /***********************/
            string accountID = "", configID = "";
            JObject rss = new JObject();
            try {
                //Serialize.
                rss = JObject.Parse(widgetText);
                //Parse.
                JObject widget = (JObject)rss["widget"];

                accountID = (string)widget["accountId"];
                configID = (string)widget["configId"];
            }
            catch (Exception e) {
                outputError(e, "ERROR PARSING WIDGET.JSON");
            }

              /*************************/
             /** Get config from CDN **/
            /*************************/
            try {
                using (var client = new WebClient()) {
                string url = "https://cdn.pricespider.com/1/"+accountID+"/"+configID+"/config.js";
                client.DownloadFile(url,"config.txt");
                Console.WriteLine("Configuration downloaded from "+url);
                }
            }
            catch (Exception e) {
                outputError(e, "ERROR DOWNLOADING CONFIG");
            }

              /******************/
             /** Parse config **/
            /******************/
            string configText = "";
            try {
                configText = System.IO.File.ReadAllText("./config.txt");
                configText = configText.Replace("PriceSpider.onload(\""+configID+"/config\",", "");
                configText = configText.Remove(configText.Length-1);
            }
            catch (Exception e) {
                outputError(e, "ERROR PARSING CONFIG.TXT");
            }


              /**************************/
             /** Write to widget.json **/
            /**************************/
            try {
                JObject config = (JObject)rss["config"];
                config = JObject.Parse(configText);

                System.IO.File.WriteAllText("widget.json",rss.ToString());

                System.IO.File.Delete("config.txt");

                Console.WriteLine("Wrote to widget.json");
            }
            catch (Exception e) {
                outputError(e, "ERROR WRITING TO WIDGET.JSON");
            }

              /*********************/
             /** Run `yo widget` **/
            /*********************/
            try {
                Console.WriteLine("Launching `yo widget`...");
                Process yo = System.Diagnostics.Process.Start("CMD.exe","/C yo widget");
                yo.WaitForExit();

            }
            catch (Exception e) {
                outputError(e, "ERROR RUNNING YO WIDGET");
            }
            /**/

            Console.WriteLine("Rebuild successful!");

        }
    }
}
