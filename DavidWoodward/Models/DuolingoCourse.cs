using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DavidWoodward
{
    public class RootObject
    {
        public List<DuolingoCourse> Courses { get; set; }
}

    public class DuolingoCourse
    {
        public string AuthourID { get; set; }
        public int Crowns { get; set; }
        public string FromLanguage { get; set; }
        public bool HealthEnabled { get; set; }
        public string Id { get; set; }
        public string LearningLanguage { get; set; }
        public bool PlacementTestAvailable { get; set; }
        public bool Preload { get; set; }
        public string Title { get; set; }
        public int Xp { get; set; }

    }
}
