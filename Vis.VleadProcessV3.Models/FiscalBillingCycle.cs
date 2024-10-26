using System;
using System.Collections.Generic;

namespace Vis.VleadProcessV3.Models;

public partial class FiscalBillingCycle
{
    public int Id { get; set; }

    public int? BillingCycleId { get; set; }

    public DateTime? MonthOne { get; set; }

    public DateTime? MonthTwo { get; set; }

    public DateTime? MonthThree { get; set; }

    public DateTime? MonthFour { get; set; }

    public DateTime? MonthFive { get; set; }

    public DateTime? MonthSix { get; set; }

    public DateTime? MonthSeven { get; set; }

    public DateTime? MonthEight { get; set; }

    public DateTime? MonthNine { get; set; }

    public DateTime? MonthTen { get; set; }

    public DateTime? MonthEleven { get; set; }

    public DateTime? MonthTwelve { get; set; }

    public bool? MonthOneIsActive { get; set; }

    public bool? MonthTwoIsActive { get; set; }

    public bool? MonthThreeIsActive { get; set; }

    public bool? MonthFourIsActive { get; set; }

    public bool? MonthFiveIsActive { get; set; }

    public bool? MonthSixIsActive { get; set; }

    public bool? MonthSevenIsActive { get; set; }

    public bool? MonthEightIsActive { get; set; }

    public bool? MonthNineIsActive { get; set; }

    public bool? MonthTenIsActive { get; set; }

    public bool? MonthElevenIsActive { get; set; }

    public bool? MonthTwelveIsActive { get; set; }

    public virtual BillingCycle? BillingCycle { get; set; }
}
