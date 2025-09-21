using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Application.Interfaces.Identity
{
    public interface ICurrentUserService
    {
        public string? GetUserId();
    }
}
