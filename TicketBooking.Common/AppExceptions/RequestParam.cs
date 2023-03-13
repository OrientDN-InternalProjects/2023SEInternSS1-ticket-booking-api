﻿namespace TicketBooking.Service.Models
{
    public class RequestParam
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public RequestParam()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }
        public RequestParam(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
        }
    }    
}

