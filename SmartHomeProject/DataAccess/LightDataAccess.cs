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
        private readonly BaseHelper _basehelper;
        private ProcessedCommand processedCommand;
        public LightDataAccess(SmartHomeDbContext db)
        {
            _basehelper = new BaseHelper();
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
            processedCommand = _basehelper.ProcessCommand(commandText);
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
                            return _basehelper.MsgCodes.ErrProcessingCommand.ToString();
                        }
                    }
                    else
                        //cannot find the object : 101
                        return _basehelper.MsgCodes.ErrCantFindObject.ToString();
                case "get":
                    if (light != null)
                    {
                        //get command responce Code : 1000
                        return processedCommand.CommandType + processedCommand.Module + processedCommand.Code + (light.Status ? "01" : "00");
                    }
                    else
                        //cannot find the object : 101
                        return _basehelper.MsgCodes.ErrCantFindObject.ToString();
                case "del":
                    if (light != null)
                    {
                        try
                        {
                            _db.Light.Remove(light);
                            if (_db.SaveChanges() > 0)
                            {
                                return _basehelper.MsgCodes.SuccItemDeleted.ToString();
                            }
                            else
                            {
                                //error processing the command : 102
                                return _basehelper.MsgCodes.ErrProcessingCommand.ToString();
                            }

                        }
                        catch (Exception)
                        {
                            //error processing the command : 102
                            return _basehelper.MsgCodes.ErrProcessingCommand.ToString();
                        }
                    }
                    else
                    {
                        //cannot find the object : 101                        
                        return _basehelper.MsgCodes.ErrCantFindObject.ToString();
                    }
                case "add":
                    if (light == null)
                    {
                        _db.Light.Add(new Light()
                        {
                            Code = processedCommand.Module + processedCommand.Code,
                            Description = processedCommand.Description ?? "No Description",
                            Status = processedCommand.Status == "00" ? false : true
                        });
                        if (_db.SaveChanges() > 0)
                        {
                            //success new item added : 103
                            return _basehelper.MsgCodes.SuccItemAdded.ToString();
                        }
                        else
                        {
                            //error processing the command : 102
                            return _basehelper.MsgCodes.ErrProcessingCommand.ToString();
                        }
                    }
                    else
                    {
                        //item already exists
                        return _basehelper.MsgCodes.ErrItemAlreadyExists.ToString();
                    }
                default:
                    //wrong command type : 100
                    return _basehelper.MsgCodes.ErrWrongCommandType.ToString(); ;
            }
        }

        public int DeleteLight(Light light)
        {
            if (LightExists(light))
            {
                _db.Light.Remove(light);
                return _db.SaveChanges();
            }
            else
            {
                //cannot find the object
                return _basehelper.MsgCodes.ErrCantFindObject;
            }
        }

        public int AddLight(Light light)
        {

            if (!LightExists(light))
            {
                _db.Light.Add(light);
                return _db.SaveChanges();
            }
            else
            {
                //item already exists
                return _basehelper.MsgCodes.ErrItemAlreadyExists;
            }
        }

        public bool LightExists(Light light)
        {
            if (light.Code == null)
            {
                return _db.Light.Any(l => l.Id == light.Id);
            }
            else
            {
                return _db.Light.Any(l => l.Code == light.Code);
            }
        }

    }
}
