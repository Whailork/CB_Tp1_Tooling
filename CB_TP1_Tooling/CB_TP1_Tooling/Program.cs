// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Text;
using System.Text.Json;
namespace CB_TP1_Tooling
{
    class Program
    {
        static void Main(string[] args)
        {
            string projectPath;
            string command;
            string packagePath;


            switch (args.Length)
            {
                case 2:
                    projectPath = args[0];
                    command = args[1];
                    Console.WriteLine(" project path : " + projectPath);
                    Console.WriteLine(" command : " + command);
                    if (command.Equals("show-infos"))
                    {
                        string infos = File.ReadAllText(projectPath);
                       
                        Console.WriteLine(infos);
                        ProjectInfo? projectInfo = GetProjectInfos(projectPath);
                        Console.WriteLine("\nProject name : " +projectInfo.Modules[0].Name);
                        //check if from source
                        if (projectInfo.EngineAssociation.StartsWith('{') && projectInfo.EngineAssociation.EndsWith('}'))
                        {
                            Console.WriteLine("\nEngine Version : from source");
                        }
                        else
                        {
                            Console.WriteLine("\nEngine Version : " +projectInfo.EngineAssociation);
                        }
                        
                        Console.WriteLine("\nPlugins : ");
                        foreach (Plugin plugin in projectInfo.Plugins)
                        {
                            Console.WriteLine(plugin.toString());
                        }
                        
                    }
                    else
                    {
                        if (command.Equals("build"))
                        {
                            Process process = new Process();
                            ProjectInfo projectInfo = GetProjectInfos(projectPath);

                            process.StartInfo.Arguments = projectInfo.Modules[0].Name + " " + projectInfo.Modules[0].Name + "EditorTarget " + " Win64 " + " Development " + projectPath + " -waitmutex";

                            process.StartInfo.FileName = "D:/UnrealEngine/Engine/Build/BatchFiles/Build.bat";
                            process.Start();

                        }
                    }
                  
                    break;
                case 3:
                    projectPath = args[0];
                    command = args[1];
                    packagePath = args[2];
                    Console.WriteLine(" project path : " + projectPath);
                    Console.WriteLine(" command : " + command);
                    Console.WriteLine(" package path : " + command);

                    if (command.Equals("package"))
                    {
                        Process process = new Process();

                       // process.StartInfo.Arguments = "-ScriptsForProject=D:/gitKrakenRepos/EchoBlade/EchoBlade.uproject BuildCookRun -project=D:/gitKrakenRepos/EchoBlade/EchoBlade.uproject -noP4 -clientconfig=Shipping -serverconfig=Shipping -nocompile -nocompileeditor -installed -unrealexe="D:\Epic Games\UE_5.4\UE_5.4\Engine\Binaries\Win64\UnrealEditor-Cmd.exe" -utf8output -platform=Win64 -build -cook -map=ThirdPersonMap+ThirdPersonMap -CookCultures=en -unversionedcookedcontent -stage -package -cmdline="ThirdPersonMap -Messaging" -addcmdline="-SessionId=FB1CDE264A4BECFEA76FDC988BD9580E -SessionOwner='berge' -SessionName='Tooling' "


                        process.StartInfo.FileName = "D:/UnrealEngine/Engine/Build/BatchFiles/RunUAT.bat";
                        process.Start();
                    }
                    break;
            }

            
            
                
            


        }

        public static ProjectInfo GetProjectInfos(string projectPath)
        {
            string infos = File.ReadAllText(projectPath);
                       
            Console.WriteLine(infos);
            return JsonSerializer.Deserialize<ProjectInfo>(infos);
        }
    }
}



public class ProjectInfo
{
    public int FileVersion { get; set; }
    public string EngineAssociation { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public Module[] Modules { get; set; }
    public Plugin[] Plugins { get; set; }
}

public class Module
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string LoadingPhase { get; set; }
    public string[] AdditionalDependencies { get; set; }
}
public class Plugin
{
    public string Name { get; set; }
    public bool Enabled { get; set; }
    public string[] TargetAllowList { get; set; }

    public string toString()
    {
        return "\nName : " + Name + " Enabled : " + Enabled + " TargetAllowList : " + TargetAllowList;
    }
}