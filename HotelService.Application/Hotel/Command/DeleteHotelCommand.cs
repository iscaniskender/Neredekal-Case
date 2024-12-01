using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Results;

namespace HotelService.Application.Hotel.Command
{
    public class DeleteHotelCommand : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }

        public DeleteHotelCommand(Guid id)
        {
            Id = id;
        }
    }
}
