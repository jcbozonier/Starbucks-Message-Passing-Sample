﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarbucksExample.MessagingSystem
{
    public interface IChannel
    {
        void Enqueue(object o);
        object Dequeue();
    }
}