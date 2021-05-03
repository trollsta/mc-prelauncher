﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace MC_Prelauncher
{
    class Program
    {
        static void Setup()
        {
            int gay = 0;
            string[] reqdir =
            {
                $@"C:\Users\{Environment.UserName}\AppData\Roaming\.minecraft\Prelauncher",
                $@"C:\Users\{Environment.UserName}\AppData\Roaming\.minecraft\Prelauncher\mods",
                $@"C:\Users\{Environment.UserName}\AppData\Roaming\.minecraft\Prelauncher\settings",
                $@"C:\Users\{Environment.UserName}\AppData\Roaming\.minecraft\Prelauncher\settings\folderconfigs"
            };

            foreach (string dir in reqdir)
            {
                if (!Directory.Exists(dir))
                {
                    gay++;
                    Directory.CreateDirectory(dir);
                }
            }
            if (gay > 0) { Console.WriteLine("You were missing some directories, so I kindly added them for you :)"); Thread.Sleep(1000); Console.Clear(); }
        }
        static void Main()
        {
            Setup();
            ModFolderDetect GetModFolder = new ModFolderDetect();
            string currentModFolder = GetModFolder.CompareCurrent();

            string pathToLauncher = "C:\\Program Files (x86)\\Minecraft Launcher\\MinecraftLauncher.exe";
            Console.WriteLine("Minecraft Pre-launcher by Trollsta_");
            Console.WriteLine("Current mod folder: {0}", currentModFolder);

            while (true)
            {
                Console.Write("$ "); string x = Console.ReadLine();

                string[] y = x.Split(' ');

                CommandList Commands = new CommandList();
                switch (y[0])
                {
                    // editmode
                    case "editmode":
                        if (y.Length <= 1)
                        {
                            Console.WriteLine("No modfolder specified.\n");
                            break;
                        }
                        else if (!File.Exists($@"C:\Users\{Environment.UserName}\AppData\Roaming\.minecraft\Prelauncher\settings\folderconfigs\{y[1]}"))
                        {
                            Console.WriteLine("No such modfolder exists.\n");
                            break;
                        }
                        Editmode editmode = new Editmode();
                        editmode.editmode(y[1]);
                        break;
                    // mod folder set
                    case "smf":
                        Commands.setModFolder(y[1]);
                        Console.Clear();
                        currentModFolder = GetModFolder.CompareCurrent();
                        Console.WriteLine("Minecraft Pre-launcher by Trollsta_");
                        Console.WriteLine("Current mod folder: {0}", currentModFolder); 
                        break;

                    case "setmodfolder":
                        Commands.setModFolder(y[1]);
                        Console.Clear();
                        currentModFolder = GetModFolder.CompareCurrent();
                        Console.WriteLine("Minecraft Pre-launcher by Trollsta_");
                        Console.WriteLine("Current mod folder: {0}", currentModFolder);
                        break;

                    // other

                    case "listmods":
                        if (y.Length > 1)
                        {
                            if (File.Exists($@"C:\Users\{Environment.UserName}\AppData\Roaming\.minecraft\Prelauncher\settings\folderconfigs\{y[1]}"))
                            {
                                Console.WriteLine("Mods present in {0}:", y[1]);
                                foreach (string modname in File.ReadAllText($@"C:\Users\{Environment.UserName}\AppData\Roaming\.minecraft\Prelauncher\settings\folderconfigs\{y[1]}").Split(';'))
                                {
                                    Console.WriteLine(modname);
                                }
                                Console.WriteLine();
                            }
                            else Console.WriteLine("No such mod folder exists");
                        }
                        else
                        {
                            Console.WriteLine("Mods present in {0}:", currentModFolder);
                            foreach (string modname in File.ReadAllText($@"C:\Users\{Environment.UserName}\AppData\Roaming\.minecraft\Prelauncher\settings\folderconfigs\{currentModFolder}").Split(';'))
                            {
                                Console.WriteLine(modname);
                            }
                            Console.WriteLine();
                        }
                        break;

                    case "modfolders":
                        Console.Write("All present mod folders: ");
                        foreach (string file in Directory.GetFiles($@"C:\Users\{Environment.UserName}\AppData\Roaming\.minecraft\Prelauncher\settings\folderconfigs"))
                        {
                            Console.Write(file.Split('\\').Last());
                        }
                        Console.WriteLine();
                        break;

                    case "help":
                        Commands.help(y[1]);
                        break;

                    case "exit":
                        Environment.Exit(0);
                        break;

                    case "launch": // ignore any other parameters since they dont interfere with launch
                        Process.Start(pathToLauncher);
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
