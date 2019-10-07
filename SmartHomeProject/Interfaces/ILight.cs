using SmartHomeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeProject.Interfaces
{
    public interface ILight
    {
        IEnumerable<Light> GetAllLights();
        int AddLight(Light light);
        string ProcessLightCommand(string commandtext);
        Light GetLightData(int id);
    }
}
