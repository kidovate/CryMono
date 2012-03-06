﻿using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

using System.IO;
using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CryEngine.Utils
{
    /// <summary>
    /// Handles retrieval of required assemblies for compiled scripts etc.
    /// </summary>
    public class AssemblyReferenceHandler
    {
        public AssemblyReferenceHandler()
        {
            Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            ReferencedAssemblies = new List<string>(loadedAssemblies.Length);
            ReferencedAssemblies.AddRange(loadedAssemblies.Select(a => a.Location));
        }

        /// <summary>
        /// Gets the required assemblies for the scripts passed to the method.
        /// Note: Does NOT exclude assemblies already loaded by CryMono.
        /// </summary>
        /// <param name="scriptFilePaths"></param>
        /// <returns></returns>
        public IEnumerable<string> GetRequiredAssembliesForScriptFiles(IEnumerable<string> scriptFilePaths)
        {
            var namespaces = new List<string>();
            var assemblyPaths = new List<string>();

            foreach (var scriptFilePath in scriptFilePaths)
            {
                foreach (var foundNamespace in GetNamespacesFromScriptFile(scriptFilePath))
                {
                    if (!namespaces.Contains(foundNamespace))
                        namespaces.Add(foundNamespace);
                }
            }

            foreach (var foundNamespace in namespaces)
                assemblyPaths.Add(GetAssemblyPathFromNamespace(foundNamespace));

            return assemblyPaths;
        }


        /// <summary>
        /// Gets the required assemblies for the script passed to the method.
        /// Note: Does NOT exclude assemblies already loaded by CryMono.
        /// </summary>
        /// <param name="scriptFilePath"></param>
        /// <returns></returns>
        private IEnumerable<string> GetNamespacesFromScriptFile(string scriptFilePath)
        {
            if (string.IsNullOrEmpty(scriptFilePath))
                return null;

            using (var stream = new FileStream(scriptFilePath, FileMode.Open))
            {
                return GetNamespacesFromStream(stream);
            }
        }

        protected IEnumerable<string> GetNamespacesFromStream(Stream stream)
        {
            var namespaces = new List<string>();

            using (var streamReader = new StreamReader(stream))
            {
                string line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    //Filter for using statements
                    var matches = Regex.Matches(line, @"using ([^;]+);");
                    foreach (Match match in matches)
                    {
                        string foundNamespace = match.Groups[1].Value;
                        if (!namespaces.Contains(foundNamespace))
                        {
                            namespaces.Add(foundNamespace);
                        }
                    }
                }
            }

            return namespaces;
        }

        private string GetAssemblyPathFromNamespace(string name)
        {
            // Avoid reloading the xml file for every call
            if (assemblyLookupDocument == null)
            {
                assemblyLookupDocument = XDocument.Load(Path.Combine(PathUtils.GetEngineFolder(), "Mono", "assemblylookup.xml"));
            }

            string[] assemblies = Directory.GetFiles(Path.Combine(PathUtils.GetEngineFolder(), "Mono", "lib", "mono", "gac"), "*.dll", SearchOption.AllDirectories);
            foreach (var node in assemblyLookupDocument.Descendants().Where(e => e.Name.LocalName == "Namespace" && e.Attribute("name").Value == name))
            {
                string assemblyName = node.Parent.Attribute("name").Value;


                foreach (var assembly in assemblies)
                {
                    if (assembly.Contains(assemblyName))
                    {
                        assemblyName = assembly;
                        break;
                    }
                }

                if (!ReferencedAssemblies.Contains(assemblyName))
                    return assemblyName;
            }

            return null;
        }

        private XDocument assemblyLookupDocument;

        /// <summary>
        /// All libraries passed through ScriptCompiler.LoadLibrariesInFolder will be automatically added to this list.
        /// </summary>
        public List<string> ReferencedAssemblies { get; protected set; }
    }
}