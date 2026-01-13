<<<<<<< HEAD
﻿using System.ComponentModel.DataAnnotations;

namespace HCAMiniEHR.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public string Specialty { get; set; }
        public string Gender { get; set; }

        // Add RowVersion as a concurrency token
        //[Timestamp]
        //public byte[] RowVersion { get; set; }
    }

}
=======
﻿using System.ComponentModel.DataAnnotations;

namespace HCAMiniEHR.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public string Specialty { get; set; }
        public string Gender { get; set; }

        // Add RowVersion as a concurrency token
        //[Timestamp]
        //public byte[] RowVersion { get; set; }
    }

}
>>>>>>> de0c1979792b9fba70a0d3608ff20cf61cb6a43b
