using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MimeMapping;

namespace Questor.Vehicle.Domain.Utils.Files
{
    public static class Base64
    {
        public static async Task<byte[]> LoadFileAsync(EPath type, string name)
        {
            var pathFile = Path.Combine(VehicleStartup.PathFiles, type.ToDescriptionString(), name);
            if (!File.Exists(pathFile)) return null;
            return await File.ReadAllBytesAsync(pathFile);
        }

        public static async Task<string> LoadBase64Async(EPath type, string name) {
            var pathFile = Path.Combine(VehicleStartup.PathFiles, type.ToDescriptionString(), name);
            if (!File.Exists(pathFile)) return null;
            var bytes = await File.ReadAllBytesAsync(pathFile);
            var base64 = Convert.ToBase64String(bytes);
            return $"data:{MimeUtility.GetMimeMapping(name)};base64,{base64}";
        }

        public static async Task SaveAsync(string base64, EPath type, string name)
        {
            var path = Path.Combine(VehicleStartup.PathFiles, type.ToDescriptionString());
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var pathFile = Path.Combine(path, name);
            await File.WriteAllBytesAsync(pathFile, Convert.FromBase64String(base64.Split("base64,").Last()));
        }

        public static void Remove(EPath type, string name)
        {
            var pathFile = Path.Combine(VehicleStartup.PathFiles, type.ToDescriptionString(), name);
            if (!File.Exists(pathFile)) return;
            File.Delete(pathFile);
        }
    }
}
