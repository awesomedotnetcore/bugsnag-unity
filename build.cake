#tool "nuget:?package=NUnit.ConsoleRunner"

var target = Argument("target", "Default");
var solution = File("./Bugsnag.Unity.sln");
var configuration = Argument("configuration", "Release");
var outputPath = Argument<string>("output", "C:/Users/marti/Documents/bugsnag-unity-test");

Task("Restore-NuGet-Packages")
    .Does(() => NuGetRestore(solution));

Task("Build")
  .IsDependentOn("Restore-NuGet-Packages")
  .Does(() => {
    MSBuild(solution, settings =>
      settings
        .SetVerbosity(Verbosity.Minimal)
        .SetConfiguration(configuration));
  });

Task("Test")
  .IsDependentOn("Build")
  .Does(() => {
    var assemblies = GetFiles($"./tests/**/bin/{configuration}/**/*.Tests.dll");
    NUnit3(assemblies);
  });

Task("CopyToUnity")
  .WithCriteria(() => outputPath != null)
  .IsDependentOn("Build")
  .Does(() => {
    CopyFileToDirectory($"./src/Bugsnag.Native/bin/{configuration}/net35/Bugsnag.Native.dll", $"{outputPath}/Assets/Plugins/x86");
    CopyFileToDirectory($"./src/Bugsnag.Native/bin/{configuration}/net35/Bugsnag.Native.dll", $"{outputPath}/Assets/Plugins/x86_64");
    CopyFileToDirectory($"./src/Bugsnag.Native.Android/bin/{configuration}/net35/Bugsnag.Native.dll", $"{outputPath}/Assets/Plugins/Android");
    CopyFileToDirectory($"./src/Bugsnag.Unity/bin/{configuration}/net35/Bugsnag.Unity.dll", $"{outputPath}/Assets/Standard Assets/Bugsnag");
    CopyFileToDirectory($"./src/Assets/Standard Assets/Bugsnag/Bugsnag.cs", $"{outputPath}/Assets/Standard Assets/Bugsnag");
  });

Task("Default")
  .IsDependentOn("Test")
  .IsDependentOn("CopyToUnity");

RunTarget(target);
