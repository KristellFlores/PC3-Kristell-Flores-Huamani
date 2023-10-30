using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace petclinic.DTO
{
    [Table("t_todo")]
    public class TodoDTO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]

        public int id { get; set; }
        public int userId { get; set; }
        

        public string? title { get; set; }
        public string Body { get; set; }
    }
}