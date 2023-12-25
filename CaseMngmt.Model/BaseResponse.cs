﻿namespace CaseMngmt.Models
{
    public class BaseResponse<TResult>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public TResult Result { get; set; }
    }
}
