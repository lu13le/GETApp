using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GETApp.Models
{
    public class Reservation
    {
        [Key]
        public int ResId { get; set; }
        [DisplayName("Ime i prezime")]
        public string Name { get; set; }
        [DisplayName("Broj telefona")]
        public string PhoneNumber { get; set; }
        [DisplayName("Datum i vreme")]
        public DateTime DateTime { get; set; }
        [DisplayName("Sto")]
        public string Table { get; set; }
        [DisplayName("Dodatne informacije")]
        public string AdditionalComment { get; set; }
    }
}
