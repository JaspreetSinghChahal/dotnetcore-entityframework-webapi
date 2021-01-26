﻿using System;

namespace Autobot.Data.Models
{
    public class PromoCodeBatchScanActivity
    {
        public long PromoCodeNumber { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public DateTime ScannedDateTime { get; set; }
        public bool IsSuccess { get; set; }
        public int FilteredCount { get; set; }
    }
}

