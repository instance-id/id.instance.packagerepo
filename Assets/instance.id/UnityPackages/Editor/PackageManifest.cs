using System;
using UnityEngine;

namespace Gameframe.Packages
{
  [Serializable]
  public class PackageManifest
  {
          [Serializable]
        public class PackageAuthor
        {
            public string name = "instance.id";
            public string email = "dan@instance.id";
            public string url = "https://github.com/instance-id";
            public string twitter = "instance_id";
            public string github = "instance-id";
        }


        public string githubUrl = "";
        public string name = "id.instance.mypackagename";
        public string displayName = "My Package Name";
        public string repositoryName = "RepositoryName";
        public string version = "0.1.0";
        public string description = "";
        public string type = "library"; //tool, module, tests, sample, template, library
        public string unity = "";
        public string unityRelease = "";
        public string[] keywords = new string[0];
        public PackageAuthor author = new PackageAuthor();


    public PackageManifest()
    {
      var versionString = Application.unityVersion;
      var splitVersion = versionString.Split('.');
      unity = $"{splitVersion[0]}.{splitVersion[1]}";
      unityRelease = splitVersion[2];
    }
  }
}
