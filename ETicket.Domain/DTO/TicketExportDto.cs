using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ETicket.Domain.DTO
{
    public class TicketExportDto
    {
        public List<string> Categories{ get; set; }
        [Display(Name = "Choose category: ")]
        public string ChosenCategory { get; set; }
        public TicketExportDto()
        {
            this.Categories = new List<string>();
        }
    }
}
