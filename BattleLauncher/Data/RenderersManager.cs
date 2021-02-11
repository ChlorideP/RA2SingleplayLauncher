using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using BattleLauncher.Data.Model;
using BattleLauncher.Data.Utils;
using BattleLauncher.Exceptions;

namespace BattleLauncher.Data
{
    public static class RenderersManager
    {
        private const string CONFIG_PATH = @"Resources\Configs\Renderers.yaml";
        private const string DLL_NAME = "ddraw.dll";
        private const string FOLDER_PATH = @"Resources\Renderers";
        private static Renderer _current;
        private static Renderer Format(this Renderer @this)
        {
            if (string.IsNullOrEmpty(@this.Main))
                throw new RendererException("找不到ddraw.dll");

            if (string.IsNullOrEmpty(@this.Dir))
                @this.Dir = @this.Id;

            return @this;
        }

        public static readonly Renderer NullRenderer = new Renderer { Id = "null" };
        public static readonly Dictionary<string, Renderer> Renderers = new Dictionary<string, Renderer>();
        public static Renderer Current => File.Exists("ddraw.dll") ? _current : NullRenderer;

        public static void Apply(string id) => Apply(Renderers[id]);

        public static void Apply(Renderer renderer)
        {
            if (renderer is null)
                throw new ArgumentNullException($"{nameof(renderer)} is null");

            Clear();

            OverAll.GlobalConfig.Head["Renderer"] = (_current = renderer).Id;
            if (renderer.Id == "null")
                return;

            var workDir = Path.GetFullPath(Path.Combine(FOLDER_PATH, renderer.Dir));

            File.Copy(
                Path.Combine(workDir, _current.Main),
                Path.Combine(OverAll.MainFolder.FullName, DLL_NAME), true);

            if (!string.IsNullOrEmpty(_current.Config))
                File.Copy(
                    Path.Combine(workDir, _current.Config),
                    Path.Combine(OverAll.MainFolder.FullName, _current.Config), true);

            if (_current.Files?.Count > 0)
                _current.Files.ForEach(i => File.Copy(
                    Path.Combine(workDir, i),
                    Path.Combine(OverAll.MainFolder.FullName, i), true));
        }

        public static void Clear()
        {
            File.Delete(
                Path.Combine(OverAll.MainFolder.FullName, DLL_NAME));

            if (!string.IsNullOrEmpty(_current.Config))
                File.Delete(
                    Path.Combine(OverAll.MainFolder.FullName, _current.Config));

            if (_current.Files?.Count > 0)
                _current.Files.ForEach(i => File.Delete(
                    Path.Combine(OverAll.MainFolder.FullName, i)));
        }

        public static void Init()
        {
            Trace.WriteLine("[Renderer.Init()]\t:初始化开始");


            Renderers.Add(NullRenderer.Id, NullRenderer);

            File.ReadAllText(Path.Combine(OverAll.MainFolder.FullName, CONFIG_PATH))
                .FromYaml<List<Renderer>>()
                .ForEach(i => Renderers.Add(i.Id, i.Format()));

            string current_id = OverAll.GlobalConfig.Head["Renderer"];

            if (string.IsNullOrEmpty(current_id) ||
                current_id.Equals("null", StringComparison.OrdinalIgnoreCase) ||
                !Renderers.TryGetValue(current_id, out _current))
                _current = NullRenderer;
        }
    }
}
