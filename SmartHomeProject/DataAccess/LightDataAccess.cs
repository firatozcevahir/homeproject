using Microsoft.EntityFrameworkCore;
using SmartHomeProject.Helpers;
using SmartHomeProject.Interfaces;
using SmartHomeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeProject.DataAccess
{
    public class LightDataAccess : ILight
    {
        private readonly SmartHomeDbContext _db;
        private readonly CommandProcessor _cmdprocessor;
        private ProcessedCommand processedCommand;
        public LightDataAccess(SmartHomeDbContext db)
        {
            _cmdprocessor = new CommandProcessor();
            _db = db;
        }
        public IEnumerable<Light> GetAllLights()
        {
            return _db.Light.ToList();
        }

        public Light GetLightData(int id)
        {
            return _db.Light.FirstOrDefault(l => l.Id == id);
        }

        public string ProcessLightCommand(string commandText)
        {
            processedCommand = _cmdprocessor.ProcessCommand(commandText);
            var light = _db.Light.FirstOrDefault(l => l.Code == processedCommand.Module + processedCommand.Code);
            switch (processedCommand.CommandType)
            {
                case "set":
                    if (light != null)
                    {
                        light.Status = processedCommand.Status == "00" ? false : true;
                        _db.Attach(light).State = EntityState.Modified;
                        try
                        {
                            //successful : 1
                            return _db.SaveChanges().ToString();
                        }
                        catch
                        {
                            //error processing the command : 102
                            return "102";
                        }
                    }
                    else
                        //cannot find the object : 101
                        return "101";
                case "get":
                    if (light != null)
                    {
                        //get command value : 1000
                        var result = "get" + light.Code + (light.Status ? "01" : "00");
                        return result;
                    }
                    else
                        //cannot find the object : 101
                        return "101";
                case "del":
                    if (light != null)
                    {
                        try
                        {
                            _db.Remove(light);
                            //successful : 1
                            return _db.SaveChanges().ToString();

                        }
                        catch (Exception)
                        {
                            //error processing the command : 102
                            return "102";
                        }
                    }
                    else
                    {
                        //cannot find the object : 101
                        return "101";
                    }

                default:
                    //wrong command type : 100
                    return "100";
            }



        }

        public int AddLight(Light light)
        {

            if (!CheckIfAlreadyExists(light))
            {
                _db.Light.Add(light);
                return _db.SaveChanges();
            }
            else
            {
                //item already exists
                return 0;
            }
        }

        public bool CheckIfAlreadyExists(Light light)
        {
            return _db.Light.Any(l => l.Code == light.Code);
        }

    }
}
