using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace i2Nova.Business.Mapping
{
    public class MapperConfig
    {
        public static List<Profile> Init()
        {
            var profileType = typeof(Profile);

            var profiles = Assembly.GetExecutingAssembly().GetTypes()
        .Where(t => profileType.IsAssignableFrom(t)
            && t.GetConstructor(Type.EmptyTypes) != null)
        .Select(Activator.CreateInstance)
        .Cast<Profile>().ToList();
            return profiles;


        }
    }
}
