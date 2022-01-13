using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using VeganSepeti.CartApi.Common;

namespace VeganSepeti.CartApi.Business
{
    public class ResourceManager
    {
        private static ResourceManager instance;
        public static ResourceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ResourceManager();
                }
                return instance;
            }
        }

        private Dictionary<string, string> resources;
        private Dictionary<string, string> Resources
        {
            get
            {
                if (resources == null || resources.Count > 0)
                {
                    resources = LoadResources();
                }
                return resources;
            }
        }

        /// <summary>
        /// Load resources from json of the resource
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> LoadResources()
        {
            string json = string.Empty;
            string resourcesPath = Path.Combine(Directory.GetCurrentDirectory(), Constants.DataPath, Constants.ResourcesFile);
            using (StreamReader streamReader = new(resourcesPath))
            {
                json = streamReader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        /// <summary>
        /// Gets the resource by resource key
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <returns></returns>
        public string GetResource(string resourceKey)
        {
            if (!Resources.ContainsKey(resourceKey))
            {
                return string.Empty;
            }

            return Resources[resourceKey];
        }
    }
}
