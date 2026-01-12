using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBoxApp.Models.Social
{
    public class LikeRequest
    {
        public Guid LikerID { get; set; }
        public Guid FotoID { get; set; } // En este contexto FotoID es el TargetID
    }
}
