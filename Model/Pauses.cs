using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilencePauseApp.Model
{
    public class Pauses : BaseEntity
    {
        public string DateEvent { get; set; }
        public int Hour { get; set; }
        public int Min { get; set; }
        public float Sec { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public float Depth { get; set; }
        public float DDep { get; set; }
        public float Ms { get; set; }
        public float Kl { get; set; }
    }
}
