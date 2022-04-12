using System;
using Volo.Abp.Domain.Entities;

namespace PPMRm.Core
{
    public class Period : AggregateRoot<int>
    {
        public Period(int year, int month) : base(Convert.ToInt32($"{year}{month:00}"))
        {
            if (month < 1 || month > 12) throw new ArgumentOutOfRangeException(nameof(month));
            if (year < 2021 || year > 2100) throw new ArgumentOutOfRangeException(nameof(year));
            Year = year;
            Month = month;
            StartDate = new DateTime(year, month, 1);
            EndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            if (month > 9) Quarter = Quarter.Q4;
            else if (month > 6) Quarter = Quarter.Q3;
            else if (month > 3) Quarter = Quarter.Q2;
            else Quarter = Quarter.Q1;
        }

        public virtual int Year { get; protected set; }
        public virtual int Month { get; protected set; }
        public virtual DateTime StartDate { get; protected set; }
        public virtual DateTime EndDate { get; protected set; }
        public virtual Quarter Quarter { get; protected set; }
    }
}
