using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Application.Exceptions
{
    public class NoAccessException : ProductBaseException
    {
        public NoAccessException(string instance) : base()
        {
            Type = "violation-access-exception";
            Title = "Access violation exception.";
            Detail = "Modifications of others' products is forbidden.";
            Instance = instance;
        }
    }
}
