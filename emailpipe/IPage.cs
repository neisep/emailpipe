﻿//
// Copyright (c) 2018 Jimmie Jönsson <jimmie@neisep.com>
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace emailpipe
{
    public interface IPage
    {
        FrameworkElement GenerateContent();
    }
}
