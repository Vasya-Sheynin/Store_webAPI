﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application;

namespace Users.Application.Validation.Commands
{
    public record ValidateUserRecoveryCommand(UserRecoveryDto userRecoveryDto) : IRequest;
}