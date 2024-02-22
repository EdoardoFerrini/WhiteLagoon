﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interfaces
{
	public interface IBookingRepository : IRepository<Amenity>
	{
		void Update(Booking entity);
	}
}