﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction
{
    public interface IValidateable<T>
    {
        AbstractValidator<T> Validator { get; }
    }
}
