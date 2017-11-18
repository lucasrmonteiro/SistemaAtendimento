using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetUtil.Util.Enums {
    public enum Restriction {
        NotEq,
        Eq,
        EqDate,
        EqDateTime,
        Ge,
        GeDate,
        GeDateTime,
        Le,
        LeDate,
        LeDateTime,
        ExtractDay,
        ExtractMonth,
        ExtractYear,
        IsNull,
        IsNotNull,
        Between,
        BetweenDateTime,
        Like,
        LikeLeft,
        LikeRight,
        In,
        NotIn
    }
}